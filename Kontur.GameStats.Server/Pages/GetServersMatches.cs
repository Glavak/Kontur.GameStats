using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server
{
    public class GetServersMatches : RequestHandler<MatchParameters>
    {
        public override object Process(MatchParameters parameters, object data, IStatsRepository sqLiteConnection)
        {
            return parameters.Timestamp;
        }
    }
}
