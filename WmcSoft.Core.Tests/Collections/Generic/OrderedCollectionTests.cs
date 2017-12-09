using System;
using Xunit;

namespace WmcSoft.Collections.Generic
{
    public class OrderedCollectionTests
    {
        [Fact]
        public void CanAddElements()
        {
            var collection = new OrderedCollection<int> { 5, 8, 2 };
            Assert.Equal(new[] { 2, 5, 8 }, collection);
        }

        [Fact]
        public void CanRemoveElement()
        {
            var collection = new OrderedCollection<int> { 5, 8, 2 };
            Assert.True(collection.Remove(5));
            Assert.Equal(new[] { 2, 8 }, collection);

            Assert.False(collection.Remove(9));
        }

        [Fact]
        public void CanRemoveMinElement()
        {
            var collection = new OrderedCollection<int> { 5, 8, 2 };
            collection.RemoveMin();
            Assert.Equal(new[] { 5, 8 }, collection);
        }

        [Fact]
        public void CanRemoveMaxElement()
        {
            var collection = new OrderedCollection<int> { 5, 8, 2 };
            collection.RemoveMax();
            Assert.Equal(new[] { 2, 5 }, collection);
        }

        [Theory]
        [InlineData(6, 18, 3)]
        [InlineData(16, 21, 2)]
        [InlineData(10, 14, 0)]
        [InlineData(5, 8, 2)]
        [InlineData(22, 25, 0)]
        [InlineData(-5, 1, 0)]
        public void CanCountBetween(int lo, int hi, int count)
        {
            var collection = new OrderedCollection<int> { 2, 5, 8, 15, 16, 20 };
            Assert.Equal(count, collection.CountBetween(lo, hi));
        }

        [Theory]
        [InlineData(6, 5)]
        [InlineData(21, 20)]
        [InlineData(5, 5)]
        public void CanGetFloor(int value, int floor)
        {
            var collection = new OrderedCollection<int> { 2, 5, 8, 15, 16, 20 };
            Assert.Equal(floor, collection.Floor(value));
        }

        [Fact]
        public void CannotGetFloorWhenLowerThanMin()
        {
            var collection = new OrderedCollection<int> { 2, 5, 8, 15, 16, 20 };
            Assert.Throws<ArgumentOutOfRangeException>(() => collection.Floor(1));
        }

        [Theory]
        [InlineData(9, 15)]
        [InlineData(0, 2)]
        [InlineData(5, 5)]
        public void CanGetCeilling(int value, int ceilling)
        {
            var collection = new OrderedCollection<int> { 2, 5, 8, 15, 16, 20 };
            Assert.Equal(ceilling, collection.Ceiling(value));
        }

        [Fact]
        public void CannotGetCeillingWhenGreaterThanMax()
        {
            var collection = new OrderedCollection<int> { 2, 5, 8, 15, 16, 20 };
            Assert.Throws<ArgumentOutOfRangeException>(() => collection.Ceiling(22));
        }
    }
}
