using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Text
{
    [TestClass]
    public class DecimalCounterTests
    {
        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void CanDecimalCounterDetectOverflowOnNegativeValues() {
            var value = new DecimalCounter(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void CanLowerLatinCounterDetectOverflow() {
            var value = DecimalCounter.MaxValue;
            value++;
        }

        [TestMethod]
        public void CanPreIncrementLowerLatinCounter() {
            var m = new DecimalCounter("3");
            var n = new DecimalCounter("4");
            var x = m;

            Assert.AreEqual(n, ++x);
        }

        [TestMethod]
        public void CanPostIncrementLowerLatinCounter() {
            var m = new DecimalCounter("3");
            var n = new DecimalCounter("4");
            var x = m;

            Assert.AreEqual(m, x++);
            Assert.AreEqual(n, x);
        }

        [TestMethod]
        public void CanLowerLatinCounterIncrementOverOneLetter() {
            var actual = new DecimalCounter("26");
            var expected = new DecimalCounter("27");
            actual++;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanCompareLowerLatinCounter() {
            var m = new DecimalCounter("3");
            var n = new DecimalCounter("4");
            var q = new DecimalCounter("4");

            Assert.IsTrue(m < n);
            Assert.IsTrue(n > m);
            Assert.IsTrue(n == q);
            Assert.IsTrue(m != q);
        }
    }
}