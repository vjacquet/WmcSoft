using System;
using Xunit;

namespace WmcSoft
{
    public class BytesFormatterTests
    {
        [Fact]
        public void CanFormatByteWithX2()
        {
            var formatter = new BytesFormatter();

            var expected = "A0";
            byte b = 0xA0;
            var actual = string.Format(formatter, "{0:X2}", b);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GenericFormatIsX4()
        {
            var formatter = new BytesFormatter();

            var b = new byte[16];
            for (int i = 0; i < b.Length; i++) {
                b[i] = (byte)i;
            }

            var expected = string.Format(formatter, "{0:X4}", b);
            var actual = string.Format(formatter, "{0}", b);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("G", "0A0B0C0D")]
        [InlineData("X0", "0A0B0C0D")]
        [InlineData("X2", "0A0B 0C0D")]
        [InlineData("B0", "00001010000010110000110000001101")]
        [InlineData("B1", "00001010 00001011 00001100 00001101")]
        [InlineData("B2", "0000101000001011 0000110000001101")]
        [InlineData("O2", "00120013 00140015")]
        public void CanFormatBytes(string format, string expected)
        {
            var formatter = new BytesFormatter();

            var b = new byte[] { 0x0A, 0x0B, 0x0C, 0x0D, };
            var actual = string.Format(formatter, "{0:" + format + "}", b);

            Assert.Equal(expected, actual);
        }
    }
}
