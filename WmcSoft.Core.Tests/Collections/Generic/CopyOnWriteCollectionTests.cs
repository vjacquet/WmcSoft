using System.Collections.Generic;
using Xunit;

namespace WmcSoft.Collections.Generic
{
    public class CopyOnWriteCollectionTests
    {
        [Fact]
        public void CanAddToCopyOnWriteCollection()
        {
            var collection = new List<int>() { 1, 2, 3, 4, 5 };
            var cow = new CopyOnWriteCollection<int>(collection);
            cow.Add(6);

            Assert.Equal(5, collection.Count);
            Assert.Equal(6, cow.Count);
        }

        [Fact]
        public void CanRemoveToCopyOnWriteCollection()
        {
            var collection = new List<int>() { 1, 2, 3, 4, 5 };
            var cow = new CopyOnWriteCollection<int>(collection);
            cow.Remove(2);

            Assert.Equal(5, collection.Count);
            Assert.True(collection.Contains(2));
            Assert.Equal(4, cow.Count);
            Assert.False(cow.Contains(2));
        }

        [Fact]
        public void CanClearCopyOnWriteCollection()
        {
            var collection = new List<int>() { 1, 2, 3, 4, 5 };
            var cow = new CopyOnWriteCollection<int>(collection);
            cow.Clear();

            Assert.Equal(5, collection.Count);
            Assert.Equal(0, cow.Count);
        }

        [Fact]
        public void CanCopyFromCopyOnWriteCollection()
        {
            var collection = new List<int>() { 1, 2, 3, 4, 5 };
            var cow = new CopyOnWriteCollection<int>(collection);
            cow.Remove(2);
            cow.Add(6);

            var buffer = new int[5];

            cow.CopyTo(buffer, 0);
            Assert.Equal(buffer, new[] { 1, 3, 4, 5, 6 });

            collection.CopyTo(buffer);
            Assert.Equal(buffer, new[] { 1, 2, 3, 4, 5 });
        }
    }
}
