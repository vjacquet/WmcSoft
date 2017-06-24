using System;
using Xunit;

namespace WmcSoft.Text
{
    public class UpperAlphaCounterTests
    {
        [Fact]
        public void CanUpperLatinCounterDetectOverflowOnNegativeValues()
        {
            Assert.Throws<OverflowException>(() => new UpperAlphaCounter(-1));
        }

        [Fact]
        public void CanUpperLatinCounterDetectOverflow()
        {
            var value = UpperAlphaCounter.MaxValue;
            Assert.Throws<OverflowException>(() => value++);
        }

        [Fact]
        public void CanPreIncrementUpperLatinCounter()
        {
            var m = new UpperAlphaCounter("C");
            var n = new UpperAlphaCounter("D");
            var x = m;

            Assert.Equal(n, ++x);
        }

        [Fact]
        public void CanPostIncrementUpperLatinCounter()
        {
            var m = new UpperAlphaCounter("C");
            var n = new UpperAlphaCounter("D");
            var x = m;

            Assert.Equal(m, x++);
            Assert.Equal(n, x);
        }

        [Fact]
        public void CanUpperLatinCounterIncrementOverOneLetter()
        {
            var actual = new UpperAlphaCounter("Z");
            var expected = new UpperAlphaCounter("AA");
            actual++;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCompareUpperLatinCounter()
        {
            var m = new UpperAlphaCounter("C");
            var n = new UpperAlphaCounter("E");
            var q = new UpperAlphaCounter("E");

            Assert.True(m < n);
            Assert.True(n > m);
            Assert.True(n == q);
            Assert.True(m != q);
        }
    }
}