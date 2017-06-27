﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace WmcSoft.Collections.Generic
{
    public class CollectionExtensionsTests
    {
        [Fact]
        public void CheckToArray()
        {
            var list = new List<Tuple<int, string>> {
                Tuple.Create(1, "a"),
                Tuple.Create(2, "b"),
                Tuple.Create(3, "c"),
            };
            var expected = new[] { 1, 2, 3 };
            var actual = list.ToArray(i => i.Item1);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(4, 1)]
        [InlineData(5, ~2)]
        [InlineData(9, ~4)]
        [InlineData(1, ~0)]
        public void CheckBinarySearch(int value, int result)
        {
            var list = new List<Tuple<int, string>> {
                Tuple.Create(2, "a"),
                Tuple.Create(4, "b"),
                Tuple.Create(6, "c"),
                Tuple.Create(8, "d"),
            };

            Assert.Equal(result, list.BinarySearch(t => Comparer.DefaultInvariant.Compare(t.Item1, value)));
        }

        [Fact]
        public void CheckBinarySearchOnEmptyCollection()
        {
            var list = new List<Tuple<int, string>> {
            };

            Assert.Equal(0, ~list.BinarySearch(t => Comparer.DefaultInvariant.Compare(t.Item1, 1)));
        }

        [Fact]
        public void CheckToTwoDimensionalArray()
        {
            var list = new List<Tuple<int, int, int>>(2);
            list.Add(Tuple.Create(1, 2, 3));
            list.Add(Tuple.Create(4, 5, 6));

            var expected = new[,] {
               { 1, 2, 3},
               { 4, 5, 6},
            };

            var comparer = new ArrayEqualityComparer<int>();
            var actual = list.ToTwoDimensionalArray(i => i.Item1, i => i.Item2, i => i.Item3);

            Assert.True(comparer.Equals(expected, actual));
        }

        [Fact]
        public void CheckNotEqualsOnCell()
        {
            var x = new[, ,] {
               { {11, 12, 13}, {14, 15, 16} },
               { {21, 22, 23}, {24, 25, 26} },
               { {31, 32, 33}, {34, 35, 36} },
               { {41, 42, 43}, {44, 45, 46} },
            };
            var y = new[, ,] {
               { {11, 12, 13}, {14, 15, 16} },
               { {21, 22, 23}, {24, 25, 26} },
               { {31, 32, 33}, {99, 35, 36} },
               { {41, 42, 43}, {44, 45, 46} },
            };

            var comparer = new ArrayEqualityComparer<int>();
            Assert.False(comparer.Equals(x, y));
        }

        [Fact]
        public void CheckNotEqualsOnRank()
        {
            var x = new[, ,] {
               { {11, 12, 13}, {14, 15, 16} },
               { {21, 22, 23}, {24, 25, 26} },
               { {31, 32, 33}, {34, 35, 36} },
               { {41, 42, 43}, {44, 45, 46} },
            };
            var y = new[,] {
                {11, 12, 13},
                {21, 22, 23},
                {31, 32, 33},
                {41, 42, 43},
            };

            var comparer = new ArrayEqualityComparer<int>();
            Assert.False(comparer.Equals(x, y));
        }

        [Fact]
        public void CheckNotEqualsOnDimensions()
        {
            var x = new[, ,] {
               { {11, 12, 13}, {14, 15, 16} },
               { {21, 22, 23}, {24, 25, 26} },
               { {31, 32, 33}, {34, 35, 36} },
               { {41, 42, 43}, {44, 45, 46} },
            };
            var y = new[, ,] {
               { {11, 12, 13}, {14, 15, 16} },
               { {21, 22, 23}, {24, 25, 26} },
               { {31, 32, 33}, {34, 35, 36} },
            };

            var comparer = new ArrayEqualityComparer<int>();
            Assert.False(comparer.Equals(x, y));
        }

        [Fact]
        public void CheckSortOnIndexedCollection()
        {
            var array = new[] { "a", "c", "e", "d", "f", "b" };
            var comparer = new SourceComparer<string>(array, StringComparer.InvariantCulture);
            var actual = new[] { 0, 1, 2, 3, 4, 5 };
            Array.Sort(actual, comparer);
            var expected = new[] { 0, 5, 1, 3, 2, 4 };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckSortedCombine()
        {
            var array = "aabbbc";
            var expected = "abc";
            var actual = new String(array.SortedCombine((char c, char g) => c).ToArray());
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckBackwards()
        {
            var sequence = new[] { 1, 2, 3, 4 };
            var actual = sequence.Backwards().ToArray();
            var expected = new[] { 4, 3, 2, 1 };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckConvertAll()
        {
            var sequence = new[] { 1, 2, 3, 4 };
            IList<int> list = new List<int>(sequence);
            var actual = list.ConvertAll(x => x * x);
            Assert.IsAssignableFrom<IList<int>>(actual);
        }

        [Fact]
        public void CheckRemoveIfOnList()
        {
            var sequence = new List<int> { 1, 2, 3, 4, 5 };
            var count = sequence.RemoveIf(i => i % 2 == 1);
            Assert.Equal(3, count);
            var expected = new[] { 2, 4 };
            Assert.Equal(expected, sequence);
        }

        [Fact]
        public void CheckRemoveIfOnCollection()
        {
            var sequence = new SortedSet<int> { 1, 2, 3, 4, 5 };
            var count = sequence.RemoveIf(i => i % 2 == 1);
            Assert.Equal(3, count);
            var expected = new[] { 2, 4 };
            Assert.Equal(expected, sequence);
        }

        [Fact]
        public void CheckRemoveIfOnLatestOnList()
        {
            var sequence = new List<int> { 1, 2, 3, 4, 5 };
            var count = sequence.RemoveIf(i => i == 5);
            Assert.Equal(1, count);
            var expected = new[] { 1, 2, 3, 4 };
            Assert.Equal(expected, sequence);
        }

        [Fact]
        public void CheckRemoveIfOnLatestOnCollection()
        {
            var sequence = new SortedSet<int> { 1, 2, 3, 4, 5 };
            var count = sequence.RemoveIf(i => i == 5);
            Assert.Equal(1, count);
            var expected = new[] { 1, 2, 3, 4 };
            Assert.Equal(expected, sequence);
        }

        [Fact]
        public void CheckIndexedRemoveIfOnLatest()
        {
            var sequence = new List<int> { 1, 2, 3, 4, 5 };
            var count = sequence.RemoveIf((x, i) => i == 4);
            Assert.Equal(1, count);
            var expected = new[] { 1, 2, 3, 4 };
            Assert.Equal(expected, sequence);
        }

        static Func<T, int> Find<T>(T value) where T : IComparable<T>
        {
            return x => -value.CompareTo(x);
        }

        [Fact]
        public void CheckBinaryFind()
        {
            var sequence = new[] { 1, 3, 5, 7 };
            var expected = 5;
            Assert.Equal(expected, sequence.BinaryFind(Find(5)));
        }

        [Fact]
        public void CheckLowerBound()
        {
            var sequence = new[] { 1, 3, 5, 7 };
            Assert.Equal(3, sequence.LowerBound(Find(4)));
            Assert.Equal(5, sequence.LowerBound(Find(5)));
            Assert.Equal(7, sequence.LowerBound(Find(9)));
            Assert.Equal(0, sequence.LowerBound(Find(-1)));
        }

        [Fact]
        public void CheckUpperBound()
        {
            var sequence = new[] { 1, 3, 5, 7 };
            Assert.Equal(5, sequence.UpperBound(Find(4)));
            Assert.Equal(5, sequence.UpperBound(Find(5)));
            Assert.Equal(0, sequence.UpperBound(Find(9)));
            Assert.Equal(1, sequence.UpperBound(Find(-1)));
        }

        [Fact]
        public void CheckBound()
        {
            var sequence = new[] { 1, 3, 5, 7 };
            Assert.Equal(Tuple.Create(3, 5), sequence.Bounds(Find(4)));
            Assert.Equal(Tuple.Create(5, 5), sequence.Bounds(Find(5)));
            Assert.Equal(Tuple.Create(7, 0), sequence.Bounds(Find(9)));
            Assert.Equal(Tuple.Create(0, 1), sequence.Bounds(Find(-1)));
        }

        [Fact]
        public void CheckInterpolatedSearch()
        {
            var sequence = new[] { 1, 2, 3, 5, 7, 9, 11, 12, 23, 42, 51 };
            Assert.Equal(4, sequence.InterpolatedSearch(7, new Int32Ordinal()));
        }

        [Fact]
        public void CheckMinElement()
        {
            var sequence = new[] { "dog", "cat", "bird", "lion" };
            Assert.Equal(2, sequence.MinElement());
        }

        [Fact]
        public void CheckMaxElement()
        {
            var sequence = new[] { "dog", "cat", "bird", "lion" };
            Assert.Equal(3, sequence.MaxElement());
        }

        [Fact]
        public void CheckMinMaxElement()
        {
            var sequence = new[] { "dog", "cat", "bird", "lion" };
            var actual = sequence.MinMaxElement();
            Assert.Equal(2, actual.Item1);
            Assert.Equal(3, actual.Item2);
        }

        [Fact]
        public void CanPop()
        {
            var actual = new List<string> { "one", "two", "three" };
            var expected = new[] { "one", "two" };
            Assert.Equal("three", actual.Pop());
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanPopAtIndex()
        {
            var actual = new List<string> { "one", "two", "three" };
            var expected = new[] { "one", "three" };
            Assert.Equal("two", actual.Pop(1));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CannotPopWhenIndexIsOutOfBound()
        {
            var actual = new List<string> { "one", "two", "three" };
            Assert.Throws<ArgumentOutOfRangeException>(() => actual.Pop(4));
        }

        [Fact]
        public void CanToggle()
        {
            var actual = new List<string> { "one", "two", "three" };
            Assert.True(actual.Toggle("four"));
            Assert.False(actual.Toggle("one"));
            var expected = new[] { "two", "three", "four" };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckElementsAt()
        {
            var data = new List<string> { "zero", "one", "two", "three", "four" };
            var expected = new[] { "three", "one", "two" };
            var actual = data.ElementsAt(3, 1, 2).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckElementsAtOnEnumerable()
        {
            var data = new List<string> { "zero", "one", "two", "three", "four" };
            var expected = new[] { "three", "one", "two" };
            var actual = data.Select(x => x).ElementsAt(3, 1, 2).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckElementsAtOrDefaultOnEnumerable()
        {
            var data = new List<string> { "zero", "one", "two", "three" };
            var expected = new[] { "three", "one", null, "two" };
            var actual = data.Select(x => x).ElementsAtOrDefault(3, 1, 4, 2).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckIsSorted()
        {
            var data = new[] { 5, 1, 2, 3, 0 };
            Assert.True(data.IsSorted(1, 3));
            Assert.False(data.IsSorted(1, 4));
        }

        [Fact]
        public void CanPartition()
        {
            var data = new[] { 1, 2, 3, 4, 6, 7, 8, 9 };
            Predicate<int> odd = e => e % 2 == 1;
            var p = data.Partition(odd);
            Assert.Equal(4, p);
            int i;
            for (i = 0; i < p; i++)
                Assert.False(odd(data[i]));
            for (; i < data.Length; ++i)
                Assert.True(odd(data[i]));
        }

        [Fact]
        public void CannotFindPartitionPointWhenNotPartitioned()
        {
            var data = new[] { 1, 2, 3, 4, 6, 7, 8, 9 };
            Predicate<int> odd = e => e % 2 == 1;
            var actual = data.FindPartitionPoint(odd);

            Assert.Equal(-1, actual);
        }

        [Fact]
        public void CanFindPartitionPointWhenPartitioned()
        {
            var data = new[] { 2, 4, 6, 7, 8, 1, 3, 9 };
            Predicate<int> odd = e => e % 2 == 1;
            var expected = data.Partition(odd);
            var actual = data.FindPartitionPoint(odd);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanStablePartition()
        {
            var data = new[] { 2, 1, 3, 4, 6, 7, 8, 9, 11 };
            Predicate<int> odd = e => e % 2 == 1;
            var p = data.StablePartition(odd);
            Assert.Equal(4, p);
            int i;
            for (i = 0; i < p; i++)
                Assert.False(odd(data[i]));
            data.IsSorted(0, p);
            for (; i < data.Length; ++i)
                Assert.True(odd(data[i]));
            data.IsSorted(p, data.Length - p);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(4, 3)]
        [InlineData(5, 4)]
        [InlineData(10, 8)]
        public void CheckBinaryRank(int value, int rank)
        {
            var data = new[] { 1, 2, 3, 4, 6, 7, 8, 9 };
            Assert.Equal(rank, data.BinaryRank(value));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(4, 3)]
        [InlineData(5, 4)]
        [InlineData(10, 8)]
        public void CheckPartialBinaryRank(int value, int rank)
        {
            var data = new[] { -5, -1,  1, 2, 3, 4, 6, 7, 8, 9, 20, 21 };
            Assert.Equal(rank, data.BinaryRank(2, 8, value));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(4, 3)]
        [InlineData(5, 4)]
        [InlineData(10, 8)]
        public void CheckBinaryRankWithFinder(int value, int rank)
        {
            var data = new[] { 1, 2, 3, 4, 6, 7, 8, 9 };
            Assert.Equal(rank, data.BinaryRank(x => Comparer<int>.Default.Compare(x, value)));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(4, 3)]
        [InlineData(5, 4)]
        [InlineData(10, 8)]
        public void CheckPartialBinaryRankWithFinder(int value, int rank)
        {
            var data = new[] { -5, -1, 1, 2, 3, 4, 6, 7, 8, 9, 20, 21 };
            Assert.Equal(rank, data.BinaryRank(2, 8, x => Comparer<int>.Default.Compare(x, value)));
        }

        [Fact]
        public void CheckPerfectInShuffle()
        {
            var data = "AGINORSTABEELMPX".ToCharArray();
            data.PerfectInShuffle();
            var actual = new string(data);
            var expected = "AABGEIENLOMRPSXT";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckPerfectInUnshuffle()
        {
            var data = "AABGEIENLOMRPSXT".ToCharArray();
            data.PerfectInUnshuffle();
            var actual = new string(data);
            var expected = "AGINORSTABEELMPX";
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void CheckPerfectOutShuffle()
        {
            var data = "AGINORSTABEELMPX".ToCharArray();
            data.PerfectOutShuffle();
            var actual = new string(data);
            var expected = "AAGBIENEOLRMSPTX";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckPerfectOutUnshuffle()
        {
            var data = "AAGBIENEOLRMSPTX".ToCharArray();
            data.PerfectOutUnshuffle();
            var actual = new string(data);
            var expected = "AGINORSTABEELMPX";
            Assert.Equal(expected, actual);
        }
    }
}