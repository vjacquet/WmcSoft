using Xunit;

namespace WmcSoft.Geometry2D
{
    public class PolygonTests
    {
        [Fact]
        public void DetectPointInside()
        {
            var p = new Polygon(new Point(-10, -10), new Point(-10, 10), new Point(10, 10), new Point(10, -10));
            var t = new Point(5d, 5d);
            Assert.True(p.Inside(t));
        }
    }
}
