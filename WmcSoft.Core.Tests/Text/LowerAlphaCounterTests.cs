using System;
using Xunit;

namespace WmcSoft.Text
{
    public class LowerAlphaCounterTests
    {
        [Fact]
        public void CanLowerLatinCounterDetectOverflowOnNegativeValues()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new LowerAlphaCounter(-1));
        }

        [Fact]
        public void CanLowerLatinCounterDetectOverflow()
        {
            var value = LowerAlphaCounter.MaxValue;
            Assert.Throws<ArgumentOutOfRangeException>(() => value++);
        }

        [Fact]
        public void CanPreIncrementLowerLatinCounter()
        {
            var m = new LowerAlphaCounter("c");
            var n = new LowerAlphaCounter("d");
            var x = m;

            Assert.Equal(n, ++x);
        }

        [Fact]
        public void CanPostIncrementLowerLatinCounter()
        {
            var m = new LowerAlphaCounter("c");
            var n = new LowerAlphaCounter("d");
            var x = m;

            Assert.Equal(m, x++);
            Assert.Equal(n, x);
        }

        [Fact]
        public void CanLowerLatinCounterIncrementOverOneLetter()
        {
            var actual = new LowerAlphaCounter("z");
            var expected = new LowerAlphaCounter("aa");
            actual++;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCompareLowerLatinCounter()
        {
            var m = new LowerAlphaCounter("c");
            var n = new LowerAlphaCounter("e");
            var q = new LowerAlphaCounter("e");

            Assert.True(m < n);
            Assert.True(n > m);
            Assert.True(n == q);
            Assert.True(m != q);
        }
    }
}
