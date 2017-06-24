using System;
using System.Linq;
using Xunit;

namespace WmcSoft
{
    public class AlgorithmsTests
    {
        [Fact]
        public void CheckCoprimes()
        {
            var collection = new[] { 3, 6, 8, 5, 8 };
            var expected = new[] { 3, 5, 8 };
            var actual = Algorithms.Coprimes(collection).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckMidpoint()
        {
            var x = 20;
            var y = 40;
            Assert.Equal(30, Algorithms.Midpoint(x, y));
            Assert.Equal(30, Algorithms.Midpoint(y, x));
        }

        [Fact]
        public void CheckMin()
        {
            var x = new DateTime(1973, 5, 2);
            var y = new DateTime(1973, 5, 1);

            var expected = y;
            var actual = Algorithms.Min(x, y);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckMinStablility()
        {
            var x = new DateTime(1973, 5, 2);
            var y = new DateTime(1973, 5, 1);

            Comparison<DateTime> comparison = (a, b) => a.Month.CompareTo(b.Month);
            var expected = x;
            var actual = Algorithms.Min(comparison, x, y);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckMinStablilityN()
        {
            var x = new DateTime(1973, 5, 2);
            var y = new DateTime(1973, 5, 1);
            var z = new DateTime(1973, 6, 2);

            Comparison<DateTime> comparison = (a, b) => a.Month.CompareTo(b.Month);
            var expected = x;
            var actual = Algorithms.Min(comparison, x, y, z);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckMax()
        {
            var x = new DateTime(1973, 5, 2);
            var y = new DateTime(1973, 5, 1);

            var expected = x;
            var actual = Algorithms.Max(x, y);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckMaxStablility()
        {
            var x = new DateTime(1973, 5, 2);
            var y = new DateTime(1973, 5, 1);

            Comparison<DateTime> comparison = (a, b) => a.Month.CompareTo(b.Month);
            var expected = y;
            var actual = Algorithms.Max(comparison, x, y);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckMaxStablilityN()
        {
            var x = new DateTime(1973, 5, 2);
            var y = new DateTime(1973, 5, 1);
            var z = new DateTime(1973, 4, 1);

            Comparison<DateTime> comparison = (a, b) => a.Month.CompareTo(b.Month);
            var expected = y;
            var actual = Algorithms.Max(comparison, x, y, z);
            Assert.Equal(expected, actual);
        }

        static bool IsLower(int a, int b)
        {
            return a < b;
        }

        [Fact]
        public void CheckMinMaxRelation()
        {
            Relation<int> relation = IsLower;
            var mm = Algorithms.MinMax(relation, 2, 5, 6, 3, 1, 9);
            Assert.Equal(1, mm.Item1);
            Assert.Equal(9, mm.Item2);
        }

        [Fact]
        public void CheckHammingDistanceForStrings()
        {
            Assert.Equal(3, Algorithms.Hamming("karolin", "kathrin"));
            Assert.Equal(3, Algorithms.Hamming("karolin", "kerstin"));
        }

        [Fact]
        public void CheckLevenshteinDistance()
        {
            Assert.Equal(3, Algorithms.Levenshtein("kitten", "sitting"));
        }

        [Fact]
        public void CheckDamereauLevenshteinDistance()
        {
            Assert.Equal(3, Algorithms.DamerauLevenshtein("kitten", "sitting"));
            Assert.Equal(1, Algorithms.DamerauLevenshtein("kitten", "iktten"));
        }
    }
}
