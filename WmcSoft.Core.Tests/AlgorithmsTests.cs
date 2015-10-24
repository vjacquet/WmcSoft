using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Collections.Generic;

namespace WmcSoft
{
    [TestClass]
    public class AlgorithmsTests
    {
        [TestMethod]
        public void CheckMin() {
            DateTime x = new DateTime(1973, 5, 2);
            DateTime y = new DateTime(1973, 5, 1);

            var expected = y;
            var actual = Algorithms.Min(x, y);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckMinStablility() {
            DateTime x = new DateTime(1973, 5, 2);
            DateTime y = new DateTime(1973, 5, 1);

            Comparison<DateTime> comparison = (a, b) => a.Month.CompareTo(b.Month);
            var expected = x;
            var actual = Algorithms.Min(comparison, x, y);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckMinStablilityN() {
            DateTime x = new DateTime(1973, 5, 2);
            DateTime y = new DateTime(1973, 5, 1);
            DateTime z = new DateTime(1973, 6, 2);

            Comparison<DateTime> comparison = (a, b) => a.Month.CompareTo(b.Month);
            var expected = x;
            var actual = Algorithms.Min(comparison, x, y, z);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckMax() {
            DateTime x = new DateTime(1973, 5, 2);
            DateTime y = new DateTime(1973, 5, 1);

            var expected = x;
            var actual = Algorithms.Max(x, y);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckMaxStablility() {
            DateTime x = new DateTime(1973, 5, 2);
            DateTime y = new DateTime(1973, 5, 1);

            Comparison<DateTime> comparison = (a, b) => a.Month.CompareTo(b.Month);
            var expected = y;
            var actual = Algorithms.Max(comparison, x, y);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckMaxStablilityN() {
            DateTime x = new DateTime(1973, 5, 2);
            DateTime y = new DateTime(1973, 5, 1);
            DateTime z = new DateTime(1973, 4, 1);

            Comparison<DateTime> comparison = (a, b) => a.Month.CompareTo(b.Month);
            var expected = y;
            var actual = Algorithms.Max(comparison, x, y,z);
            Assert.AreEqual(expected, actual);
        }
    }
}
