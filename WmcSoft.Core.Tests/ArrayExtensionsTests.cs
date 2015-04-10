using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var expected = new [] {2, 5};
            var actual = array.GetColumn(1).ToArray();
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void CheckGetRow() {
            var array = new[,] {
               { 1, 2, 3},
               { 4, 5, 6},
            };
            var expected = new[] { 4, 5, 6 };
            var actual = array.GetRow(1).ToArray();
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}
