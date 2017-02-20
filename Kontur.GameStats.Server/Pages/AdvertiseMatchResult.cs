using System;
using System.Linq;
using Fclp.Internals.Extensions;
using Kontur.GameStats.Server.Model;
using LiteDB;
using Newtonsoft.Json.Linq;

namespace Kontur.GameStats.Server
{
    public class AdvertiseMatchResult : RequestHandler<MatchParameters>
    {
        public override object Process(MatchParameters parameters, dynamic data, LiteDatabase database)
        {
            var matchesTable = database.GetCollection<Model.Match>("matches");
            var serversTable = database.GetCollection<Model.Server>("servers");

            if (!serversTable.Exists(x => x.Endpoint == parameters.Endpoint))
            {
                // Don't allow submitting mathes with invalid server's endpoint
                throw new BadRequestException("Server with given endpoint does not exist");
            }

            Match match = new Match()
            {
                Endpoint = parameters.Endpoint,
                Results = data.ToObject<MatchResults>(),
                Timestamp = parameters.Timestamp
            };

            matchesTable.Insert(match);

            return new object();
        }
    }
}
