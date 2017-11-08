using System.Collections.Generic;
using Xunit;

namespace WmcSoft.Collections.Generic
{
    public class CopyOnWriteDictionaryTests
    {
        [Fact]
        public void CanAddToCopyOnWriteDictionary()
        {
            var dictionary = new Dictionary<int, string>() { { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" } };
            var cow = new CopyOnWriteDictionary<int, string>(dictionary);
            cow.Add(6, "six");

            Assert.Equal(5, dictionary.Count);
            Assert.Equal(6, cow.Count);
        }

        [Fact]
        public void CanRemoveToCopyOnWriteDictionary()
        {
            var dictionary = new Dictionary<int, string>() { { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" } };
            var cow = new CopyOnWriteDictionary<int, string>(dictionary);
            cow.Remove(2);

            Assert.Equal(5, dictionary.Count);
            Assert.True(dictionary.ContainsKey(2));
            Assert.Equal(4, cow.Count);
            Assert.False(cow.ContainsKey(2));
        }

        [Fact]
        public void CanClearCopyOnWriteDictionary()
        {
            var dictionary = new Dictionary<int, string>() { { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" } };
            var cow = new CopyOnWriteDictionary<int, string>(dictionary);
            cow.Clear();

            Assert.Equal(5, dictionary.Count);
            Assert.Empty(cow);
        }

        [Fact]
        public void CanSetOrGetFromCopyOnWriteDictionary()
        {
            var dictionary = new Dictionary<int, string>() { { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" } };
            var cow = new CopyOnWriteDictionary<int, string>(dictionary);
            cow[2] = "deux";

            Assert.Equal("deux", cow[2]);
            Assert.Equal("two", dictionary[2]);
        }
    }
}
