using Kontur.GameStats.Server.Model;

namespace Kontur.GameStats.Server
{
    public class GetServerStats : RequestHandler<ServerParameters>
    {
        private readonly IRepository<Model.Server> serversTable;

        public GetServerStats(IRepository<Model.Server> serversTable)
        {
            this.serversTable = serversTable;
        }

        public override object Process(ServerParameters parameters, object data)
        {
            var server = serversTable.GetOne(x => x.Endpoint == parameters.Endpoint);

            if (server == null)
            {
                throw new PageNotFoundException("Server with given endpoint does not exist");
            }

            return new ServerStatsServer(server);
        }
    }
}
