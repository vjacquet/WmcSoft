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
using System.Text;

namespace WmcSoft
{
    /// <summary>
    /// Stores a range of objects within a single object. This class is useful to use as the
    /// T of a list.
    /// </summary>
    /// <remarks>This class is immutable.</remarks>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    [ImmutableObject(true)]
    public struct Range<T> :
        IComparable,
        IComparable<Range<T>>,
        IEquatable<Range<T>>
        where T : IComparable<T>
    {
        #region Special cases

        public static readonly Range<T> Empty = new Range<T>();

        #endregion

        #region Public Fields

        /// <summary>
        /// The lower bound of the range.
        /// </summary>
        public T Lower { get { return _lower; } }
        readonly T _lower;

        /// <summary>
        /// The upper bound of the range.
        /// </summary>
        public T Upper { get { return _upper; } }
        readonly T _upper;

        #endregion

        #region Lifecycle

        /// <summary>
        /// Creates a new range with given lower and upper bounds.
        /// </summary>
        /// <param name="x">The lower bound of the range.</param>
        /// <param name="y">The upper bound of the range.</param>
        public Range(T x, T y) {
            if (x.CompareTo(y) > 0) {
                _lower = y;
                _upper = x;
            } else {
                _lower = x;
                _upper = y;
            }
        }

        #endregion

        #region Properties

        public bool IsEmpty {
            get {
                return _upper.CompareTo(_lower) <= 0;
            }
        }

        #endregion

        #region Methods

        public bool IsLower(T value) {
            return value.CompareTo(_upper) > 0;
        }

        public bool IsUpper(T value) {
            return value.CompareTo(_lower) < 0;
        }

        public bool IsAdjacent(Range<T> other) {
            return _upper.CompareTo(other._lower) == 0
                || _lower.CompareTo(other._upper) == 0;
        }

        public bool Includes(Range<T> other) {
            return other.IsLower(_upper) && other.IsUpper(_lower);
        }

        public bool Includes<S>(T value, S strategy) where S : IBoundStrategy<T> {
            return strategy.IsWithinRange(value, _lower, _upper);
        }

        public bool Includes(T value) {
            return Includes(value, BoundStrategy<T>.Inclusive);
        }

        public bool Overlaps(Range<T> other) {
            return (this.IsLower(other._upper) && this.Includes(other._lower))
            || (this.IsUpper(other._lower) && this.Includes(other._upper));
        }

        public bool IsDistinct(Range<T> other) {
            return other.IsLower(_lower) || other.IsUpper(_upper);
        }

        public Range<T> GapBetween(Range<T> other) {
            if (this.Overlaps(other))
                return Range<T>.Empty;
            else if (this < other)
                return new Range<T>(_upper, other._lower);
            else
                return new Range<T>(other._upper, _lower);
        }

        public Range<T> Merge(Range<T> other) {
            var ranges = new[] { this, other };
            return Merge(ranges);
        }

        public static bool IsContiguous(IEnumerable<Range<T>> enumerable) {
            List<Range<T>> list = new List<Range<T>>(enumerable);
            list.Sort();
            return IsContiguous(list);
        }

        private static bool IsContiguous(List<Range<T>> list) {
            for (int i = 1; i < list.Count; i++) {
                if (!list[i - 1].IsAdjacent(list[i]))
                    return false;
            }
            return true;
        }

        public static Range<T> Merge(IEnumerable<Range<T>> enumerable) {
            List<Range<T>> list = new List<Range<T>>(enumerable);
            list.Sort();
            if (!IsContiguous(list))
                throw new ArgumentException("Unable to merge ranges", "enumerable");
            return new Range<T>(list[0].Lower, list[list.Count - 1].Upper);
        }

        private static Range<T> Merge(List<Range<T>> list) {
            return new Range<T>(list[0].Lower, list[list.Count - 1].Upper);
        }

        public bool PartitionedBy(IEnumerable<Range<T>> enumerable) {
            List<Range<T>> list = new List<Range<T>>(enumerable);
            list.Sort();
            if (!IsContiguous(list))
                return false;
            return this.Equals(Merge(list));
        }

        #endregion

        #region Overridables

        /// <summary>
        /// Returns a string representation of the range. The string representation of the range is
        /// of the form:
        /// <enumerable>[{0}, {1}]</enumerable>
        /// where {0} is the isBusy of First.ToString(), and {1} is the isBusy of Second.ToString() (or
        /// empty if they are null.)
        /// </summary>
        /// <returns> The string representation of the range.</returns>
        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            builder.Append('[');
            if (_lower != null) {
                builder.Append(_lower.ToString());
            }
            builder.Append(", ");
            if (_upper != null) {
                builder.Append(_upper.ToString());
            }
            builder.Append(']');
            return builder.ToString();
        }

        /// <summary>
        /// Returns a hash code for the range, suitable for use in a hash-table or other hashed list.
        /// Two ranges that compare equal (using Equals) will have the same hash code. The hash code for
        /// the range is derived by combining the hash codes for each of the two elements of the range.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode() {
            int hashFirst = (_lower == null) ? 0x0 : _lower.GetHashCode();
            int hashSecond = (_upper == null) ? 0x0 : _upper.GetHashCode();
            return hashFirst ^ hashSecond;
        }

        /// <summary>Indicates whether the current range is equal to another range of the same type.</summary>
        /// <param name="other">A range to compare with this range.</param>
        /// <returns>true if the current range is equal to the other parameter; otherwise, false.</returns>
        public override bool Equals(object obj) {
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
        public bool Equals(Range<T> other) {
            var comparer = EqualityComparer<T>.Default;
            return comparer.Equals(_lower, other._lower)
                && comparer.Equals(_upper, other._upper);
        }

        #endregion

        #region IComparable<Range<T>> Members

        /// <summary>Compares the current range with another range of the same type.</summary>
        /// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: 
        /// <list type="table">
        /// <listheader>
        ///   <description>Value</description>
        ///   <description>Meaning</description>
        /// </listheader>
        /// <item>
        ///   <description>Less than zero</description>
        ///   <description> This instance is less than obj.</description>
        /// </item>
        /// <item>
        ///   <description>Zero</description>
        ///   <description>This instance is equal to obj.</description>
        /// </item>
        /// <item>
        ///   <description>Greater than zero</description>
        ///   <description>This instance is greater than obj.</description>
        /// </item>
        /// </list>
        /// </returns>
        /// <param name="obj">An object to compare with this instance. </param>
        /// <exception cref="T:System.ArgumentException">obj is not the same type as this instance. </exception>
        public int CompareTo(Range<T> other) {
            var comparer = Comparer<T>.Default;
            int firstCompare = comparer.Compare(_lower, other._lower);
            if (firstCompare == 0)
                return comparer.Compare(_upper, other._upper);
            return firstCompare;
        }

        #endregion

        #region IComparable Members

        /// <summary>Compares the current range with another range of the same type.</summary>
        /// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: 
        /// <list type="table">
        /// <listheader>
        ///   <description>Value</description>
        ///   <description>Meaning</description>
        /// </listheader>
        /// <item>
        ///   <description>Less than zero</description>
        ///   <description> This instance is less than obj.</description>
        /// </item>
        /// <item>
        ///   <description>Zero</description>
        ///   <description>This instance is equal to obj.</description>
        /// </item>
        /// <item>
        ///   <description>Greater than zero</description>
        ///   <description>This instance is greater than obj.</description>
        /// </item>
        /// </list>
        /// </returns>
        /// <param name="obj">An object to compare with this instance. </param>
        /// <exception cref="T:System.ArgumentException">obj is not the same type as this instance. </exception>
        int IComparable.CompareTo(object obj) {
            if (obj == null || obj.GetType() != typeof(Range<T>))
                return 1;

            var range = (Range<T>)obj;
            return CompareTo(range);
        }

        #endregion

        #region Operators

        /// <summary>
        /// Determines whether the specified ranges are equal.
        /// </summary>
        /// <param name="x">The first range to compare.</param>
        /// <param name="y">The second range to compare.</param>
        /// <returns>true if the specified ranges are equal; otherwise, false.</returns>
        public static bool operator ==(Range<T> x, Range<T> y) {
            return x.Equals(y);
        }

        /// <summary>
        /// Determines whether the specified ranges are not equal.
        /// </summary>
        /// <param name="x">The first range to compare.</param>
        /// <param name="y">The second range to compare.</param>
        /// <returns>true if the specified ranges are not equal; otherwise, false.</returns>
        public static bool operator !=(Range<T> x, Range<T> y) {
            return !x.Equals(y);
        }

        /// <summary>
        /// Determines whether the first range is less than the second.
        /// </summary>
        /// <param name="x">The first range to compare.</param>
        /// <param name="y">The second range to compare.</param>
        /// <returns>true if the specified ranges are equal; otherwise, false.</returns>
        public static bool operator <(Range<T> x, Range<T> y) {
            return x.CompareTo(y) < 0;
        }

        /// <summary>
        /// Determines whether the first range is less or equal than the second.
        /// </summary>
        /// <param name="x">The first range to compare.</param>
        /// <param name="y">The second range to compare.</param>
        /// <returns>true if the specified ranges are equal; otherwise, false.</returns>
        public static bool operator <=(Range<T> x, Range<T> y) {
            return x.CompareTo(y) <= 0;
        }

        /// <summary>
        /// Determines whether the first range is greater than the second.
        /// </summary>
        /// <param name="x">The first range to compare.</param>
        /// <param name="y">The second range to compare.</param>
        /// <returns>true if the specified ranges are equal; otherwise, false.</returns>
        public static bool operator >(Range<T> x, Range<T> y) {
            return x.CompareTo(y) > 0;
        }

        /// <summary>
        /// Determines whether the first range is less than the second.
        /// </summary>
        /// <param name="x">The first range to compare.</param>
        /// <param name="y">The second range to compare.</param>
        /// <returns>true if the specified ranges are equal; otherwise, false.</returns>
        public static bool operator >=(Range<T> x, Range<T> y) {
            return x.CompareTo(y) >= 0;
        }

        #endregion

    }
}
