using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class BagSetTests
    {
        [TestMethod]
        public void CheckBagIsCollection() {
            ContractAssert.Collection(new BagSet<int>());
        }

        [TestMethod]
        public void CanAddToBag() {
            var bag = new BagSet<int>() { 1, 2, 3, 4, 5, 6 };
            Assert.AreEqual(6, bag.Count);
            Assert.IsFalse(bag.Contains(0));
            Assert.IsFalse(bag.Contains(8));
            Assert.IsTrue(bag.Contains(2));
        }

        [TestMethod]
        public void CanRemoveFromBag() {
            var bag = new BagSet<int>() { 1, 2, 3, 4, 5, 6 };
            Assert.AreEqual(6, bag.Count);

            Assert.IsTrue(bag.Remove(4));
            Assert.IsFalse(bag.Remove(0));
            Assert.AreEqual(5, bag.Count);
        }

        [TestMethod]
        public void CanEnumerateBag() {
            var bag = new BagSet<int>() { 1, 2, 3, 4, 5, 6 };

            var expected = new[] { 1, 2, 3 };
            var actual = bag.Where(i => i < 4).ToArray();
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void CanAddNullItems() {
            var bag = new BagSet<string> { "a", "b", null, "d", "e" };

            Assert.IsTrue(bag.Contains("a"));
            Assert.IsFalse(bag.Contains("c"));
            Assert.IsTrue(bag.Contains(null));
        }
    }

}