using System;
using Xunit;

namespace WmcSoft.Units.Tests
{
    public class RoundingTests
    {
        [Fact]
        public void NoRoundPolicy()
        {
            RoundingPolicy policy = null;
            var quantity = new Quantity(4.45m, SI.Meter);
            Assert.Throws<ArgumentNullException>(() => quantity.Round(policy).Amount);
        }

        [Fact]
        public void ZeroStepPolicy()
        {
            Assert.Throws<DivideByZeroException>(() => new RoundingPolicy(0.0m));
        }

        [Fact]
        public void RoundUp()
        {
            var policy = new RoundingPolicy(1, RoundingStrategy.RoundUp);
            Quantity quantity;
            decimal rounded;

            quantity = new Quantity(4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.Equal(4.5m, rounded);

            quantity = new Quantity(-4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.Equal(-4.5m, rounded);
        }

        [Fact]
        public void RoundDown()
        {
            var policy = new RoundingPolicy(1, RoundingStrategy.RoundDown);
            Quantity quantity;
            decimal rounded;

            quantity = new Quantity(4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.Equal(4.4m, rounded);

            quantity = new Quantity(-4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.Equal(-4.4m, rounded);
        }

        [Fact]
        public void RoundWhenRoundingDigitIs5()
        {
            var policy = new RoundingPolicy(1, RoundingStrategy.Round);
            Quantity quantity;
            decimal rounded;

            quantity = new Quantity(4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.Equal(4.5m, rounded);

            quantity = new Quantity(-4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.Equal(-4.5m, rounded);
        }

        [Fact]
        public void RoundWhenRoundingDigitIs6()
        {
            var policy = new RoundingPolicy(1, 6, RoundingStrategy.Round);
            Quantity quantity;
            decimal rounded;

            quantity = new Quantity(4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.Equal(4.4m, rounded);

            quantity = new Quantity(-4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.Equal(-4.4m, rounded);
        }

        [Fact]
        public void RoundWhenRoundingDigitIs4()
        {
            var policy = new RoundingPolicy(1, 4, RoundingStrategy.Round);
            Quantity quantity;
            decimal rounded;

            quantity = new Quantity(4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.Equal(4.5m, rounded);

            quantity = new Quantity(-4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.Equal(-4.5m, rounded);
        }

        [Fact]
        public void RoundUpByStep()
        {
            var policy = new RoundingPolicy(0.25m, RoundingStrategy.RoundUpByStep);
            Quantity quantity;
            decimal rounded;

            quantity = new Quantity(4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.Equal(4.50m, rounded);

            quantity = new Quantity(-4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.Equal(-4.50m, rounded);
        }

        [Fact]
        public void RoundDownByStep()
        {
            var policy = new RoundingPolicy(0.25m, RoundingStrategy.RoundDownByStep);
            Quantity quantity;
            decimal rounded;

            quantity = new Quantity(4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.Equal(4.25m, rounded);

            quantity = new Quantity(-4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.Equal(-4.25m, rounded);
        }

        [Fact]
        public void RoundTowardsPositive()
        {
            var policy = new RoundingPolicy(1, RoundingStrategy.RoundTowardsPositive);
            Quantity quantity;
            decimal rounded;

            quantity = new Quantity(4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.Equal(4.5m, rounded);

            quantity = new Quantity(-4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.Equal(-4.4m, rounded);
        }

        [Fact]
        public void RoundTowardsNegative()
        {
            var policy = new RoundingPolicy(1, RoundingStrategy.RoundTowardsNegative);
            Quantity quantity;
            decimal rounded;

            quantity = new Quantity(4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.Equal(4.4m, rounded);

            quantity = new Quantity(-4.45m, SI.Meter);
            rounded = quantity.Round(policy).Amount;
            Assert.Equal(-4.5m, rounded);
        }
    }
}