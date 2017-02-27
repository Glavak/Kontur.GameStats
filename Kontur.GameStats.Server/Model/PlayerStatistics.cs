using System;
using System.Collections.Generic;
using LiteDB;

namespace Kontur.GameStats.Server.Model
{
    public class PlayerStatistics
    {
        // Id is required for db to work properly
        public int Id { get; set; }

        /// <summary>
        /// Lowercased player name, for case-insensitive search
        /// </summary>
        [BsonIndex]
        public string LowercaseName { get; private set; }

        /// <summary>
        /// Originally advertised players name
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
            set
            {
                displayName = value;
                LowercaseName = value.ToLower();
            }
        }

        private string displayName;

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

        /// <summary>
        /// Average matches per day, todays matches not included in calculation
        /// </summary>
        public float AverageMatchesPerDay { get; set; }

        /// <summary>
        /// </summary>
        /// <returns>AverageMatchesPerDay, including today's data</returns>
        public float GetActualAverageMatchesPerDay()
        {
            return MyMath.UpdateAverage(AverageMatchesPerDay, DaysActive - 1, TodayMathcesPlayed);
        }

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

            AverageScoreboardPercent = MyMath.UpdateAverage(AverageScoreboardPercent, TotalMatchesPlayed, scoreboardPercent);
            TotalMatchesPlayed++;

            if (LastMatchPlayed.Day < time.Day)
            {
                // It's a new day, update average and reset today matches
                AverageMatchesPerDay = DaysActive == 0 ? 0 : MyMath.UpdateAverage(AverageMatchesPerDay, DaysActive-1, TodayMathcesPlayed);
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
