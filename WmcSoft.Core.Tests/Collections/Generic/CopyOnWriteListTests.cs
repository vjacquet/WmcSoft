using System.Collections.Generic;
using Xunit;

namespace WmcSoft.Collections.Generic
{
    public class CopyOnWriteListTests
    {
        [Fact]
        public void CanAddToCopyOnWriteList()
        {
            var list = new List<int>() { 1, 2, 3, 4, 5 };
            var cow = new CopyOnWriteList<int>(list);
            cow.Add(6);

            Assert.Equal(5, list.Count);
            Assert.Equal(6, cow.Count);
        }

        [Fact]
        public void CanRemoveToCopyOnWriteList()
        {
            var list = new List<int>() { 1, 2, 3, 4, 5 };
            var cow = new CopyOnWriteList<int>(list);
            cow.Remove(2);

            Assert.Equal(5, list.Count);
            Assert.Contains(2, list);
            Assert.Equal(4, cow.Count);
            Assert.DoesNotContain(2, cow);
        }

        [Fact]
        public void CanRemoveAtToCopyOnWriteList()
        {
            var list = new List<int>() { 1, 2, 3, 4, 5 };
            var cow = new CopyOnWriteList<int>(list);
            cow.RemoveAt(1);

            Assert.Equal(5, list.Count);
            Assert.Contains(2, list);
            Assert.Equal(4, cow.Count);
            Assert.DoesNotContain(2, cow);
        }

        [Fact]
        public void CanClearCopyOnWriteList()
        {
            var list = new List<int>() { 1, 2, 3, 4, 5 };
            var cow = new CopyOnWriteList<int>(list);
            cow.Clear();

            Assert.Equal(5, list.Count);
            Assert.Empty(cow);
        }

        [Fact]
        public void CanCopyFromCopyOnWriteList()
        {
            var list = new List<int>() { 1, 2, 3, 4, 5 };
            var cow = new CopyOnWriteList<int>(list);
            cow.Remove(2);
            cow.Add(6);

            var buffer = new int[5];

            cow.CopyTo(buffer, 0);
            Assert.Equal(buffer, new[] { 1, 3, 4, 5, 6 });

            list.CopyTo(buffer);
            Assert.Equal(buffer, new[] { 1, 2, 3, 4, 5 });
        }

        [Fact]
        public void CanSetOrGetFromCopyOnWriteList()
        {
            var list = new List<int>() { 1, 2, 3, 4, 5 };
            var cow = new CopyOnWriteList<int>(list);
            cow[2] = 6;

            Assert.Equal(6, cow[2]);
            Assert.Equal(3, list[2]);
        }
    }
}
