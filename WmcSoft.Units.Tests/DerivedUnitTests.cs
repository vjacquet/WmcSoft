using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Units
{
    [TestClass]
    public class DerivedUnitTests
    {
        [TestMethod]
        public void SpeedSymbol() {
            DerivedUnit speed = new DerivedUnit("speed", new DerivedUnitTerm[] { new DerivedUnitTerm(SI.Meter), new DerivedUnitTerm(SI.Second, -1) });
            Assert.AreEqual(speed.Symbol, "ms\u207B\u00B9");
        }
    }
}
