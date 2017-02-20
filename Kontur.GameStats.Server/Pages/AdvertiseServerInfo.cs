using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Kontur.GameStats.Server.Model;

namespace Kontur.GameStats.Server
{
    public class AdvertiseServerInfo : RequestHandler<ServerParameters>
    {
        public override object Process(ServerParameters parameters, object data, IStatsRepository sqLiteConnection)
        {
            /*string query = "INSERT OR REPLACE INTO `servers` VALUES ('" +
                parameters.Endpoint + "', '" +
                data.name + "', '" +
                string.Join("|", data.gameModes) + "')";

            SQLiteCommand command = new SQLiteCommand(query, sqLiteConnection);
            int succeded = command.ExecuteNonQuery();

            if (succeded != 1)
            {
                throw new Exception("Error querying database");
            }*/
            
            return new object();
        }
    }
}
