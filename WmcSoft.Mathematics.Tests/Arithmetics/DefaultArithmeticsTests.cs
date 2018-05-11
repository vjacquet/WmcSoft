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
    }
}
