using System;
using Kontur.GameStats.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class ParameterTimetampTests
    {
        [TestMethod]
        [ExpectedException(typeof(BadRequestException))]
        public void ParameterRequired()
        {
            var param = new ParameterTimestamp();
            param.ParseFromString(null);
        }

        [TestMethod]
        public void ParameterDefaultValue()
        {
            var expected = new DateTime(12541324);

            var param = new ParameterTimestamp {Required = false, DefaultValue = expected};

            object actual = param.ParseFromString(null);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParameterString()
        {
            var param = new ParameterTimestamp {Required = true};

            var expected = new DateTime(2017, 2, 25, 19, 52, 23);
            object actual = param.ParseFromString("2017-02-25T19:52:23Z");

            Assert.AreEqual(expected, actual);
        }
    }
}