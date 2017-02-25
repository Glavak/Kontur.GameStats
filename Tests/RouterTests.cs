using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kontur.GameStats.Server;
using Moq;
using LiteDB;
using Microsoft.Practices.Unity;

namespace Tests
{
    [TestClass]
    public class RouterTests
    {
        Router router;

        public RouterTests()
        {
            var container = new UnityContainer();
            router = new Router(container);

            router.Bind<MockHandler>(@"/some/test/path/?", "GET");
            router.Bind<MockHandler>(@"/" + 
                RoutingRules.endpointRegexp + "/" + 
                RoutingRules.timestampRegexp + "/?", 
                "PUT");
        }

        [TestMethod]
        public void SimpleRouting()
        {
            MockHandler.countCalled = 0;
            MockHandler.lastDataCalled = null;

            router.RouteRequest("/some/test/path", 42, "GET");
            Assert.AreEqual(1, MockHandler.countCalled);
            Assert.AreEqual(42, MockHandler.lastDataCalled);
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
            MockHandler.countCalled = 0;
            MockHandler.lastDataCalled = null;

            var result = router.RouteRequest("/some/test/path/", "Another test data", "GET");

            Assert.AreEqual(1, MockHandler.countCalled);
            Assert.AreEqual("Another test data", MockHandler.lastDataCalled);
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
            MockHandler.countCalled = 0;
            MockHandler.lastDataCalled =null;

            router.RouteRequest("/hostname-6565/2017-02-25T19:52:23Z", "Datadatadata", "PUT");

            Assert.AreEqual(1, MockHandler.countCalled);
            Assert.AreEqual("Datadatadata", MockHandler.lastDataCalled);
        }

        // TODO: test parameter passing to RequestHandler
    }
    
    class MockHandler : RequestHandler<EmptyParameters>
    {
        public static int countCalled;
        public static object lastDataCalled;

        public override object Process(EmptyParameters parameters, object data)
        {
            countCalled++;
            lastDataCalled = data;

            return "Test returned object";
        }
    }
}
