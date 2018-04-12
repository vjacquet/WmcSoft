using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using WmcSoft.Diagnostics;
using WmcSoft.Text;

namespace WmcSoft.Collections.Generic
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void CheckToString()
        {
            var list = new List<int> { 1, 2, 3, };

            var actual = list.ToString("g");
            var expected = "1;2;3";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckQuorum()
        {
            var list = new[] { 1, 2, 3, 4, 5, 6, 7 };
            Assert.True(list.Quorum(3, i => (i & 1) == 1));
        }

        [Fact]
        public void CheckAnyOrEmpty()
        {
            var empty = new int[] { };
            var list = new[] { 1, 2, 3, 4, 5, 6, 7 };

            Assert.True(empty.AnyOrEmpty(i => i % 3 == 0));
            Assert.True(list.AnyOrEmpty(i => i % 3 == 0));
            Assert.False(list.AnyOrEmpty(i => i % 9 == 0));
        }

        [Theory]
        [InlineData("A", new[] { "A", "B", null, "", "A", "C", "B", "A" })]
        [InlineData("B", new[] { "A", "B", null, "", "A", "C", "B", "A", "B", "B" })]
        [InlineData("A", new[] { "A", "B", null, "", "A", "C", "B", "A", "B" })]
        public void CheckElectedOrDefaultWithPredicate(string expected, string[] array)
        {
            Assert.Equal(expected, array.ElectedOrDefault(s => !string.IsNullOrEmpty(s)));
        }

        [Theory]
        [InlineData("A", new[] { "A", "B", null, "", "A", "C", "B", "A" })]
        [InlineData("B", new[] { "A", "B", null, "", "A", "C", "B", "A", "B", "B" })]
        [InlineData("A", new[] { "A", "B", null, "", "A", "C", "B", "A", "B" })]
        public void CheckElectedOrDefaultWithoutPredicate(string expected, string[] array)
        {
            Assert.Equal(expected, array.ElectedOrDefault());
        }

        [Fact]
        public void CheckCollateRepeat()
        {
            var array = new[] { "a", "b", "c" };
            var actual = string.Concat(array.Repeat(3));
            var expected = "abcabcabc";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckEnumerableCollateRepeat()
        {
            var array = new[] { "a", "b", "c" };
            var actual = string.Concat(array.Repeat(3, collate: true));
            var expected = "abcabcabc";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckEnumerableCollateRepeatToListOptimization()
        {
            const int Repeat = 3;
            var array = new[] { "a", "b", "c" };
            var actual = array.Repeat(Repeat, collate: true).ToList();
            var expected = new List<string>();
            for (int i = 0; i < Repeat; i++) {
                expected.AddRange(array);
            }
            Assert.NotEqual(expected.Capacity, actual.Capacity);
            Assert.Equal(expected.Count, actual.Capacity);
        }

        [Fact]
        public void CheckGroupedRepeat()
        {
            var array = new[] { "a", "b", "c" };
            var actual = string.Concat(array.Repeat(3, collate: false));
            var expected = "aaabbbccc";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckCollateRepeatToListOptimization()
        {
            const int Repeat = 3;
            var array = new[] { "a", "b", "c" };
            var actual = array.Repeat(Repeat, collate: false).ToList();
            var expected = new List<string>();
            for (int i = 0; i < Repeat; i++) {
                expected.AddRange(array);
            }
            Assert.NotEqual(expected.Capacity, actual.Capacity);
            Assert.Equal(expected.Count, actual.Capacity);
        }

        [Fact]
        public void CheckInterleavesWithIdenticalCount()
        {
            char[] s1 = { 'a', 'b', 'c' };
            char[] s2 = { 'M', 'N', 'P' };
            char[] s3 = { '1', '2', '3' };
            char[][] s = { s1, s2, s3 };
            // extensions methods in the same namespace takes precedence over the others, so force Enumerable.Sum
            var actual = new string(s.Interleave().ToArray(Enumerable.Sum(s, i => i.Length)));
            Assert.Equal("aM1bN2cP3", actual);
        }

        [Fact]
        public void CheckInterleaves()
        {
            char[] s1 = { 'a', 'b', 'c', 'd', 'e' };
            char[] s2 = { 'M', 'N', 'P' };
            char[] s3 = { '1', '2', '3', '4' };
            char[][] s = { s1, s2, s3 };
            // extensions methods in the same namespace takes precedence over the others, so force Enumerable.Sum
            var actual = new string(s.Interleave().ToArray(Enumerable.Sum(s, i => i.Length)));
            Assert.Equal("aM1bN2cP3d4e", actual);
        }

        [Fact]
        public void CheckTailOnCollection()
        {
            var collection = new[] { 1, 2, 3, 4, 5 };
            var expected = new[] { 3, 4, 5 };
            var actual = collection.Tail(3).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckTailWhenCollectionHasLessElements()
        {
            var expected = new[] { 1, 2, 3, 4, 5 };
            var actual = expected.Tail(6).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckTailWhenCollectionHasSameElementCount()
        {
            var expected = new[] { 1, 2, 3, 4, 5 };
            var actual = expected.Tail(5).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckTailOnEnumerable()
        {
            var enumerable = new[] { 1, 2, 3, 4, 5 }.ToEnumerable();
            var expected = new[] { 3, 4, 5 };
            var actual = enumerable.Tail(3).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckTailWhenEnumerableHasLessElements()
        {
            var expected = new[] { 1, 2, 3, 4, 5 }.ToEnumerable();
            var actual = expected.Tail(6).ToArray();
            Assert.Equal(expected.ToArray(), actual);
        }

        [Fact]
        public void CheckTailWhenEnumerableHasSameElementCount()
        {
            var expected = new[] { 1, 2, 3, 4, 5 }.ToEnumerable();
            var actual = expected.Tail(5).ToArray();
            Assert.Equal(expected.ToArray(), actual);
        }

        [Fact]
        public void CheckStride()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var expected = new[] { 1, 4, 7 };
            var actual = collection.Stride(3).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ScanActionIsCalledOnEveryItems()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var count = 0;
            var called = new List<int>();
            var list = collection.Scan(i => { called.Add(i); count++; }).ToList();
            Assert.Equal(collection, list);
            Assert.Equal(list.Count, count);
            Assert.Equal(list, called);
        }

        [Fact]
        public void ScanActionIsCalledOnlyOnTakenItems()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var count = 0;
            var called = new List<int>();
            var list = collection.Scan(i => { called.Add(i); count++; }).Take(4).ToList();
            Assert.Equal(4, count);
            Assert.Equal(list.Count, count);
            Assert.Equal(list, called);
            Assert.Equal(collection.Take(4), called);
        }

        [Fact]
        public void CheckTakeEveryNthElements()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var expected = new[] { 3, 6, 9 };
            var actual = collection.TakeEveryNthElements(3).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckTakeEveryNthElements2()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var expected = new[] { 3, 5, 6, 9 };
            var actual = collection.TakeEveryNthElements(3, 5).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckTakeEveryNthElementsN()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var expected = new[] { 3, 5, 6, 8, 9, 10 };
            var actual = collection.TakeEveryNthElements(3, 5, 8).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckTakeEveryNthElementsNWithMultiples()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var expected = new[] { 3, 5, 6, 8, 9, 10 };
            var actual = collection.TakeEveryNthElements(3, 6, 5, 8).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckDiscretize()
        {
            var data = new[] { 4, 40, 50, 60, 450, 451, 240, 600 };
            var actual = data.Discretize(51, 91, 151, 231, 331, 451).ToArray();
            var expected = new[] { 0, 0, 0, 1, 5, 6, 4, 6 };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckAtLeast()
        {
            var data = new[] { 4, 40, 50, 60, 450, 451, 240, 600 };
            Assert.False(data.AtLeast(2, i => i < 10));
            Assert.True(data.AtLeast(2, i => i > 100));
        }

        [Fact]
        public void CheckAtMost()
        {
            var data = new[] { 4, 40, 50, 60, 450, 451, 240, 600 };
            Assert.True(data.AtMost(2, i => i < 10));
            Assert.False(data.AtMost(2, i => i > 100));
        }

#pragma warning disable IDE0039 // Use local function
        [Fact]
        public void CheckChoose()
        {
            var data = new DisposeMonitorEnumerable<char>("a1bcd2ef3");

            Func<char, bool> vowel = c => "aeiouy".Contains(c);
            Func<char, bool> digits = char.IsDigit;
            Func<char, bool> uppercase = char.IsUpper;
            Func<char, bool> letter = char.IsLetter;
            Func<char, bool> white = char.IsWhiteSpace;

            Assert.Equal("ae", string.Join("", data.Choose(vowel, uppercase, digits)));
            Assert.Equal("abcdef", string.Join("", data.Choose(letter, vowel, digits)));
            Assert.Equal("123", string.Join("", data.Choose(digits, letter, vowel)));
            Assert.Equal("123", string.Join("", data.Choose(digits, white)));
            Assert.Equal("123", string.Join("", data.Choose(uppercase, white, digits)));
            Assert.Equal("123", string.Join("", data.Choose(uppercase, digits, white)));
            Assert.Equal("", string.Join("", data.Choose(uppercase, white)));

            Assert.Equal(7, data.Tally);
        }
#pragma warning restore IDE0039 // Use local function

        [Fact]
        public void CheckMinMax()
        {
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var expected = (1, 9);
            var actual = data.MinMax();
            Assert.Equal(expected, actual);

        }

        [Fact]
        public void CheckMinMaxOnNullable()
        {
            (int?, int?) undefined = (null, null);
            var empty = new List<int?>();
            Assert.Equal(undefined, empty.MinMax());

            empty.Add(null);
            empty.Add(null);
            Assert.Equal(undefined, empty.MinMax());

            var data = new List<int?> { null, 1, 2, 3, 4, null, 5, 6, 7, 8, 9 };
            (int?, int?) expected = (1, 9);
            var actual = data.MinMax();
            Assert.Equal(expected, actual);
        }

        static IEnumerable<Tuple<T, int>> ZipIndices<T>(IEnumerable<T> source)
        {
            var i = 0;
            foreach (var item in source)
                yield return Tuple.Create(item, i++);
        }

        [Fact]
        public void CheckMinOnExpectedDouble()
        {
            var data = new[] { -1d, 5d, double.NaN, -10d, 5d, -10d };
            var expected = data.Min();
            var actual = data.Min(x => Expected.Success(x));
            Assert.Equal(expected, actual.GetValueOrDefault());
        }

        [Fact]
        public void CheckMaxOnExpectedDouble()
        {
            var data = new[] { -1d, 5d, double.NaN, -10d };
            var expected = data.Max();
            var actual = data.Max(x => Expected.Success(x));
            Assert.Equal(expected, actual.GetValueOrDefault());
        }

        [Fact]
        public void CheckMinMaxOnExpectedDouble()
        {
            var data = new[] { -1d, 5d, double.NaN, -10d };
            var expected = Tuple.Create(data.Min(), data.Max());
            var actual = data.MinMax(x => Expected.Success(x));
            Assert.Equal(expected, Tuple.Create(actual.Item1.GetValueOrDefault(), actual.Item2.GetValueOrDefault()));
        }

        [Fact]
        public void CheckSumOnDouble()
        {
            var data = new[] { 1d, 2d, 3d, 4d, 5d };
            var expected = data.Max();
            var actual = data.Max(x => Expected.Success(x));
            Assert.Equal(expected, actual.GetValueOrDefault());
        }

        [Fact]
        public void EnsureDrawLotsDistributionIsUniform()
        {
            var counts = new int[10];
            var values = Enumerable.Range(0, counts.Length);
            var random = new Random(1664);
            var N = 10_000;
            for (var i = 0; i < N; i++) {
                var n = values.DrawLots(random);
                counts[n]++;
            }

            var r = counts.Length;
            var Nr = (double)N / r;
            var chi2 = 0d;
            for (var i = 0; i < r; i++) {
                var f = counts[i] - Nr;
                chi2 += f * f;
            }
            chi2 /= Nr;
            // critical value of 11.070 at 95% significance level
            Assert.True(chi2 < 11.070d);
        }

        [Fact]
        public void EnsureDrawLotsOnListCallsRandomOnce()
        {
            var random = new MockRandom((x, y) => 2);
            var list = new[] { 1, 2, 3, 4, 5 };
            var actual = list.DrawLots(random);
            Assert.Equal(1, random.Called);
            Assert.Equal(3, actual);
        }

        [Fact]
        public void CheckZipAll()
        {
            var x = new[] { "A", "B", "C", "D" };
            var y = new[] { "A", "B", "C", "D", "E", "F" };

            Assert.Equal("AABBCCDDXEXF", string.Concat(x.ZipAll(y, (a, b) => a + b, "X", "Y")));
            Assert.Equal("AABBCCDDEYFY", string.Concat(y.ZipAll(x, (a, b) => a + b, "X", "Y")));
            Assert.Equal("AABBCCDD", string.Concat(x.ZipAll(x, (a, b) => a + b, "X", "Y")));
        }

        [Fact]
        public void CheckToDictionaryThrowExceptionPolicy()
        {
            var source = new[] { 0, 1, 2, 3, 2, 4, 5 };
            try {
                var dictionary = source.ToDictionary(DuplicatePolicy.ThrowException, x => 'A' + x);
                Assert.True(false, "Inconclusive");
            } catch (Exception e) {
                Assert.Equal(67, e.GetCapturedEntry("key"));
            }
        }

        [Fact]
        public void CheckToDictionaryKeepFirstPolicy()
        {
            var source = new[] { 0, 1, 2, 3, 4, 5 };
            var dictionary = source.ToDictionary(DuplicatePolicy.KeepFirst, x => x % 3);
            Assert.Equal(3, dictionary.Count);
            Assert.Equal(0, dictionary[0]);
            Assert.Equal(1, dictionary[1]);
            Assert.Equal(2, dictionary[2]);
            Assert.False(dictionary.ContainsKey(3));
        }

        [Fact]
        public void CheckToDictionaryKeepLastPolicy()
        {
            var source = new[] { 0, 1, 2, 3, 4, 5 };
            var dictionary = source.ToDictionary(DuplicatePolicy.KeepLast, x => x % 3);
            Assert.Equal(3, dictionary.Count);
            Assert.Equal(3, dictionary[0]);
            Assert.Equal(4, dictionary[1]);
            Assert.Equal(5, dictionary[2]);
            Assert.False(dictionary.ContainsKey(3));
        }

        [Fact]
        public void CheckUnless()
        {
            var source = new[] { 1, 2, 3, 4, 5, 6, 7 };
            var actual = source.Unless(x => x % 3 == 0).ToArray();
            var expected = new[] { 1, 2, 4, 5, 7 };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckUnlessWithIndex()
        {
            var source = new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            var actual = source.Unless((x, i) => i % 3 == 0).ToArray();
            var expected = new[] { 'B', 'C', 'E', 'F', 'H' };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckNGrams()
        {
            var source = new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            var ngrams = source.NGrams(3);
            var actual = ngrams.Select(g => string.Join("", g)).ToList();
            var expected = new[] { "ABC", "BCD", "CDE", "DEF", "EFG", "FGH" };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckMatch()
        {
            var data = new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            var selected = new[] { true, false, false, true, true, false, false, true };
            var expected = new[] { 'A', 'D', 'E', 'H' };
            var actual = data.Match(selected, (x, s) => s).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckMismatch()
        {
            var data = new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            var selected = new[] { true, false, false, true, true, false, false, true };
            var expected = new[] { 'B', 'C', 'F', 'G' };
            var actual = data.Mismatch(selected, (x, s) => s).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCountOccurences()
        {
            var data = "Hello World";
            var occurences = data.CountOccurences();
            Assert.Equal(8, occurences.Count);
            Assert.Equal(1, occurences['e']);
            Assert.Equal(2, occurences['o']);
            Assert.Equal(3, occurences['l']);
        }

        [Fact]
        public void CanCountNullOccurences()
        {
            var data = new object[] { null, null, null };
            var occurences = data.CountOccurences(out int nullOccurences);
            Assert.Empty(occurences);
            Assert.Equal(data.Length, nullOccurences);
        }

        [Fact]
        public void CanCountOccurencesWithEqualityComparer()
        {
            var data = "Aa";
            var occurences = data.CountOccurences(CaseInsensitiveCharComparer.InvariantCulture);
            Assert.Equal(2, occurences['A']);
            Assert.Equal(2, occurences['a']);
            Assert.Single(occurences);
            Assert.Equal('A', occurences.Single().Key);
        }

        [Fact]
        public void CanUseExceptWithVariadicParameters()
        {
            var first = new List<int> { 1, 2, 3, 4, 5 };
            var expected = new[] { 1, 2, 4 };
            var actual = first.Except(3, 5);
            Assert.Equal(expected, actual);
        }
    }
}
