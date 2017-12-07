using System;
using System.Globalization;
using Xunit;

namespace WmcSoft
{
    public class DecimalExtensionsTests
    {
        [Theory]
        [InlineData(0, "1")]
        [InlineData(2, "0.10")]
        [InlineData(1, "100.0")]
        [InlineData(0, "123")]
        [InlineData(4, "1.2345")]
        [InlineData(8, "1.23456789")]
        [InlineData(3, "12.345")]
        public void CanGetScale(int expected, string value)
        {
            var d = decimal.Parse(value, CultureInfo.InvariantCulture);
            Assert.Equal(expected, d.Scale());
        }

        [Fact]
        public void CanGetScaleOfLiteralWithExponentialNotation()
        {
            Assert.Equal(1, 12.3456e+3m.Scale());
        }

        [Theory]
        [InlineData(1, "0")]
        [InlineData(1, "0.0")]
        [InlineData(1, "1")]
        [InlineData(2, "1.0")]
        [InlineData(2, "0.10")]
        [InlineData(2, "10")]
        [InlineData(6, "123.234")]
        [InlineData(3, "523")]
        [InlineData(4, "523.0")]
        [InlineData(4, "5230")]
        public void CanGetPrecision(int expected, string value)
        {
            var d = decimal.Parse(value, CultureInfo.InvariantCulture);
            Assert.Equal(expected, d.Precision());
        }

        [Fact]
        public void CanGetPrecisionOfMaxValue()
        {
            Assert.Equal(29, decimal.MaxValue.Precision());
        }

        [Fact]
        public void CanCheckStackoverflowQuestion()
        {
            // see <http://stackoverflow.com/questions/763942/calculate-system-decimal-precision-and-scale>
            decimal d;

            d = 0m;
            Assert.Equal(0, d.Scale());
            Assert.Equal(1, d.Precision());

            d = 0.0m;
            Assert.Equal(1, d.Scale());
            Assert.NotEqual(2, d.Precision()); // true in the SO question.
            Assert.Equal(1, d.Precision());

            d = 12.45m;
            Assert.Equal(2, d.Scale());
            Assert.Equal(4, d.Precision());

            d = 12.4500m;
            Assert.Equal(4, d.Scale());
            Assert.Equal(6, d.Precision());

            d = 770m;
            Assert.Equal(0, d.Scale());
            Assert.Equal(3, d.Precision());
        }

        [Theory]
        [InlineData("+5.5", +6, +5, +6, +5, +6, +5, +6, null)]
        [InlineData("+2.5", +3, +2, +3, +2, +3, +2, +2, null)]
        [InlineData("+1.6", +2, +1, +2, +1, +2, +2, +2, null)]
        [InlineData("+1.1", +2, +1, +2, +1, +1, +1, +1, null)]
        [InlineData("+1.0", +1, +1, +1, +1, +1, +1, +1, 1)]
        [InlineData("-1.0", -1, -1, -1, -1, -1, -1, -1, -1)]
        [InlineData("-1.1", -2, -1, -1, -2, -1, -1, -1, null)]
        [InlineData("-1.6", -2, -1, -1, -2, -2, -2, -2, null)]
        [InlineData("-2.5", -3, -2, -2, -3, -3, -2, -2, null)]
        [InlineData("-5.5", -6, -5, -5, -6, -6, -5, -6, null)]
        void CheckRoundingModes(string value, int up, int down, int ceiling, int floor, int halfUp, int halfDown, int halfEven, int? unnecessary)
        {
            var d = decimal.Parse(value, CultureInfo.InvariantCulture);

            Assert.Equal(up, d.Round(RoundingMode.Up));
            Assert.Equal(down, d.Round(RoundingMode.Down));
            Assert.Equal(ceiling, d.Round(RoundingMode.Ceiling));
            Assert.Equal(floor, d.Round(RoundingMode.Floor));
            Assert.Equal(halfUp, d.Round(RoundingMode.HalfUp));
            Assert.Equal(halfDown, d.Round(RoundingMode.HalfDown));
            Assert.Equal(halfEven, d.Round(RoundingMode.HalfEven));
            if (unnecessary.HasValue) {
                Assert.Equal(unnecessary.GetValueOrDefault(), d.Round(RoundingMode.Unnecessary));
            } else {
                Assert.Throws<OverflowException>(() => d.Round(RoundingMode.Unnecessary));
            }
        }

        [Theory]
        [InlineData("0.12345", 2, RoundingMode.Down, "0.12")]
        public void CanRoundAtAGivenScale(string value, int scale, RoundingMode mode, string result)
        {
            var d = decimal.Parse(value, CultureInfo.InvariantCulture);
            var expected = decimal.Parse(result, CultureInfo.InvariantCulture);
            var actual = d.Round(scale, mode);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanPowerUp()
        {
            var expected = 200m;
            var actual = 2m.Times10Pow(2);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanPowerDown()
        {
            var expected = 0.02m;
            var actual = 2m.Times10Pow(-2);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckOverflow()
        {
            Assert.Throws<OverflowException>(() => 123456789m.Times10Pow(28));
        }
    }
}
