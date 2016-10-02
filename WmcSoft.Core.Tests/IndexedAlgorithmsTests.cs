using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class IndexedAlgorithmsTests
    {
        [TestMethod]
        public void CheckSwapItems() {
            var expected = new[] { 1, 4, 3, 2, 5 };
            var actual = new[] { 1, 2, 3, 4, 5 };
            actual.SwapItems(1, 3);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckCopyBackwardsTo() {
            var expected = new[] { 1, 4, 3, 2, 5 };
            var data = new[] { 1, 2, 3, 4, 5 };
            var actual = new[] { 1, 0, 0, 0, 5 };
            data.CopyBackwardsTo(1, actual, 1, 3);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckRotateLeft() {
            var expected = new[] { 4, 5, 6, 7, 8, 9, 1, 2, 3 };
            var actual = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var startIndex = actual.Rotate(-3);
            Assert.AreEqual(6, startIndex);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckRotateRight() {
            var expected = new[] { 7, 8, 9, 1, 2, 3, 4, 5, 6, };
            var actual = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var startIndex = actual.Rotate(3);
            Assert.AreEqual(3, startIndex);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckInsertionSort() {
            var expected = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var actual = new[] { 7, 8, 9, 1, 2, 3, 4, 5, 6 };
            actual.InsertionSort();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckPartialInsertionSort() {
            var expected = new[] { 7, 1, 2, 3, 4, 5, 8, 9, 6 };
            var actual = new[] { 7, 8, 9, 1, 2, 3, 4, 5, 6 };
            actual.InsertionSort(1, 7);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckKeyCountingSort() {
            var expected = new[] { 10, 11, 21, 23, 33, 47, 54, 50 };
            var actual = new[] { 10, 21, 23, 54, 11, 47, 33, 50 };
            actual.KeyIndexCountingSort(i => i / 10, 9);
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
