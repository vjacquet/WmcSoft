using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Text
{
    [TestClass]
    public class DecimalLeadingZeroCounterTests
    {
        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void CanDecimalLeadingZeroCounterDetectOverflowOnNegativeValues() {
            var value = new DecimalLeadingZeroCounter(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void CanDecimalLeadingZeroCounterDetectOverflow() {
            var value = DecimalLeadingZeroCounter.MaxValue;
            value++;
        }

        [TestMethod]
        public void CanPreIncrementDecimalLeadingZeroCounter() {
            var m = new DecimalLeadingZeroCounter("03");
            var n = new DecimalLeadingZeroCounter("04");
            var x = m;

            Assert.AreEqual(n, ++x);
        }

        [TestMethod]
        public void CanPostIncrementDecimalLeadingZeroCounter() {
            var m = new DecimalLeadingZeroCounter("03");
            var n = new DecimalLeadingZeroCounter("04");
            var x = m;

            Assert.AreEqual(m, x++);
            Assert.AreEqual(n, x);
        }

        [TestMethod]
        public void CanDecimalLeadingZeroCounterIncrementOverOneLetter() {
            var actual = new DecimalLeadingZeroCounter("09");
            var expected = new DecimalLeadingZeroCounter("10");
            actual++;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanCompareDecimalLeadingZeroCounter() {
            var m = new DecimalLeadingZeroCounter("03");
            var n = new DecimalLeadingZeroCounter("04");
            var q = new DecimalLeadingZeroCounter("04");

            Assert.IsTrue(m < n);
            Assert.IsTrue(n > m);
            Assert.IsTrue(n == q);
            Assert.IsTrue(m != q);
        }
    }
}