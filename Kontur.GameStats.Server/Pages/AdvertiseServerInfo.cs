using System;
using System.Linq;
using Fclp.Internals.Extensions;
using Kontur.GameStats.Server.Model;
using LiteDB;
using Newtonsoft.Json.Linq;

namespace Kontur.GameStats.Server
{
    public class AdvertiseServerInfo : RequestHandler<ServerParameters>
    {
        private IRepository<Model.Server> serversTable;

        public AdvertiseServerInfo(IRepository<Model.Server> serversTable)
        {
            this.serversTable = serversTable;
        }

        public override object Process(ServerParameters parameters, dynamic data)
        {
            var server = serversTable.GetOne(x => x.Endpoint == parameters.Endpoint);

            if (server != null)
            {
                // Update already existing server
                server.Info = data.ToObject<ServerInfo>();
                server.AdvertisingTime = DateTime.Now; // TODO: should we count it lifetime from last or first advertising?

                serversTable.Update(server);
            }
            else
            {
                // First time we met this endpoint, insert new server
                server = new Model.Server()
                {
                    Endpoint = parameters.Endpoint,
                    Info = data.ToObject<ServerInfo>(),
                    AdvertisingTime = DateTime.Now
                };

                serversTable.Insert(server);
            }

            return new object();
        }
    }
}
