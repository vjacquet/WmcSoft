using System;
using Xunit;

namespace WmcSoft
{
    public class SuidTests
    {
        static bool IsValid(string input)
        {
            Suid dummy;
            return Suid.TryParseExact(input, "S", out dummy);
        }

        [Theory]
        [InlineData("AAAAAAAAAAAAAAAAAAAAA0", true)]
        [InlineData("AaBb0123456789-_AAAAA0", true)]
        [InlineData("AAAAAAàAAAAAAAAAAAAAA0", false)]
        [InlineData("AAA)AAAAAAAAAAAAAAAAA0", false)]
        [InlineData("AAAAAAAA+AAAAAAAAAAAA0", false)]
        public void CanParseStraightSuid(string value, bool isValid)
        {
            Assert.Equal(isValid, IsValid(value));
        }

        [Fact]
        public void CheckGuidEmptyAndSuidEmptyHaveEqualHashCode()
        {
            var expected = Guid.Empty.GetHashCode();
            var actual = Suid.Empty.GetHashCode();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanToString()
        {
            var empty = Suid.Empty;
            var actual = empty.ToString();
            var expected = "0000000000000000000000";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCompareToEmpty()
        {
            var empty = Suid.Empty;

            var a = new Suid(new Guid("{FB6326AE-ED2C-43C3-8E14-A2DD490F6637}"));
            Assert.True(empty.CompareTo(a) < 0);

            var b = new Suid("AAAAAAAAAAAAAAAAAAAAA0");
            Assert.True(empty.CompareTo(b) < 0);

            var c = new Suid("---------------------G");
            Assert.True(empty.CompareTo(c) < 0);
        }

        [Fact]
        public void CanRoundTrip()
        {
            var data = new string[] { "AAAAAAAAAAAAAAAAAAAAA0", "AazZ09-_ko1664OKmNoPq0" };
            foreach (var expected in data) {
                var actual = new Suid(expected).ToString();
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void CanRoundTripOnAllLastByteValue()
        {
            unchecked {
                var bytes = new byte[16];
                for (int i = 0; i < 256; i++) {
                    bytes[15] = (byte)i;
                    var s = new Suid(bytes);
                    var expected = s.ToString();
                    var actual = new Suid(expected).ToString();
                    Assert.Equal(expected, actual);
                }
            }
        }
    }
}
