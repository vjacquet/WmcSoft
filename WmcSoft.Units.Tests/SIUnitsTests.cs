using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Units
{
    [TestClass]
    public class SIUnitsTests
    {
        [TestMethod]
        public void CanGetKilometer()
        {
            var m = SI.Meter;
            var km = m.WithPrefix(SIPrefix.Kilo);
            Assert.AreEqual("km", km.Symbol);
        }

        [TestMethod]
        public void ConvertGetGram()
        {
            var kg = SI.Kilogram;
            var g = kg.WithPrefix(SIPrefix.None);
            Assert.AreEqual("g", g.Symbol);
        }
    }
}
