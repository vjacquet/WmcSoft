using System.Linq;
using Xunit;
using WmcSoft.TestTools.UnitTesting;

namespace WmcSoft.Collections.Specialized
{
    public class RingTests
    {
        [Fact]
        public void CheckRingIsCollection()
        {
            ContractAssert.Collection(new Ring<int>(5));
        }

        [Fact]
        public void CanRemoveWhenGapIsAtThenEnd()
        {
            var ring = new Ring<int>(8);
            for (int i = 0; i < 10; i++) {
                ring.Enqueue(i);
            }
            Assert.True(ring.Remove(5));
            var actual = ring.ToArray();
            Assert.Equal(new[] { 2, 3, 4, 6, 7, 8, 9 }, actual);
        }

        [Fact]
        public void CanRemoveWhenGapIsAtThenStart()
        {
            var ring = new Ring<int>(8);
            for (int i = 0; i < 12; i++) {
                ring.Enqueue(i);
            }
            Assert.True(ring.Remove(9));
            var actual = ring.ToArray();
            Assert.Equal(new[] { 4, 5, 6, 7, 8, 10, 11 }, actual);
        }

        [Fact]
        public void CanOverwrite()
        {
            var ring = new Ring<int>(3);
            Assert.Equal(default(int), ring.Enqueue(1));
            Assert.Equal(default(int), ring.Enqueue(2));
            Assert.Equal(default(int), ring.Enqueue(3));
            Assert.Equal(1, ring.Enqueue(4));
            Assert.Equal(3, ring.Count);
            Assert.Equal(2, ring.Dequeue());
            Assert.Equal(3, ring.Dequeue());
            Assert.Equal(4, ring.Dequeue());
            Assert.Equal(0, ring.Count);
        }

        [Fact]
        public void CanUnderwrite()
        {
            var ring = new Ring<int>(5);
            Assert.Equal(default(int), ring.Enqueue(1));
            Assert.Equal(default(int), ring.Enqueue(2));
            Assert.Equal(default(int), ring.Enqueue(3));
            Assert.Equal(3, ring.Count);
            Assert.Equal(1, ring.Dequeue());
            Assert.Equal(2, ring.Dequeue());
            Assert.Equal(3, ring.Dequeue());
            Assert.Equal(0, ring.Count);
        }

        [Fact]
        public void CanIncreaseCapacityWhenFull()
        {
            var ring = new Ring<int>(3);
            Assert.Equal(default(int), ring.Enqueue(1));
            Assert.Equal(default(int), ring.Enqueue(2));
            Assert.Equal(default(int), ring.Enqueue(3));
            Assert.Equal(3, ring.Count);
            ring.Capacity = 4;
            Assert.Equal(3, ring.Count);
            Assert.Equal(1, ring.Dequeue());
            Assert.Equal(2, ring.Dequeue());
            Assert.Equal(3, ring.Dequeue());
            Assert.Equal(0, ring.Count);
        }

        [Fact]
        public void CanDecreaseCapacityWhenFull()
        {
            var ring = new Ring<int>(3);
            Assert.Equal(default(int), ring.Enqueue(1));
            Assert.Equal(default(int), ring.Enqueue(2));
            Assert.Equal(default(int), ring.Enqueue(3));
            Assert.Equal(3, ring.Count);
            ring.Capacity = 2;
            Assert.Equal(2, ring.Count);
            Assert.Equal(1, ring.Dequeue());
            Assert.Equal(2, ring.Dequeue());
            Assert.Equal(0, ring.Count);
        }

        [Fact]
        public void CanEnumerateWhenFull()
        {
            var ring = new Ring<int>(3);
            Assert.Equal(default(int), ring.Enqueue(1));
            Assert.Equal(default(int), ring.Enqueue(2));
            Assert.Equal(default(int), ring.Enqueue(3));
            var i = 0;
            foreach (var value in ring)
                Assert.Equal(++i, value);
            Assert.Equal(i, 3);
        }

        [Fact]
        public void CanEnumerateWhenOverflown()
        {
            var ring = new Ring<int>(3);
            ring.Enqueue(1);
            ring.Enqueue(2);
            ring.Enqueue(3);
            ring.Enqueue(4);
            var i = 1;
            foreach (var value in ring)
                Assert.Equal(++i, value);
        }

        [Fact]
        public void CanEnumerateWhenPartiallyFull()
        {
            var ring = new Ring<int>(3);
            Assert.Equal(default(int), ring.Enqueue(1));
            Assert.Equal(default(int), ring.Enqueue(2));
            var i = 0;
            foreach (var value in ring)
                Assert.Equal(++i, value);
            Assert.Equal(i, 2);
        }

        [Fact]
        public void CanEnumerateWhenEmpty()
        {
            var ring = new Ring<int>(3);
            Assert.Empty(ring);
        }
    }
}
