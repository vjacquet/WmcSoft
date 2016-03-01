using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    public abstract class AssertDictionaryContractTests<TKey, TValue>
        where TKey : class
    {
        protected abstract IDictionary<TKey, TValue> CreateDictionary();
        protected abstract IEnumerable<KeyValuePair<TKey, TValue>> GetSamples();

        [TestMethod]
        public void CanConstructListDictionary() {
            var dictionary = CreateDictionary();
            Assert.IsNotNull(dictionary);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void KeyCannotBeNullOnAdd() {
            var dictionary = CreateDictionary();
            dictionary.Add(null, default(TValue));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void KeyCannotBeNullOnRemove() {
            var dictionary = CreateDictionary();
            var sample = GetSamples().ToArray();
            dictionary.Add(sample[0].Key, sample[0].Value);
            dictionary.Remove(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotAddDuplicateKeys() {
            var dictionary = CreateDictionary();
            var samples = GetSamples().ToArray();
            dictionary.Add(samples[0].Key, samples[0].Value);
            dictionary.Add(samples[0].Key, samples[1].Value);
        }

        [TestMethod]
        public void CanRemoveKey() {
            var dictionary = CreateDictionary();
            var samples = GetSamples().ToArray();
            foreach (var sample in samples)
                dictionary.Add(sample);
            Assert.IsTrue(dictionary.Remove(samples[1].Key));
            Assert.AreEqual(samples.Length - 1, dictionary.Count);
        }

        [TestMethod]
        public void CanRemoveItem() {
            var dictionary = CreateDictionary();
            var samples = GetSamples().ToArray();
            foreach (var sample in samples)
                dictionary.Add(sample);
            var item = samples[1];
            Assert.IsTrue(dictionary.Remove(item));
            Assert.AreEqual(samples.Length - 1, dictionary.Count);
        }

        [TestMethod]
        public void CannotRemoveItemWithDifferentValue() {
            var dictionary = CreateDictionary();
            var samples = GetSamples().ToArray();
            foreach (var sample in samples)
                dictionary.Add(sample);
            var item = new KeyValuePair<TKey, TValue>(samples[1].Key, samples[0].Value);
            Assert.IsFalse(dictionary.Remove(item));
            Assert.AreEqual(samples.Length, dictionary.Count);
        }

    }
}
