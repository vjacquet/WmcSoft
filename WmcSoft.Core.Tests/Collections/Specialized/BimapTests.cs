using System;
using System.Collections.Generic;
using Xunit;

namespace WmcSoft.Collections.Specialized
{
    public class BimapTests
    {
        [Fact]
        public void CanAddToBimap()
        {
            var bimap = new Bimap<int, string>() { { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" } };
            Assert.Equal(5, bimap.Count);
            Assert.Equal("two", bimap[2]);

            bimap.Add(6, "six");
            Assert.Equal(6, bimap.Count);
            Assert.Equal("six", bimap[6]);
        }

        [Fact]
        public void CanRemoveToBimap()
        {
            var bimap = new Bimap<int, string>() { { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" } };
            Assert.Equal(5, bimap.Count);
            Assert.True(bimap.ContainsKey(2));

            bimap.Remove(2);
            Assert.Equal(4, bimap.Count);
            Assert.False(bimap.ContainsKey(2));
        }

        [Fact]
        public void CanClearBimap()
        {
            var bimap = new Bimap<int, string>() { { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" } };
            Assert.Equal(5, bimap.Count);

            bimap.Clear();
            Assert.Empty(bimap);
        }

        [Fact]
        public void CanInverseBimap()
        {
            var bimap = new Bimap<int, string>() { { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" } };
            var pamib = bimap.Inverse();

            foreach (var entry in bimap)
                Assert.Equal(entry.Key, pamib[entry.Value]);
        }

        [Fact]
        public void CheckBimapInverseIsViewOnBimap()
        {
            var bimap = new Bimap<int, string>() { { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" } };
            var pamib = bimap.Inverse();
            Assert.Equal(bimap.Count, pamib.Count);

            bimap.Remove(2);
            Assert.Equal(4, bimap.Count);
            Assert.Equal(bimap.Count, pamib.Count);

            pamib.Remove("four");
            Assert.Equal(3, bimap.Count);
            Assert.Equal(bimap.Count, pamib.Count);

            foreach (var entry in bimap)
                Assert.Equal(entry.Key, pamib[entry.Value]);
        }

        [Fact]
        public void EnsureInvariantAfterAttemtpingToAddTwiceTheSameValue()
        {
            var bimap = new Bimap<string, int>();
            var pamib = bimap.Inverse();

            bimap.Add("one", 1);
            bimap.Add("two", 2);
            Assert.Equal(2, bimap.Count);
            Assert.Equal(2, bimap.Keys.Count);
            Assert.Equal(2, bimap.Values.Count);

            Assert.Throws<ArgumentException>(() => bimap.Add("un", 1));
            Assert.Equal("one", pamib[1]);
            Assert.Equal(2, bimap.Keys.Count);
            Assert.Equal(2, bimap.Values.Count);

            Assert.Throws<ArgumentException>(() => bimap["deux"] = 2);
            Assert.Equal("two", pamib[2]);
            Assert.Equal(2, bimap.Keys.Count);
            Assert.Equal(2, bimap.Values.Count);
        }

        [Fact]
        public void EnsureInvariantOnHomogeneousBimapAfterAttemtpingToAddTwiceTheSameValue()
        {
            var bimap = new Bimap<string>();
            var pamib = bimap.Inverse();

            bimap.Add("one", "1");
            bimap.Add("two", "2");
            Assert.Equal(2, bimap.Count);
            Assert.Equal(2, bimap.Keys.Count);
            Assert.Equal(2, bimap.Values.Count);

            Assert.Throws<ArgumentException>(() => bimap.Add("un", "1"));
            Assert.Equal("one", pamib["1"]);
            Assert.Equal(2, bimap.Keys.Count);
            Assert.Equal(2, bimap.Values.Count);

            Assert.Throws<ArgumentException>(() => bimap["deux"] = "2");
            Assert.Equal("two", pamib["2"]);
            Assert.Equal(2, bimap.Keys.Count);
            Assert.Equal(2, bimap.Values.Count);
        }
    }
}
