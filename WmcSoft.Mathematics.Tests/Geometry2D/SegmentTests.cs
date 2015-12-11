using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Geometry2D
{
    [TestClass]
    public class SegmentTests
    {
        [TestMethod]
        public void CheckIntersectWith() {
            var s1 = new Segment(new Point(1d, 3d), new Point(3d, 0d));
            var s2 = new Segment(new Point(2d, 0d), new Point(4d, 4d));
            Assert.IsTrue(s1.Intersectwith(s2));
        }

        [TestMethod]
        public void ParallelDoNotIntersect() {
            var s1 = new Segment(new Point(1d, 3d), new Point(3d, 0d));
            var s2 = new Segment(new Point(2d, 3d), new Point(4d, 0d));
            Assert.IsTrue(!s1.Intersectwith(s2));
        }
    }
}
