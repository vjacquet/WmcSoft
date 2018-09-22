using Xunit;

namespace WmcSoft.Statistics
{
    public class QuantilesTests
    {
        [Fact]
        public void CanComputeQuartiles()
        {
            var q = new Quantiles<int>(3, 6, 7, 8, 8, 10, 13, 15, 16, 20);

            Assert.Equal(7, q[0.25]);
            Assert.Equal(8, q[0.50]);
            Assert.Equal(15, q[0.75]);
        }
    }
}
