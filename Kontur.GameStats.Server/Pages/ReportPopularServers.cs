using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server
{
    public class ReportPopularServers : RequestHandler<CountParameters>
    {
        private Model.PopularServersServer[] popularServersCache;
        private DateTime lasTimeRecached;

        public override object Process(CountParameters parameters, object data, LiteDB.LiteDatabase database)
        {
            if (popularServersCache == null || (DateTime.Now - lasTimeRecached).TotalMinutes > 1)
            {
                // Need to recache

                var table = database.GetCollection<Model.Server>("servers");

                var servers = table
                    .Find(Query.All("AverageMatchesPerDay", Query.Descending), 0, CountParameters.MaximumCountValue)
                    .ToArray();

                popularServersCache = servers
                    .Select(x => new Model.PopularServersServer(x))
                    .ToArray();

                lasTimeRecached = DateTime.Now;
            }

            return popularServersCache.Take(parameters.Count);
        }
    }
}
