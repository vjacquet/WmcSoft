using System;
using Xunit;

namespace WmcSoft
{
    public class BytesFormatterTests
    {
        [Fact]
        public void CanFormatByteWithX2() {
            var formatter = new BytesFormatter();

            var expected = "A0";
            byte b = 0xA0;
            var actual = String.Format(formatter, "{0:X2}", b);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanFormatBytesWithX0() {
            var formatter = new BytesFormatter();

            var expected = "0A0B0C0D";
            var b = new byte[] { 0x0A, 0x0B, 0x0C, 0x0D, };
            var actual = String.Format(formatter, "{0:X0}", b);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanFormatBytesWithX2() {
            var formatter = new BytesFormatter();

            var expected = "0A0B 0C0D";
            var b = new byte[] { 0x0A, 0x0B, 0x0C, 0x0D, };
            var actual = String.Format(formatter, "{0:X2}", b);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanFormatBytesWithB1() {
            var formatter = new BytesFormatter();

            var expected = "00001010 00001011 00001100 00001101";
            var b = new byte[] { 0x0A, 0x0B, 0x0C, 0x0D, };
            var actual = String.Format(formatter, "{0:B1}", b);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanFormatBytesWithB2() {
            var formatter = new BytesFormatter();

            var expected = "0000101000001011 0000110000001101";
            var b = new byte[] { 0x0A, 0x0B, 0x0C, 0x0D, };
            var actual = String.Format(formatter, "{0:B2}", b);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanFormatBytesWithO2() {
            var formatter = new BytesFormatter();

            var expected = "00120013 00140015";
            var b = new byte[] { 0x0A, 0x0B, 0x0C, 0x0D, };
            var actual = String.Format(formatter, "{0:O2}", b);

            Assert.Equal(expected, actual);
        }
    }
}