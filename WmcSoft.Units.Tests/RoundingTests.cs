using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Units.Tests
{
    [TestClass]
    public class RoundingTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NoRoundPolicy()
        {
            RoundingPolicy policy = null;
            var quantity = new Quantity(4.45m, SI.Meter);
            var rounded = quantity.Round(policy).Amount;
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void ZeroStepPolicy()
        {
            var policy = new RoundingPolicy(0.0m);
        }

        [TestMethod]
        public void RoundUp()
        {
            RoundingPolicy policy = new RoundingPolicy(1, RoundingStrategy.RoundUp);
            Quantity quantity;
            decimal rounded;

            quantity = new Quantity(4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.AreEqual(4.5m, rounded);

            quantity = new Quantity(-4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.AreEqual(-4.5m, rounded);
        }

        [TestMethod]
        public void RoundDown()
        {
            RoundingPolicy policy = new RoundingPolicy(1, RoundingStrategy.RoundDown);
            Quantity quantity;
            decimal rounded;

            quantity = new Quantity(4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.AreEqual(4.4m, rounded);

            quantity = new Quantity(-4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.AreEqual(-4.4m, rounded);
        }

        [TestMethod]
        public void RoundWhenRoundingDigitIs5()
        {
            RoundingPolicy policy = new RoundingPolicy(1, RoundingStrategy.Round);
            Quantity quantity;
            decimal rounded;

            quantity = new Quantity(4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.AreEqual(4.5m, rounded);

            quantity = new Quantity(-4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.AreEqual(-4.5m, rounded);
        }

        [TestMethod]
        public void RoundWhenRoundingDigitIs6()
        {
            RoundingPolicy policy = new RoundingPolicy(1, 6, RoundingStrategy.Round);
            Quantity quantity;
            decimal rounded;

            quantity = new Quantity(4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.AreEqual(4.4m, rounded);

            quantity = new Quantity(-4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.AreEqual(-4.4m, rounded);
        }

        [TestMethod]
        public void RoundWhenRoundingDigitIs4()
        {
            RoundingPolicy policy = new RoundingPolicy(1, 4, RoundingStrategy.Round);
            Quantity quantity;
            decimal rounded;

            quantity = new Quantity(4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.AreEqual(4.5m, rounded);

            quantity = new Quantity(-4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.AreEqual(-4.5m, rounded);
        }

        [TestMethod]
        public void RoundUpByStep()
        {
            RoundingPolicy policy = new RoundingPolicy(0.25m, RoundingStrategy.RoundUpByStep);
            Quantity quantity;
            decimal rounded;

            quantity = new Quantity(4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.AreEqual(4.50m, rounded);

            quantity = new Quantity(-4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.AreEqual(-4.50m, rounded);
        }

        [TestMethod]
        public void RoundDownByStep()
        {
            RoundingPolicy policy = new RoundingPolicy(0.25m, RoundingStrategy.RoundDownByStep);
            Quantity quantity;
            decimal rounded;

            quantity = new Quantity(4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.AreEqual(4.25m, rounded);

            quantity = new Quantity(-4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.AreEqual(-4.25m, rounded);
        }

        [TestMethod]
        public void RoundTowardsPositive()
        {
            RoundingPolicy policy = new RoundingPolicy(1, RoundingStrategy.RoundTowardsPositive);
            Quantity quantity;
            decimal rounded;

            quantity = new Quantity(4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.AreEqual(4.5m, rounded);

            quantity = new Quantity(-4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.AreEqual(-4.4m, rounded);
        }

        [TestMethod]
        public void RoundTowardsNegative()
        {
            RoundingPolicy policy = new RoundingPolicy(1, RoundingStrategy.RoundTowardsNegative);
            Quantity quantity;
            decimal rounded;

            quantity = new Quantity(4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.AreEqual(4.4m, rounded);

            quantity = new Quantity(-4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.AreEqual(-4.5m, rounded);
        }
    }
}
