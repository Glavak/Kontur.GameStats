using System;
using System.Collections.Generic;

namespace Kontur.GameStats.Server.Model
{
    public class PlayerStatsPlayer
    {
        public int TotalMatchesPlayed { get; set; }

        public int TotalMatchesWon { get; set; }

        public string FavoriteServer { get; set; }

        public int UniqueServers { get; set; }

        public string FavoriteGameMode { get; set; }

        public float AverageScoreboardPercent { get; set; }

        public int MaximumMathcesPerDay { get; set; }

        public float AverageMatchesPerDay { get; set; }

        public DateTime LastMatchPlayed { get; set; }

        public float KillToDeathRatio { get; set; }

        public PlayerStatsPlayer(PlayerStatistics prototype)
        {
            TotalMatchesPlayed = prototype.TotalMatchesPlayed;
            TotalMatchesWon = prototype.TotalMatchesWon;
            FavoriteServer = GetMaxKey(prototype.ServerMatchesCount);
            UniqueServers = prototype.ServerMatchesCount.Count;
            FavoriteGameMode = GetMaxKey(prototype.GameModesPlayedCount);
            AverageScoreboardPercent = prototype.AverageScoreboardPercent;
            MaximumMathcesPerDay = prototype.MaximumMathcesPerDay;
            LastMatchPlayed = prototype.LastMatchPlayed;

            // Also count today's matches, that not counted in prototype.AverageMatchesPerDay
            AverageMatchesPerDay = MyMath.UpdateAverage(
                prototype.AverageMatchesPerDay, prototype.DaysActive, prototype.TodayMathcesPlayed);

            // Divide k/d, not use field from prototype, as it may contain 0
            // to exclude player from best players top
            KillToDeathRatio = (float)prototype.Kills / prototype.Deaths;
        }

        private static string GetMaxKey(Dictionary<string, int> dictionary)
        {
            string maximumKey = null;
            int maximumValue = int.MinValue;
            foreach (var item in dictionary)
            {
                if (item.Value > maximumValue)
                {
                    maximumKey = item.Key;
                    maximumValue = item.Value;
                }
            }
            return maximumKey;
        }
    }
}
