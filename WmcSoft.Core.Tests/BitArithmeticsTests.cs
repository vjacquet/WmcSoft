using Microsoft.VisualStudio.TestTools.UnitTesting;
using static WmcSoft.BitArithmetics;

namespace WmcSoft
{
    [TestClass]
    public class BitArithmeticsTests
    {
        [TestMethod]
        public void CanTurnOffRightMostOne() {
            uint x = 0x58; // 0101 1000;
            uint y = 0x50; // 0101 0000;
            Assert.AreEqual(y, TurnOffRightMostOne(x));
        }

        [TestMethod]
        public void CanTurnOnRightMostZero() {
            uint x = 0xC7; // 1010 0111;
            uint y = 0xCF; // 1010 1111;
            Assert.AreEqual(y, TurnOnRightMostZero(x));
        }

        [TestMethod]
        public void CanTurnOffTrailingOnes() {
            uint x = 0xC7; // 1010 0111;
            uint y = 0xC0; // 1010 0000;
            Assert.AreEqual(y, TurnOffTrailingOnes(x));
        }

        [TestMethod]
        public void CanTurnOnTrailingZeroes() {
            uint x = 0xC8; // 1010 1000;
            uint y = 0xCF; // 1010 1111;
            Assert.AreEqual(y, TurnOnTrailingZeroes(x));
        }

        [TestMethod]
        public void CanMarkRightMostZero() {
            uint x = 0xC7; // 1010 0111;
            uint y = 0x08; // 0000 1000;
            Assert.AreEqual(y, MarkRightMostZero(x));
        }

        [TestMethod]
        public void CanMarkRightMostOne() {
            uint x = 0xC8; // 1010 1000;
            uint y = 0x08; // 0000 1000;
            Assert.AreEqual(y, MarkRightMostOne(x));
        }

        [TestMethod]
        public void CanCountBits() {
            uint x = 0xC8; // 1010 1000;
            Assert.AreEqual(3, CountBits(x));
        }

        [TestMethod]
        public void CheckBitwiseHammingDistance() {
            //Assert.AreEqual(2, Algorithms.Hamming(0b01011101, 0b01001001));
            Assert.AreEqual(2, Hamming(0x5d, 0x49));
        }

        [TestMethod]
        public void CheckGrayConversions() {
            var a = ToGray(31);
            var b = ToGray(32);

            Assert.AreEqual(31u, FromGray(a));
            Assert.AreEqual(32u, FromGray(b));
            Assert.AreEqual(1, Hamming(a, b));
        }
    }
}
