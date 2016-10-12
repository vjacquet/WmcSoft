using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class DecimalExtensionsTests
    {
        [TestMethod]
        public void CanGetScale() {
            Assert.AreEqual(0, 1m.Scale());
            Assert.AreEqual(2, 0.10m.Scale());
            Assert.AreEqual(1, 100.0m.Scale());
            Assert.AreEqual(4, 1.2345m.Scale());
            Assert.AreEqual(8, 1.23456789m.Scale());
            Assert.AreEqual(3, 12.345m.Scale());
            Assert.AreEqual(1, 12.3456E+3m.Scale());
        }

        [TestMethod]
        public void CanGetPrecision() {
            Assert.AreEqual(1, 0m.Precision());
            Assert.AreEqual(1, 0.0m.Precision());
            Assert.AreEqual(1, 1m.Precision());
            Assert.AreEqual(2, 1.0m.Precision());
            Assert.AreEqual(2, 0.10m.Precision());
            Assert.AreEqual(2, 10m.Precision());
            Assert.AreEqual(6, 123.234m.Precision());
            Assert.AreEqual(3, 523m.Precision());
            Assert.AreEqual(4, 523.0m.Precision());
            Assert.AreEqual(4, 5230m.Precision());
            Assert.AreEqual(29, decimal.MaxValue.Precision());
        }
    }
}