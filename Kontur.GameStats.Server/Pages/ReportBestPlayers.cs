using System;
using System.Linq;
using LiteDB;

namespace Kontur.GameStats.Server
{
    public class ReportBestPlayers : RequestHandler<CountParameters>
    {
        private Model.BestPlayersPlayer[] bestPlayersCache;
        private DateTime lasTimeRecached;

        public override object Process(CountParameters parameters, object data, LiteDatabase database)
        {
            if (bestPlayersCache == null || (DateTime.Now - lasTimeRecached).TotalMinutes > 1)
            {
                // Need to recache

                var table = database.GetCollection<Model.PlayerStatistics>("playerStatistics");

                bestPlayersCache = table
                    .Find(Query.All("KillDeathRatio", Query.Descending), 0, CountParameters.MaximumCountValue)
                    .Where(x => x.KillToDeathRatio > 0) // If kd == 0, player has not yet played 10 matches or have no deaths
                    .Select(x => new Model.BestPlayersPlayer(x))
                    .ToArray();

                lasTimeRecached = DateTime.Now;
            }

            return bestPlayersCache.Take(parameters.Count);
        }
    }
}
