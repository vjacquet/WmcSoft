using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Text
{
    [TestClass]
    public class LowerAlphaCounterTests
    {
        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void CanLowerLatinCounterDetectOverflowOnNegativeValues() {
            var value = new LowerAlphaCounter(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void CanLowerLatinCounterDetectOverflow() {
            var value = LowerAlphaCounter.MaxValue;
            value++;
        }

        [TestMethod]
        public void CanPreIncrementLowerLatinCounter() {
            var m = new LowerAlphaCounter("c");
            var n = new LowerAlphaCounter("d");
            var x = m;

            Assert.AreEqual(n, ++x);
        }

        [TestMethod]
        public void CanPostIncrementLowerLatinCounter() {
            var m = new LowerAlphaCounter("c");
            var n = new LowerAlphaCounter("d");
            var x = m;

            Assert.AreEqual(m, x++);
            Assert.AreEqual(n, x);
        }

        [TestMethod]
        public void CanLowerLatinCounterIncrementOverOneLetter() {
            var actual = new LowerAlphaCounter("z");
            var expected = new LowerAlphaCounter("aa");
            actual++;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanCompareLowerLatinCounter() {
            var m = new LowerAlphaCounter("c");
            var n = new LowerAlphaCounter("e");
            var q = new LowerAlphaCounter("e");

            Assert.IsTrue(m < n);
            Assert.IsTrue(n > m);
            Assert.IsTrue(n == q);
            Assert.IsTrue(m != q);
        }
    }
}