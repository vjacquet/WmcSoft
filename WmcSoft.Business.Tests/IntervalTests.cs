using Xunit;

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
            var actual = new Interval<int>(IntervalLimit.Lower(1, true), IntervalLimit.Upper(1, false));
            Assert.False(actual.IsOpen());
            Assert.False(actual.IsEmpty());
        }

        [Fact]
        public void CanCreateMixedInterval()
        {
            var actual = new Interval<int>(IntervalLimit<int>.Unbounded, IntervalLimit.Upper(5, true));
            Assert.False(actual.HasLowerLimit);
            Assert.True(actual.HasUpperLimit);
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
            var interval = new Interval<int>(lower, upper);
            var actual = interval.ToString();
            Assert.Equal(expected, actual);
        }
    }
}