using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace WmcSoft.Collections.Generic
{
    public class ZipFixedSizeDictionaryTests
    {
        [Fact]
        public void CheckContainsKey()
        {
            var keys = new[] { 1, 2, 3, 4, 5 };
            var values = new[] { "one", "two", "three", "four", "five" };
            var dictionary = new ZipFixedSizeDictionary<int, string>(keys, values);

            Assert.True(dictionary.ContainsKey(1));
            Assert.False(dictionary.ContainsKey(0));
        }

        [Fact]
        public void CheckCountKeysAndValues()
        {
            var keys = new[] { 1, 2, 3, 4, 5 };
            var values = new[] { "one", "two", "three", "four", "five" };
            var dictionary = new ZipFixedSizeDictionary<int, string>(keys, values);

            Assert.Equal(5, dictionary.Count);
            Assert.Equal(keys, dictionary.Keys.ToArray());
            Assert.Equal(values, dictionary.Values.ToArray());
        }

        [Fact]
        public void CheckTryGetValue()
        {
            var keys = new[] { 1, 2, 3, 4, 5 };
            var values = new[] { "one", "two", "three", "four", "five" };
            var dictionary = new ZipFixedSizeDictionary<int, string>(keys, values);

            string one;
            Assert.True(dictionary.TryGetValue(1, out one));
            Assert.Equal("one", one);

            string zero;
            Assert.False(dictionary.TryGetValue(0, out zero));
        }

        [Fact]
        public void CheckEnumerator()
        {
            var keys = new[] { 1, 2, 3, 4, 5 };
            var values = new[] { "one", "two", "three", "four", "five" };
            var dictionary = new ZipFixedSizeDictionary<int, string>(keys, values);

            var expected = keys.Zip(values, (k, v) => new KeyValuePair<int, string>(k, v)).ToList();
            var actual = dictionary.ToList();
            Assert.Equal(expected, actual);
        }
    }
}