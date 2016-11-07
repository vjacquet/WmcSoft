using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    /// <summary>
    /// Description résumée pour IntervalTests
    /// </summary>
    [TestClass]
    public class IntervalTests
    {
        [TestMethod]
        public void CheckIsEmpty() {
            var empty = Interval.Open(1, 1);
            Assert.IsTrue(empty.IsOpen());
            Assert.IsTrue(empty.IsEmpty());
        }

        [TestMethod]
        public void CheckIsSingleElement() {
            var empty = Interval.Closed(1, 1);
            Assert.IsTrue(empty.IsClosed());
            Assert.IsTrue(empty.IsSingleElement());
        }

        [TestMethod]
        public void CheckMixedIsEmpty() {
            var actual = new Interval<int>(IntervalLimit.Lower(1, true), IntervalLimit.Upper(1, false));
            Assert.IsFalse(actual.IsOpen());
            Assert.IsFalse(actual.IsEmpty());
        }

        [TestMethod]
        public void CheckIncludesOnOpenInterval() {
            var open = Interval.Open(1, 10);

            Assert.IsTrue(open.Includes(4));
            Assert.IsTrue(open.Includes(8));
            Assert.IsFalse(open.Includes(1));
            Assert.IsFalse(open.Includes(10));
            Assert.IsFalse(open.Includes(11));
            Assert.IsFalse(open.Includes(0));
        }

        [TestMethod]
        public void CheckIncludesOnClosedInterval() {
            var open = Interval.Closed(1, 10);

            Assert.IsTrue(open.Includes(4));
            Assert.IsTrue(open.Includes(8));
            Assert.IsTrue(open.Includes(1));
            Assert.IsTrue(open.Includes(10));
            Assert.IsFalse(open.Includes(11));
            Assert.IsFalse(open.Includes(0));
        }
    }
}
