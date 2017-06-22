using System;
using System.Drawing;
using Xunit;

namespace WmcSoft.Drawing
{
    public class HSLColorTests
    {
        [Fact]
        public void CanConvertHSL2RGB()
        {
            var expected = Color.FromArgb(32, 64, 128);
            var hsl = new HSLColor(expected);
            var actual = (Color)hsl;
            //Assert.Equal(expected, actual);
            Assert.True(Math.Abs(expected.R - actual.R) <= 1);
            Assert.True(Math.Abs(expected.G - actual.G) <= 1);
            Assert.True(Math.Abs(expected.B - actual.B) <= 1);
        }
    }
}
