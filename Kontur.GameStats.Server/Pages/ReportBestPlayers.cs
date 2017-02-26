using System;
using System.Linq;
using LiteDB;

namespace Kontur.GameStats.Server
{
    public class ReportBestPlayers : CachedRequestHandler<Model.BestPlayersPlayer>
    {
        private IRepository<Model.PlayerStatistics> statisticsTable;

        public ReportBestPlayers(IRepository<Model.PlayerStatistics> statisticsTable)
        {
            this.statisticsTable = statisticsTable;
        }

        // TODO: extract some RequestHandlerCached abstract class

        public override Model.BestPlayersPlayer[] Recache()
        {
            return statisticsTable
                .GetSorted("KillDeathRatio", Query.Descending, CountParameters.MaximumCountValue)
                .Where(x => x.KillToDeathRatio > 0) // If kd == 0, player has not yet played 10 matches or have no deaths
                .Select(x => new Model.BestPlayersPlayer(x))
                .ToArray();
        }
    }
}
