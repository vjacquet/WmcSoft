using System;
using System.Linq;
using Xunit;
using WmcSoft.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    public class BagTests
    {
        [Fact]
        public void CheckBagIsCollection()
        {
            ContractAssert.Collection(new Bag<int>());
        }

        [Fact]
        public void CanAddToBag()
        {
            var bag = new Bag<int>() { 1, 2, 3, 4, 5, 6 };
            Assert.Equal(6, bag.Count);
            Assert.False(bag.Contains(0));
            Assert.False(bag.Contains(8));
            Assert.True(bag.Contains(2));
        }

        [Fact]
        public void CanRemoveFromBag()
        {
            var bag = new Bag<int>() { 1, 2, 3, 4, 5, 6 };
            Assert.Equal(6, bag.Count);

            Assert.True(bag.Remove(4));
            Assert.False(bag.Remove(0));
            Assert.Equal(5, bag.Count);
        }

        [Fact]
        public void CanRemoveAllFromBag()
        {
            var bag = new Bag<int>() { 8, 1, 2, 7, 3, 4, 5, 6 };

            var expected = new[] { 1, 2, 3 };
            var removed = bag.RemoveAll(i => i >= 4);
            var actual = bag.ToArray();
            Array.Sort(actual);

            Assert.Equal(5, removed);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanEnumerateBag()
        {
            var bag = new Bag<int>() { 1, 2, 3, 4, 5, 6 };

            var expected = new[] { 1, 2, 3 };
            var actual = bag.Where(i => i < 4).ToArray();
            Array.Sort(actual);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddNullItemsToBag()
        {
            var bag = new Bag<string> { "a", "b", null, "d", "e" };

            Assert.True(bag.Contains("a"));
            Assert.False(bag.Contains("c"));
            Assert.True(bag.Contains(null));
        }
    }
}