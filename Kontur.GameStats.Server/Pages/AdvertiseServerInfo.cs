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

            var server = new Model.Server()
            {
                Endpoint = parameters.Endpoint,
                Info = data.ToObject<ServerInfo>()
            };

            bool updated = table.Update(server);

            if (!updated)
            {
                // First time we met this endpoint
                table.Insert(server);
            }

            return new object();
        }
    }
}
