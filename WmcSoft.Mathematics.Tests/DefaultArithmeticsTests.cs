using Xunit;

namespace WmcSoft.Arithmetics
{
    public class DefaultArithmeticsTests
    {
        [Fact]
        public void CanAddTwoInts()
        {
            var a = Arithmetics<int>.Default;
            Assert.Equal(2, a.Add(1, 1));
        }

        [Fact]
        public void CanDivideTwoDecimals()
        {
            var a = Arithmetics<decimal>.Default;
            Assert.Equal(0.25m, a.Divide(1m, 4m));
        }
    }
}
