using Kontur.GameStats.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var param = new ParameterString {Required = false, DefaultValue = "Default"};

            object actual = param.ParseFromString(null);

            Assert.AreEqual("Default", actual);
        }

        [TestMethod]
        public void ParameterString()
        {
            var param = new ParameterString {Required = false, DefaultValue = "Default"};

            object actual = param.ParseFromString("Value");

            Assert.AreEqual("Value", actual);
        }

        [TestMethod]
        public void CaseInsensitivity()
        {
            var param = new ParameterString {Required = true, Lowercase = true};

            var actual1 = (string) param.ParseFromString("VaLuE");
            var actual2 = (string) param.ParseFromString("vAlUe");

            Assert.IsTrue(actual1 == actual2);
        }
    }
}