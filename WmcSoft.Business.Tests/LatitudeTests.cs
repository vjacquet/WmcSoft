using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class LatitudeTests
    {
        [TestMethod]
        public void CanToString() {
            var iv = CultureInfo.InvariantCulture;
            var latitude = new Latitude(49, 30);

            Assert.AreEqual("49° 30' 00\"", latitude.ToString(iv));
            Assert.AreEqual("49° 30.00'", latitude.ToString("M", iv));
            Assert.AreEqual("49.5000°", latitude.ToString("D", iv));
        }
    }
}
