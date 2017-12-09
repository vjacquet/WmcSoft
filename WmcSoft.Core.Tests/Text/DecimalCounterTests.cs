using System;
using Xunit;

namespace WmcSoft.Text
{
    public class DecimalCounterTests
    {
        [Fact]
        public void CanDecimalCounterDetectOverflowOnNegativeValues()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DecimalCounter(-1));
        }

        [Fact]
        public void CanDecimalCounterDetectOverflow()
        {
            var value = DecimalCounter.MaxValue;
            Assert.Throws<ArgumentOutOfRangeException>(() => value++);
        }

        [Fact]
        public void CanPreIncrementDecimalCounter()
        {
            var m = new DecimalCounter("3");
            var n = new DecimalCounter("4");
            var x = m;

            Assert.Equal(n, ++x);
        }

        [Fact]
        public void CanPostIncrementDecimalCounter()
        {
            var m = new DecimalCounter("3");
            var n = new DecimalCounter("4");
            var x = m;

            Assert.Equal(m, x++);
            Assert.Equal(n, x);
        }

        [Fact]
        public void CanDecimalCounterIncrementOverOneLetter()
        {
            var actual = new DecimalCounter("9");
            var expected = new DecimalCounter("10");
            actual++;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCompareDecimalCounter()
        {
            var m = new DecimalCounter("3");
            var n = new DecimalCounter("4");
            var q = new DecimalCounter("4");

            Assert.True(m < n);
            Assert.True(n > m);
            Assert.True(n == q);
            Assert.True(m != q);
        }
    }
}
