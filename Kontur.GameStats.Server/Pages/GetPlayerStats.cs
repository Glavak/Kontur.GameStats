using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server
{
    public class GetPlayerStats : RequestHandler<PlayerNameParameters>
    {
        private IRepository<Model.PlayerStatistics> statisticsTable;

        public GetPlayerStats(IRepository<Model.PlayerStatistics> statisticsTable)
        {
            this.statisticsTable = statisticsTable;
        }

        public override object Process(PlayerNameParameters parameters, object data)
        {
            var playerStatistics = statisticsTable.GetOne(x => x.Name == parameters.Name);

            var player = new Model.PlayerStatsPlayer(playerStatistics);

            return player;
        }
    }
}
