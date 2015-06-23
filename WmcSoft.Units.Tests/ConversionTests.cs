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
            var km = new KnownScaledUnit(SIPrefix.Kilo, m);
            var value = new Quantity(1000, m);
            var expected = new Quantity(1, km);
            var actual = UnitConverter.Convert(value, km);
            Assert.AreEqual(expected, actual);
        }
    }
}
