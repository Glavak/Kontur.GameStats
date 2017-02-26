using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;

namespace Kontur.GameStats.Server
{
    internal class StatServer : IDisposable
    {
        private readonly HttpListener listener;
        private readonly Router router;
        private readonly LiteDatabase database;
        private readonly LoggerToFile logger;

        private Thread listenerThread;
        private bool disposed;
        private volatile bool isRunning;

        public StatServer()
        {
            listener = new HttpListener();

            var unityContainer = new UnityContainer();

            database = new LiteDatabase("MyDataBase.db");
            unityContainer.RegisterInstance(database);
            unityContainer.RegisterType
                <IRepository<Model.Server>, LiteDBRepository<Model.Server>>
                (new ContainerControlledLifetimeManager());

            logger = new LoggerToFile("log.txt");
            unityContainer.RegisterInstance(logger);
            unityContainer.RegisterType
                <ILogger, LoggerToFile>
                (new ContainerControlledLifetimeManager());

            unityContainer.RegisterType
                <ICurrentTimeGetter, SystemTimeGetter>
                (new ContainerControlledLifetimeManager());

            unityContainer.RegisterType<IRepository<Model.Server>, LiteDBRepository<Model.Server>>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IRepository<Model.Match>, LiteDBRepository<Model.Match>>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IRepository<Model.PlayerStatistics>, LiteDBRepository<Model.PlayerStatistics>>(new ContainerControlledLifetimeManager());

            router = new Router(unityContainer);
            RoutingRules.BindRules(router);
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
            logger.Dispose();
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
                    logger.Log(MessageType.Error,
                        "Internal server error generated. Exception: \"" +
                        error.Message +
                        "\". Stack trace: \"" +
                        error.StackTrace +
                        "\"");
                }
            }
        }

        private async Task HandleContextAsync(HttpListenerContext listenerContext)
        {
            string resultText = "";

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = NamesContractResolver.Instance,
                DateFormatString = "yyyy-MM-dd\\THH:mm:ss\\Z"
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
            catch (Exception error)
            {
                // Some internal error, catch it to log and not break the server

                logger.Log(MessageType.Error,
                    "Internal server error generated. Exception: \"" +
                    error.Message +
                    "\". Stack trace: \"" +
                    error.StackTrace +
                    "\"");

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
