using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Drawing
{
    [TestClass]
    public class HSLColorTests
    {
        [TestMethod]
        public void CanConvertHSL2RGB() {
            var expected = Color.FromArgb(32, 64, 128);
            var hsl = new HSLColor(expected);
            var actual = (Color)hsl;
            //Assert.AreEqual(expected, actual);
            Assert.IsTrue(Math.Abs(expected.R - actual.R) <= 1);
            Assert.IsTrue(Math.Abs(expected.G - actual.G) <= 1);
            Assert.IsTrue(Math.Abs(expected.B - actual.B) <= 1);
        }
    }
}
