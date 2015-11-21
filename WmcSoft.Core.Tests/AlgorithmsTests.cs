using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class AlgorithmsTests
    {
        [TestMethod]
        public void CheckMediam() {
            var x = 20;
            var y = 40;
            Assert.AreEqual(30, Algorithms.Median(x, y));
            Assert.AreEqual(30, Algorithms.Median(y, x));
        }

        [TestMethod]
        public void CheckMin() {
            var x = new DateTime(1973, 5, 2);
            var y = new DateTime(1973, 5, 1);

            var expected = y;
            var actual = Algorithms.Min(x, y);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckMinStablility() {
            var x = new DateTime(1973, 5, 2);
            var y = new DateTime(1973, 5, 1);

            Comparison<DateTime> comparison = (a, b) => a.Month.CompareTo(b.Month);
            var expected = x;
            var actual = Algorithms.Min(comparison, x, y);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckMinStablilityN() {
            var x = new DateTime(1973, 5, 2);
            var y = new DateTime(1973, 5, 1);
            var z = new DateTime(1973, 6, 2);

            Comparison<DateTime> comparison = (a, b) => a.Month.CompareTo(b.Month);
            var expected = x;
            var actual = Algorithms.Min(comparison, x, y, z);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckMax() {
            var x = new DateTime(1973, 5, 2);
            var y = new DateTime(1973, 5, 1);

            var expected = x;
            var actual = Algorithms.Max(x, y);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckMaxStablility() {
            var x = new DateTime(1973, 5, 2);
            var y = new DateTime(1973, 5, 1);

            Comparison<DateTime> comparison = (a, b) => a.Month.CompareTo(b.Month);
            var expected = y;
            var actual = Algorithms.Max(comparison, x, y);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckMaxStablilityN() {
            var x = new DateTime(1973, 5, 2);
            var y = new DateTime(1973, 5, 1);
            var z = new DateTime(1973, 4, 1);

            Comparison<DateTime> comparison = (a, b) => a.Month.CompareTo(b.Month);
            var expected = y;
            var actual = Algorithms.Max(comparison, x, y,z);
            Assert.AreEqual(expected, actual);
        }
    }
}
