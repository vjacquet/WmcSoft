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
            Assert.AreEqual(0, 123m.Scale());
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

        [TestMethod]
        public void CanCheckStackoverflowQuestion() {
            // see <http://stackoverflow.com/questions/763942/calculate-system-decimal-precision-and-scale>
            decimal d;

            d = 0m;
            Assert.AreEqual(0, d.Scale());
            Assert.AreEqual(1, d.Precision());

            d = 0.0m;
            Assert.AreEqual(1, d.Scale());
            Assert.AreNotEqual(2, d.Precision()); // true in the SO question.
            Assert.AreEqual(1, d.Precision());

            d = 12.45m;
            Assert.AreEqual(2, d.Scale());
            Assert.AreEqual(4, d.Precision());

            d = 12.4500m;
            Assert.AreEqual(4, d.Scale());
            Assert.AreEqual(6, d.Precision());

            d = 770m;
            Assert.AreEqual(0, d.Scale());
            Assert.AreEqual(3, d.Precision());
        }
    }
}