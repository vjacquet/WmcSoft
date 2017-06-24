using System.Linq;
using Xunit;
using WmcSoft.TestTools.UnitTesting;

namespace WmcSoft.Collections.Specialized
{
    public class GapListTests
    {
        [Fact]
        public void CheckGapListIsCollection()
        {
            ContractAssert.Collection(new GapList<int>(10));
        }

        [Fact]
        public void CanInsert()
        {
            var list = new GapList<int>(10);
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);
            list.Add(6);
            list.Insert(4, 9);
            list.Add(10);
            list.Insert(7, 7);
            list.Add(8);
            var expected = new[] { 1, 2, 3, 4, 9, 10, 5, 7, 8, 6 };
            var actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanInsertWithoutMoving()
        {
            var list = new GapList<int>(5);
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);
            list.Add(6);
            list.Insert(4, 9);
            list.Add(10);
            list.Insert(6, 7);
            list.Add(8);
            var expected = list.ToArray();

            list = new GapList<int>(5);
            list.Clear();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);
            list.Add(6);
            list.Insert(4, 9);
            list.Add(10);
            list.Add(7);
            list.Add(8);

            var actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanRemove()
        {
            var list = new GapList<int>(5);
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);
            list.Add(6);
            list.Insert(4, 9);
            list.Add(10);
            list.Insert(7, 7);
            list.Add(8);
            list.Remove(3);
            list.Remove(10);

            var expected = new[] { 1, 2, 4, 9, 5, 7, 8, 6 };
            var actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCopyToArrayWithGap()
        {
            var list = new GapList<int>(16);
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);
            list.Add(6);
            list.Insert(4, 9);
            list.Add(10);
            list.Insert(7, 7);
            list.Add(8);
            var expected = new[] { 1, 2, 3, 4, 9, 10, 5, 7, 8, 6 };
            var actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanEnumerateWithGap()
        {
            var list = new GapList<int>(16);
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);
            list.Add(6);
            list.Insert(4, 9);
            list.Add(10);
            list.Insert(7, 7);
            list.Add(8);
            var expected = new[] { 1, 2, 3, 4, 9, 10, 5, 7, 8, 6 };
            var actual = list.Select(i => i).ToArray();
            Assert.Equal(expected, actual);
        }
    }
}
