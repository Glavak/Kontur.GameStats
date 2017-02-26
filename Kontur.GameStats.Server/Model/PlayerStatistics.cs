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

        public PlayerStatistics()
        {
            ServerMatchesCount = new Dictionary<string, int>();
            GameModesPlayedCount = new Dictionary<string, int>();
        }

        public void MatchPlayed(string server, string gameMode, float scoreboardPercent, DateTime currentTime)
        {
            TotalMatchesPlayed++;

            IncrementDictionaryValue(ServerMatchesCount, server);
            IncrementDictionaryValue(GameModesPlayedCount, gameMode);

            if (LastMatchPlayed.Day != currentTime.Day)
            {
                // It's a new day, reset today matches
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

            LastMatchPlayed = currentTime;

            AverageScoreboardPercent =
                (AverageScoreboardPercent * (TotalMatchesPlayed - 1)
                + scoreboardPercent) / TotalMatchesPlayed;
        }

        private static void IncrementDictionaryValue(IDictionary<string, int> dictionary, string key)
        {
            int oldValue;
            if (dictionary.TryGetValue(key, out oldValue))
            {
                dictionary[key] = oldValue + 1;
            }
            else
            {
                dictionary.Add(key, 1);
            }
        }
    }
}
