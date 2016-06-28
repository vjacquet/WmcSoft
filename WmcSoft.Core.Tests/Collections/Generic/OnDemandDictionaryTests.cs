using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class OnDemandDictionaryTests
    {
        [TestMethod]
        public void GetValueDoesGenerateValue() {
            var dictionary = new OnDemandDictionary<int, string>(x => x.ToString());

            Assert.AreEqual(0, dictionary.Count);

            var value = dictionary[4];
            Assert.AreEqual("4", value);
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual(4, dictionary.Keys.Single());

        }

        [TestMethod]
        public void TryGetValueDoesNotGenerateValue() {
            var dictionary = new OnDemandDictionary<int, string>(x => x.ToString());

            Assert.AreEqual(0, dictionary.Count);

            string value;
            var found = dictionary.TryGetValue(1, out value);
            Assert.IsFalse(found);
            Assert.AreEqual(0, dictionary.Count);
        }
    }
}
