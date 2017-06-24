using System.Linq;
using Xunit;
using WmcSoft.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    public class SequenceSetTests
    {
        [Fact]
        public void CheckSequenceIsSet()
        {
            ContractAssert.Set(new SequenceSet<int>());
        }

        [Fact]
        public void CheckIntersectWith()
        {
            var actual = new SequenceSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new SequenceSet<char>();
            other.AddRange('b', 'd');
            actual.IntersectWith(other);

            var expected = new[] { 'b' };
            Assert.Equal(expected, actual.ToArray());
        }

        [Fact]
        public void CheckIntersectWithCollectionWithDuplicates()
        {
            var actual = new SequenceSet<char>();
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
            var actual = new SequenceSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new SequenceSet<char>();
            other.AddRange('b', 'd');
            actual.UnionWith(other);

            var expected = new[] { 'a', 'b', 'c', 'd' };
            Assert.Equal(expected, actual.ToArray());
        }

        [Fact]
        public void CheckExceptWith()
        {
            var actual = new SequenceSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new SequenceSet<char>();
            other.AddRange('b', 'd');
            actual.ExceptWith(other);

            var expected = new[] { 'a', 'c' };
            Assert.Equal(expected, actual.ToArray());
        }

        [Fact]
        public void CheckSymmetricExceptWith()
        {
            var actual = new SequenceSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new SequenceSet<char>();
            other.AddRange('b', 'd');
            actual.SymmetricExceptWith(other);

            var expected = new[] { 'a', 'c', 'd' };
            Assert.Equal(expected, actual.ToArray());
        }

        [Fact]
        public void CheckOverlaps()
        {
            var low = new SequenceSet<int>(Enumerable.Range(1, 4));
            var all = new SequenceSet<int>(Enumerable.Range(0, 10));

            Assert.True(low.Overlaps(all));
        }

        [Fact]
        public void CheckSubsetAndProperSubsetOnDifferentSets()
        {
            var set1 = new SequenceSet<int>(Enumerable.Range(1, 4));
            var set2 = new SequenceSet<int>(Enumerable.Range(0, 10));

            Assert.False(set1.SetEquals(set2));
            Assert.True(set1.IsSubsetOf(set2));
            Assert.True(set1.IsProperSubsetOf(set2));
            Assert.True(set2.IsSupersetOf(set1));
            Assert.True(set2.IsProperSupersetOf(set1));
        }

        [Fact]
        public void CheckSubsetAndProperSubsetOnEquivalentSets()
        {
            var set1 = new SequenceSet<int>(Enumerable.Range(1, 4));
            var set2 = new SequenceSet<int>(Enumerable.Range(1, 4));

            Assert.True(set1.SetEquals(set2));
            Assert.True(set1.IsSubsetOf(set2));
            Assert.False(set1.IsProperSubsetOf(set2));
            Assert.True(set2.IsSupersetOf(set1));
            Assert.False(set2.IsProperSupersetOf(set1));
        }
    }
}
