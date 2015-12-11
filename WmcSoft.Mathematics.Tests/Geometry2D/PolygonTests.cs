using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Geometry2D
{
    [TestClass]
    public class PolygonTests
    {
        [TestMethod]
        public void DetectPointInside() {
            var p = new Polygon(new Point(-10, -10), new Point(-10, 10), new Point(10, 10), new Point(10, -10));
            var t = new Point(5d, 5d);
            Assert.IsTrue(p.Inside(t));
        }
    }
}
