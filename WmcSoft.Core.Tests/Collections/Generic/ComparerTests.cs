using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class ComparerTests
    {
        [TestMethod]
        public void CheckSelectCompare() {
            var expected = new[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            var comparer = new SelectComparer<int, int>(x => -x);
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Array.Sort(data, comparer);
            CollectionAssert.AreEquivalent(expected, data);
        }

        [TestMethod]
        public void CheckCustomSortCompare() {
            var comparer = new CustomSortComparer<int>(2, 4, 8);
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 4 };
            Array.Sort(data, comparer);
            Assert.AreEqual(2, data[0]);
            Assert.AreEqual(4, data[1]);
            Assert.AreEqual(4, data[2]);
            Assert.AreEqual(8, data[3]);
        }
    }
}
