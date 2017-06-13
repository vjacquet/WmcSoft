using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class ZipFixedSizeDictionaryTests
    {
        [TestMethod]
        public void CheckContainsKey()
        {
            var keys = new[] { 1, 2, 3, 4, 5 };
            var values = new[] { "one", "two", "three", "four", "five" };
            var dictionary = new ZipFixedSizeDictionary<int, string>(keys, values);

            Assert.IsTrue(dictionary.ContainsKey(1));
            Assert.IsFalse(dictionary.ContainsKey(0));
        }

        [TestMethod]
        public void CheckCountKeysAndValues()
        {
            var keys = new[] { 1, 2, 3, 4, 5 };
            var values = new[] { "one", "two", "three", "four", "five" };
            var dictionary = new ZipFixedSizeDictionary<int, string>(keys, values);

            Assert.AreEqual(5, dictionary.Count);
            CollectionAssert.AreEqual(keys, dictionary.Keys.ToArray());
            CollectionAssert.AreEqual(values, dictionary.Values.ToArray());
        }

        [TestMethod]
        public void CheckTryGetValue()
        {
            var keys = new[] { 1, 2, 3, 4, 5 };
            var values = new[] { "one", "two", "three", "four", "five" };
            var dictionary = new ZipFixedSizeDictionary<int, string>(keys, values);

            string one;
            Assert.IsTrue(dictionary.TryGetValue(1, out one));
            Assert.AreEqual("one", one);

            string zero;
            Assert.IsFalse(dictionary.TryGetValue(0, out zero));
        }

        [TestMethod]
        public void CheckEnumerator()
        {
            var keys = new[] { 1, 2, 3, 4, 5 };
            var values = new[] { "one", "two", "three", "four", "five" };
            var dictionary = new ZipFixedSizeDictionary<int, string>(keys, values);

            var expected = keys.Zip(values, (k, v) => new KeyValuePair<int, string>(k, v)).ToList();
            var actual = dictionary.ToList();
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}