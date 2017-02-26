using Kontur.GameStats.Server;
using Kontur.GameStats.Server.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class PopularServersTests
    {
        private readonly MockRepository<Server> serversTable;

        public PopularServersTests()
        {
            serversTable = new MockRepository<Server>();

            serversTable.Elements.Add(new Server { Endpoint = "1.1.1.1-123", Info = new ServerInfo { }, AverageMatchesPerDay = 1 });
            serversTable.Elements.Add(new Server { Endpoint = "1.2.3.4-567", Info = new ServerInfo { }, AverageMatchesPerDay = 50 });
            serversTable.Elements.Add(new Server { Endpoint = "hostname-6556", Info = new ServerInfo { }, AverageMatchesPerDay = 10 });
        }

        [TestMethod]
        public void SimpleTest()
        {
            var report = new ReportPopularServers(serversTable, new MockTimeGetter(new DateTime(51634312)));

            var result = ((IEnumerable<PopularServersServer>)report.Process(new CountParameters { Count = 10 }, new object())).ToArray();
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual("1.2.3.4-567", result[0].Endpoint);
            Assert.AreEqual("hostname-6556", result[1].Endpoint);
            Assert.AreEqual("1.1.1.1-123", result[2].Endpoint);
        }

        [TestMethod]
        public void SmallCountTest()
        {
            var report = new ReportPopularServers(serversTable, new MockTimeGetter(new DateTime(51634312)));

            var result = ((IEnumerable<PopularServersServer>)report.Process(new CountParameters { Count = 2 }, new object())).ToArray();
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("1.2.3.4-567", result[0].Endpoint);
            Assert.AreEqual("hostname-6556", result[1].Endpoint);
        }

        [TestMethod]
        public void CachingTest()
        {
            var timeGetter = new MockTimeGetter(new DateTime(51634312));
            var report = new ReportPopularServers(serversTable, timeGetter);

            var result = ((IEnumerable<PopularServersServer>)report.Process(new CountParameters { Count = 10 }, new object())).ToArray();

            serversTable.Elements.Add(new Server { Endpoint = "another-42", Info = new ServerInfo { }, AverageMatchesPerDay = 0 });
            var result2 = ((IEnumerable<PopularServersServer>)report.Process(new CountParameters { Count = 10 }, new object())).ToArray();

            serversTable.Elements.Add(new Server { Endpoint = "another-4242", Info = new ServerInfo { }, AverageMatchesPerDay = 0 });
            timeGetter.Time += new TimeSpan(0, 15, 0);
            var result3 = ((IEnumerable<PopularServersServer>)report.Process(new CountParameters { Count = 10 }, new object())).ToArray();

            // Should be both 3, as result is cached
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual(3, result2.Length);
            // This should be 5, as we added 2 elements and 15 minutes "passed"
            Assert.AreEqual(5, result3.Length);
        }
    }
}
