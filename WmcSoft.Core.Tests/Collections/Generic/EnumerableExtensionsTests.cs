using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class EnumerableExtensionsTests
    {
        [TestMethod]
        public void CheckToString()
        {
            var list = new List<int> { 1, 2, 3, };

            var actual = list.ToString("g");
            var expected = "1;2;3";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckQuorum()
        {
            var list = new[] { 1, 2, 3, 4, 5, 6, 7 };
            Assert.IsTrue(list.Quorum(3, i => (i & 1) == 1));
        }

        [TestMethod]
        public void CheckElectedOrDefaultWithPredicate()
        {
            string[] array;

            array = new[] { "A", "B", null, "", "A", "C", "B", "A" };
            Assert.AreEqual("A", array.ElectedOrDefault(s => !string.IsNullOrEmpty(s)));

            array = new[] { "A", "B", null, "", "A", "C", "B", "A", "B", "B" };
            Assert.AreEqual("B", array.ElectedOrDefault(s => !string.IsNullOrEmpty(s)));

            array = new[] { "A", "B", null, "", "A", "C", "B", "A", "B" };
            Assert.AreEqual("A", array.ElectedOrDefault(s => !string.IsNullOrEmpty(s)));
        }

        [TestMethod]
        public void CheckElectedOrDefaultWithoutPredicate()
        {
            string[] array;

            array = new[] { "A", "B", null, "", "A", "C", "B", "A" };
            Assert.AreEqual("A", array.ElectedOrDefault());

            array = new[] { "A", "B", null, "", "A", "C", "B", "A", "B", "B" };
            Assert.AreEqual("B", array.ElectedOrDefault());

            array = new[] { "A", "B", null, "", "A", "C", "B", "A", "B" };
            Assert.AreEqual("A", array.ElectedOrDefault());
        }

        [TestMethod]
        public void CheckCollateRepeat()
        {
            var array = new[] { "a", "b", "c" };
            var actual = string.Concat(array.Repeat(3));
            var expected = "abcabcabc";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckEnumerableCollateRepeat()
        {
            var array = new[] { "a", "b", "c" };
            var actual = string.Concat(array.Repeat(3, collate: true));
            var expected = "abcabcabc";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckEnumerableCollateRepeatToListOptimization()
        {
            const int Repeat = 3;
            var array = new[] { "a", "b", "c" };
            var actual = array.Repeat(Repeat, collate: true).ToList();
            var expected = new List<string>();
            for (int i = 0; i < Repeat; i++) {
                expected.AddRange(array);
            }
            Assert.AreNotEqual(expected.Capacity, actual.Capacity);
            Assert.AreEqual(expected.Count, actual.Capacity);
        }

        [TestMethod]
        public void CheckGroupedRepeat()
        {
            var array = new[] { "a", "b", "c" };
            var actual = string.Concat(array.Repeat(3, collate: false));
            var expected = "aaabbbccc";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckCollateRepeatToListOptimization()
        {
            const int Repeat = 3;
            var array = new[] { "a", "b", "c" };
            var actual = array.Repeat(Repeat, collate: false).ToList();
            var expected = new List<string>();
            for (int i = 0; i < Repeat; i++) {
                expected.AddRange(array);
            }
            Assert.AreNotEqual(expected.Capacity, actual.Capacity);
            Assert.AreEqual(expected.Count, actual.Capacity);
        }

        [TestMethod]
        public void CheckInterleavesWithIdenticalCount()
        {
            char[] s1 = { 'a', 'b', 'c' };
            char[] s2 = { 'M', 'N', 'P' };
            char[] s3 = { '1', '2', '3' };
            char[][] s = { s1, s2, s3 };
            // extensions methods in the same namespace takes precedence over the others, so force Enumerable.Sum
            var actual = new string(s.Interleave().ToArray(Enumerable.Sum(s, i => i.Length)));
            Assert.AreEqual("aM1bN2cP3", actual);
        }

        [TestMethod]
        public void CheckInterleaves()
        {
            char[] s1 = { 'a', 'b', 'c', 'd', 'e' };
            char[] s2 = { 'M', 'N', 'P' };
            char[] s3 = { '1', '2', '3', '4' };
            char[][] s = { s1, s2, s3 };
            // extensions methods in the same namespace takes precedence over the others, so force Enumerable.Sum
            var actual = new string(s.Interleave().ToArray(Enumerable.Sum(s, i => i.Length)));
            Assert.AreEqual("aM1bN2cP3d4e", actual);
        }

        [TestMethod]
        public void CheckTailOnCollection()
        {
            var collection = new[] { 1, 2, 3, 4, 5 };
            var expected = new[] { 3, 4, 5 };
            var actual = collection.Tail(3).ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckTailWhenCollectionHasLessElements()
        {
            var expected = new[] { 1, 2, 3, 4, 5 };
            var actual = expected.Tail(6).ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckTailWhenCollectionHasSameElementCount()
        {
            var expected = new[] { 1, 2, 3, 4, 5 };
            var actual = expected.Tail(5).ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckTailOnEnumerable()
        {
            var enumerable = new[] { 1, 2, 3, 4, 5 }.ToEnumerable();
            var expected = new[] { 3, 4, 5 };
            var actual = enumerable.Tail(3).ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckTailWhenEnumerableHasLessElements()
        {
            var expected = new[] { 1, 2, 3, 4, 5 }.ToEnumerable();
            var actual = expected.Tail(6).ToArray();
            CollectionAssert.AreEqual(expected.ToArray(), actual);
        }

        [TestMethod]
        public void CheckTailWhenEnumerableHasSameElementCount()
        {
            var expected = new[] { 1, 2, 3, 4, 5 }.ToEnumerable();
            var actual = expected.Tail(5).ToArray();
            CollectionAssert.AreEqual(expected.ToArray(), actual);
        }

        [TestMethod]
        public void CheckStride()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var expected = new[] { 1, 4, 7 };
            var actual = collection.Stride(3).ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckNthElements()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var expected = new[] { 3, 6, 9 };
            var actual = collection.NthElements(3).ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckNthElements2()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var expected = new[] { 3, 5, 6, 9 };
            var actual = collection.NthElements(3, 5).ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckNthElementsN()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var expected = new[] { 3, 5, 6, 8, 9, 10 };
            var actual = collection.NthElements(3, 5, 8).ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckNthElementsNWithMultiples()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var expected = new[] { 3, 5, 6, 8, 9, 10 };
            var actual = collection.NthElements(3, 6, 5, 8).ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckDiscretize()
        {
            var data = new[] { 4, 40, 50, 60, 450, 451, 240, 600 };
            var actual = data.Discretize(51, 91, 151, 231, 331, 451).ToArray();
            var expected = new[] { 0, 0, 0, 1, 5, 6, 4, 6 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckAtLeast()
        {
            var data = new[] { 4, 40, 50, 60, 450, 451, 240, 600 };
            Assert.IsFalse(data.AtLeast(2, i => i < 10));
            Assert.IsTrue(data.AtLeast(2, i => i > 100));
        }

        [TestMethod]
        public void CheckAtMost()
        {
            var data = new[] { 4, 40, 50, 60, 450, 451, 240, 600 };
            Assert.IsTrue(data.AtMost(2, i => i < 10));
            Assert.IsFalse(data.AtMost(2, i => i > 100));
        }

        [TestMethod]
        public void CheckChoose()
        {
            var data = new DisposeMonitorEnumerable<char>("a1bcd2ef3");

            Func<char, bool> vowel = c => "aeiouy".Contains(c);
            Func<char, bool> digits = c => Char.IsDigit(c);
            Func<char, bool> uppercase = c => Char.IsUpper(c);
            Func<char, bool> letter = c => Char.IsLetter(c);
            Func<char, bool> white = c => Char.IsWhiteSpace(c);

            Assert.AreEqual("ae", string.Join("", data.Choose(vowel, uppercase, digits)));
            Assert.AreEqual("abcdef", string.Join("", data.Choose(letter, vowel, digits)));
            Assert.AreEqual("123", string.Join("", data.Choose(digits, letter, vowel)));
            Assert.AreEqual("123", string.Join("", data.Choose(digits, white)));
            Assert.AreEqual("123", string.Join("", data.Choose(uppercase, white, digits)));
            Assert.AreEqual("123", string.Join("", data.Choose(uppercase, digits, white)));
            Assert.AreEqual("", string.Join("", data.Choose(uppercase, white)));

            Assert.AreEqual(7, data.Tally);
        }

        [TestMethod]
        public void CheckMinMax()
        {
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var expected = Tuple.Create(1, 9);
            var actual = data.MinMax();
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void CheckMinMaxOnNullable()
        {
            var empty = new List<int?>();
            Assert.AreEqual(new Tuple<int?, int?>(null, null), empty.MinMax());

            empty.Add(null);
            empty.Add(null);
            Assert.AreEqual(new Tuple<int?, int?>(null, null), empty.MinMax());

            var data = new List<int?> { null, 1, 2, 3, 4, null, 5, 6, 7, 8, 9 };
            var expected = Tuple.Create<int?, int?>(1, 9);
            var actual = data.MinMax();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckMinOnExpectedDouble()
        {
            var data = new[] { -1d, 5d, double.NaN,-10d };
            var expected = data.Min();
            var actual = data.Select(x => Expected.Success(x)).Min().GetValueOrDefault();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckMaxOnExpectedDouble()
        {
            var data = new[] { -1d, 5d, double.NaN, -10d };
            var expected = data.Max();
            var actual = data.Select(x => Expected.Success(x)).Max().GetValueOrDefault();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnsureDrawLotsDistributionIsUniform()
        {
            var counts = new int[6];
            var values = Enumerable.Range(0, counts.Length).ToList();
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
            Assert.IsTrue(chi2 < 11.070d);
        }
    }
}