using System.Collections.Generic;
using System.Data.SQLite;
using Kontur.GameStats.Server.Model;

namespace Kontur.GameStats.Server
{
    public class GetServersInfo : RequestHandler<CountParameters>
    {
        public override object Process(CountParameters parameters, object data, IStatsRepository sqLiteConnection)
        {
            /*List<Model.Server> result = new List<Model.Server>();

            const string query = "SELECT * FROM `servers`";
            SQLiteCommand command = new SQLiteCommand(query, sqLiteConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ServerInfo info = new ServerInfo
                {
                    Name = (string) reader["name"],
                    GameModes = ((string) reader["game_modes"]).Split('|')
                };

                Model.Server server = new Model.Server
                {
                    Endpoint = (string) reader["endpoint"],
                    Info = info
                };

                result.Add(server);
            }

            return result;*/
            return new object();
        }
    }
}
