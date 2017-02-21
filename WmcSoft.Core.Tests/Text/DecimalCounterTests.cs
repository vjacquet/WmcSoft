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
        public void CanDecimalCounterDetectOverflow() {
            var value = DecimalCounter.MaxValue;
            value++;
        }

        [TestMethod]
        public void CanPreIncrementDecimalCounter() {
            var m = new DecimalCounter("3");
            var n = new DecimalCounter("4");
            var x = m;

            Assert.AreEqual(n, ++x);
        }

        [TestMethod]
        public void CanPostIncrementDecimalCounter() {
            var m = new DecimalCounter("3");
            var n = new DecimalCounter("4");
            var x = m;

            Assert.AreEqual(m, x++);
            Assert.AreEqual(n, x);
        }

        [TestMethod]
        public void CanDecimalCounterIncrementOverOneLetter() {
            var actual = new DecimalCounter("9");
            var expected = new DecimalCounter("10");
            actual++;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanCompareDecimalCounter() {
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