using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Specialized
{
    [TestClass]
    public class GapListTests
    {
        [TestMethod]
        public void CanInsert() {
            var list = new GapList<int>(10);
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
            var expected = new[] { 1, 2, 3, 4, 9, 10, 5, 7, 8, 6 };
            var actual = list.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanInsertWithoutMoving() {
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
            list.Add(6);
            list.Insert(4, 9);
            list.Add(10);
            list.Add(7);
            list.Add(8);

            var actual = list.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanRemove() {
            var list = new GapList<int>(5);
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
            list.Remove(3);
            list.Remove(10);

            var expected = new[] { 1, 2, 4, 9, 5, 7, 8, 6 };
            var actual = list.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanEnumerateWithGap() {
            var list = new GapList<int>(16);
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
            var expected = new[] { 1, 2, 3, 4, 9, 10, 5, 7, 8, 6 };
            var actual = list.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
