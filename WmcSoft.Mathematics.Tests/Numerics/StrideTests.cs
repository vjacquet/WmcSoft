using Xunit;

namespace WmcSoft.Numerics
{
    public class StrideTests
    {
        [Fact]
        public void CanCopyToWithStrideNotOne()
        {
            var data = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var stride = new Band<int>(data, 1, 3, 2);
            var expected = new int[] { 1, 3, 5 };
            var actual = new int[3];
            stride.CopyTo(actual, 0);
            Assert.Equal(expected, actual);
        }
    }
}
