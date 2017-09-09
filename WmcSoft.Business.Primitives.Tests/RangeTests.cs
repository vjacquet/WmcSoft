using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace WmcSoft.Business
{
    public class RangeTests
    {
        static readonly Dictionary<string, Range<int>[]> TestCases = new Dictionary<string, Range<int>[]> {
            ["UnsortedContiguous"] = new[] {
                Range.Create(7, 9),
                Range.Create(3, 5),
                Range.Create(1, 3),
                Range.Create(5, 7),
            },
            ["SortedContiguous"] = new[] {
                Range.Create(1, 3),
                Range.Create(3, 5),
                Range.Create(5, 7),
                Range.Create(7, 9),
            },
            ["Disjoined"] = new[] {
                Range.Create(1, 3),
                Range.Create(3, 4),
                Range.Create(5, 7),
                Range.Create(7, 9),
            },
            ["Overlapping"] = new[] {
                Range.Create(1, 3),
                Range.Create(3, 6),
                Range.Create(5, 7),
                Range.Create(7, 9),
            },
        };

        [Fact]
        public void CanCreateRange()
        {
            var actual = new Range<int>(2, 5);

            Assert.Equal(2, actual.Lower);
            Assert.Equal(5, actual.Upper);
        }

        [Fact]
        public void CanDeconstructRange()
        {
            var actual = new Range<int>(2, 5);
            var (lo, hi) = actual;
            Assert.Equal(2, lo);
            Assert.Equal(5, hi);
        }

        [Fact]
        public void OutOfOrderArgumentsThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Range<int>(5, 2));
        }

        [Fact]
        public void CanCreateEmptyRange()
        {
            var actual = new Range<int>();

            Assert.Equal(true, actual.IsEmpty);
        }

        [Fact]
        public void CanEqualRange()
        {
            var x = new Range<int>(2, 5);
            var y = new Range<int>(2, 5);

            Assert.True(x == y);
        }

        [Theory]
        [InlineData(1, 10, "fr-FR", "G", "[1; 10)")]
        [InlineData(1, 10, "fr-FR", "M", "[1; 10)")]
        [InlineData(1, 10, "fr-FR", "B", "[1; 10[")]
        [InlineData(1, 10, "en-US", "G", "[1; 10)")]
        [InlineData(1, 10, "en-US", "M", "[1; 10)")]
        [InlineData(1, 10, "en-US", "B", "[1; 10[")]
        [InlineData(1, 10, null, "G", "[1; 10)")]
        [InlineData(1, 10, null, "M", "[1; 10)")]
        [InlineData(1, 10, null, "B", "[1; 10[")]
        public void CanFormatRange(decimal i, decimal j, string culture, string format, string expected)
        {
            var ci = culture == null
                ? CultureInfo.InvariantCulture
                : CultureInfo.GetCultureInfoByIetfLanguageTag(culture);
            var range = Range.Create(i, j);
            var actual = range.ToString(format, ci);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RangeIsEmptyWhenLowerEqualsUpper()
        {
            var actual = new Range<int>(2, 2);

            Assert.Equal(true, actual.IsEmpty);
        }

        [Fact]
        public void CheckIncludes()
        {
            var actual = new Range<int>(2, 5);
            var included = new Range<int>(3, 4);
            var overlapped = new Range<int>(3, 7);
            var distinct = new Range<int>(8, 10);

            Assert.True(actual.Includes(included));
            Assert.False(actual.Includes(overlapped));
            Assert.False(overlapped.Includes(actual));
            Assert.False(actual.Includes(distinct));
            Assert.False(distinct.Includes(actual));
        }

        [Fact]
        public void CheckOverlaps()
        {
            var actual = new Range<int>(2, 5);
            var included = new Range<int>(3, 4);
            var overlapped = new Range<int>(3, 7);
            var distinct = new Range<int>(8, 10);

            Assert.False(actual.Overlaps(included));
            Assert.True(actual.Overlaps(overlapped));
            Assert.True(overlapped.Overlaps(actual));
            Assert.False(actual.Overlaps(distinct));
            Assert.False(distinct.Overlaps(actual));
        }

        [Fact]
        public void CheckIsDistinct()
        {
            var actual = new Range<int>(2, 5);
            var included = new Range<int>(3, 4);
            var overlapped = new Range<int>(3, 7);
            var distinct = new Range<int>(8, 10);

            Assert.False(actual.IsDistinct(included));
            Assert.False(actual.IsDistinct(overlapped));
            Assert.False(overlapped.IsDistinct(actual));
            Assert.True(actual.IsDistinct(distinct));
            Assert.True(distinct.IsDistinct(actual));
        }

        [Fact]
        public void CheckIntersect()
        {
            //  0    5   10   15   20
            //  !....!....!....!....!
            //    a  b  c d    e
            int a = 2, b = 5, c = 8, d = 10, e = 15;

            Assert.Equal(Range<int>.Empty, Range.Intersect(new Range<int>(a, b), new Range<int>(c, d)));
            Assert.Equal(new Range<int>(c, d), Range.Intersect(new Range<int>(b, e), new Range<int>(c, d)));
            Assert.Equal(new Range<int>(b, b), Range.Intersect(new Range<int>(a, b), new Range<int>(b, c)));
            Assert.Equal(new Range<int>(b, d), Range.Intersect(new Range<int>(a, d), new Range<int>(b, e)));
        }

        [Theory]
        [InlineData("SortedContiguous")]
        [InlineData("UnsortedContiguous")]
        public void IsContiguousReturnsTrueOnRanges(string name)
        {
            var ranges = TestCases[name];
            Assert.True(Range.IsContiguous(ranges));
        }

        [Theory]
        [InlineData("Disjoined")]
        [InlineData("Overlapping")]
        public void IsContiguousReturnsFalseOnRanges(string name)
        {
            var ranges = TestCases[name];
            Assert.False(Range.IsContiguous(ranges));
        }

        [Theory]
        [InlineData("SortedContiguous")]
        [InlineData("UnsortedContiguous")]
        public void CanMergeRanges(string name)
        {
            var ranges = TestCases[name];
            var actual = Range.Merge(ranges);
            var expected = Range.Create(1, 9);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("Disjoined")]
        [InlineData("Overlapping")]
        public void CannotMergeRanges(string name)
        {
            var ranges = TestCases[name];
            Assert.Throws<ArgumentException>(() => Range.Merge(ranges));
        }

        [Fact]
        public void CanMergeWithAbutRange()
        {
            var x = Range.Create(1, 5);
            var y = Range.Create(5, 8);
            var z = Range.Create(1, 8);

            Assert.Equal(z, x.MergeWith(y));
            Assert.Equal(z, y.MergeWith(x));
        }

        [Fact]
        public void CanMergeWithOverlappingRange()
        {
            var x = Range.Create(1, 5);
            var y = Range.Create(4, 8);
            var z = Range.Create(1, 8);

            Assert.Equal(z, x.MergeWith(y));
            Assert.Equal(z, y.MergeWith(x));
        }

        [Fact]
        public void CheckPartialMerge()
        {
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
            var actual = data.PartialMerge();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckPartialMergeOnOverlappingRanges()
        {
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
            var actual = data.PartialMerge();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData("GetIntersectingRanges")]
        public void UnionOfIntersectingRangesReturnsTheHullRange(Range<int> x, Range<int> y)
        {
            Assert.Equal(Range.Hull(x, y), Range.Union(x, y));
        }

        [Theory]
        [MemberData("GetDisjoinedgRanges")]
        public void UnionOfDisjoinedRangesThrows(Range<int> x, Range<int> y)
        {
            Assert.Throws<ArgumentException>(() => Range.Union(x, y));
        }

        public static IEnumerable<object[]> GetIntersectingRanges()
        {
            yield return new object[] { Range.Create(1, 5), Range.Create(4, 7) };
            yield return new object[] { Range.Create(1, 5), Range.Create(2, 4) };
            yield return new object[] { Range.Create(3, 5), Range.Create(2, 4) };
            yield return new object[] { Range.Create(1, 5), Range.Create(5, 7) };
        }

        public static IEnumerable<object[]> GetDisjoinedgRanges()
        {
            yield return new object[] { Range.Create(1, 5), Range.Create(7, 8) };
            yield return new object[] { Range.Create(7, 8), Range.Create(1, 5) };
        }
    }
}
