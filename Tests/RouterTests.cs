using Kontur.GameStats.Server;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class RouterTests
    {
        private readonly Router router;

        public RouterTests()
        {
            var container = new UnityContainer();
            router = new Router(container);

            router.Bind<MockHandler>(@"/some/test/path/?", "GET");
            router.Bind<MockHandler>(@"/" +
                                     RoutingRules.EndpointRegexp + "/" +
                                     RoutingRules.TimestampRegexp + "/?",
                "PUT");
            router.Bind<MockHandler>(@"/" +
                                     RoutingRules.PlayerNameRegexp + "/?",
                "POST");
        }

        [TestMethod]
        public void SimpleRouting()
        {
            MockHandler.CountCalled = 0;
            MockHandler.LastDataCalled = null;

            router.RouteRequest("/some/test/path", 42, "GET");
            Assert.AreEqual(1, MockHandler.CountCalled);
            Assert.AreEqual(42, MockHandler.LastDataCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(PageNotFoundException))]
        public void WrongHttpMethod()
        {
            router.RouteRequest("/some/test/path/", 42, "PUT");
        }

        [TestMethod]
        [ExpectedException(typeof(PageNotFoundException))]
        public void WrongAddress()
        {
            router.RouteRequest("/some_test/path", 42, "GET");
        }

        [TestMethod]
        public void RoutingReturnObject()
        {
            MockHandler.CountCalled = 0;
            MockHandler.LastDataCalled = null;

            object result = router.RouteRequest("/some/test/path/", "Another test data", "GET");

            Assert.AreEqual(1, MockHandler.CountCalled);
            Assert.AreEqual("Another test data", MockHandler.LastDataCalled);
            Assert.AreEqual("Test returned object", result);
        }

        [TestMethod]
        [ExpectedException(typeof(PageNotFoundException))]
        public void WrongRegexpRouting()
        {
            router.RouteRequest("/1.incorrect.ip.address-42/2017-02-25T19:52:23Z", 42, "GET");
        }

        [TestMethod]
        [ExpectedException(typeof(PageNotFoundException))]
        public void WrongRegexpRouting2()
        {
            router.RouteRequest("/hostname-6565/2017-02-25T19:52:23wrongDateFormat", 42, "GET");
        }

        [TestMethod]
        public void RegexpRouting()
        {
            MockHandler.CountCalled = 0;
            MockHandler.LastDataCalled = null;

            router.RouteRequest("/hostname-6565/2017-02-25T19:52:23Z", "Datadatadata", "PUT");

            Assert.AreEqual(1, MockHandler.CountCalled);
            Assert.AreEqual("Datadatadata", MockHandler.LastDataCalled);
        }

        [TestMethod]
        public void PlayerUrlencodedName()
        {
            MockHandler.CountCalled = 0;
            MockHandler.LastDataCalled = null;

            router.RouteRequest("/players/Tricky%20%20Name+0f_A_PLAY3R", "Datadatadata", "POST");

            Assert.AreEqual(1, MockHandler.CountCalled);
            Assert.AreEqual("Datadatadata", MockHandler.LastDataCalled);
        }
    }

    internal class MockHandler : RequestHandler<EmptyParameters>
    {
        public static int CountCalled;
        public static object LastDataCalled;

        public override object Process(EmptyParameters parameters, object data)
        {
            CountCalled++;
            LastDataCalled = data;

            return "Test returned object";
        }
    }
}