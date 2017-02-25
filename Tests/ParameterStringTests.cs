using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kontur.GameStats.Server;

namespace Tests
{
    [TestClass]
    public class ParameterStringTests
    {
        [TestMethod]
        [ExpectedException(typeof(BadRequestException))]
        public void ParameterRequired()
        {
            var param = new ParameterString();
            param.ParseFromString(null);
        }

        [TestMethod]
        public void ParameterDefaultValue()
        {
            var param = new ParameterString { Required = false, DefaultValue = "Default" };

            var actual = param.ParseFromString(null);

            Assert.AreEqual("Default", actual);
        }

        [TestMethod]
        public void ParameterString()
        {
            var param = new ParameterString { Required = false, DefaultValue = "Default" };

            var actual = param.ParseFromString("Value");

            Assert.AreEqual("Value", actual);
        }
    }
}
