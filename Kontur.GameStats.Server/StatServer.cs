using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using LiteDB;
using Newtonsoft.Json;

namespace Kontur.GameStats.Server
{
    internal class StatServer : IDisposable
    {
        private readonly HttpListener listener;
        private readonly Router router;
        private readonly LiteDatabase database;

        private Thread listenerThread;
        private bool disposed;
        private volatile bool isRunning;

        public StatServer()
        {
            listener = new HttpListener();

            database = new LiteDatabase("MyDataBase.db");

            router = new Router(database);

            const string endpointRegexp = @"(([a-zA-Z]+|\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})-(\d+))";
            const string timestampRegexp = @"(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}Z)";
            const string countRegexp = @"(\d+)";

            router.Bind("^/servers/" + endpointRegexp + "/info/?$", "PUT", new AdvertiseServerInfo());
            router.Bind("^/servers/" + endpointRegexp + "/matches/" + timestampRegexp + "/?$", "PUT", new AdvertiseMatchResult());

            router.Bind("^/servers/" + endpointRegexp + "/info/?$", "GET", new GetServerInfo());
            router.Bind("^/servers/info/?$", "GET", new GetServersInfo());
            router.Bind("^/servers/" + endpointRegexp + "/matches/" + timestampRegexp + "/?$", "GET", new GetServersMatches());
        }

        public void Start(string prefix)
        {
            lock (listener)
            {
                if (isRunning) return;

                listener.Prefixes.Clear();
                listener.Prefixes.Add(prefix);
                listener.Start();

                listenerThread = new Thread(Listen)
                {
                    IsBackground = true,
                    Priority = ThreadPriority.Highest
                };
                listenerThread.Start();

                isRunning = true;
            }
        }

        public void Stop()
        {
            lock (listener)
            {
                if (!isRunning)
                    return;

                listener.Stop();

                listenerThread.Abort();
                listenerThread.Join();

                isRunning = false;
            }
        }

        public void Dispose()
        {
            if (disposed)
                return;

            disposed = true;

            Stop();

            listener.Close();

            database.Dispose();
        }

        private void Listen()
        {
            while (true)
            {
                try
                {
                    if (listener.IsListening)
                    {
                        var context = listener.GetContext();
                        Task.Run(() => HandleContextAsync(context));
                    }
                    else Thread.Sleep(0);
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception error)
                {
                    // TODO: log errors
                }
            }
        }

        private async Task HandleContextAsync(HttpListenerContext listenerContext)
        {
            string resultText = "";

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = NamesContractResolver.Instance,
                DateFormatString = "s\\Z"
            };

            try
            {
                string requestBody;
                using (StreamReader reader = new StreamReader(listenerContext.Request.InputStream))
                {
                    requestBody = reader.ReadToEnd();
                }

                object requestData = JsonConvert.DeserializeObject(requestBody, serializerSettings);

                object result = router.RouteRequest(
                    listenerContext.Request.RawUrl,
                    requestData,
                    listenerContext.Request.HttpMethod);

                listenerContext.Response.StatusCode = (int)HttpStatusCode.OK;
                resultText = JsonConvert.SerializeObject(result, serializerSettings);
            }
            catch (BadRequestException e)
            {
                listenerContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                resultText = e.Message;
            }
            catch (PageNotFoundException e)
            {
                listenerContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                resultText = e.Message;
            }
            catch (Exception e)
            {
                // Some internal error, catch it to log and not break the server

                // TODO: log error
                listenerContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            finally
            {
                using (var writer = new StreamWriter(listenerContext.Response.OutputStream))
                {
                    writer.WriteLine(resultText);
                }
            }
        }
    }
}
