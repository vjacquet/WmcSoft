using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class CopyOnWriteDictionaryTests
    {
        [TestMethod]
        public void CanAddToCopyOnWriteDictionary() {
            var dictionary = new Dictionary<int, string>() { { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" } };
            var cow = new CopyOnWriteDictionary<int, string>(dictionary);
            cow.Add(6, "six");

            Assert.AreEqual(5, dictionary.Count);
            Assert.AreEqual(6, cow.Count);
        }

        [TestMethod]
        public void CanRemoveToCopyOnWriteDictionary() {
            var dictionary = new Dictionary<int, string>() { { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" } };
            var cow = new CopyOnWriteDictionary<int, string>(dictionary);
            cow.Remove(2);

            Assert.AreEqual(5, dictionary.Count);
            Assert.IsTrue(dictionary.ContainsKey(2));
            Assert.AreEqual(4, cow.Count);
            Assert.IsFalse(cow.ContainsKey(2));
        }

        [TestMethod]
        public void CanClearCopyOnWriteDictionary() {
            var dictionary = new Dictionary<int, string>() { { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" } };
            var cow = new CopyOnWriteDictionary<int, string>(dictionary);
            cow.Clear();

            Assert.AreEqual(5, dictionary.Count);
            Assert.AreEqual(0, cow.Count);
        }

        [TestMethod]
        public void CanSetOrGetFromCopyOnWriteDictionary() {
            var dictionary = new Dictionary<int, string>() { { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" } };
            var cow = new CopyOnWriteDictionary<int, string>(dictionary);
            cow[2] = "deux";

            Assert.AreEqual("deux", cow[2]);
            Assert.AreEqual("two", dictionary[2]);
        }
    }
}