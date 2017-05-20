using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Units
{
    [TestClass]
    public class EqualsTests
    {
        [TestMethod]
        public void EqualsUnit()
        {
            var expected = new DerivedUnit("speed", "km/h", SI.Meter, SI.Second ^ -1);
            var actual = new DerivedUnit("speed", "km/h", SI.Meter, SI.Second ^ -1);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void op_equalityUnit()
        {
            var expected = new DerivedUnit("speed", "km/h", SI.Meter, SI.Second ^ -1);
            var actual = new DerivedUnit("speed", "km/h", SI.Meter, SI.Second ^ -1);
            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void op_greaterthanQuantity()
        {
            Quantity q1 = new Quantity(5, SI.Meter);
            Quantity q2 = new Quantity(10, SI.Meter);
            Assert.IsTrue(q1 < q2);
        }

        [TestMethod]
        public void op_equalityQuantity()
        {
            Quantity q1 = new Quantity(5, SI.Meter);
            Quantity q2 = new Quantity(5, SI.Meter);
            Assert.IsTrue(q1 == q2);
            Assert.IsFalse(null == q2);
            Assert.IsFalse(q1 == null);
        }
    }
}
