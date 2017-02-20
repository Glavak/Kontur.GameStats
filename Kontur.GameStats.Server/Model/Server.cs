using LiteDB;

namespace Kontur.GameStats.Server.Model
{
    public class Server
    {
        [BsonId]
        public string Endpoint { get; set; }

        public ServerInfo Info { get; set; }
    }
}
