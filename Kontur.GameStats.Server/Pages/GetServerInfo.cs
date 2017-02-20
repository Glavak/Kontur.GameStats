using LiteDB;

namespace Kontur.GameStats.Server
{
    public class GetServerInfo : RequestHandler<ServerParameters>
    {
        public override object Process(ServerParameters parameters, object data, LiteDatabase database)
        {
            var table = database.GetCollection<Model.Server>("servers");

            Model.ServerInfo result = table
                .FindOne(x => x.Endpoint == parameters.Endpoint)
                .Info;

            if (result == null)
            {
                throw new PageNotFoundException("Server with given endpoint does not exist");
            }

            return result;
        }
    }
}
