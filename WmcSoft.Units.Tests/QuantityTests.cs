using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Units
{
    [TestClass]
    public class QuantityTests
    {
        [TestMethod]
        public void CanCreateSpeed()
        {
            Quantity<Meter> m = 1m;
            Quantity<Second> s = 1m;

            var speed = 2 * m / s;
            Assert.AreEqual(typeof(Quantity), speed.GetType());
            var unit = speed.Metric as DerivedUnit;
            Assert.IsNotNull(unit);
            Assert.AreEqual(2, unit.Terms.Length);
            Assert.AreEqual(typeof(Meter), unit.Terms[0].Unit.GetType());
            Assert.AreEqual(1, unit.Terms[0].Power);
            Assert.AreEqual(typeof(Second), unit.Terms[1].Unit.GetType());
            Assert.AreEqual(-1, unit.Terms[1].Power);
        }
    }
}
