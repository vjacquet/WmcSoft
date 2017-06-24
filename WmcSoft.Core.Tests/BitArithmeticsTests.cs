using Xunit;
using static WmcSoft.BitArithmetics;

namespace WmcSoft
{
    public class BitArithmeticsTests
    {
        [Fact]
        public void CanTurnOffRightMostOne() {
            uint x = 0x58; // 0101 1000;
            uint y = 0x50; // 0101 0000;
            Assert.Equal(y, TurnOffRightMostOne(x));
        }

        [Fact]
        public void CanTurnOnRightMostZero() {
            uint x = 0xC7; // 1010 0111;
            uint y = 0xCF; // 1010 1111;
            Assert.Equal(y, TurnOnRightMostZero(x));
        }

        [Fact]
        public void CanTurnOffTrailingOnes() {
            uint x = 0xC7; // 1010 0111;
            uint y = 0xC0; // 1010 0000;
            Assert.Equal(y, TurnOffTrailingOnes(x));
        }

        [Fact]
        public void CanTurnOnTrailingZeroes() {
            uint x = 0xC8; // 1010 1000;
            uint y = 0xCF; // 1010 1111;
            Assert.Equal(y, TurnOnTrailingZeroes(x));
        }

        [Fact]
        public void CanMarkRightMostZero() {
            uint x = 0xC7; // 1010 0111;
            uint y = 0x08; // 0000 1000;
            Assert.Equal(y, MarkRightMostZero(x));
        }

        [Fact]
        public void CanMarkRightMostOne() {
            uint x = 0xC8; // 1010 1000;
            uint y = 0x08; // 0000 1000;
            Assert.Equal(y, MarkRightMostOne(x));
        }

        [Fact]
        public void CanCountBits() {
            uint x = 0xC8; // 1010 1000;
            Assert.Equal(3, CountBits(x));
        }

        [Fact]
        public void CheckBitwiseHammingDistance() {
            //Assert.Equal(2, Algorithms.Hamming(0b01011101, 0b01001001));
            Assert.Equal(2, Hamming(0x5d, 0x49));
        }

        [Fact]
        public void CheckGrayConversions() {
            var a = ToGray(31);
            var b = ToGray(32);

            Assert.Equal(31u, FromGray(a));
            Assert.Equal(32u, FromGray(b));
            Assert.Equal(1, Hamming(a, b));
        }
    }
}
