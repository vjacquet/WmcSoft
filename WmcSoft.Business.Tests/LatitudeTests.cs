using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class LatitudeTests
    {
        [TestMethod]
        public void CanToString() {
            var latitude = new Latitude(49, 30);
            var expected = "49° 30' 00''";
            var actual = latitude.ToString();
            Assert.AreEqual(expected, actual);
        }
    }
}
