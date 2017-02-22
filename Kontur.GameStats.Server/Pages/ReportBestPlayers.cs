using System;
using System.Linq;
using Kontur.GameStats.Server.Model;
using LiteDB;

namespace Kontur.GameStats.Server
{
    public class ReportBestPlayers : RequestHandler<CountParameters>
    {
        private PlayerStatistics[] bestPlayersCache;
        private DateTime lasTimeRecached;

        public override object Process(CountParameters parameters, object data, LiteDatabase database)
        {
            if (bestPlayersCache == null || (DateTime.Now - lasTimeRecached).TotalMinutes > 1)
            {
                // Need to recache

                var table = database.GetCollection<Model.PlayerStatistics>("playerStatistics");

                bestPlayersCache = table
                    .Find(Query.All("KillDeathRatio", Query.Descending), 0, CountParameters.MaximumCountValue)
                    .Where(x => x.KillDeathRatio > 0)
                    .ToArray();

                lasTimeRecached = DateTime.Now;
            }

            return bestPlayersCache.Take(parameters.Count);
        }
    }
}
