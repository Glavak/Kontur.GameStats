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
        public override object Process(ServerParameters parameters, dynamic data, LiteDatabase database)
        {
            var table = database.GetCollection<Model.Server>("servers");

            var server = table.FindOne(x => x.Endpoint == parameters.Endpoint);

            if (server != null)
            {
                // Update already existing server
                server.Info = data.ToObject<ServerInfo>();
                server.AdvertisingTime = DateTime.Now; // TODO: should we count it lifetime from last or first advertising?

                table.Update(server);
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

                table.Insert(server);
            }

            return new object();
        }
    }
}
