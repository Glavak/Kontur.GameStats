using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kontur.GameStats.Server;

namespace Tests
{
    [TestClass]
    public class ParameterIntegerTests
    {
        [TestMethod]
        [ExpectedException(typeof(BadRequestException))]
        public void ParameterRequired()
        {
            var param = new ParameterInteger();
            param.ParseFromString(null);
        }

        [TestMethod]
        public void ParameterDefaultValue()
        {
            var param = new ParameterInteger { Required = false, DefaultValue = 42 };

            var actual = param.ParseFromString(null);

            Assert.AreEqual(42, actual);
        }

        [TestMethod]
        public void ParameterString()
        {
            var param = new ParameterInteger { Required = false, DefaultValue = 18 };

            var actual = param.ParseFromString("113");

            Assert.AreEqual(113, actual);
        }

        [TestMethod]
        public void MinMax()
        {
            var param = new ParameterInteger { Required = false, DefaultValue = 20, MaxValue = 100, MinValue = 10 };

            var actual = param.ParseFromString("113");
            var actual2 = param.ParseFromString("-4");

            Assert.AreEqual(100, actual);
            Assert.AreEqual(10, actual2);
        }
    }
}
