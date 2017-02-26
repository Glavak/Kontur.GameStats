namespace Kontur.GameStats.Server
{
    public class GetServerInfo : RequestHandler<ServerParameters>
    {
        private readonly IRepository<Model.Server> serversTable;

        public GetServerInfo(IRepository<Model.Server> serversTable)
        {
            this.serversTable = serversTable;
        }

        public override object Process(ServerParameters parameters, object data)
        {
            Model.ServerInfo result = serversTable
                .GetOne(x => x.Endpoint == parameters.Endpoint)
                .Info;

            if (result == null)
            {
                throw new PageNotFoundException("Server with given endpoint does not exist");
            }

            return result;
        }
    }
}
