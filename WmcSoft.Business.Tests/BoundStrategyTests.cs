using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Business
{
    [TestClass]
    public class BoundStrategyTests
    {
        [TestMethod]
        public void CheckInclusiveStrategy() {
            var strategy = BoundStrategy<int>.Inclusive;
            var lo = 2;
            var hi = 5;

            Assert.AreEqual(-1, strategy.Compare(1, lo, hi));
            Assert.AreEqual(0, strategy.Compare(lo, lo, hi));
            Assert.AreEqual(0, strategy.Compare(4, lo, hi));
            Assert.AreEqual(0, strategy.Compare(hi, lo, hi));
            Assert.AreEqual(1, strategy.Compare(6, lo, hi));

            Assert.IsFalse(strategy.IsWithinRange(1, lo, hi));
            Assert.IsTrue(strategy.IsWithinRange(lo, lo, hi));
            Assert.IsTrue(strategy.IsWithinRange(4, lo, hi));
            Assert.IsTrue(strategy.IsWithinRange(hi, lo, hi));
            Assert.IsFalse(strategy.IsWithinRange(6, lo, hi));
        }

        [TestMethod]
        public void CheckExclusiveStrategy() {
            var strategy = BoundStrategy<int>.Exclusive;
            var lo = 2;
            var hi = 5;

            Assert.AreEqual(-1, strategy.Compare(1, lo, hi));
            Assert.AreEqual(-1, strategy.Compare(lo, lo, hi));
            Assert.AreEqual(0, strategy.Compare(4, lo, hi));
            Assert.AreEqual(1, strategy.Compare(hi, lo, hi));
            Assert.AreEqual(1, strategy.Compare(6, lo, hi));

            Assert.IsFalse(strategy.IsWithinRange(1, lo, hi));
            Assert.IsFalse(strategy.IsWithinRange(lo, lo, hi));
            Assert.IsTrue(strategy.IsWithinRange(4, lo, hi));
            Assert.IsFalse(strategy.IsWithinRange(hi, lo, hi));
            Assert.IsFalse(strategy.IsWithinRange(6, lo, hi));
        }

        [TestMethod]
        public void CheckLowerExclusiveStrategy() {
            var strategy = BoundStrategy<int>.LowerExclusive;
            var lo = 2;
            var hi = 5;

            Assert.AreEqual(-1, strategy.Compare(1, lo, hi));
            Assert.AreEqual(-1, strategy.Compare(lo, lo, hi));
            Assert.AreEqual(0, strategy.Compare(4, lo, hi));
            Assert.AreEqual(0, strategy.Compare(hi, lo, hi));
            Assert.AreEqual(1, strategy.Compare(6, lo, hi));

            Assert.IsFalse(strategy.IsWithinRange(1, lo, hi));
            Assert.IsFalse(strategy.IsWithinRange(lo, lo, hi));
            Assert.IsTrue(strategy.IsWithinRange(4, lo, hi));
            Assert.IsTrue(strategy.IsWithinRange(hi, lo, hi));
            Assert.IsFalse(strategy.IsWithinRange(6, lo, hi));
        }

        [TestMethod]
        public void CheckUpperExclusiveStrategy() {
            var strategy = BoundStrategy<int>.UpperExclusive;
            var lo = 2;
            var hi = 5;

            Assert.AreEqual(-1, strategy.Compare(1, lo, hi));
            Assert.AreEqual(0, strategy.Compare(lo, lo, hi));
            Assert.AreEqual(0, strategy.Compare(4, lo, hi));
            Assert.AreEqual(1, strategy.Compare(hi, lo, hi));
            Assert.AreEqual(1, strategy.Compare(6, lo, hi));

            Assert.IsFalse(strategy.IsWithinRange(1, lo, hi));
            Assert.IsTrue(strategy.IsWithinRange(lo, lo, hi));
            Assert.IsTrue(strategy.IsWithinRange(4, lo, hi));
            Assert.IsFalse(strategy.IsWithinRange(hi, lo, hi));
            Assert.IsFalse(strategy.IsWithinRange(6, lo, hi));
        }
    }
}