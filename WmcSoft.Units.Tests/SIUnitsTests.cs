using Xunit;

namespace WmcSoft.Units
{
    public class SIUnitsTests
    {
        [Fact]
        public void CanGetKilometer()
        {
            var m = SI.Meter;
            var km = m.WithPrefix(SIPrefix.Kilo);
            Assert.Equal("km", km.Symbol);
        }

        [Fact]
        public void ConvertGetGram()
        {
            var kg = SI.Kilogram;
            var g = kg.WithPrefix(SIPrefix.None);
            Assert.Equal("g", g.Symbol);
        }
    }
}
