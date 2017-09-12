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
using System.Runtime.CompilerServices;
using System.Text;

namespace WmcSoft
{
    /// <summary>
    /// Stores a range of values within a single object. This class is useful to use as the
    /// <typeparamref name="T"/> of a list.
    /// </summary>
    /// <remarks><see cref="Lower"/> is included while <see cref="Upper"/> is not.</remarks>
    /// <typeparam name="T">The type of items in the range.</typeparam>
    [Serializable]
    [ImmutableObject(true)]
    [DebuggerDisplay("{ToString(\"M\"),nq}")]
    public struct Range<T> : IEquatable<Range<T>>, IFormattable
        where T : IComparable<T>
    {
        // Comparing with https://martinfowler.com/eaaDev/Range.html

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

        internal Range(T lower, T upper, Range.UninitializedTag tag)
        {
            Lower = lower;
            Upper = upper;
        }

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

        public bool Abuts(Range<T> other)
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

        public Range<T> MergeWith(Range<T> other)
        {
            if (!TryMergeWith(other, out Range<T> result))
                throw new ArgumentException("Unable to merge ranges", nameof(other));
            return result;
        }

        public bool TryMergeWith(Range<T> other, out Range<T> result)
        {
            if (Lower.CompareTo(other.Lower) < 0)
                return Range.UnguardedTryMerge(this, other, out result);
            return Range.UnguardedTryMerge(other, this, out result);
        }

        public bool PartitionedBy(IEnumerable<Range<T>> enumerable)
        {
            return Range.TryMerge(enumerable, out Range<T> dummy);
        }

        #endregion

        #region Overridables

        /// <summary>
        /// Formats the value of the current instance using the specified format.
        /// </summary>
        /// <param name="format">
        ///   The format to use.
        ///   -or- 
        ///   A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="IFormattable"/> implementation.
        /// </param>
        /// <param name="formatProvider">
        ///   The provider to use to format the value.
        ///   -or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting
        ///   of the operating system.
        /// </param>
        /// <returns>The value of the current instance in the specified format.</returns>
        public string ToString(string format, IFormatProvider formatProvider = null)
        {
            format = format ?? "G";

            var builder = new StringBuilder();
            builder.Append('[');
            builder.Append(Lower);
            builder.Append("; ");
            builder.Append(Upper);
            builder.Append(GetClosing(format));
            return builder.ToString();
        }

        static char GetClosing(string format)
        {
            switch (format) {
            case "G":
            case "M":
            case "m":
                return ')';
            case "B":
            case "b":
                return '[';
            default:
                throw new FormatException();
            }
        }

        /// <summary>
        /// Formats the value of the current instance.
        /// </summary>
        /// <param name="formatProvider">
        ///   The provider to use to format the value.
        ///   -or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting
        ///   of the operating system.
        /// </param>
        /// <returns>The value of the current instance in the specified format.</returns>
        public string ToString(IFormatProvider formatProvider)
        {
            return ToString(null, formatProvider);
        }


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
            return ToString(null, null);
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
        internal struct UninitializedTag { }
        static readonly UninitializedTag uninitialized = default(UninitializedTag);

        /// <summary>
        /// Creates a new range with given lower and upper bounds.
        /// </summary>
        /// <typeparam name="T">The type of items in the range.</typeparam>
        /// <param name="lower">The lower bound of the range.</param>
        /// <param name="upper">The upper bound of the range.</param>
        /// <exception cref="ArgumentException"><paramref name="lower"/> is not less than or equal to <paramref name="upper"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<T> Create<T>(T lower, T upper)
            where T : IComparable<T>
        {
            return new Range<T>(lower, upper);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static T Min<T>(T x, T y) where T : IComparable<T>
        {
            return y.CompareTo(x) < 0 ? y : x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static T Max<T>(T x, T y) where T : IComparable<T>
        {
            return y.CompareTo(x) < 0 ? x : y;
        }

        /// <summary>
        /// Returns the intersection of both ranges.
        /// </summary>
        /// <typeparam name="T">The type of items in the range.</typeparam>
        /// <param name="x">The first range.</param>
        /// <param name="y">The second range.</param>
        /// <returns>The intersection of both ranges.</returns>
        public static Range<T> Intersect<T>(Range<T> x, Range<T> y)
            where T : IComparable<T>
        {
            if (x.Upper.CompareTo(y.Lower) < 0 || x.Lower.CompareTo(y.Upper) > 0)
                return Range<T>.Empty;
            return new Range<T>(Max(x.Lower, y.Lower), Min(x.Upper, y.Upper), uninitialized);
        }

        /// <summary>
        /// Returns the union of both ranges.
        /// </summary>
        /// <typeparam name="T">The type of items in the range.</typeparam>
        /// <param name="x">The first range.</param>
        /// <param name="y">The second range.</param>
        /// <returns>The union of both ranges.</returns>
        /// <exception cref="ArgumentException"><paramref name="x"/> and <paramref name="y"/> does not intersect.</exception>
        public static Range<T> Union<T>(Range<T> x, Range<T> y)
            where T : IComparable<T>
        {
            if (x.Upper.CompareTo(y.Lower) < 0 || x.Lower.CompareTo(y.Upper) > 0)
                throw new ArgumentException();

            return new Range<T>(Min(x.Lower, y.Lower), Max(x.Upper, y.Upper), uninitialized);
        }

        /// <summary>
        /// Returns the smallest range that includes both ranges.
        /// </summary>
        /// <typeparam name="T">The type of items in the range.</typeparam>
        /// <param name="x">The first range.</param>
        /// <param name="y">The second range.</param>
        /// <returns>The smallest range that includes both ranges.</returns>
        public static Range<T> Hull<T>(Range<T> x, Range<T> y)
            where T : IComparable<T>
        {
            return new Range<T>(Min(x.Lower, y.Lower), Max(x.Upper, y.Upper), uninitialized);
        }

        #region Extensions on enumerable or collections

        internal static bool UnguardedIsContiguous<T>(IList<Range<T>> list)
            where T : IComparable<T>
        {
            // requires list is sorted
            for (int i = 1; i < list.Count; i++) {
                if (!list[i - 1].Abuts(list[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determines wether the ranges are contiguous.
        /// </summary>
        /// <typeparam name="T">The type of items in the range.</typeparam>
        /// <param name="ranges">The ranges.</param>
        /// <returns><c>true</c> if <paramref name="ranges"/> are contiguous; otherwise <c>false</c>.</returns>
        public static bool IsContiguous<T>(IEnumerable<Range<T>> ranges)
            where T : IComparable<T>
        {
            if (ranges == null) throw new ArgumentNullException(nameof(ranges));

            var list = new List<Range<T>>(ranges);
            list.Sort(RangeComparer<T>.Lexicographical);
            return UnguardedIsContiguous(list);
        }

        private static Range<T> UnguardedMerge<T>(IList<Range<T>> list)
            where T : IComparable<T>
        {
            // requires list is sorted
            return new Range<T>(list[0].Lower, list[list.Count - 1].Upper, uninitialized);
        }

        /// <summary>
        /// Merges the ranges.
        /// </summary>
        /// <typeparam name="T">The type of items in the range.</typeparam>
        /// <param name="ranges">The ranges.</param>
        /// <returns>The merged range.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ranges"/> is null.</exception>
        /// <exception cref="ArgumentException">There are gaps between the <paramref name="ranges"/>.</exception>
        public static Range<T> Merge<T>(this IEnumerable<Range<T>> ranges)
            where T : IComparable<T>
        {
            if (!TryMerge(ranges, out Range<T> result))
                throw new ArgumentException("Unable to merge ranges", nameof(ranges));
            return result;
        }

        internal static bool UnguardedTryMerge<T>(Range<T> x, Range<T> y, out Range<T> result)
            where T : IComparable<T>
        {
            if (x.Upper.CompareTo(y.Lower) >= 0) {
                result = new Range<T>(x.Lower, Max(x.Upper, y.Upper), uninitialized);
                return true;
            }
            result = default(Range<T>);
            return false;
        }

        static bool UnguardedTryMerge<T>(IList<Range<T>> list, out Range<T> result)
            where T : IComparable<T>
        {
            // requires list is sorted and has more than one item
            var lower = list[0].Lower;
            var upper = list[0].Upper;
            var i = 1;
            for (; i < list.Count; i++) {
                if (list[i].Lower.CompareTo(upper) > 0) {
                    result = default(Range<T>);
                    return false;
                } else if (list[i].Upper.CompareTo(upper) >= 0) {
                    upper = list[i].Upper;
                }
            }
            result = new Range<T>(lower, upper, uninitialized);
            return true;
        }
        /// <summary>
        /// Tries to merge the ranges.
        /// </summary>
        /// <typeparam name="T">The type of items in the range.</typeparam>
        /// <param name="ranges">The ranges.</param>
        /// <param name="result">The merged range.</param>
        /// <returns><c>true</c> if <paramref name="ranges"/> can be merged; otherwise <c>false</c>.</returns>
        public static bool TryMerge<T>(IEnumerable<Range<T>> ranges, out Range<T> result)
            where T : IComparable<T>
        {
            if (ranges == null) throw new ArgumentNullException(nameof(ranges));

            result = default(Range<T>);
            var list = new List<Range<T>>(ranges);
            if (list.Count == 0)
                return false;
            list.Sort(RangeComparer<T>.Lexicographical);
            return UnguardedTryMerge(list, out result);
        }

        static IEnumerable<Range<T>> UnguardedPartialMerge<T>(IList<Range<T>> list)
            where T : IComparable<T>
        {
            // requires list is sorted and has more than one item
            var lower = list[0].Lower;
            var upper = list[0].Upper;
            var i = 1;
            for (; i < list.Count; i++) {
                if (list[i].Lower.CompareTo(upper) > 0) {
                    yield return new Range<T>(lower, upper, uninitialized);
                    lower = list[i].Lower;
                    upper = list[i].Upper;
                } else if (list[i].Upper.CompareTo(upper) >= 0) {
                    upper = list[i].Upper;
                }
            }
            yield return new Range<T>(lower, upper, uninitialized);
        }

        /// <summary>
        /// Merges the ranges that can be merged.
        /// </summary>
        /// <typeparam name="T">The type of items in the range.</typeparam>
        /// <param name="ranges">The ranges.</param>
        /// <returns>The merged ranges.</returns>
        public static IEnumerable<Range<T>> PartialMerge<T>(this IEnumerable<Range<T>> ranges)
            where T : IComparable<T>
        {
            if (ranges == null) throw new ArgumentNullException(nameof(ranges));

            var list = new List<Range<T>>(ranges);
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
