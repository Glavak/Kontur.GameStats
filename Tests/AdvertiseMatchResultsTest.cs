using System;
using System.Collections.Generic;
using Kontur.GameStats.Server;
using Kontur.GameStats.Server.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class AdvertiseMatchResultsTest
    {
        private readonly MockRepository<Match> matchesTable;
        private readonly MockRepository<Server> serversTable;
        private readonly MockRepository<PlayerStatistics> statisticsTable;

        public AdvertiseMatchResultsTest()
        {
            serversTable = new MockRepository<Server>();

            serversTable.Elements.Add(new Server
            {
                Endpoint = "1.1.1.1-123",
                Info = new ServerInfo(),
                AverageMatchesPerDay = 1
            });
            serversTable.Elements.Add(new Server
            {
                Endpoint = "1.2.3.4-567",
                Info = new ServerInfo(),
                AverageMatchesPerDay = 50
            });
            serversTable.Elements.Add(new Server
            {
                Endpoint = "hostname-6556",
                Info = new ServerInfo(),
                AverageMatchesPerDay = 10
            });

            matchesTable = new MockRepository<Match>();

            statisticsTable = new MockRepository<PlayerStatistics>();

            statisticsTable.Elements.Add(new PlayerStatistics { DisplayName = "Existing" });
        }

        [TestMethod]
        [ExpectedException(typeof(BadRequestException))]
        public void WrongEndpoint()
        {
            var timeGetter = new MockTimeGetter(new DateTime(51634312));
            var handler = new AdvertiseMatchResult(serversTable, matchesTable, statisticsTable, timeGetter);

            handler.Process(new MatchParameters { Endpoint = "notExistingEndpoint-1", Timestamp = timeGetter.Time },
                new object());
        }

        [TestMethod]
        public void HandlerCall()
        {
            var timeGetter = new MockTimeGetter(new DateTime(51634312));
            var handler = new AdvertiseMatchResult(serversTable, matchesTable, statisticsTable, timeGetter);

            var data = new MockMatchData
            {
                ReturnValue = new MatchResults
                {
                    FragLimit = 20,
                    GameMode = "DM",
                    Map = "dedust",
                    Scoreboard = new List<PlayerScore>
                    {
                        new PlayerScore {Name = "Existing"},
                        new PlayerScore {Name = "Not+Existing"}
                    },
                    TimeElapsed = 12.2f,
                    TimeLimit = 10
                }
            };

            handler.Process(new MatchParameters { Endpoint = "hostname-6556", Timestamp = timeGetter.Time }, data);

            // Assert inserted match:
            Assert.AreEqual(1, matchesTable.InsertCalledTimes);
            Assert.AreEqual("hostname-6556", matchesTable.InsertElements[0].Server);
            Assert.AreEqual(timeGetter.Time, matchesTable.InsertElements[0].Timestamp);
            Assert.AreEqual(data.ReturnValue, matchesTable.InsertElements[0].Results);

            // Assert updated server:
            Assert.AreEqual(1, serversTable.UpdateCalledTimes);
            Assert.AreEqual("hostname-6556", serversTable.UpdateElements[0].Endpoint);
            Assert.AreEqual(1, serversTable.UpdateElements[0].TotalMatchesPlayed);
            Assert.AreEqual(2, serversTable.UpdateElements[0].AveragePopulation);

            // Assert updated/inserted statistics:
            Assert.AreEqual(1, statisticsTable.UpdateCalledTimes);
            Assert.AreEqual(1, statisticsTable.InsertCalledTimes);
            Assert.AreEqual("Not+Existing", statisticsTable.InsertElements[0].DisplayName);
            Assert.AreEqual("Existing", statisticsTable.UpdateElements[0].DisplayName);
        }

        [TestMethod]
        public void ScoreboardPercentCalculation()
        {
            var handler = new AdvertiseMatchResult(serversTable, matchesTable, statisticsTable, new MockTimeGetter(new DateTime(51634312)));

            var data = new MockMatchData
            {
                ReturnValue = new MatchResults
                {
                    GameMode = "DM",
                    Map = "dedust",
                    Scoreboard = new List<PlayerScore>
                    {
                        new PlayerScore {Name = "gold"},
                        new PlayerScore {Name = "silver"},
                        new PlayerScore {Name = "bronze"},
                        new PlayerScore {Name = "nothing"},
                        new PlayerScore {Name = "nothingAtAll"}
                    }
                }
            };


            handler.Process(new MatchParameters { Endpoint = "hostname-6556" }, data);

            Assert.AreEqual(5, statisticsTable.InsertCalledTimes);

            Assert.AreEqual(0, statisticsTable.InsertElements[4].AverageScoreboardPercent);
            Assert.AreEqual(25, statisticsTable.InsertElements[3].AverageScoreboardPercent);
            Assert.AreEqual(50, statisticsTable.InsertElements[2].AverageScoreboardPercent);
            Assert.AreEqual(75, statisticsTable.InsertElements[1].AverageScoreboardPercent);
            Assert.AreEqual(100, statisticsTable.InsertElements[0].AverageScoreboardPercent);
        }

        [TestMethod]
        public void ScoreboardPercentOnePlayer()
        {
            var handler = new AdvertiseMatchResult(serversTable, matchesTable, statisticsTable, new MockTimeGetter(new DateTime(51634312)));

            var data = new MockMatchData
            {
                ReturnValue = new MatchResults
                {
                    GameMode = "DM",
                    Map = "dedust",
                    Scoreboard = new List<PlayerScore>
                    {
                        new PlayerScore {Name = "gold"}
                    }
                }
            };


            handler.Process(new MatchParameters { Endpoint = "hostname-6556" }, data);

            Assert.AreEqual(1, statisticsTable.InsertCalledTimes);

            Assert.AreEqual(100, statisticsTable.InsertElements[0].AverageScoreboardPercent);
        }
    }

    public class MockMatchData
    {
        public MatchResults ReturnValue;

        public MatchResults ToObject<TMatchResult>()
        {
            return ReturnValue;
        }
    }
}