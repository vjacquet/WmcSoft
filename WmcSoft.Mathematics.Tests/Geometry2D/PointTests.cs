using System.Globalization;
using Xunit;
using static WmcSoft.Geometry2D.Helpers;

namespace WmcSoft.Geometry2D
{
    public class PointTests
    {
        [Fact]
        public void CanComparePoints()
        {
            var p1 = new Point(2d, 4d);
            var p2 = new Point(2d, 4d);
            Assert.Equal(p1, p2);
        }

        [Fact]
        public void CanFormat()
        {
            var p1 = new Point("P1", 2d, 4d);
            var iv = CultureInfo.InvariantCulture;
            Assert.Equal("P1", p1.ToString("N", iv));
            Assert.Equal("(2,4)", p1.ToString("C", iv));
            Assert.Equal("P1 (2,4)", p1.ToString(iv));
        }

        [Fact]
        public void CanTurnCounterClockwise()
        {
            var p0 = new Point(2d, 0);
            var p1 = new Point(1.5d, 1d);
            var p2 = new Point(0.5d, 1d);

            Assert.Equal(1, CounterClockwise(p0, p1, p2));
            Assert.Equal(-1, CounterClockwise(p2, p1, p0));
        }
    }
}
