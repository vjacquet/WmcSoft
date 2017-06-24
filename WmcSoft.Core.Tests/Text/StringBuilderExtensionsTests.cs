using System.Text;
using Xunit;

namespace WmcSoft.Text
{
    public class StringBuilderExtensionsTests
    {
        [Fact]
        public void CanPrependNullString()
        {
            string prefix = null;
            var sb = new StringBuilder("abc");
            sb.Prepend(prefix);
            Assert.Equal("abc", sb.ToString());
        }

        [Theory]
        [InlineData("abc", "a", "c")]
        [InlineData("bc", null, "c")]
        [InlineData("ab", "a", null)]
        [InlineData("b", null, null)]
        public void CheckSurroundWith(string expected, string prefix, string suffix)
        {
            Assert.Equal(expected, new StringBuilder("b").SurroundWith(prefix, suffix).ToString());
        }
    }
}