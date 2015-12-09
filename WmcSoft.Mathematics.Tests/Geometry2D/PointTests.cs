using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            Assert.AreEqual("P1", p1.ToString("N"));
            Assert.AreEqual("(2,4)", p1.ToString("C"));
            Assert.AreEqual("P1 (2,4)", p1.ToString());
        }
    }
}
