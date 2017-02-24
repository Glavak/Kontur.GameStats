using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public int AverageMathcesPerDay { get; set; }

        public DateTime LastMatchPlayed { get; set; }

        public float KillToDeathRatio { get; set; }

        public PlayerStatsPlayer(PlayerStatistics prototype)
        {
            this.TotalMatchesPlayed = prototype.TotalMatchesPlayed;
            this.TotalMatchesWon = prototype.TotalMatchesWon;
            this.FavoriteServer = GetMaxKey(prototype.ServerMatchesCount);
            this.UniqueServers = prototype.ServerMatchesCount.Count;
            this.FavoriteGameMode = GetMaxKey(prototype.GameModesPlayedCount);
            this.AverageScoreboardPercent = prototype.AverageScoreboardPercent;
            this.MaximumMathcesPerDay = prototype.MaximumMathcesPerDay;
            int daysPlayed = (prototype.LastMatchPlayed - prototype.FirstMatchPlayed).Days + 1;
            this.AverageMathcesPerDay = prototype.TotalMatchesPlayed / daysPlayed;
            this.LastMatchPlayed = prototype.LastMatchPlayed;

            // Divide k/d, not use field from prototype, as it may contain 0
            // to exclude player from best players top
            this.KillToDeathRatio = (float)prototype.Kills / prototype.Deaths;
        }

        private string GetMaxKey(Dictionary<string, int> dictionary)
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
