using System;
using Kontur.GameStats.Server.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class ModelServerTests
    {
        [TestMethod]
        public void ServerTest()
        {
            var server = new Server();
            var someDate = new DateTime(1233152);

            server.MatchPlayed("DM", "dedust", 4, someDate);
            someDate += new TimeSpan(2, 0, 0, 0);
            server.MatchPlayed("TDM", "map", 42, someDate);
            server.MatchPlayed("DM", "other-map", 2, someDate);
            server.MatchPlayed("CTF", "dedust", 10, someDate);

            Assert.AreEqual(14.5, server.AveragePopulation);
            Assert.AreEqual(42, server.MaximumPopulation);
            Assert.AreEqual(4, server.TotalMatchesPlayed);
            Assert.AreEqual(3, server.MaximumMathcesPerDay);
            Assert.AreEqual(3, server.TodayMathcesPlayed);
            // Should be 1, as server have been active for only 1 full day (with 1 match)
            Assert.AreEqual(1, server.DaysActive);

            var serverStatsServer = new ServerStatsServer(server);

            Assert.AreEqual(3, serverStatsServer.Top5GameModes.Length);
            Assert.AreEqual("DM", serverStatsServer.Top5GameModes[0]);

            Assert.AreEqual(3, serverStatsServer.Top5Maps.Length);
            Assert.AreEqual("dedust", serverStatsServer.Top5Maps[0]);

            Assert.AreEqual(2, serverStatsServer.AverageMatchesPerDay);
        }
    }
}