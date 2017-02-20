using System;
using System.Data.SQLite;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Kontur.GameStats.Server
{
    internal class StatServer : IDisposable
    {
        private readonly HttpListener listener;
        private readonly Router router;
        private readonly IStatsRepository statsRepository;

        private Thread listenerThread;
        private bool disposed;
        private volatile bool isRunning;

        public StatServer()
        {
            listener = new HttpListener();

            StatsDbContext context = new StatsDbContext();
            statsRepository = new DatabaseStatsRepository(context);

            /*sqLiteConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            sqLiteConnection.Open();*/

            router = new Router(statsRepository);
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
            try
            {
                string requestBody;
                using (StreamReader reader = new StreamReader(listenerContext.Request.InputStream))
                {
                    requestBody = reader.ReadToEnd();
                }
                object result = router.RouteRequest(
                    listenerContext.Request.RawUrl,
                    JsonConvert.DeserializeObject(requestBody),
                    listenerContext.Request.HttpMethod);

                listenerContext.Response.StatusCode = (int)HttpStatusCode.OK;
                resultText = JsonConvert.SerializeObject(result);
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
