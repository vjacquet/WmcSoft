using WmcSoft.Internals;
using Xunit;

namespace WmcSoft
{
    public class IdentifierTests
    {
        [Theory]
        [InlineData("2", "1", 1)]
        [InlineData("1", "2", -1)]
        [InlineData("1", "1", 0)]
        [InlineData("alpha1", "alpha2", -1)]
        [InlineData("alpha10", "alpha2", -1)] // check the specs.
        [InlineData("12", "1", 1)]
        public void CanCompareParts(string x, string y, int expected)
        {
            var result = IdentifierComparer.Default.Compare(x, y);
            switch (expected) {
            case -1:
                Assert.True(result < 0);
                break;
            case 0:
                Assert.True(result == 0);
                break;
            case 1:
                Assert.True(result > 0);
                break;
            }
        }
    }
}
