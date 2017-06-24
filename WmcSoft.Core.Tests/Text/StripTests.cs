using System;
using System.Linq;
using Xunit;

namespace WmcSoft.Text
{
    public class StripTests
    {
        [Fact]
        public void CanCompareIgnoreCase()
        {
            var a = new Strip("abcdef", 1, 3);
            var b = new Strip("aBCdef", 1, 3);
            Assert.Equal(0, Strip.Compare(a, b, true));
        }

        [Fact]
        public void CanCompareWithNull()
        {
            var a = new Strip("abcdef", 1, 3);
            Strip b = null;
            Assert.Equal(1, Strip.Compare(a, b));
        }

        [Fact]
        public void CanTrim()
        {
            var data = new Strip("  abc   ");
            var expected = "abc";
            Assert.Equal<string>(expected, data.Trim());
        }

        [Fact]
        public void CanTrimEmpty()
        {
            var data = new Strip("");
            var expected = "";
            Assert.Equal<string>(expected, data.Trim());
        }

        [Fact]
        public void CanTrimWhitespaces()
        {
            var data = new Strip("    ");
            var expected = "";
            Assert.Equal<string>(expected, data.Trim());
        }

        [Fact]
        public void CanTrimChar()
        {
            var data = new Strip("    abc    ");
            var expected = "abc";
            Assert.Equal<string>(expected, data.Trim(' '));
        }

        [Fact]
        public void CanTrimChars()
        {
            var data = new Strip("  ..abc..  ");
            var expected = "abc";
            Assert.Equal<string>(expected, data.Trim('.', ' '));
        }

        [Fact]
        public void CanTrimStartWhitespaces()
        {
            var data = new Strip("   abc");
            var expected = "abc";
            Assert.Equal<string>(expected, data.TrimStart());
        }

        [Fact]
        public void CanTrimStartChar()
        {
            var data = new Strip("    abc");
            var expected = "abc";
            Assert.Equal<string>(expected, data.TrimStart(' '));
        }

        [Fact]
        public void CanTrimStartChars()
        {
            var data = new Strip("  ..abc");
            var expected = "abc";
            Assert.Equal<string>(expected, data.TrimStart('.', ' '));
        }

        [Fact]
        public void CanTrimEndWhitespaces()
        {
            var data = new Strip("abc   ");
            var expected = "abc";
            Assert.Equal<string>(expected, data.TrimEnd());
        }

        [Fact]
        public void CanTrimEndChar()
        {
            var data = new Strip("abc    ");
            var expected = "abc";
            Assert.Equal<string>(expected, data.TrimEnd(' '));
        }

        [Fact]
        public void CanTrimEndChars()
        {
            var data = new Strip("abc..  ");
            var expected = "abc";
            Assert.Equal<string>(expected, data.TrimEnd('.', ' '));
        }

        [Fact]
        public void CheckStartsWith()
        {
            var data = new Strip("abcdefghijklmnopqrstuvwxyz");
            Assert.True(data.StartsWith("abc"));
        }

        [Fact]
        public void CheckEndsWith()
        {
            var data = new Strip("abcdefghijklmnopqrstuvwxyz");
            Assert.True(data.EndsWith("xyz"));
        }

        [Fact]
        public void CheckIndexOf()
        {
            var data = "...abc...";
            var s = new Strip(data, 3, 3);
            Assert.Equal(1, s.IndexOf('b'));
        }
    }
}
