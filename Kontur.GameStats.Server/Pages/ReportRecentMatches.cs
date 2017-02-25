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

        private IRepository<Model.Match> matchesTable;

        public ReportRecentMatches(IRepository<Model.Match> matchesTable)
        {
            this.matchesTable = matchesTable;
        }

        public override object Process(CountParameters parameters, object data)
        {
            if (recentMatchesCache == null || (DateTime.Now - lasTimeRecached).TotalMinutes > 1)
            {
                // Need to recache

                recentMatchesCache = matchesTable
                    .GetSorted("Timestamp", Query.Descending, CountParameters.MaximumCountValue)
                    .ToArray();

                lasTimeRecached = DateTime.Now;
            }

            return recentMatchesCache.Take(parameters.Count);
        }
    }
}
