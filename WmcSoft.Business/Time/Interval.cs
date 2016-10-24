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

 ****************************************************************************
 * Adapted from Interval.java
 * ---------------------------
 * Copyright (c) 2005 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System;

namespace WmcSoft.Time
{
    [Serializable]
    public struct Interval<T> : IComparable<Interval<T>>, IEquatable<Interval<T>>
        where T : IComparable<T>
    {
        private readonly IntervalLimit<T> _lower;
        private readonly IntervalLimit<T> _upper;

        public Interval(IntervalLimit<T> lower, IntervalLimit<T> upper) {
            _lower = lower;
            _upper = upper;
        }

        public T Lower { get { return _lower.Value; } }
        public T Upper { get { return _upper.Value; } }

        public bool IncludesLowerLimit() {
            return _lower.IsClosed;
        }
        public bool IncludesUpperLimit() {
            return _upper.IsClosed;
        }

        public bool IsOpen() {
            return !IncludesLowerLimit() && !IncludesUpperLimit();
        }
        public bool IsClosed() {
            return IncludesLowerLimit() && IncludesUpperLimit();
        }

        public bool IsEmpty() {
            throw new NotImplementedException();
        }

        public int CompareTo(Interval<T> other) {
            var result = _upper.CompareTo(other._upper);
            if (result != 0)
                return result;
            if (IncludesLowerLimit() && !other.IncludesLowerLimit())
                return -1;
            if (!IncludesLowerLimit() && other.IncludesLowerLimit())
                return 1;
            return _lower.CompareTo(other._lower);
        }

        public bool Equals(Interval<T> other) {
            return CompareTo(other) == 0;
        }

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != typeof(Interval<T>))
                return false;
            var other = (Interval<T>)obj;
            return Equals(other);
        }

        public override int GetHashCode() {
            var h1 = _lower.GetHashCode();
            var h2 = _upper.GetHashCode();
            return h1 ^ h2;
        }

        public override string ToString() {
            return base.ToString();
        }

        public bool Includes(T value) {
            throw new NotImplementedException();
        }

        public bool Covers(T value) {
            throw new NotImplementedException();
        }

        public bool Intersects(Interval<T> other) {
            throw new NotImplementedException();
        }

        public Interval<T> Intersect(Interval<T> other) {
            throw new NotImplementedException();
        }

        public Interval<T> Gap(Interval<T> other) {
            throw new NotImplementedException();
        }

        #region Operators

        public static bool operator ==(Interval<T> a, Interval<T> b) {
            return a.Equals(b);
        }
        public static bool operator !=(Interval<T> a, Interval<T> b) {
            return !a.Equals(b);
        }

        public static bool operator <(Interval<T> x, Interval<T> y) {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(Interval<T> x, Interval<T> y) {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator >(Interval<T> x, Interval<T> y) {
            return x.CompareTo(y) > 0;
        }
        public static bool operator >=(Interval<T> x, Interval<T> y) {
            return x.CompareTo(y) >= 0;
        }

        #endregion
    }

    public static class Interval
    {
        public static Interval<T> Closed<T>(T lower, T upper)
            where T : IComparable<T> {
            return new Interval<T>(IntervalLimit.Lower(true, lower), IntervalLimit.Upper(true, upper));
        }

        public static Interval<T> Open<T>(T lower, T upper)
            where T : IComparable<T> {
            return new Interval<T>(IntervalLimit.Lower(false, lower), IntervalLimit.Upper(false, upper));
        }

        public static Interval<T> Over<T>(T lower, bool lowerIncluded, T upper, bool upperIncluded)
            where T : IComparable<T> {
            return new Interval<T>(IntervalLimit.Lower(lowerIncluded, lower), IntervalLimit.Upper(upperIncluded, upper));
        }
    }
}
