using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server
{
    public class GetPlayerStats : RequestHandler<PlayerNameParameters>
    {
        public override object Process(PlayerNameParameters parameters, object data, LiteDB.LiteDatabase database)
        {
            var table = database.GetCollection<Model.PlayerStatistics>("playerStatistics");

            var playerStatistics = table.FindOne(x=>x.Name == parameters.Name);

            var player = new Model.PlayerStatsPlayer(playerStatistics);

            return player;
        }
    }
}
