using System.Collections.Generic;
using System.Data.SQLite;
using Kontur.GameStats.Server.Model;

namespace Kontur.GameStats.Server
{
    public class GetServerInfo : RequestHandler<ServerParameters>
    {
        public override object Process(ServerParameters parameters, object data, IStatsRepository sqLiteConnection)
        {
            /*string query = "SELECT * FROM `servers` WHERE `endpoint` = '" + parameters.Endpoint + "'";
            SQLiteCommand command = new SQLiteCommand(query, sqLiteConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.Read())
            {
                throw new PageNotFoundException("Server with given endpoint does not exists");
            }

            ServerInfo info = new ServerInfo
            {
                Name = (string)reader["name"],
                GameModes = ((string)reader["game_modes"]).Split('|')
            };


            return info;*/
            return sqLiteConnection.GetServers();
        }
    }
}
