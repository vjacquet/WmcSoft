using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace WmcSoft.Collections.Generic
{
    public abstract class AssertDictionaryContractTests<TKey, TValue>
        where TKey : class
    {
        protected abstract IDictionary<TKey, TValue> CreateDictionary();
        protected abstract IEnumerable<KeyValuePair<TKey, TValue>> GetSamples();

        [Fact]
        public void CanConstructListDictionary()
        {
            var dictionary = CreateDictionary();
            Assert.NotNull(dictionary);
        }

        [Fact]
        public void KeyCannotBeNullOnAdd()
        {
            var dictionary = CreateDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary.Add(null, default));
        }

        [Fact]
        public void KeyCannotBeNullOnRemove()
        {
            var dictionary = CreateDictionary();
            var sample = GetSamples().ToArray();
            dictionary.Add(sample[0].Key, sample[0].Value);
            Assert.Throws<ArgumentNullException>(() => dictionary.Remove(null));
        }

        [Fact]
        public void CannotAddDuplicateKeys()
        {
            var dictionary = CreateDictionary();
            var samples = GetSamples().ToArray();
            Assert.Throws<ArgumentException>(() => {
                dictionary.Add(samples[0].Key, samples[0].Value);
                dictionary.Add(samples[0].Key, samples[1].Value);
            });
        }

        [Fact]
        public void CanRemoveKey()
        {
            var dictionary = CreateDictionary();
            var samples = GetSamples().ToArray();
            foreach (var sample in samples)
                dictionary.Add(sample);
            Assert.True(dictionary.Remove(samples[1].Key));
            Assert.Equal(samples.Length - 1, dictionary.Count);
        }

        [Fact]
        public void CanRemoveItem()
        {
            var dictionary = CreateDictionary();
            var samples = GetSamples().ToArray();
            foreach (var sample in samples)
                dictionary.Add(sample);
            var item = samples[1];
            Assert.True(dictionary.Remove(item));
            Assert.Equal(samples.Length - 1, dictionary.Count);
        }

        [Fact]
        public void CannotRemoveItemWithDifferentValue()
        {
            var dictionary = CreateDictionary();
            var samples = GetSamples().ToArray();
            foreach (var sample in samples)
                dictionary.Add(sample);
            var item = new KeyValuePair<TKey, TValue>(samples[1].Key, samples[0].Value);
            Assert.False(dictionary.Remove(item));
            Assert.Equal(samples.Length, dictionary.Count);
        }
    }
}
