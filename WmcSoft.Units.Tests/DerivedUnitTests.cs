using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Units
{
    [TestClass]
    public class DerivedUnitTests
    {
        [TestMethod]
        public void SpeedSymbol()
        {
            var speed = new DerivedUnit("speed", SI.Meter, SI.Second ^ -1);
            Assert.AreEqual(speed.Symbol, "ms\u207B\u00B9");
        }

        [TestMethod]
        public void Area()
        {
            var square = SI.Meter ^ 2;
            Assert.AreEqual(square.Symbol, "m²");
        }
    }
}
