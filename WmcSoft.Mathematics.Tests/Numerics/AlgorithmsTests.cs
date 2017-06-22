using Xunit;

namespace WmcSoft.Numerics
{
    public class AlgorithmsTests
    {
        [Fact]
        public void CanFindGreatestCommonDivisor()
        {
            var gcd = Algorithms.GreatestCommonDivisor(14, 42, 21);
            Assert.Equal(7, gcd);
        }
    }
}
