﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class AlgorithmsTests
    {
        [TestMethod]
        public void CheckCoprimes() {
            var collection = new[] { 3, 6, 8, 5, 8 };
            var expected = new[] { 3, 5, 8 };
            var actual = Algorithms.Coprimes(collection).ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckMidpoint() {
            var x = 20;
            var y = 40;
            Assert.AreEqual(30, Algorithms.Midpoint(x, y));
            Assert.AreEqual(30, Algorithms.Midpoint(y, x));
        }

        [TestMethod]
        public void CheckMin() {
            var x = new DateTime(1973, 5, 2);
            var y = new DateTime(1973, 5, 1);

            var expected = y;
            var actual = Algorithms.Min(x, y);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckMinStablility() {
            var x = new DateTime(1973, 5, 2);
            var y = new DateTime(1973, 5, 1);

            Comparison<DateTime> comparison = (a, b) => a.Month.CompareTo(b.Month);
            var expected = x;
            var actual = Algorithms.Min(comparison, x, y);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckMinStablilityN() {
            var x = new DateTime(1973, 5, 2);
            var y = new DateTime(1973, 5, 1);
            var z = new DateTime(1973, 6, 2);

            Comparison<DateTime> comparison = (a, b) => a.Month.CompareTo(b.Month);
            var expected = x;
            var actual = Algorithms.Min(comparison, x, y, z);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckMax() {
            var x = new DateTime(1973, 5, 2);
            var y = new DateTime(1973, 5, 1);

            var expected = x;
            var actual = Algorithms.Max(x, y);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckMaxStablility() {
            var x = new DateTime(1973, 5, 2);
            var y = new DateTime(1973, 5, 1);

            Comparison<DateTime> comparison = (a, b) => a.Month.CompareTo(b.Month);
            var expected = y;
            var actual = Algorithms.Max(comparison, x, y);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckMaxStablilityN() {
            var x = new DateTime(1973, 5, 2);
            var y = new DateTime(1973, 5, 1);
            var z = new DateTime(1973, 4, 1);

            Comparison<DateTime> comparison = (a, b) => a.Month.CompareTo(b.Month);
            var expected = y;
            var actual = Algorithms.Max(comparison, x, y, z);
            Assert.AreEqual(expected, actual);
        }

        static bool IsLower(int a, int b) {
            return a < b;
        }

        [TestMethod]
        public void CheckMinMaxRelation() {
            Relation<int> relation = IsLower;
            var mm = Algorithms.MinMax(relation, 2, 5, 6, 3, 1, 9);
            Assert.AreEqual(1, mm.Item1);
            Assert.AreEqual(9, mm.Item2);
        }

        [TestMethod]
        public void CheckHammingDistanceForUInt32() {
            //Assert.AreEqual(2, Algorithms.Hamming(0b01011101, 0b01001001));
            Assert.AreEqual(2, Algorithms.Hamming(0x5d, 0x49));
        }

        [TestMethod]
        public void CheckHammingDistanceForStrings() {
            Assert.AreEqual(3, Algorithms.Hamming("karolin", "kathrin"));
            Assert.AreEqual(3, Algorithms.Hamming("karolin", "kerstin"));
        }

        [TestMethod]
        public void CheckLevenshteinDistance() {
            Assert.AreEqual(3, Algorithms.Levenshtein("kitten", "sitting"));
        }

        [TestMethod]
        public void CheckDamereauLevenshteinDistance() {
            Assert.AreEqual(3, Algorithms.DamerauLevenshtein("kitten", "sitting"));
            Assert.AreEqual(1, Algorithms.DamerauLevenshtein("kitten", "iktten"));
        }
    }
}
