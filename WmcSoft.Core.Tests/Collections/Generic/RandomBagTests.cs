using System;
using System.Linq;
using Xunit;
using WmcSoft.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    public class RandomBagTests
    {
        [Fact]
        public void CheckRandomBagIsCollection()
        {
            ContractAssert.Collection(new RandomBag<int>(new Random(1164)));
        }

        [Fact]
        public void CanAddToRandomBag()
        {
            var bag = new RandomBag<int>(new Random(1164)) { 1, 2, 3, 4, 5, 6 };
            Assert.Equal(6, bag.Count);
            Assert.False(bag.Contains(0));
            Assert.False(bag.Contains(8));
            Assert.True(bag.Contains(2));
        }

        [Fact]
        public void CanRemoveFromRandomBag()
        {
            var bag = new RandomBag<int>(new Random(1164)) { 1, 2, 3, 4, 5, 6 };
            Assert.Equal(6, bag.Count);

            Assert.True(bag.Remove(4));
            Assert.False(bag.Remove(0));
            Assert.Equal(5, bag.Count);
        }

        [Fact]
        public void CanRemoveAllFromRandomBag()
        {
            var bag = new RandomBag<int>(new Random(1164)) { 8, 1, 2, 7, 3, 4, 5, 6 };

            var expected = new[] { 1, 2, 3 };
            var removed = bag.RemoveAll(i => i >= 4);
            var actual = bag.ToArray();
            Array.Sort(actual);
            Assert.Equal(5, removed);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanEnumerateRandomBag()
        {
            var bag = new RandomBag<int>(new Random(1164)) { 1, 2, 3, 4, 5, 6 };

            var expected = new[] { 1, 2, 3 };
            var actual = bag.Where(i => i < 4).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddNullItemsToRandomBag()
        {
            var bag = new RandomBag<string>(new Random(1164)) { "a", "b", null, "d", "e" };

            Assert.True(bag.Contains("a"));
            Assert.False(bag.Contains("c"));
            Assert.True(bag.Contains(null));
        }

        [Fact]
        public void CanPickAllItemsFromRandomBag()
        {
            var bag = new RandomBag<int>(new Random(1164)) { 1, 2, 3, 4, 5, 6 };

            int count = bag.Count;
            while (count > 0) {
                bag.Pick();
                count--;
            }
            Assert.True(bag.Count == 0);
        }
    }
}