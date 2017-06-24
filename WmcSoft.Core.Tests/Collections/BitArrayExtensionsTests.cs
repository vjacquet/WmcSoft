using System;
using System.Collections;
using Xunit;
using WmcSoft.Collections;
using WmcSoft.Collections.Generic;

namespace WmcSoft
{
    public class BitArrayExtensionsTests
    {
        [Fact]
        public void CanApplyMask()
        {
            var x = "__XX__XZ";
            var y = "YY_Y___X";
            var mask = x.ToBitArray(_ => _ != '_');
            var actual = mask.Mask(y, x);
            var expected = "YYXX__XZ";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckValidationOnCount()
        {
            var x = "__XX__XZ";
            var y = "YY_Y__";
            var mask = x.ToBitArray(_ => _ != '_');
            Assert.Throws<ArgumentException>(() => mask.Mask(y, x));
        }

        [Fact]
        public void CanResize()
        {
            var bits = new BitArray(48, true);
            bits.Resize(64, true);
            Assert.Equal(true, bits[48]);
        }

        [Fact]
        public void CanConcat()
        {
            var x = new BitArray(16, false);
            var y = new BitArray(16, true);
            var actual = x.Concat(y);
            Assert.Equal(32, actual.Length);
            var buffer = new int[1];
            actual.CopyTo(buffer, 0);
            Assert.Equal(-65536, buffer[0]);
        }

        [Fact]
        public void CheckCardinalityOnBitArray()
        {
            var bits = new BitArray(36);
            bits[5] = true;
            bits[15] = true;
            bits[25] = true;
            bits[35] = true;
            Assert.Equal(4, bits.Cardinality());
        }

        [Fact]
        public void CheckCardinalityOnBitArrayAfterSetAll()
        {
            var bits = new BitArray(36);
            bits.SetAll(true);
            Assert.Equal(36, bits.Cardinality());
        }
    }
}