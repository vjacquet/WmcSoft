using System.Globalization;
using Xunit;

namespace WmcSoft
{
    public class LongitudeTests
    {
        [Fact]
        public void CanToStringLongitude()
        {
            var iv = CultureInfo.InvariantCulture;
            var l = new Longitude(49, 30, 15);

            Assert.Equal("49° 30' 15\"", l.ToString(iv));
            Assert.Equal("49° 30.25'", l.ToString("M", iv));
            Assert.Equal("49.5042°", l.ToString("D", iv));
        }

        [Fact]
        public void CanToStringLatitudeWithNegativeValues()
        {
            var iv = CultureInfo.InvariantCulture;
            var l = new Latitude(-49, 30);

            Assert.Equal("-49° 30' 00\"", l.ToString(iv));
            Assert.Equal("-49° 30.00'", l.ToString("M", iv));
            Assert.Equal("-49.5000°", l.ToString("D", iv));
        }

        [Fact]
        public void CanDeconstructLongitude()
        {
            var l = new Longitude(10, 15, 30);
            var (d, m, s) = l;

            Assert.Equal(10, d);
            Assert.Equal(15, m);
            Assert.Equal(30, s);
        }

        [Fact]
        public void CanDeconstructLatitude()
        {
            var l = new Latitude(-10, 15, 30);
            var (d, m, s) = l;

            Assert.Equal(-10, d);
            Assert.Equal(15, m);
            Assert.Equal(30, s);
        }
    }
}
