using System;
using Xunit;

namespace WmcSoft.Text
{
    public class DecimalLeadingZeroCounterTests
    {
        [Fact]
        public void CanDecimalLeadingZeroCounterDetectOverflowOnNegativeValues()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DecimalLeadingZeroCounter(-1));
        }

        [Fact]
        public void CanDecimalLeadingZeroCounterDetectOverflow()
        {
            var value = DecimalLeadingZeroCounter.MaxValue;
            Assert.Throws<ArgumentOutOfRangeException>(() => value++);
        }

        [Fact]
        public void CanPreIncrementDecimalLeadingZeroCounter()
        {
            var m = new DecimalLeadingZeroCounter("03");
            var n = new DecimalLeadingZeroCounter("04");
            var x = m;

            Assert.Equal(n, ++x);
        }

        [Fact]
        public void CanPostIncrementDecimalLeadingZeroCounter()
        {
            var m = new DecimalLeadingZeroCounter("03");
            var n = new DecimalLeadingZeroCounter("04");
            var x = m;

            Assert.Equal(m, x++);
            Assert.Equal(n, x);
        }

        [Fact]
        public void CanDecimalLeadingZeroCounterIncrementOverOneLetter()
        {
            var actual = new DecimalLeadingZeroCounter("09");
            var expected = new DecimalLeadingZeroCounter("10");
            actual++;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanDecimalLeadingZeroCounterIncrementOverAHundred()
        {
            var actual = new DecimalLeadingZeroCounter("99");
            var expected = new DecimalLeadingZeroCounter("100");
            actual++;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCompareDecimalLeadingZeroCounter()
        {
            var m = new DecimalLeadingZeroCounter("03");
            var n = new DecimalLeadingZeroCounter("04");
            var q = new DecimalLeadingZeroCounter("04");

            Assert.True(m < n);
            Assert.True(n > m);
            Assert.True(n == q);
            Assert.True(m != q);
        }
    }
}
