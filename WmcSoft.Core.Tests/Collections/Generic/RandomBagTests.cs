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
            Assert.DoesNotContain(0, bag);
            Assert.DoesNotContain(8, bag);
            Assert.Contains(2, bag);
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

            Assert.Contains("a", bag);
            Assert.DoesNotContain("c", bag);
            Assert.Contains(null, bag);
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
