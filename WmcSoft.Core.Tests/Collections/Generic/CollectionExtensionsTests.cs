using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic.Tests
{
    [TestClass]
    public class CollectionExtensionsTests
    {
        [TestMethod]
        public void CheckToString() {
            var list = new List<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);

            var actual = list.ToString("g");
            var expected = "1;2;3";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckToArray() {
            var list = new List<Tuple<int, string>>();
            list.Add(Tuple.Create(1, "a"));
            list.Add(Tuple.Create(2, "b"));
            list.Add(Tuple.Create(3, "c"));

            var expected = new[] { 1, 2, 3 };
            var actual = list.ToArray(i => i.Item1);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckBinarySearch() {
            var list = new List<Tuple<int, string>>();
            list.Add(Tuple.Create(2, "a"));
            list.Add(Tuple.Create(4, "b"));
            list.Add(Tuple.Create(6, "c"));
            list.Add(Tuple.Create(8, "d"));

            Assert.AreEqual(1, list.BinarySearch(t => Comparer.DefaultInvariant.Compare(t.Item1, 4)));
            Assert.AreEqual(2, ~list.BinarySearch(t => Comparer.DefaultInvariant.Compare(t.Item1, 5)));
        }
    }
}
