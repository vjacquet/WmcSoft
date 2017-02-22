using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class RandomBagTests
    {
        [TestMethod]
        public void CheckRandomBagIsCollection() {
            ContractAssert.Collection(new RandomBag<int>(new Random(1164)));
        }

        [TestMethod]
        public void CanAddToRandomBag() {
            var bag = new RandomBag<int>(new Random(1164)) { 1, 2, 3, 4, 5, 6 };
            Assert.AreEqual(6, bag.Count);
            Assert.IsFalse(bag.Contains(0));
            Assert.IsFalse(bag.Contains(8));
            Assert.IsTrue(bag.Contains(2));
        }

        [TestMethod]
        public void CanRemoveFromRandomBag() {
            var bag = new RandomBag<int>(new Random(1164)) { 1, 2, 3, 4, 5, 6 };
            Assert.AreEqual(6, bag.Count);

            Assert.IsTrue(bag.Remove(4));
            Assert.IsFalse(bag.Remove(0));
            Assert.AreEqual(5, bag.Count);
        }

        [TestMethod]
        public void CanRemoveAllFromRandomBag() {
            var bag = new RandomBag<int>(new Random(1164)) { 8, 1, 2, 7, 3, 4, 5, 6 };

            var expected = new[] { 1, 2, 3 };
            var removed = bag.RemoveAll(i => i >= 4);
            var actual = bag.ToArray();
            Assert.AreEqual(5, removed);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void CanEnumerateRandomBag() {
            var bag = new RandomBag<int>(new Random(1164)) { 1, 2, 3, 4, 5, 6 };

            var expected = new[] { 1, 2, 3 };
            var actual = bag.Where(i => i < 4).ToArray();
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void CanAddNullItemsToRandomBag() {
            var bag = new RandomBag<string>(new Random(1164)) { "a", "b", null, "d", "e" };

            Assert.IsTrue(bag.Contains("a"));
            Assert.IsFalse(bag.Contains("c"));
            Assert.IsTrue(bag.Contains(null));
        }

        [TestMethod]
        public void CanPickAllItemsFromRandomBag() {
            var bag = new RandomBag<int>(new Random(1164)) { 1, 2, 3, 4, 5, 6 };

            int count = bag.Count;
            while (count > 0) {
                bag.Pick();
                count--;
            }
            Assert.IsTrue(bag.Count == 0);
        }
    }
}