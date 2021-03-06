﻿using Xunit;

namespace WmcSoft
{
    public class IntervalTests
    {
        [Fact]
        public void CheckIsEmpty()
        {
            var empty = Interval.Open(1, 1);
            Assert.True(empty.IsOpen());
            Assert.True(empty.IsEmpty());
        }

        [Fact]
        public void CheckIsSingleElement()
        {
            var empty = Interval.Closed(1, 1);
            Assert.True(empty.IsClosed());
            Assert.True(empty.IsSingleElement());
        }

        [Fact]
        public void CheckMixedIsEmpty()
        {
            var actual = new Interval<int>(Interval.LowerLimit(1), Interval.UpperLimit(1, exclusive: true));
            Assert.False(actual.IsOpen());
            Assert.False(actual.IsEmpty());
        }

        [Theory]
        [InlineData(4, null)]
        [InlineData(null, 5)]
        public void CanCreateMixedInterval(int? lower, int? upper)
        {
            var lo = lower.HasValue ? Interval.LowerLimit(lower.GetValueOrDefault()) : IntervalLimit<int>.UnboundedLower;
            var up = upper.HasValue ? Interval.UpperLimit(upper.GetValueOrDefault()) : IntervalLimit<int>.UnboundedUpper;
            var actual = new Interval<int>(lo,up);
            Assert.Equal(lower.HasValue, actual.HasLowerLimit);
            Assert.Equal(upper.HasValue, actual.HasUpperLimit);
        }

        [Fact]
        public void CheckIncludesOnOpenInterval()
        {
            var interval = Interval.Open(1, 10);

            Assert.True(interval.Includes(4));
            Assert.True(interval.Includes(8));
            Assert.False(interval.Includes(1));
            Assert.False(interval.Includes(10));
            Assert.False(interval.Includes(11));
            Assert.False(interval.Includes(0));
        }

        [Fact]
        public void CheckIncludesOnClosedInterval()
        {
            var interval = Interval.Closed(1, 10);

            Assert.True(interval.Includes(4));
            Assert.True(interval.Includes(8));
            Assert.True(interval.Includes(1));
            Assert.True(interval.Includes(10));
            Assert.False(interval.Includes(11));
            Assert.False(interval.Includes(0));
        }

        [Theory]
        [InlineData(4, 10, "[4, 10]")]
        public void CheckToString(int lower, int upper, string expected)
        {
            var interval = Interval.Closed(lower, upper);
            var actual = interval.ToString();
            Assert.Equal(expected, actual);
        }
    }
}