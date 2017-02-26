using Kontur.GameStats.Server.Model;

namespace Kontur.GameStats.Server
{
    public class AdvertiseServerInfo : RequestHandler<ServerParameters>
    {
        private readonly IRepository<Model.Server> serversTable;
        private readonly ICurrentTimeGetter timeGetter;

        public AdvertiseServerInfo(IRepository<Model.Server> serversTable, ICurrentTimeGetter timeGetter)
        {
            this.serversTable = serversTable;
            this.timeGetter = timeGetter;
        }

        public override object Process(ServerParameters parameters, dynamic data)
        {
            var server = serversTable.GetOne(x => x.Endpoint == parameters.Endpoint);

            if (server != null)
            {
                // Update already existing server
                server.Info = data.ToObject<ServerInfo>();
                server.AdvertisingTime = timeGetter.GetCurrentTime(); // TODO: should we count it lifetime from last or first advertising?

                serversTable.Update(server);
            }
            else
            {
                // First time we met this endpoint, insert new server
                server = new Model.Server()
                {
                    Endpoint = parameters.Endpoint,
                    Info = data.ToObject<ServerInfo>(),
                    AdvertisingTime = timeGetter.GetCurrentTime()
                };

                serversTable.Insert(server);
            }

            return new object();
        }
    }
}
