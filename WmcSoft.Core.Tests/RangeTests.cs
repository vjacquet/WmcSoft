using System;
using System.Linq;
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
        [ExpectedException(typeof(ArgumentException))]
        public void CheckInvalidRange() {
            var actual = new Range<int>(5, 2);
            Assert.Fail();
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

        [TestMethod]
        public void CheckIntersect() {
            //  0    5   10   15   20
            //  !....!....!....!....!
            //    a  b  c d    e
            int a = 2, b = 5, c = 8, d = 10, e = 15;

            Assert.AreEqual(Range<int>.Empty, Range<int>.Intersect(new Range<int>(a, b), new Range<int>(c, d)));
            Assert.AreEqual(new Range<int>(c, d), Range<int>.Intersect(new Range<int>(b, e), new Range<int>(c, d)));
            Assert.AreEqual(new Range<int>(b, b), Range<int>.Intersect(new Range<int>(a, b), new Range<int>(b, c)));
            Assert.AreEqual(new Range<int>(b, d), Range<int>.Intersect(new Range<int>(a, d), new Range<int>(b, e)));
        }

        [TestMethod]
        public void CheckPartialMerge() {
            var data = new[] {
                Range.Create(1, 3),
                Range.Create(4, 5),
                Range.Create(5, 7),
                Range.Create(7, 9),
            };
            var expected = new[] {
                Range.Create(1, 3),
                Range.Create(4, 9),
            };
            var actual = data.PartialMerge().ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckPartialMergeOnOverlappingRanges() {
            var data = new[] {
                Range.Create(1, 3),
                Range.Create(4, 5),
                Range.Create(5, 7),
                Range.Create(7, 9),
                Range.Create(12, 16),
                Range.Create(8, 13)
            };
            var expected = new[] {
                Range.Create(1, 3),
                Range.Create(4, 16),
            };
            var actual = data.PartialMerge().ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}