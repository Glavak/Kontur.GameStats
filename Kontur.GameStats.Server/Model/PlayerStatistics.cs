using System;
using System.Collections.Generic;
using LiteDB;

namespace Kontur.GameStats.Server.Model
{
    public class PlayerStatistics
    {
        // Id is required for db to work properly
        public int Id { get; set; }

        public string Name { get; set; }

        public int TotalMatchesPlayed { get; set; }

        public int TotalMatchesWon { get; set; }

        public int Kills { get; set; }

        public int Deaths { get; set; }

        // Should be equal to 0 if this player is hidden from players top
        [BsonIndex]
        public float KillToDeathRatio { get; set; }

        public Dictionary<string, int> ServerMatchesCount { get; set; }

        public Dictionary<string, int> GameModesPlayedCount { get; set; }

        public float AverageScoreboardPercent { get; set; }

        public DateTime FirstMatchPlayed { get; set; }

        public DateTime LastMatchPlayed { get; set; }

        public int TodayMathcesPlayed { get; set; }

        public int MaximumMathcesPerDay { get; set; }

        public float AverageMatchesPerDay { get; set; }

        public int DaysActive { get; set; }

        public PlayerStatistics()
        {
            ServerMatchesCount = new Dictionary<string, int>();
            GameModesPlayedCount = new Dictionary<string, int>();
        }

        public void MatchPlayed(string server, string gameMode, float scoreboardPercent, DateTime time)
        {
            ServerMatchesCount.IncrementValue(server);
            GameModesPlayedCount.IncrementValue(gameMode);

            AverageScoreboardPercent = MyMath.UpdateAverage(AverageScoreboardPercent, TodayMathcesPlayed, scoreboardPercent);
            TotalMatchesPlayed++;

            if (LastMatchPlayed.Day != time.Day)
            {
                // It's a new day, update average and reset today matches
                AverageMatchesPerDay = MyMath.UpdateAverage(AverageMatchesPerDay, DaysActive, TodayMathcesPlayed);
                DaysActive++;

                TodayMathcesPlayed = 1;
            }
            else
            {
                TodayMathcesPlayed++;
            }

            if (TodayMathcesPlayed > MaximumMathcesPerDay)
            {
                MaximumMathcesPerDay = TodayMathcesPlayed;
            }

            LastMatchPlayed = time;

        }
    }
}
