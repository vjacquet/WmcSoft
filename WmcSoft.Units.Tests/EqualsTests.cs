using Xunit;

namespace WmcSoft.Units
{
    public class EqualsTests
    {
        [Fact]
        public void IdenticalUnitsAreEqual()
        {
            var expected = new DerivedUnit("speed", "km/h", SI.Meter, SI.Second ^ -1);
            var actual = new DerivedUnit("speed", "km/h", SI.Meter, SI.Second ^ -1);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCompareUnitsUsingEqualityOperator()
        {
            var expected = new DerivedUnit("speed", "km/h", SI.Meter, SI.Second ^ -1);
            var actual = new DerivedUnit("speed", "km/h", SI.Meter, SI.Second ^ -1);
            Assert.True(expected == actual);
        }

        [Fact]
        public void CanCompareQuantitiesUsingLessThanOperator()
        {
            Quantity q1 = new Quantity(5, SI.Meter);
            Quantity q2 = new Quantity(10, SI.Meter);
            Assert.True(q1 < q2);
        }

        [Fact]
        public void CanCompareQuantitiesUsingEqualityOperator()
        {
            Quantity q1 = new Quantity(5, SI.Meter);
            Quantity q2 = new Quantity(5, SI.Meter);
            Assert.True(q1 == q2);
            Assert.False(null == q2);
            Assert.False(q1 == null);
        }
    }
}
