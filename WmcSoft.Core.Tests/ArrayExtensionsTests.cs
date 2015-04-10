using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Collections.Generic;

namespace WmcSoft.Tests
{
    [TestClass]
    public class ArrayExtensionsTests
    {
        [TestMethod]
        public void CheckGetColumn() {
            var array = new[,] {
               { 1, 2, 3},
               { 4, 5, 6},
            };
            var expected = new[] { 2, 5 };
            var actual = array.GetColumn(1).ToArray();
            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void CheckGetRow() {
            var array = new[,] {
               { 1, 2, 3},
               { 4, 5, 6},
            };
            var expected = new[] { 4, 5, 6 };
            var actual = array.GetRow(1).ToArray();
            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void CheckStructuralEquals() {
            var expected = new[] { 1, 2, 3 };
            var actual = new[] { 1, 2, 3 };
            Assert.IsTrue(expected.SequenceEqual(actual));
        }
    }
}
