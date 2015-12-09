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
    }
}
