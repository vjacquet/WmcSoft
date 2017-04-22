using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Specialized
{
    [TestClass]
    public class BimapTests
    {
        [TestMethod]
        public void CanAddToBimap() {
            var bimap = new Bimap<int, string>() { { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" } };
            Assert.AreEqual(5, bimap.Count);
            Assert.AreEqual("two", bimap[2]);

            bimap.Add(6, "six");
            Assert.AreEqual(6, bimap.Count);
            Assert.AreEqual("six", bimap[6]);
        }

        [TestMethod]
        public void CanRemoveToBimap() {
            var bimap = new Bimap<int, string>() { { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" } };
            Assert.AreEqual(5, bimap.Count);
            Assert.IsTrue(bimap.ContainsKey(2));

            bimap.Remove(2);
            Assert.AreEqual(4, bimap.Count);
            Assert.IsFalse(bimap.ContainsKey(2));
        }

        [TestMethod]
        public void CanClearBimap() {
            var bimap = new Bimap<int, string>() { { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" } };
            Assert.AreEqual(5, bimap.Count);

            bimap.Clear();
            Assert.AreEqual(0, bimap.Count);
        }

        [TestMethod]
        public void CanInverseBimap()
        {
            var bimap = new Bimap<int, string>() { { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" } };
            var pamib = bimap.Inverse();

            foreach (var entry in bimap)
                Assert.AreEqual(entry.Key, pamib[entry.Value]);
        }

        [TestMethod]
        public void CheckBimapInverseIsViewOnBimap()
        {
            var bimap = new Bimap<int, string>() { { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" } };
            var pamib = bimap.Inverse();
            Assert.AreEqual(bimap.Count, pamib.Count);

            bimap.Remove(2);
            Assert.AreEqual(4, bimap.Count);
            Assert.AreEqual(bimap.Count, pamib.Count);

            pamib.Remove("four");
            Assert.AreEqual(3, bimap.Count);
            Assert.AreEqual(bimap.Count, pamib.Count);

            foreach (var entry in bimap)
                Assert.AreEqual(entry.Key, pamib[entry.Value]);
        }

    }
}