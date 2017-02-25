using System;
using System.Linq;
using LiteDB;

namespace Kontur.GameStats.Server
{
    public class ReportBestPlayers : RequestHandler<CountParameters>
    {
        private IRepository<Model.PlayerStatistics> statisticsTable;

        public ReportBestPlayers(IRepository<Model.PlayerStatistics> statisticsTable)
        {
            this.statisticsTable = statisticsTable;
        }

        // TODO: extract some RequestHandlerCached abstract class

        private Model.BestPlayersPlayer[] bestPlayersCache;
        private DateTime lasTimeRecached;

        public override object Process(CountParameters parameters, object data)
        {
            if (bestPlayersCache == null || (DateTime.Now - lasTimeRecached).TotalMinutes > 1)
            {
                // Need to recache

                bestPlayersCache = statisticsTable
                    .GetSorted("KillDeathRatio", Query.Descending, CountParameters.MaximumCountValue)
                    .Where(x => x.KillToDeathRatio > 0) // If kd == 0, player has not yet played 10 matches or have no deaths
                    .Select(x => new Model.BestPlayersPlayer(x))
                    .ToArray();

                lasTimeRecached = DateTime.Now;
            }

            return bestPlayersCache.Take(parameters.Count);
        }
    }
}
