using Xunit;

namespace WmcSoft.Units
{
    public class ConversionTests
    {
        [Fact]
        public void FahrenheitAndCelsiusEqualPoint()
        {
            var expected = new Quantity(-40, SI.Celsius);
            var value = new Quantity(-40, ImperialSystemOfUnit.Fahrenheit);
            var actual = UnitConverter.Convert(value, SI.Celsius);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertYardToMeter()
        {
            var expected = new Quantity(0.9144m, SI.Meter);
            var value = new Quantity(1, ImperialSystemOfUnit.Yard);
            var actual = UnitConverter.Convert(value, SI.Meter);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertMeterToKilometer()
        {
            var m = SI.Meter;
            var km = m.WithPrefix(SIPrefix.Kilo);
            var value = new Quantity(1000, m);
            var expected = new Quantity(1, km);
            var actual = UnitConverter.Convert(value, km);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertKilogramToGram()
        {
            var kg = SI.Kilogram;
            var g = kg.WithPrefix(SIPrefix.None);
            var value = new Quantity(1, kg);
            var expected = new Quantity(1000, g);
            var actual = UnitConverter.Convert(value, g);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCreateCompositeConversion()
        {
            var yd2ft = new LinearConversion(ImperialSystemOfUnit.Yard, ImperialSystemOfUnit.Foot, 3m);
            var ft2in = new LinearConversion(ImperialSystemOfUnit.Foot, ImperialSystemOfUnit.Inch, 12m);
            var composite = UnitConverter.Compose(yd2ft, ft2in);

            Assert.Equal(composite.Source, ImperialSystemOfUnit.Yard);
            Assert.Equal(composite.Target, ImperialSystemOfUnit.Inch);
            Assert.IsType<LinearConversion>(composite);
            Assert.Equal(36m, composite.Convert(1m));
        }
    }
}
