using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static WmcSoft.Geometry2D.Helpers;

namespace WmcSoft.Geometry2D
{
    [TestClass]
    public class PointTests
    {
        [TestMethod]
        public void CanComparePoints() {
            var p1 = new Point(2d, 4d);
            var p2 = new Point(2d, 4d);
            Assert.AreEqual<Point>(p1, p2);
        }

        [TestMethod]
        public void CanFormat() {
            var p1 = new Point("P1", 2d, 4d);
            var iv = CultureInfo.InvariantCulture;
            Assert.AreEqual("P1", p1.ToString("N", iv));
            Assert.AreEqual("(2,4)", p1.ToString("C", iv));
            Assert.AreEqual("P1 (2,4)", p1.ToString(iv));
        }

        [TestMethod]
        public void CanTurnCounterClockwise() {
            var p0 = new Point(2d, 0);
            var p1 = new Point(1.5d, 1d);
            var p2 = new Point(0.5d, 1d);

            Assert.AreEqual(1, CounterClockwise(p0, p1, p2));
            Assert.AreEqual(-1, CounterClockwise(p2, p1, p0));
        }
    }
}
