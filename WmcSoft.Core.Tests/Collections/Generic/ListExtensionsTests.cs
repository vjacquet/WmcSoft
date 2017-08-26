using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace WmcSoft.Collections.Generic
{
    public class ListExtensionsTests
    {
        [Theory]
        [InlineData(0, new[] { 0, 1, 2, 3 })]
        [InlineData(3, new[] { 3, 4, 5 })]
        [InlineData(-1, new[] { 3, 5, 4 })]
        [InlineData(7, new[] { 7, 8, 9 })]
        [InlineData(-1, new[] { 7, 8, 9, 10 })]
        public void CheckIndexOfList(int expected, int[] sequence)
        {
            var data = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Assert.Equal(expected, data.IndexOf(sequence));
        }

        [Theory]
        [InlineData(-1, 0, 3, 3)]
        [InlineData(-1, 2, 3, 3)]
        [InlineData(3, 3, 3, 3)]
        [InlineData(4, 4, 3, 3)]
        [InlineData(5, 5, 3, 3)]
        [InlineData(-1, 6, 3, 3)]
        [InlineData(-1, 9, 3, 3)]
        public void CheckIndexOfPartial(int expected, int value, int startIndex, int count)
        {
            var data = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Assert.Equal(expected, data.IndexOf(value, startIndex, count));
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(-1, 2)]
        [InlineData(0, 3)]
        [InlineData(1, 4)]
        [InlineData(2, 5)]
        [InlineData(-1, 6)]
        [InlineData(-1, 9)]
        public void CheckSublistIndexOf(int expected, int item)
        {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var data = list.Sublist(3, 3);
            Assert.Equal(expected, data.IndexOf(item));
        }

        [Fact]
        public void CheckSublistToArray()
        {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var data = list.Sublist(3, 3);
            var actual = data.ToArray();
            var expected = new[] { 3, 4, 5 };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddInsideTheSublist()
        {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var data = list.Sublist(3, 3);
            data.Add(10);
            Assert.Equal(4, data.Count);

            var expected = new[] { 0, 1, 2, 3, 4, 5, 10, 6, 7, 8, 9 };
            var actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanInsertInsideTheSublist()
        {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var data = list.Sublist(3, 3);
            data.Insert(1, 10);
            Assert.Equal(4, data.Count);

            var expected = new[] { 0, 1, 2, 3, 10, 4, 5, 6, 7, 8, 9 };
            var actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanRemoveAtInsideTheSublist()
        {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var data = list.Sublist(3, 3);
            data.RemoveAt(1);
            Assert.Equal(2, data.Count);

            var expected = new[] { 0, 1, 2, 3, 5, 6, 7, 8, 9 };
            var actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanRemoveInsideTheSublist()
        {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var data = list.Sublist(3, 3);
            Assert.True(data.Remove(4));
            Assert.Equal(2, data.Count);

            var expected = new[] { 0, 1, 2, 3, 5, 6, 7, 8, 9 };
            var actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanRemoveAfterTheSublist()
        {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var data = list.Sublist(3, 3);
            Assert.False(data.Remove(8));
            Assert.True(list.Remove(8));
            Assert.Equal(3, data.Count);

            var expected = new[] { 3, 4, 5 };
            var actual = data.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanRemoveBeforeTheSublist()
        {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var data = list.Sublist(3, 3);
            Assert.False(data.Remove(1));
            Assert.True(list.Remove(1));
            Assert.Equal(3, data.Count);

            var expected = new[] { 4, 5, 6 };
            var actual = data.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanClearTheSublist()
        {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var data = list.Sublist(3, 3);
            data.Clear();
            Assert.Equal(0, data.Count);

            var expected = new[] { 0, 1, 2, 6, 7, 8, 9 };
            var actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanRemoveMoreThanTheSublistCapacity()
        {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, };
            var data = list.Sublist(3, 3);
            list.RemoveAt(0);
            list.RemoveAt(0);
            Assert.Equal(2, data.Count);

            var expected = new[] { 5, 6 };
            var actual = data.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckRepeat()
        {
            var data = new[] { 1, 2, 3 };
            var expected = new[] { 1, 2, 3, 1, 2, 3, 1, 2, 3 };
            var actual = data.Repeat(3);
            Assert.Equal(9, actual.Count);
            Assert.Equal(3, actual[5]);
            Assert.Equal(expected, actual.ToArray());
        }

        [Fact]
        public void CanFindRotationPoint()
        {
            var list = new[] { 1, 2, 3, 4, 5, 6 };
            var target = new[] { 3, 4, 5, 6, 1, 2 };

            Assert.Equal(4, list.FindRotationPoint(target));

            list.Rotate(4);
            Assert.Equal(target, list);
        }

        #region Slice

        [Fact]
        public void CanSliceArray()
        {
            var fruits = new[] { "Banana", "Orange", "Lemon", "Apple", "Mango" };
            var citrus = fruits.Slice(1, 3);
            Assert.Equal(fruits.Skip(1).Take(2), citrus);
        }

        #endregion

        #region Splice

        [Fact]
        public void CanRemove0ElementsFromIndex2AndInsert1Element()
        {
            var myFish = new List<string> { "angel", "clown", "mandarin", "sturgeon" };
            var removed = myFish.Splice(2, 0, "drum");

            Assert.Equal(myFish, new[] { "angel", "clown", "drum", "mandarin", "sturgeon" });
            Assert.Equal(removed, new string[0]);
        }

        [Fact]
        public void CanRemove1ElementFromIndex3()
        {
            var myFish = new List<string> { "angel", "clown", "drum", "mandarin", "sturgeon" };
            var removed = myFish.Splice(3, 1);

            Assert.Equal(myFish, new[] { "angel", "clown", "drum", "sturgeon" });
            Assert.Equal(removed, new[] { "mandarin" });
        }

        [Fact]
        public void CanRemove1ElementFromIndex2AndInsert1Element()
        {
            var myFish = new List<string> { "angel", "clown", "drum", "sturgeon" };
            var removed = myFish.Splice(2, 1, "trumpet");

            Assert.Equal(myFish, new[] { "angel", "clown", "trumpet", "sturgeon" });
            Assert.Equal(removed, new[] { "drum" });
        }

        [Fact]
        public void CanRemove2ElementsFromIndex0AndInsert3Elements()
        {
            var myFish = new List<string> { "angel", "clown", "trumpet", "sturgeon" };
            var removed = myFish.Splice(0, 2, "parrot", "anemone", "blue");

            Assert.Equal(myFish, new[] { "parrot", "anemone", "blue", "trumpet", "sturgeon" });
            Assert.Equal(removed, new[] { "angel", "clown" });
        }

        [Fact]
        public void CanRemove2ElementsFromIndex2()
        {
            var myFish = new List<string> { "parrot", "anemone", "blue", "trumpet", "sturgeon" };
            var removed = myFish.Splice(myFish.Count - 3, 2);

            Assert.Equal(myFish, new[] { "parrot", "anemone", "sturgeon" });
            Assert.Equal(removed, new[] { "blue", "trumpet" });
        }

        [Fact]
        public void CanRemove1ElementFromIndexMinus2()
        {
            var myFish = new List<string> { "angel", "clown", "mandarin", "sturgeon" };
            var removed = myFish.Splice(-2, 1);

            Assert.Equal(myFish, new[] { "angel", "clown", "sturgeon" });
            Assert.Equal(removed, new[] { "mandarin" });
        }

        [Fact]
        public void CanRemoveAllElementsAfterIndex2()
        {
            var myFish = new List<string> { "angel", "clown", "mandarin", "sturgeon" };
            var removed = myFish.Splice(2);

            Assert.Equal(myFish, new[] { "angel", "clown" });
            Assert.Equal(removed, new[] { "mandarin", "sturgeon" });
        }

        #endregion
    }
}
