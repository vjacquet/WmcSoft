using System.Linq;
using Xunit;

namespace WmcSoft.Text
{
    public class TokenizerTests
    {
        [Fact]
        public void CanTokenizeString()
        {
            var expected = new[] { "a", "b", "c" };
            var actual = "a b c".Tokenize().ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanTokenizeStringWithEmptyTokens()
        {
            var expected = new[] { "a", "b", "c" };
            var actual = " a  b c ".Tokenize().ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanTokenizeOnChar()
        {
            var expected = new[] { "a", "b", "c" };
            var actual = " a  b c ".Tokenize(' ').ToArray();
            Assert.Equal(expected, actual);
        }
    }
}
