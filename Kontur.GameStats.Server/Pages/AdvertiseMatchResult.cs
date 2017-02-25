using Kontur.GameStats.Server.Model;
using LiteDB;
using System;

namespace Kontur.GameStats.Server
{
    public class AdvertiseMatchResult : RequestHandler<MatchParameters>
    {
        private IRepository<Model.Server> serversTable;
        private IRepository<Model.Match> matchesTable;
        private IRepository<Model.PlayerStatistics> statisticsTable;

        public AdvertiseMatchResult(IRepository<Model.Server> serversTable, IRepository<Model.Match> matchesTable, IRepository<Model.PlayerStatistics> statisticsTable)
        {
            this.serversTable = serversTable;
            this.matchesTable = matchesTable;
            this.statisticsTable = statisticsTable;
        }

        public override object Process(MatchParameters parameters, dynamic data)
        {
            var server = serversTable.GetOne(x => x.Endpoint == parameters.Endpoint);

            if (server == null)
            {
                // Don't allow submitting mathes with invalid server's endpoint
                throw new BadRequestException("Server with given endpoint does not exist");
            }

            server.MatchesCount++;
            int daysSinceServerAdvertisment = (DateTime.Now - server.AdvertisingTime).Days + 1;
            server.AverageMatchesPerDay = (float)server.MatchesCount / daysSinceServerAdvertisment;
            serversTable.Update(server);

            Match match = new Match()
            {
                Server = parameters.Endpoint,
                Results = data.ToObject<MatchResults>(),
                Timestamp = parameters.Timestamp
            };

            bool firstPlayer = true;
            float scoreboardPercent = 100;
            foreach (PlayerScore playerScore in match.Results.Scoreboard)
            {
                UpdatePlayerStatistics(match, playerScore, firstPlayer, scoreboardPercent);
                scoreboardPercent -= 100 / (match.Results.Scoreboard.Count - 1);
                firstPlayer = false;
            }

            matchesTable.Insert(match);

            return new object();
        }

        private void UpdatePlayerStatistics(Match match,
            PlayerScore playerScore,
            bool isWinner,
            float scoreboardPercent)
        {
            PlayerStatistics statistics = statisticsTable.GetOne(x => x.Name == playerScore.Name);

            if (statistics != null)
            {
                // Update already existing player stats
                if (isWinner) statistics.TotalMatchesWon++;
                statistics.Kills += playerScore.Kills;
                statistics.Deaths += playerScore.Deaths;

                // TODO: check is all fields from PlayerStatistics get updated

                if (statistics.TotalMatchesPlayed >= 10 && statistics.Deaths > 0)
                {
                    // If player has less than 10 matches or no deaths, set his
                    // kd to 0, to move him down in ordering by kd
                    statistics.KillToDeathRatio = (float)statistics.Kills / statistics.Deaths;
                }

                statistics.MatchPlayed(match.Server, match.Results.GameMode, scoreboardPercent, DateTime.Now);

                statisticsTable.Update(statistics);
            }
            else
            {
                // It's' first match of this player
                // Create new entry in statistics collection
                statistics = new PlayerStatistics()
                {
                    Name = playerScore.Name,
                    Kills = playerScore.Kills,
                    Deaths = playerScore.Deaths,
                    TotalMatchesWon = isWinner ? 1 : 0
                };

                statistics.FirstMatchPlayed = DateTime.Now;

                statistics.MatchPlayed(match.Server, match.Results.GameMode, scoreboardPercent, DateTime.Now);

                statisticsTable.Insert(statistics);
            }
        }
    }
}
