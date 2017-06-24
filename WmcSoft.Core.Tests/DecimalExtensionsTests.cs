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

        [Fact]
        public void CheckRoundingModes()
        {
            // UP DOWN  CEILING FLOOR HALF_UP HALF_DOWN HALF_EVEN UNNECESSARY
            CheckRounding(+5.5m, +6, +5, +6, +5, +6, +5, +6, null);
            CheckRounding(+2.5m, +3, +2, +3, +2, +3, +2, +2, null);
            CheckRounding(+1.6m, +2, +1, +2, +1, +2, +2, +2, null);
            CheckRounding(+1.1m, +2, +1, +2, +1, +1, +1, +1, null);
            CheckRounding(+1.0m, +1, +1, +1, +1, +1, +1, +1, 1);
            CheckRounding(-1.0m, -1, -1, -1, -1, -1, -1, -1, -1);
            CheckRounding(-1.1m, -2, -1, -1, -2, -1, -1, -1, null);
            CheckRounding(-1.6m, -2, -1, -1, -2, -2, -2, -2, null);
            CheckRounding(-2.5m, -3, -2, -2, -3, -3, -2, -2, null);
            CheckRounding(-5.5m, -6, -5, -5, -6, -6, -5, -6, null);
        }

        static void CheckRounding(decimal value, int up, int down, int ceiling, int floor, int halfUp, int halfDown, int halfEven, int? unnecessary)
        {
            Assert.Equal(up, value.Round(RoundingMode.Up));
            Assert.Equal(down, value.Round(RoundingMode.Down));
            Assert.Equal(ceiling, value.Round(RoundingMode.Ceiling));
            Assert.Equal(floor, value.Round(RoundingMode.Floor));
            Assert.Equal(halfUp, value.Round(RoundingMode.HalfUp));
            Assert.Equal(halfDown, value.Round(RoundingMode.HalfDown));
            Assert.Equal(halfEven, value.Round(RoundingMode.HalfEven));
            if (unnecessary.HasValue) {
                Assert.Equal(unnecessary.GetValueOrDefault(), value.Round(RoundingMode.Unnecessary));
            } else {
                try {
                    Assert.Equal(unnecessary.GetValueOrDefault(), value.Round(RoundingMode.Unnecessary));
                    Assert.True(false, "Inconclusive");
                } catch (OverflowException) {
                } catch (Exception) {
                    Assert.True(false, "Inconclusive");
                }
            }
        }

        [Fact]
        public void CanPowerUp()
        {
            var expected = 200m;
            var actual = 2m.Pow10(2);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanPowerDown()
        {
            var expected = 0.02m;
            var actual = 2m.Pow10(-2);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckOverflow()
        {
            Assert.Throws<OverflowException>(() => 123456789m.Pow10(28));
        }
    }
}