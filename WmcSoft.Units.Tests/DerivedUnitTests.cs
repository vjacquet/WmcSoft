using Xunit;

namespace WmcSoft.Units
{
    public class DerivedUnitTests
    {
        [Fact]
        public void SpeedSymbol()
        {
            var speed = new DerivedUnit("speed", SI.Meter, SI.Second ^ -1);
            Assert.Equal("ms\u207B\u00B9", speed.Symbol);
        }

        [Fact]
        public void Area()
        {
            var square = SI.Meter ^ 2;
            Assert.Equal("m²", square.Symbol);
        }
    }
}
