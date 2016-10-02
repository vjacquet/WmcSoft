using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Collections.Generic;

namespace WmcSoft
{
    [TestClass]
    public class ArrayExtensionsTests
    {
        [TestMethod]
        public void CheckGetColumn() {
            var array = new[,] {
               { 1, 2, 3},
               { 4, 5, 6},
            };
            var expected = new[] { 2, 5 };
            var actual = array.GetColumn(1).ToArray();
            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void CheckGetRow() {
            var array = new[,] {
               { 1, 2, 3},
               { 4, 5, 6},
            };
            var expected = new[] { 4, 5, 6 };
            var actual = array.GetRow(1).ToArray();
            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void CheckStructuralEquals() {
            var expected = new[] { 1, 2, 3 };
            var actual = new[] { 1, 2, 3 };
            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void CheckTranspose() {
            var expected = new[,] { { 1, 3 }, { 2, 4 } };
            var array = new[,] { { 1, 2 }, { 3, 4 } };
            var actual = array.Transpose();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckToMultiDimensional() {
            var expected = new[,] { { 1, 2 }, { 3, 4 } };
            var array = new int[][] { new[] { 1, 2 }, new[] { 3, 4 } };
            var actual = array.ToMultiDimensional();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckFlatten() {
            var expected = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var array = new int[][] { new[] { 1, 2 }, null, new[] { 3, 4 }, new[] { 5 }, new[] { 6, 7, 8, 9 } };
            var actual = array.Flatten();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckRotateLeft() {
            var actual = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var expected = new[] { 4, 5, 6, 7, 8, 9, 1, 2, 3 };
            var position = actual.Rotate(-3);
            Assert.AreEqual(6, position);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckRotateRight() {
            var actual = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var expected = new[] { 7, 8, 9, 1, 2, 3, 4, 5, 6 };
            var position = actual.Rotate(3);
            Assert.AreEqual(3, position);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanEnumerateRange() {
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var expected = new[] { 3, 4, 5, 6 };
            var list = new List<int>();
            using(var enumerator = data.GetEnumerator(2, 4)) {
                while (enumerator.MoveNext())
                    list.Add(enumerator.Current);
            }
            var actual = list.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}