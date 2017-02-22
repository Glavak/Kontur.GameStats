using Kontur.GameStats.Server.Model;
using LiteDB;

namespace Kontur.GameStats.Server
{
    public class AdvertiseMatchResult : RequestHandler<MatchParameters>
    {
        public override object Process(MatchParameters parameters, dynamic data, LiteDatabase database)
        {
            var matchesTable = database.GetCollection<Model.Match>("matches");
            var serversTable = database.GetCollection<Model.Server>("servers");

            if (!serversTable.Exists(x => x.Endpoint == parameters.Endpoint))
            {
                // Don't allow submitting mathes with invalid server's endpoint
                throw new BadRequestException("Server with given endpoint does not exist");
            }

            Match match = new Match()
            {
                Endpoint = parameters.Endpoint,
                Results = data.ToObject<MatchResults>(),
                Timestamp = parameters.Timestamp
            };

            foreach (PlayerScore playerScore in match.Results.Scoreboard)
            {
                UpdatePlayerStatistics(playerScore, database);
            }

            // Create not unique index on timestamp field
            matchesTable.EnsureIndex("Timestamp", false);

            matchesTable.Insert(match);

            return new object();
        }

        private static void UpdatePlayerStatistics(PlayerScore playerScore, LiteDatabase database)
        {
            var table = database.GetCollection<Model.PlayerStatistics>("playerStatistics");

            PlayerStatistics statistics = table.FindOne(x => x.Name == playerScore.Name);

            if (statistics == null)
            {
                statistics = new PlayerStatistics() { Name = playerScore.Name };
            }

            statistics.Matches++;
            statistics.Kills += playerScore.Kills;
            statistics.Deaths += playerScore.Deaths;

            if (statistics.Matches >= 10 && statistics.Kills > 0)
            {
                statistics.KillDeathRatio = (double)statistics.Kills / statistics.Deaths;
            }

            bool updated = table.Update(statistics);
            if (!updated)
            {
                // First match by this player
                table.Insert(statistics);
            }
        }
    }
}