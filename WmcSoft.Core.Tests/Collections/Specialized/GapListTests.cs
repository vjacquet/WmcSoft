using System.Linq;
using Xunit;
using WmcSoft.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

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
            var expected = CreateList();
            var actual = CreateListWithGap();
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
            list.Add(7);
            list.Add(6);
            list.Insert(4, 9);
            list.Add(10);
            list.Add(8);

            var actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanRemove()
        {
            var list = CreateListWithGap();
            list.Remove(3);
            list.Remove(10);

            var expected = new[] { 1, 2, 4, 9, 5, 6, 7, 8 };
            var actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCopyToArrayWithGap()
        {
            var expected = CreateList();
            var list = CreateListWithGap();
            var actual = new int[list.Count];
            list.CopyTo(actual, 0);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanEnumerateWithGap()
        {
            var expected = CreateList();
            var list = CreateListWithGap();
            var actual = list.AsEnumerable();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IndexOfReturnsMinusOneOnNotFound()
        {
            var list = CreateListWithGap();
            var found = list.IndexOf(0);
            Assert.Equal(-1, found);
        }

        [Fact]
        public void IndexOfReturnsTheCorrectIndexAfterTheGap()
        {
            var list = CreateListWithGap();
            var found = list.IndexOf(0);
            Assert.Equal(-1, found);
        }

        static List<int> CreateList()
        {
            return Populate(new List<int>(16));
        }

        static GapList<int> CreateListWithGap()
        {
            return Populate(new GapList<int>(16));
        }

        static TList Populate<TList>(TList list)
            where TList : IList<int>
        {
            // |_1|_2|_3|_4|_9|_5|_6|_7|10|_8|
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
            return list;
        }

        [Fact]
        public void EnumerationFailsWhenClearingTheGapList()
        {
            var list = new GapList<int>() { 1, 2, 3, 4, 5, 6 };
            using (var enumerator = list.GetEnumerator()) {
                enumerator.MoveNext();

                list.Clear();

                Assert.Throws<InvalidOperationException>(() => {
                    enumerator.MoveNext();
                });
            }
        }

    }
}
