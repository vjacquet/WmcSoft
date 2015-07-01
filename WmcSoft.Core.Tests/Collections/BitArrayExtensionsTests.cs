using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
