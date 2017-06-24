using System;
using Xunit;
using WmcSoft.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    public class SortedSequenceTests
    {
        [Fact]
        public void CheckSortedSequenceCollectionInvariants()
        {
            ContractAssert.Collection(new SortedSequence<int>());
        }

        [Fact]
        public void CanAddToSortedSequence()
        {
            var collection = new SortedSequence<char>();
            collection.AddRange(new[] { 'b', 'a', 'c' });
            var expected = "abc";
            var actual = String.Concat(collection);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddToSortedSequenceWithCollision()
        {
            var collection = new SortedSequence<string>(StringComparer.InvariantCultureIgnoreCase);
            collection.AddRange(new[] { "b", "a", "c", "B" });
            var expected = "abBc";
            var actual = String.Concat(collection);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanRemoveFromSortedSequence()
        {
            var collection = new SortedSequence<char>();
            collection.AddRange(new[] { 'b', 'a', 'c' });
            Assert.True(collection.Remove('a'));
            var expected = "bc";
            var actual = String.Concat(collection);
            Assert.Equal(expected, actual);
            Assert.False(collection.Remove('a'));
        }

    }
}