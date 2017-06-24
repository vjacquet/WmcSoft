using System.Linq;
using Xunit;

namespace WmcSoft.Collections.Generic
{
    public class IndexTests
    {
        [Fact]
        public void CanAddAndRemoveToIndex()
        {
            var index = new Index<int, string>();
            index.Add(1, "one");
            index.Add(1, "un");
            Assert.Equal(2, index.Count);

            index.Add(2, "two");

            Assert.Equal(3, index.Count);
            Assert.Equal("one un", index[1].JoinWith(' '));

            index.Remove(1, "one");
            Assert.Equal(2, index.Count);
            Assert.Equal("un", index[1].JoinWith(' '));

            index.Remove(1, "un");
            Assert.Equal("", index[1].JoinWith(' '));
        }

        [Fact]
        public void CanManipulateAsLookup()
        {
            var index = new Index<int, string> {
                { 1, "one" },
                { 1, "un" },
                { 2, "two" }
            };

            var lookup = index.AsLookup();
            Assert.Equal(3, lookup.Count);
            Assert.True(lookup.Contains(2));
            Assert.Equal(new[] { "one", "un" }, lookup[1].ToArray());
            Assert.Equal(new[] { "two" }, lookup[2].ToArray());
            Assert.Equal(new string[0], lookup[0].ToArray());
            var groups = lookup.ToList();

            Assert.Equal(1, groups[0].Key);
        }
    }
}
