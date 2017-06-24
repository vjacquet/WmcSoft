using System.Linq;
using Xunit;
using WmcSoft.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    public class SortedSequenceSetTests
    {
        [Fact]
        public void CheckSortedSequenceIsSet()
        {
            ContractAssert.Set(new SortedSequenceSet<int>());
        }

        [Fact]
        public void CheckIntersectWith()
        {
            var actual = new SortedSequenceSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new SortedSequenceSet<char>();
            other.AddRange('b', 'd');
            actual.IntersectWith(other);

            var expected = new[] { 'b' };
            Assert.Equal(expected, actual.ToArray());
        }

        [Fact]
        public void CheckIntersectWithCollectionWithDuplicates()
        {
            var actual = new SortedSequenceSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new SortedSequence<char>();
            other.AddRange('b', 'b', 'd');
            actual.IntersectWith(other);

            var expected = new[] { 'b' };
            Assert.Equal(expected, actual.ToArray());
        }

        [Fact]
        public void CheckUnionWithAnotherSet()
        {
            var actual = new SortedSequenceSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new SortedSequenceSet<char>();
            other.AddRange('b', 'd');
            actual.UnionWith(other);

            var expected = new[] { 'a', 'b', 'c', 'd' };
            Assert.Equal(expected, actual.ToArray());
        }

        [Fact]
        public void CheckUnionWithAnEnumerable()
        {
            var actual = new SortedSequenceSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new[] { 'b', 'd' };
            actual.UnionWith(other);

            var expected = new[] { 'a', 'b', 'c', 'd' };
            Assert.Equal(expected, actual.ToArray());
        }

        [Fact]
        public void CheckExceptWithAnotherSet()
        {
            var actual = new SortedSequenceSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new SortedSequenceSet<char>();
            other.AddRange('b', 'd');
            actual.ExceptWith(other);

            var expected = new[] { 'a', 'c' };
            Assert.Equal(expected, actual.ToArray());
        }

        [Fact]
        public void CheckExceptWithAnEnumerable()
        {
            var actual = new SortedSequenceSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new[] { 'b', 'd' };
            actual.ExceptWith(other);

            var expected = new[] { 'a', 'c' };
            Assert.Equal(expected, actual.ToArray());
        }


        [Fact]
        public void CheckSymmetricExceptWithAnotherSet()
        {
            var actual = new SortedSequenceSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new SortedSequenceSet<char>();
            other.AddRange('b', 'd');
            actual.SymmetricExceptWith(other);

            var expected = new[] { 'a', 'c', 'd' };
            Assert.Equal(expected, actual.ToArray());
        }

        [Fact]
        public void CheckSymmetricExceptWithAnEnumerable()
        {
            var actual = new SortedSequenceSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new[] { 'b', 'd' };
            actual.SymmetricExceptWith(other);

            var expected = new[] { 'a', 'c', 'd' };
            Assert.Equal(expected, actual.ToArray());
        }

        [Fact]
        public void CheckOverlaps()
        {
            var low = new SortedSequenceSet<int>(Enumerable.Range(1, 4));
            var all = new SortedSequenceSet<int>(Enumerable.Range(0, 10));

            Assert.True(low.Overlaps(all));
        }

        [Fact]
        public void CheckSubsetAndProperSubsetOnDifferentSets()
        {
            var set1 = new SortedSequenceSet<int>(Enumerable.Range(1, 4));
            var set2 = new SortedSequenceSet<int>(Enumerable.Range(0, 10));

            Assert.False(set1.SetEquals(set2));
            Assert.True(set1.IsSubsetOf(set2));
            Assert.True(set1.IsProperSubsetOf(set2));
            Assert.True(set2.IsSupersetOf(set1));
            Assert.True(set2.IsProperSupersetOf(set1));
        }

        [Fact]
        public void CheckSubsetAndProperSubsetOnEquivalentSets()
        {
            var set1 = new SortedSequenceSet<int>(Enumerable.Range(1, 4));
            var set2 = new SortedSequenceSet<int>(Enumerable.Range(1, 4));

            Assert.True(set1.SetEquals(set2));
            Assert.True(set1.IsSubsetOf(set2));
            Assert.False(set1.IsProperSubsetOf(set2));
            Assert.True(set2.IsSupersetOf(set1));
            Assert.False(set2.IsProperSupersetOf(set1));
        }
    }
}
