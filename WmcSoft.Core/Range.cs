#region Licence

/****************************************************************************
          Copyright 1999-2015 Vincent J. Jacquet.  All rights reserved.

    Permission is granted to anyone to use this software for any purpose on
    any computer system, and to alter it and redistribute it, subject
    to the following restrictions:

    1. The author is not responsible for the consequences of use of this
       software, no matter how awful, even if they arise from flaws in it.

    2. The origin of this software must not be misrepresented, either by
       explicit claim or by omission.  Since few users ever read sources,
       credits must appear in the documentation.

    3. Altered versions must be plainly marked as such, and must not be
       misrepresented as being the original software.  Since few users
       ever read sources, credits must appear in the documentation.

    4. This notice may not be removed or altered.

 ****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using static WmcSoft.Algorithms;

namespace WmcSoft
{
    /// <summary>
    /// Stores a range of values within a single object. This class is useful to use as the
    /// <typeparamref name="T"/> of a list.
    /// </summary>
    /// <remarks><see cref="Lower"/> is included while <see cref="Upper"/> is not.</remarks>
    /// <typeparam name="T">The type</typeparam>
    [Serializable]
    [ImmutableObject(true)]
    [DebuggerDisplay("{ToString(),nq}")]
    public struct Range<T> : IEquatable<Range<T>>
        where T : IComparable<T>
    {
        #region Special cases

        public static readonly Range<T> Empty = new Range<T>();

        #endregion

        #region Public Fields

        /// <summary>
        /// The lower bound of the range.
        /// </summary>
        public T Lower { get; }

        /// <summary>
        /// The upper bound of the range.
        /// </summary>
        public T Upper { get; }

        #endregion

        #region Lifecycle

        /// <summary>
        /// Creates a new range with given lower and upper bounds.
        /// </summary>
        /// <param name="lower">The lower bound of the range.</param>
        /// <param name="upper">The upper bound of the range.</param>
        /// <exception cref="ArgumentException"><paramref name="lower"/> is not less than or equal to <paramref name="upper"/>.</exception>
        public Range(T lower, T upper)
        {
            if (lower == null) throw new ArgumentNullException(nameof(lower));
            if (upper == null) throw new ArgumentNullException(nameof(upper));
            if (lower.CompareTo(upper) > 0) throw new ArgumentException();

            Lower = lower;
            Upper = upper;
        }

        public void Deconstruct(out T lower, out T upper)
        {
            lower = Lower;
            upper = Upper;
        }

        #endregion

        #region Properties

        public bool IsEmpty {
            get { return Upper.CompareTo(Lower) == 0; }
        }

        #endregion

        #region Methods

        public bool IsLowerThan(T value)
        {
            return value.CompareTo(Upper) > 0;
        }

        public bool IsUpperThan(T value)
        {
            return value.CompareTo(Lower) < 0;
        }

        public bool IsAdjacentTo(Range<T> other)
        {
            return Upper.CompareTo(other.Lower) == 0 || Lower.CompareTo(other.Upper) == 0;
        }

        public bool Includes(Range<T> other)
        {
            return other.IsLowerThan(Upper) && other.IsUpperThan(Lower);
        }

        //public bool Includes<TStrategy>(T value, TStrategy strategy) where TStrategy : IBoundStrategy<T> {
        //    return strategy.IsWithinRange(value, Lower, Upper);
        //}

        public bool Includes(T value)
        {
            return Lower.CompareTo(value) <= 0 && Upper.CompareTo(value) > 0;
        }

        public bool Overlaps(Range<T> other)
        {
            return IsLowerThan(other.Upper) && Includes(other.Lower) || IsUpperThan(other.Lower) && Includes(other.Upper);
        }

        public bool IsDistinct(Range<T> other)
        {
            return other.IsLowerThan(Lower) || other.IsUpperThan(Upper);
        }

        public Range<T> GapBetween(Range<T> other)
        {
            if (Overlaps(other))
                return Empty;
            else if (Upper.CompareTo(other.Lower) < 0)
                return new Range<T>(Upper, other.Lower);
            else
                return new Range<T>(other.Upper, Lower);
        }

        public Range<T> Merge(Range<T> other)
        {
            var ranges = new[] { this, other };
            return Merge(ranges);
        }

        public static bool IsContiguous(IEnumerable<Range<T>> enumerable)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));

            var list = new List<Range<T>>(enumerable);
            list.Sort();
            return IsContiguous(list);
        }

        private static bool IsContiguous(IList<Range<T>> list)
        {
            // requires list is sorted
            for (int i = 1; i < list.Count; i++) {
                if (!list[i - 1].IsAdjacentTo(list[i]))
                    return false;
            }
            return true;
        }

        public static Range<T> Merge(IEnumerable<Range<T>> enumerable)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));

            var list = new List<Range<T>>(enumerable);
            list.Sort();
            if (!IsContiguous(list))
                throw new ArgumentException("Unable to merge ranges", nameof(enumerable));
            return Merge(list);
        }

        private static Range<T> Merge(IList<Range<T>> list)
        {
            // requires list is sorted
            return new Range<T>(list[0].Lower, list[list.Count - 1].Upper);
        }

        public bool PartitionedBy(IEnumerable<Range<T>> enumerable)
        {
            var list = new List<Range<T>>(enumerable);
            list.Sort();
            if (!IsContiguous(list))
                return false;
            return Equals(Merge(list));
        }

        #endregion

        #region Overridables

        /// <summary>
        /// Returns a string representation of the range. The string representation of the range is
        /// of the form:
        /// <enumerable>[{0}, {1}]</enumerable>
        /// where {0} is the result of First.ToString(), and {1} is the result of Second.ToString() (or
        /// empty if they are null.)
        /// </summary>
        /// <returns> The string representation of the range.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('[');
            builder.Append(Lower);
            builder.Append(", ");
            builder.Append(Upper);
            builder.Append(')');
            return builder.ToString();
        }

        /// <summary>
        /// Returns a hash code for the range, suitable for use in a hash-table or other hashed list.
        /// Two ranges that compare equal (using Equals) will have the same hash code. The hash code for
        /// the range is derived by combining the hash codes for each of the two elements of the range.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return Lower.GetHashCode() ^ Upper.GetHashCode();
        }

        /// <summary>Indicates whether the current range is equal to another range of the same type.</summary>
        /// <param name="other">A range to compare with this range.</param>
        /// <returns>true if the current range is equal to the other parameter; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Range<T>))
                return false;
            var other = (Range<T>)obj;
            return Equals(other);
        }

        #endregion

        #region IEquatable<Range<T>> Members

        /// <summary>Indicates whether the current range is equal to another range of the same type.</summary>
        /// <param name="other">A range to compare with this range.</param>
        /// <returns>true if the current range is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(Range<T> other)
        {
            var comparer = EqualityComparer<T>.Default;
            return comparer.Equals(Lower, other.Lower)
                && comparer.Equals(Upper, other.Upper);
        }

        #endregion

        #region Operators

        /// <summary>
        /// Determines whether the specified ranges are equal.
        /// </summary>
        /// <param name="x">The first range to compare.</param>
        /// <param name="y">The second range to compare.</param>
        /// <returns>true if the specified ranges are equal; otherwise, false.</returns>
        public static bool operator ==(Range<T> x, Range<T> y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Determines whether the specified ranges are not equal.
        /// </summary>
        /// <param name="x">The first range to compare.</param>
        /// <param name="y">The second range to compare.</param>
        /// <returns>true if the specified ranges are not equal; otherwise, false.</returns>
        public static bool operator !=(Range<T> x, Range<T> y)
        {
            return !x.Equals(y);
        }

        #endregion
    }

    public static class Range
    {
        public static Range<T> Create<T>(T x, T y)
            where T : IComparable<T>
        {
            return new Range<T>(x, y);
        }

        public static Range<T> Inflate<T, O>(this Range<T> range, int delta, O ordinal)
            where T : IComparable<T>
            where O : IOrdinal<T>
        {
            return new Range<T>(ordinal.Advance(range.Lower, -delta), ordinal.Advance(range.Upper, delta));
        }

        public static Range<T> Shift<T, O>(this Range<T> range, int delta, O ordinal)
            where T : IComparable<T>
            where O : IOrdinal<T>
        {
            return new Range<T>(ordinal.Advance(range.Lower, delta), ordinal.Advance(range.Upper, delta));
        }

        public static Range<T> Intersect<T>(Range<T> x, Range<T> y)
            where T : IComparable<T>
        {
            if (x.Upper.CompareTo(y.Lower) < 0 || x.Lower.CompareTo(y.Upper) > 0)
                return Range<T>.Empty;
            return new Range<T>(Max(x.Lower, y.Lower), Min(x.Upper, y.Upper));
        }

        #region Extensions on enumerable or collections

        static IEnumerable<Range<T>> UnguardedPartialMerge<T>(IList<Range<T>> list)
            where T : IComparable<T>
        {
            // requires list is sorted as has more than one item
            var lower = list[0].Lower;
            var upper = list[0].Upper;
            var i = 1;
            for (; i < list.Count; i++) {
                if (list[i].Lower.CompareTo(upper) > 0) {
                    yield return new Range<T>(lower, upper);
                    lower = list[i].Lower;
                    upper = list[i].Upper;
                } else if (list[i].Upper.CompareTo(upper) >= 0) {
                    upper = list[i].Upper;
                }
            }
            yield return new Range<T>(lower, upper);
        }

        public static IEnumerable<Range<T>> PartialMerge<T>(this IEnumerable<Range<T>> source)
            where T : IComparable<T>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var list = new List<Range<T>>(source);
            switch (list.Count) {
            case 0:
                return Enumerable.Empty<Range<T>>();
            case 1:
                return list;
            default:
                list.Sort(RangeComparer<T>.Lexicographical);
                return UnguardedPartialMerge(list);
            }
        }

        #endregion
    }
}
