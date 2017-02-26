using System;
using Kontur.GameStats.Server.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class ModelPlayerStatisticsTests
    {
        [TestMethod]
        public void PlayerStatisticsTest()
        {
            var playerStatistics = new PlayerStatistics();
            var someDate = new DateTime(1233152);

            playerStatistics.MatchPlayed("serv-1", "DM", 10f, someDate);
            playerStatistics.MatchPlayed("serv-2", "TDM", 30f, someDate);
            playerStatistics.MatchPlayed("serv-1", "TDM", 50f, someDate + new TimeSpan(2, 0, 0, 0));

            Assert.AreEqual(3, playerStatistics.TotalMatchesPlayed);
            Assert.AreEqual(1, playerStatistics.TodayMathcesPlayed);
            Assert.AreEqual(2, playerStatistics.MaximumMathcesPerDay);
            Assert.AreEqual(30, playerStatistics.AverageScoreboardPercent);

            var playerStatsPlayer = new PlayerStatsPlayer(playerStatistics);

            Assert.AreEqual("serv-1", playerStatsPlayer.FavoriteServer);
            Assert.AreEqual("TDM", playerStatsPlayer.FavoriteGameMode);
            Assert.AreEqual(1.5, playerStatsPlayer.AverageMatchesPerDay);
        }
    }
}