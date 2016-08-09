using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class ListExtensionsTests
    {
        [TestMethod]
        public void CheckIndexOfList() {
            var data = new [] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Assert.AreEqual(0, data.IndexOf(new[] { 0, 1, 2, 3 }));
            Assert.AreEqual(3, data.IndexOf(new[] { 3, 4, 5 }));
            Assert.AreEqual(-1, data.IndexOf(new[] { 3, 5, 4 }));
            Assert.AreEqual(7, data.IndexOf(new[] { 7, 8, 9 }));
            Assert.AreEqual(-1, data.IndexOf(new[] { 7, 8, 9, 10 }));
        }

        [TestMethod]
        public void CheckIndexOfPartial() {
            var data = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Assert.AreEqual(-1, data.IndexOf(0, 3, 3));
            Assert.AreEqual(-1, data.IndexOf(2, 3, 3));
            Assert.AreEqual(3, data.IndexOf(3, 3, 3));
            Assert.AreEqual(4, data.IndexOf(4, 3, 3));
            Assert.AreEqual(5, data.IndexOf(5, 3, 3));
            Assert.AreEqual(-1, data.IndexOf(6, 3, 3));
            Assert.AreEqual(-1, data.IndexOf(9, 3, 3));
        }

        [TestMethod]
        public void CheckSublistIndexOf() {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var data = list.Sublist(3, 3);
            Assert.AreEqual(-1, data.IndexOf(0));
            Assert.AreEqual(-1, data.IndexOf(2));
            Assert.AreEqual(0, data.IndexOf(3));
            Assert.AreEqual(1, data.IndexOf(4));
            Assert.AreEqual(2, data.IndexOf(5));
            Assert.AreEqual(-1, data.IndexOf(6));
            Assert.AreEqual(-1, data.IndexOf(9));
        }

        [TestMethod]
        public void CheckSublistToArray() {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var data = list.Sublist(3, 3);
            var actual = data.ToArray();
            var expected = new[] { 3, 4, 5 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanAddInsideTheSublist() {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var data = list.Sublist(3, 3);
            data.Add(10);
            Assert.AreEqual(4, data.Count);

            var expected = new[] { 0, 1, 2, 3, 4, 5, 10, 6, 7, 8, 9 };
            var actual = list.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanInsertInsideTheSublist() {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var data = list.Sublist(3, 3);
            data.Insert(1, 10);
            Assert.AreEqual(4, data.Count);

            var expected = new[] { 0, 1, 2, 3, 10, 4, 5, 6, 7, 8, 9 };
            var actual = list.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanRemoveAtInsideTheSublist() {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var data = list.Sublist(3, 3);
            data.RemoveAt(1);
            Assert.AreEqual(2, data.Count);

            var expected = new[] { 0, 1, 2, 3, 5, 6, 7, 8, 9 };
            var actual = list.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanRemoveInsideTheSublist() {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var data = list.Sublist(3, 3);
            Assert.IsTrue(data.Remove(4));
            Assert.AreEqual(2, data.Count);

            var expected = new[] { 0, 1, 2, 3, 5, 6, 7, 8, 9 };
            var actual = list.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanRemoveAfterTheSublist() {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var data = list.Sublist(3, 3);
            Assert.IsFalse(data.Remove(8));
            Assert.IsTrue(list.Remove(8));
            Assert.AreEqual(3, data.Count);

            var expected = new[] { 3, 4, 5 };
            var actual = data.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanRemoveBeforeTheSublist() {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var data = list.Sublist(3, 3);
            Assert.IsFalse(data.Remove(1));
            Assert.IsTrue(list.Remove(1));
            Assert.AreEqual(3, data.Count);

            var expected = new[] { 4, 5, 6 };
            var actual = data.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanClearTheSublist() {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var data = list.Sublist(3, 3);
            data.Clear();
            Assert.AreEqual(0, data.Count);

            var expected = new[] { 0, 1, 2, 6, 7, 8, 9 };
            var actual = list.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanRemoveMoreThanTheSublistCapacity() {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6,  };
            var data = list.Sublist(3, 3);
            list.RemoveAt(0);
            list.RemoveAt(0);
            Assert.AreEqual(2, data.Count);

            var expected = new[] { 5, 6 };
            var actual = data.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}