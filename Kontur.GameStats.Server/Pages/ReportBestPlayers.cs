using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace Kontur.GameStats.Server
{
    public class ReportBestPlayers : CachedRequestHandler<Model.BestPlayersPlayer>
    {
        private readonly IRepository<Model.PlayerStatistics> statisticsTable;

        public ReportBestPlayers(IRepository<Model.PlayerStatistics> statisticsTable, ICurrentTimeGetter timeGetter) : base(timeGetter)
        {
            this.statisticsTable = statisticsTable;
        }

        public override IEnumerable<Model.BestPlayersPlayer> Recache()
        {
            return statisticsTable
                .GetSorted("KillDeathRatio", Query.Descending, CountParameters.MaximumCountValue)
                .Where(x => x.KillToDeathRatio > 0) // If kd == 0, player has not yet played 10 matches or have no deaths
                .Select(x => new Model.BestPlayersPlayer(x)); // Convert to type, that contains required for this report page fields
        }
    }
}
