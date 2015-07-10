using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Units
{
    [TestClass]
    public class ConversionTests
    {
        [TestMethod]
        public void FahrenheitAndCelsiusEqualPoint() {
            var expected = new Quantity(-40, SI.Celsius);
            var value = new Quantity(-40, ImperialSystemOfUnit.Fahrenheit);
            var actual = UnitConverter.Convert(value, SI.Celsius);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertYardToMeter() {
            var expected = new Quantity(0.9144m, SI.Meter);
            var value = new Quantity(1, ImperialSystemOfUnit.Yard);
            var actual = UnitConverter.Convert(value, SI.Meter);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertMeterToKilometer() {
            var m = SI.Meter;
            var km = m.WithPrefix(SIPrefix.Kilo);
            var value = new Quantity(1000, m);
            var expected = new Quantity(1, km);
            var actual = UnitConverter.Convert(value, km);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertKilogramToGram() {
            var kg = SI.Kilogram;
            var g = kg.WithPrefix(SIPrefix.None);
            var value = new Quantity(1, kg);
            var expected = new Quantity(1000, g);
            var actual = UnitConverter.Convert(value, g);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanCreateCompositeConversion() {
            var yd2ft = new LinearConversion(ImperialSystemOfUnit.Yard, ImperialSystemOfUnit.Foot, 3m);
            var ft2in = new LinearConversion(ImperialSystemOfUnit.Foot, ImperialSystemOfUnit.Inch, 12m);
            var composite = UnitConverter.Compose(yd2ft, ft2in);

            Assert.AreEqual(composite.Source, ImperialSystemOfUnit.Yard);
            Assert.AreEqual(composite.Target, ImperialSystemOfUnit.Inch);
            Assert.IsInstanceOfType(composite, typeof(LinearConversion));
            Assert.AreEqual(36m, composite.Convert(1m));
        }
    }
}
