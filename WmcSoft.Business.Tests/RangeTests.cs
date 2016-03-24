using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Business
{
    [TestClass]
    public class RangeTests
    {
        [TestMethod]
        public void CanCreateRange() {
            var actual = new Range<int>(2, 5);

            Assert.AreEqual(2, actual.Lower);
            Assert.AreEqual(5, actual.Upper);
        }

        [TestMethod]
        public void CanFixInvalidRange() {
            var actual = new Range<int>(5, 2);

            Assert.AreEqual(2, actual.Lower);
            Assert.AreEqual(5, actual.Upper);
        }

        [TestMethod]
        public void CanCreateEmptyRange() {
            var actual = new Range<int>();

            Assert.AreEqual(true, actual.IsEmpty);
        }

        [TestMethod]
        public void CanEqualRange() {
            var x = new Range<int>(2, 5);
            var y = new Range<int>(2, 5);

            Assert.IsTrue(x == y);
        }

        [TestMethod]
        public void CheckIsEmpty() {
           var actual = new Range<int>(2, 2);

            Assert.AreEqual(true, actual.IsEmpty);
        }

        [TestMethod]
        public void CheckIncludes() {
            var actual = new Range<int>(2, 5);
            var included = new Range<int>(3, 4);
            var overlapped = new Range<int>(3, 7);
            var distinct = new Range<int>(8, 10);

            Assert.IsTrue(actual.Includes(included));
            Assert.IsFalse(actual.Includes(overlapped));
            Assert.IsFalse(overlapped.Includes(actual));
            Assert.IsFalse(actual.Includes(distinct));
            Assert.IsFalse(distinct.Includes(actual));
        }

        [TestMethod]
        public void CheckOverlaps() {
            var actual = new Range<int>(2, 5);
            var included = new Range<int>(3, 4);
            var overlapped = new Range<int>(3, 7);
            var distinct = new Range<int>(8, 10);

            Assert.IsFalse(actual.Overlaps(included));
            Assert.IsTrue(actual.Overlaps(overlapped));
            Assert.IsTrue(overlapped.Overlaps(actual));
            Assert.IsFalse(actual.Overlaps(distinct));
            Assert.IsFalse(distinct.Overlaps(actual));
        }

        [TestMethod]
        public void CheckIsDistinct() {
            var actual = new Range<int>(2, 5);
            var included = new Range<int>(3, 4);
            var overlapped = new Range<int>(3, 7);
            var distinct = new Range<int>(8, 10);

            Assert.IsFalse(actual.IsDistinct(included));
            Assert.IsFalse(actual.IsDistinct(overlapped));
            Assert.IsFalse(overlapped.IsDistinct(actual));
            Assert.IsTrue(actual.IsDistinct(distinct));
            Assert.IsTrue(distinct.IsDistinct(actual));
        }
    }
}
