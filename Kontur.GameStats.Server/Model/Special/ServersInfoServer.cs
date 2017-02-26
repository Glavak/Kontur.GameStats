namespace Kontur.GameStats.Server.Model
{
    public class ServersInfoServer
    {
        public string Endpoint { get; set; }

        public ServerInfo Info { get; set; }

        public ServersInfoServer(Server prototype)
        {
            Endpoint = prototype.Endpoint;
            Info = prototype.Info;
        }
    }
}
