using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Text
{
    [TestClass]
    public class UpperAlphaCounterTests
    {
        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void CanUpperLatinCounterDetectOverflowOnNegativeValues() {
            var value = new UpperAlphaCounter(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void CanUpperLatinCounterDetectOverflow() {
            var value = UpperAlphaCounter.MaxValue;
            value++;
        }

        [TestMethod]
        public void CanPreIncrementUpperLatinCounter() {
            var m = new UpperAlphaCounter("C");
            var n = new UpperAlphaCounter("D");
            var x = m;

            Assert.AreEqual(n, ++x);
        }

        [TestMethod]
        public void CanPostIncrementUpperLatinCounter() {
            var m = new UpperAlphaCounter("C");
            var n = new UpperAlphaCounter("D");
            var x = m;

            Assert.AreEqual(m, x++);
            Assert.AreEqual(n, x);
        }

        [TestMethod]
        public void CanUpperLatinCounterIncrementOverOneLetter() {
            var actual = new UpperAlphaCounter("Z");
            var expected = new UpperAlphaCounter("AA");
            actual++;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanCompareUpperLatinCounter() {
            var m = new UpperAlphaCounter("C");
            var n = new UpperAlphaCounter("E");
            var q = new UpperAlphaCounter("E");

            Assert.IsTrue(m < n);
            Assert.IsTrue(n > m);
            Assert.IsTrue(n == q);
            Assert.IsTrue(m != q);
        }
    }
}