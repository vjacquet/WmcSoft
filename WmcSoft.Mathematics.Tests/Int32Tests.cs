using Xunit;

namespace WmcSoft
{
    public class Int32Tests
    {
        [Fact]
        public void CheckToInt32()
        {
            var digits = new[] { 1, 2, 3 };
            Assert.Equal(123, digits.ToInt32());
        }
    }
}
