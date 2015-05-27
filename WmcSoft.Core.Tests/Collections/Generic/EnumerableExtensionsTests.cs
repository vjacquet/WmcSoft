﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic.Tests
{
    [TestClass]
    public class EnumerableExtensionsTests
    {
        [TestMethod]
        public void CheckQuorum() {
            var list = new[] { 1, 2, 3, 4, 5, 6, 7 };
            Assert.IsTrue(list.Quorum(3, i => (i & 1) == 1));
        }

        [TestMethod]
        public void CheckElectedOrDefaultWithPredicate() {
            string[] array;

            array = new[] { "A", "B", null, "", "A", "C", "B", "A" };
            Assert.AreEqual("A", array.ElectedOrDefault(s => !String.IsNullOrEmpty(s)));

            array = new[] { "A", "B", null, "", "A", "C", "B", "A", "B", "B" };
            Assert.AreEqual("B", array.ElectedOrDefault(s => !String.IsNullOrEmpty(s)));

            array = new[] { "A", "B", null, "", "A", "C", "B", "A", "B" };
            Assert.AreEqual("A", array.ElectedOrDefault(s => !String.IsNullOrEmpty(s)));
        }

        [TestMethod]
        public void CheckElectedOrDefaultWithoutPredicate() {
            string[] array;

            array = new[] { "A", "B", null, "", "A", "C", "B", "A" };
            Assert.AreEqual("A", array.ElectedOrDefault());

            array = new[] { "A", "B", null, "", "A", "C", "B", "A", "B", "B" };
            Assert.AreEqual("B", array.ElectedOrDefault());

            array = new[] { "A", "B", null, "", "A", "C", "B", "A", "B" };
            Assert.AreEqual("A", array.ElectedOrDefault());
        }

        [TestMethod]
        public void CheckInterleavesWithIdenticalCount() {
            char[] s1 = { 'a', 'b', 'c'};
            char[] s2 = { 'M', 'N', 'P' };
            char[] s3 = { '1', '2', '3' };
            char[][] s = { s1, s2, s3 };
            var actual = new String(s.Interleave().ToArray(s.Sum(i=>i.Length)));
            Assert.AreEqual("aM1bN2cP3", actual);
        }

        [TestMethod]
        public void CheckInterleaves() {
            char[] s1 = { 'a', 'b', 'c', 'd', 'e' };
            char[] s2 = { 'M', 'N', 'P' };
            char[] s3 = { '1', '2', '3', '4' };
            char[][] s = { s1, s2, s3 };
            var actual = new String(s.Interleave().ToArray(s.Sum(i => i.Length)));
            Assert.AreEqual("aM1bN2cP3d4e", actual);
        }
    }
}
