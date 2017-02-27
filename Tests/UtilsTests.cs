using System;
using System.Text;
using System.Collections.Generic;
using Kontur.GameStats.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class UtilsTests
    {
        public UtilsTests()
        {
        }

        [TestMethod]
        public void UpdateAverageTest()
        {
            float average = 1f;
            int count = 2;

            average = MyMath.UpdateAverage(average, count++, 4);
            Assert.AreEqual(2, average);

            average = MyMath.UpdateAverage(average, count++, 3);
            Assert.AreEqual(2.25f, average);

            average = MyMath.UpdateAverage(average, count++, 8);
            Assert.AreEqual(3.4f, average);
        }

        [TestMethod]
        public void IncrementDictionaryExisting()
        {
            Dictionary<string, int> testDictionary = new Dictionary<string, int>
            {
                {"Key1", 42},
                {"Key2", -11}
            };

            testDictionary.IncrementValue("Key1");
            Assert.AreEqual(43, testDictionary["Key1"]);

            testDictionary.IncrementValue("Key1");
            Assert.AreEqual(44, testDictionary["Key1"]);

            testDictionary.IncrementValue("Key2");
            Assert.AreEqual(-10, testDictionary["Key2"]);
        }

        [TestMethod]
        public void IncrementDictionaryNotExisting()
        {
            Dictionary<string, int> testDictionary = new Dictionary<string, int>
            {
                {"Key1", 42},
                {"Key2", -11}
            };

            testDictionary.IncrementValue("Key3");
            Assert.AreEqual(1, testDictionary["Key3"]);
        }
    }
}
