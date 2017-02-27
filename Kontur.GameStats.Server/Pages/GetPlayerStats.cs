using System.Threading;

namespace Kontur.GameStats.Server
{
    public class GetPlayerStats : RequestHandler<PlayerNameParameters>
    {
        private readonly IRepository<Model.PlayerStatistics> statisticsTable;

        public GetPlayerStats(IRepository<Model.PlayerStatistics> statisticsTable)
        {
            this.statisticsTable = statisticsTable;
        }

        public override object Process(PlayerNameParameters parameters, object data)
        {
            var playerStatistics = statisticsTable.GetOne(x => x.Name == parameters.Name);

            if (playerStatistics == null)
            {
                throw new PageNotFoundException("Player with given name does not exist");
            }

            var player = new Model.PlayerStatsPlayer(playerStatistics);

            return player;
        }
    }
}
