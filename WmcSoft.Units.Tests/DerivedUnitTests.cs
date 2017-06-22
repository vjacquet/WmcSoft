using Xunit;

namespace WmcSoft.Units
{
    public class DerivedUnitTests
    {
        [Fact]
        public void SpeedSymbol()
        {
            var speed = new DerivedUnit("speed", SI.Meter, SI.Second ^ -1);
            Assert.Equal(speed.Symbol, "ms\u207B\u00B9");
        }

        [Fact]
        public void Area()
        {
            var square = SI.Meter ^ 2;
            Assert.Equal(square.Symbol, "m²");
        }
    }
}
