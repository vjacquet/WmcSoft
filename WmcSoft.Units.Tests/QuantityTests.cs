using Xunit;

namespace WmcSoft.Units
{
    public class QuantityTests
    {
        [Fact]
        public void CanCreateSpeed()
        {
            Quantity<Meter> m = 1m;
            Quantity<Second> s = 1m;

            var speed = 2 * m / s;
            Assert.Equal(typeof(Quantity), speed.GetType());
            var unit = speed.Metric as DerivedUnit;
            Assert.NotNull(unit);
            Assert.Equal(2, unit.Terms.Length);
            Assert.Equal(typeof(Meter), unit.Terms[0].Unit.GetType());
            Assert.Equal(1, unit.Terms[0].Power);
            Assert.Equal(typeof(Second), unit.Terms[1].Unit.GetType());
            Assert.Equal(-1, unit.Terms[1].Power);
        }
    }
}
