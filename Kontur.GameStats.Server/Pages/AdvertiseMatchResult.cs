using System;
using Kontur.GameStats.Server.Model;

namespace Kontur.GameStats.Server
{
    public class AdvertiseMatchResult : RequestHandler<MatchParameters>
    {
        private readonly IRepository<Model.Server> serversTable;
        private readonly IRepository<Match> matchesTable;
        private readonly IRepository<PlayerStatistics> statisticsTable;

        private readonly ICurrentTimeGetter timeGetter;

        public AdvertiseMatchResult(IRepository<Model.Server> serversTable, IRepository<Match> matchesTable, IRepository<PlayerStatistics> statisticsTable, ICurrentTimeGetter timeGetter)
        {
            this.serversTable = serversTable;
            this.matchesTable = matchesTable;
            this.statisticsTable = statisticsTable;
            this.timeGetter = timeGetter;
        }

        public override object Process(MatchParameters parameters, dynamic data)
        {
            var server = serversTable.GetOne(x => x.Endpoint == parameters.Endpoint);

            if (server == null)
            {
                // Don't allow submitting mathes with invalid server's endpoint
                throw new BadRequestException("Server with given endpoint does not exist");
            }

            Match match = new Match()
            {
                Server = parameters.Endpoint,
                Results = data.ToObject<MatchResults>(),
                Timestamp = parameters.Timestamp
            };

            server.MatchPlayed(
                match.Results.GameMode,
                match.Results.Map,
                match.Results.Scoreboard.Count,
                parameters.Timestamp);
            serversTable.Update(server);

            bool firstPlayer = true;
            float scoreboardPercent = 100f;
            foreach (PlayerScore playerScore in match.Results.Scoreboard)
            {
                UpdatePlayerStatistics(match, playerScore, firstPlayer, scoreboardPercent, parameters.Timestamp);
                scoreboardPercent -= 100f / (match.Results.Scoreboard.Count - 1);
                firstPlayer = false;
            }

            matchesTable.Insert(match);

            return new object();
        }

        private void UpdatePlayerStatistics(Match match, PlayerScore playerScore, bool isWinner, float scoreboardPercent, DateTime matchTime)
        {
            PlayerStatistics statistics = statisticsTable.GetOne(x => x.Name == playerScore.Name);

            if (statistics != null)
            {
                // Update already existing player stats
                if (isWinner) statistics.TotalMatchesWon++;
                statistics.Kills += playerScore.Kills;
                statistics.Deaths += playerScore.Deaths;

                if (statistics.TotalMatchesPlayed >= 10 && statistics.Deaths > 0)
                {
                    // If player has less than 10 matches or no deaths, set his
                    // kd to 0, to move him down in ordering by kd
                    statistics.KillToDeathRatio = (float)statistics.Kills / statistics.Deaths;
                }

                statistics.MatchPlayed(match.Server, match.Results.GameMode, scoreboardPercent, matchTime);

                statisticsTable.Update(statistics);
            }
            else
            {
                // It's' first match of this player
                // Create new entry in statistics collection
                statistics = new PlayerStatistics
                {
                    Name = playerScore.Name,
                    Kills = playerScore.Kills,
                    Deaths = playerScore.Deaths,
                    TotalMatchesWon = isWinner ? 1 : 0,
                    FirstMatchPlayed = timeGetter.GetCurrentTime()
                };


                statistics.MatchPlayed(match.Server, match.Results.GameMode, scoreboardPercent, matchTime);

                statisticsTable.Insert(statistics);
            }
        }
    }
}
