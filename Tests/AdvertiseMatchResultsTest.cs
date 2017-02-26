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

            statisticsTable.Elements.Add(new PlayerStatistics {Name = "Existing"});
        }

        [TestMethod]
        [ExpectedException(typeof(BadRequestException))]
        public void WrongEndpoint()
        {
            var timeGetter = new MockTimeGetter(new DateTime(51634312));
            var handler = new AdvertiseMatchResult(serversTable, matchesTable, statisticsTable, timeGetter);

            handler.Process(new MatchParameters {Endpoint = "notExistingEndpoint-1", Timestamp = timeGetter.Time},
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
                        new PlayerScore {Name = "Not Existing"}
                    },
                    TimeElapsed = 12.2f,
                    TimeLimit = 10
                }
            };

            handler.Process(new MatchParameters {Endpoint = "hostname-6556", Timestamp = timeGetter.Time}, data);

            // Assert inserted match:
            Assert.AreEqual(1, matchesTable.InsertCalledTimes);
            Assert.AreEqual("hostname-6556", matchesTable.LastInsertElement.Server);
            Assert.AreEqual(timeGetter.Time, matchesTable.LastInsertElement.Timestamp);
            Assert.AreEqual(data.ReturnValue, matchesTable.LastInsertElement.Results);

            // Assert updated server:
            Assert.AreEqual(1, serversTable.UpdateCalledTimes);
            Assert.AreEqual("hostname-6556", serversTable.LastUpdateElement.Endpoint);
            Assert.AreEqual(1, serversTable.LastUpdateElement.TotalMatchesPlayed);
            Assert.AreEqual(2, serversTable.LastUpdateElement.AveragePopulation);

            // Assert updated/inserted statistics:
            Assert.AreEqual(1, statisticsTable.UpdateCalledTimes);
            Assert.AreEqual(1, statisticsTable.InsertCalledTimes);
            Assert.AreEqual("Not Existing", statisticsTable.LastInsertElement.Name);
            Assert.AreEqual("Existing", statisticsTable.LastUpdateElement.Name);
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