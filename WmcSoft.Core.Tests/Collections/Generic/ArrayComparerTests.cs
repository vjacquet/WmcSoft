using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class ArrayComparerTests
    {
        [TestMethod]
        public void CheckArrayShapeEqualityComparer() {
            var a = new int[3, 2];
            var b = new int[2, 3];
            var c = new double[3, 2];
            var comparer = ArrayShapeEqualityComparer.Default;

            Assert.IsFalse(comparer.Equals(a, b));
            Assert.IsTrue(comparer.Equals(a, c));
        }

        [TestMethod]
        public void CheckArrayEqualityComparer() {
            var a = new int[3, 2];
            var b = new int[2, 3];
            var c = new int[3, 2];
            var comparer = new ArrayEqualityComparer<int>();

            Assert.IsFalse(comparer.Equals(a, b));
            Assert.IsTrue(comparer.Equals(a, c));
        }

    }
}
