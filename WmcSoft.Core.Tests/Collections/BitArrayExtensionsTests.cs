using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Collections;
using WmcSoft.Collections.Generic;

namespace WmcSoft
{
    [TestClass]
    public class BitArrayExtensionsTests
    {
        [TestMethod]
        public void CanApplyMask() {
            var x = "__XX__XZ";
            var y = "YY_Y___X";
            var mask = x.ToBitArray(_ => _ != '_');
            var actual = mask.Mask(y, x);
            var expected = "YYXX__XZ";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckValidationOnCount() {
            var x = "__XX__XZ";
            var y = "YY_Y__";
            var mask = x.ToBitArray(_ => _ != '_');
            var actual = mask.Mask(y, x);
        }

        [TestMethod]
        public void CanResize() {
            var bits = new BitArray(48, true);
            bits.Resize(64, true);
            Assert.AreEqual(true, bits[48]);
        }

        [TestMethod]
        public void CanConcat() {
            var x = new BitArray(16, false);
            var y = new BitArray(16, true);
            var actual = x.Concat(y);
            Assert.AreEqual(32, actual.Length);
            var buffer = new int[1];
            actual.CopyTo(buffer, 0);
            Assert.AreEqual(-65536, buffer[0]);
        }
    }
}
