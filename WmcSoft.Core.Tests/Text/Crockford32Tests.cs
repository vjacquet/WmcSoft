using System;
using System.Linq;
using Xunit;

namespace WmcSoft.Text
{
    public class Crockford32Tests
    {
        [Theory]
        [InlineData("ABCDEFGHJKMNPQRSTVWXYZ", "ABCDEFGHJKMNPQRSTVWXYZ")]
        [InlineData("abcdefghjkmnpqrstvwxyz", "ABCDEFGHJKMNPQRSTVWXYZ")]
        [InlineData("abcde-fghjk-0123", "ABCDEFGHJK0123")]
        [InlineData("abcde-fghjk-0123$", "ABCDEFGHJK0123$")]
        public void CanNormalizeCorrectStrings(string text, string normalized)
        {
            var actual = Crockford32.Normalize(text);
            Assert.Equal(normalized, actual);
        }

        [Theory]
        [InlineData("abc;")]
        [InlineData("abcde-fghjk-0123;")]
        [InlineData("$abc")]
        public void CannotNormalizeStringsWithUnsupportedChars(string text)
        {
            Assert.Throws<ArgumentException>(() => { var _ = Crockford32.Normalize(text); });
        }

        [Fact]
        public void ShouldDecomposeCorrectly()
        {
            var bytes = new byte[] { 0b_0001_1111, 0b_0111_1100, 0b_1111_0000, 0b_1100_0001 };
            var expected = new byte[] { 0b_0001_1111, 0b_0000_0000, 0b_0001_1111, 0b_0000_0000, 0b_0001_1111, 0b_0000_0000, 0b_0000_0011 };
            var actual = Crockford32.Decompose(bytes);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1234, false, 0, "16J")]
        [InlineData(1234, true, 0, "16JD")]
        [InlineData(0, true, 0, "00")]
        [InlineData(123456, false, 2, "3R-J0")]
        [InlineData(123456, false, 3, "3RJ-0")]
        public void CanEncodeInt32(int value, bool checksum, int group, string encoded)
        {
            var d2 = new DateTime(2020, 01, 01);
            var epoch = new DateTime(2000, 01, 01);
            var m = d2.Year * 12 + d2.Month - 1;
            var actual = Crockford32.Encode(value, checksum, group);
            Assert.Equal(encoded, actual);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 2)]
        [InlineData(2, 4)]
        [InlineData(3, 5)]
        [InlineData(4, 7)]
        [InlineData(5, 8)]
        public void DecomposOnByteArrayeShouldYieldAllBytes(int n, int expected)
        {
            var bytes = new byte[n];
            var decomposition = Crockford32.Decompose(bytes).ToList();
            Assert.Equal(expected, decomposition.Count);
        }
    }
}
