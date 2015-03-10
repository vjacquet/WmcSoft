using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Units.Tests
{
    [TestClass]
    public class ConversionTests
    {
        [TestMethod]
        public void FahrenheitAndCelsiusEqualPoint() {
            Quantity expected = new Quantity(-40, SI.Celsius);
            Quantity value = new Quantity(-40, ImperialSystemOfUnit.Fahrenheit);

            Assert.AreEqual(expected, UnitConverter.Convert(value, SI.Celsius));
        }
    }
}
