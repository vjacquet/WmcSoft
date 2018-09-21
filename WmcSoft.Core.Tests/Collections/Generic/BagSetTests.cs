using System.Linq;
using Xunit;
using WmcSoft.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    public class BagSetTests
    {
        [Fact]
        public void CheckBagSetIsSet()
        {
            ContractAssert.Set(new BagSet<int>());
        }

        [Fact]
        public void CanAddToBagSet()
        {
            var bag = new BagSet<int>() { 1, 2, 3, 4, 5, 6 };
            Assert.Equal(6, bag.Count);
            Assert.DoesNotContain(0, bag);
            Assert.DoesNotContain(8, bag);
            Assert.Contains(2, bag);
        }

        [Fact]
        public void CanRemoveFromBagSet()
        {
            var bag = new BagSet<int>() { 1, 2, 3, 4, 5, 6 };
            Assert.Equal(6, bag.Count);

            Assert.True(bag.Remove(4));
            Assert.False(bag.Remove(0));
            Assert.Equal(5, bag.Count);
        }

        [Fact]
        public void CanEnumerateBagSet()
        {
            var bag = new BagSet<int>() { 1, 2, 3, 4, 5, 6 };

            var expected = new[] { 1, 2, 3 };
            var actual = bag.Where(i => i < 4).ToArray();
            Assert.True(expected.Equivalent(actual));
        }

        [Fact]
        public void CanAddNullItemsToBagSet()
        {
            var bag = new BagSet<string> { "a", "b", null, "d", "e" };

            Assert.Contains("a", bag);
            Assert.DoesNotContain("c", bag);
            Assert.Contains(null, bag);
        }

        [Fact]
        public void CheckIntersectWith()
        {
            var actual = new BagSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new BagSet<char>();
            other.AddRange('b', 'd');
            actual.IntersectWith(other);

            var expected = new[] { 'b' };
            Assert.Equal(expected, actual.ToArray());
        }

        [Fact]
        public void CheckIntersectWithCollectionWithDuplicates()
        {
            var actual = new BagSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new SortedSequence<char>();
            other.AddRange('b', 'b', 'd');
            actual.IntersectWith(other);

            var expected = new[] { 'b' };
            Assert.Equal(expected, actual.ToArray());
        }

        [Fact]
        public void CheckUnionWith()
        {
            var actual = new BagSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new BagSet<char>();
            other.AddRange('b', 'd');
            actual.UnionWith(other);

            var expected = new[] { 'a', 'b', 'c', 'd' };
            Assert.Equal(expected, actual.ToArray().Sort());
        }

        [Fact]
        public void CheckExceptWith()
        {
            var actual = new BagSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new BagSet<char>();
            other.AddRange('b', 'd');
            actual.ExceptWith(other);

            var expected = new[] { 'a', 'c' };
            Assert.Equal(expected, actual.ToArray().Sort());
        }

        [Fact]
        public void CheckSymmetricExceptWith()
        {
            var actual = new BagSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new BagSet<char>();
            other.AddRange('b', 'd');
            actual.SymmetricExceptWith(other);

            var expected = new[] { 'a', 'c', 'd' };
            Assert.Equal(expected, actual.ToArray().Sort());
        }

        [Fact]
        public void CheckOverlaps()
        {
            var low = new BagSet<int>(Enumerable.Range(1, 4));
            var all = new BagSet<int>(Enumerable.Range(0, 10));

            Assert.True(low.Overlaps(all));
        }

        [Fact]
        public void CheckSubsetAndProperSubsetOnDifferentSets()
        {
            var set1 = new BagSet<int>(Enumerable.Range(1, 4));
            var set2 = new BagSet<int>(Enumerable.Range(0, 10));

            Assert.False(set1.SetEquals(set2));
            Assert.True(set1.IsSubsetOf(set2));
            Assert.True(set1.IsProperSubsetOf(set2));
            Assert.True(set2.IsSupersetOf(set1));
            Assert.True(set2.IsProperSupersetOf(set1));
        }

        [Fact]
        public void CheckSubsetAndProperSubsetOnEquivalentSets()
        {
            var set1 = new BagSet<int>(Enumerable.Range(1, 4));
            var set2 = new BagSet<int>(Enumerable.Range(1, 4));

            Assert.True(set1.SetEquals(set2));
            Assert.True(set1.IsSubsetOf(set2));
            Assert.False(set1.IsProperSubsetOf(set2));
            Assert.True(set2.IsSupersetOf(set1));
            Assert.False(set2.IsProperSupersetOf(set1));
        }

    }
}
