using Xunit;

namespace WmcSoft.Geometry2D
{
    public class SegmentTests
    {
        [Fact]
        public void CheckIntersectWith()
        {
            var s1 = new Segment(new Point(1d, 3d), new Point(3d, 0d));
            var s2 = new Segment(new Point(2d, 0d), new Point(4d, 4d));
            Assert.True(s1.Intersectwith(s2));
        }

        [Fact]
        public void ParallelDoNotIntersect()
        {
            var s1 = new Segment(new Point(1d, 3d), new Point(3d, 0d));
            var s2 = new Segment(new Point(2d, 3d), new Point(4d, 0d));
            Assert.True(!s1.Intersectwith(s2));
        }
    }
}