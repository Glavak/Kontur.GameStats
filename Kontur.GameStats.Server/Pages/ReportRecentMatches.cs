using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Model;
using LiteDB;

namespace Kontur.GameStats.Server
{
    public class ReportRecentMatches : RequestHandler<CountParameters>
    {
        private Match[] recentMatchesCache;
        private DateTime lasTimeRecached;

        public override object Process(CountParameters parameters, object data, LiteDatabase database)
        {
            if (recentMatchesCache == null || (DateTime.Now - lasTimeRecached).TotalMinutes > 1)
            {
                // Need to recache

                var table = database.GetCollection<Model.Match>("matches");
                
                recentMatchesCache = table
                    .Find(Query.All("Timestamp", Query.Descending), 0, 50)
                    .ToArray();

                lasTimeRecached = DateTime.Now;
            }

            return recentMatchesCache.Take(parameters.Count);
        }
    }
}
