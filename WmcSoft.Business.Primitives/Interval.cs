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
using System.ComponentModel;
using System.Text;

namespace WmcSoft
{
    /// <summary>
    /// Stores a range of values within a single object. 
    /// </summary>
    /// <remarks>This class stores the open/closed state of the bounds.</remarks>
    [Serializable]
    [ImmutableObject(true)]
    public struct Interval<T> : IInterval<T>, IComparable<Interval<T>>, IEquatable<Interval<T>>
        where T : struct, IComparable<T>
    {
        private readonly IntervalLimit<T> _lower;
        private readonly IntervalLimit<T> _upper;

        public Interval(IntervalLimit<T> lower, IntervalLimit<T> upper)
        {
            if (lower.IsUpper) throw new ArgumentException(nameof(lower));
            if (upper.IsLower) throw new ArgumentException(nameof(upper));

            if (lower.HasValue && upper.HasValue && lower.CompareTo(upper) > 0) throw new ArgumentException();

            _lower = lower;
            _upper = upper;
        }

        public void Deconstruct(out IntervalLimit<T> lower, out IntervalLimit<T> upper)
        {
            lower = _lower;
            upper = _upper;
        }

        public T? Lower { get { return (T?)_lower; } }
        public bool HasLowerLimit { get { return _lower.HasValue; } }
        public T GetLowerOrDefault()
        {
            return _lower.GetValueOrDefault();
        }
        public bool IsLowerIncluded {
            get { return _lower.IsClosed; }
        }

        public T? Upper { get { return (T?)_upper; } }
        public bool HasUpperLimit { get { return _upper.HasValue; } }
        public T GetUpperOrDefault()
        {
            return _upper.GetValueOrDefault();
        }
        public bool IsUpperIncluded {
            get { return _lower.IsClosed; }
        }

        public bool IsOpen()
        {
            return !IsLowerIncluded && !IsUpperIncluded;
        }
        public bool IsClosed()
        {
            return IsLowerIncluded && IsUpperIncluded;
        }

        public bool IsEmpty()
        {
            return IsOpen() && _lower.HasValue && _lower.CompareTo(_upper) == 0;
        }

        public bool IsSingleElement()
        {
            return IsClosed() && _lower.HasValue && _lower.CompareTo(_upper) == 0;
        }

        public bool IsBelow(T value)
        {
            if (!HasUpperLimit) return false;
            int comparison = GetUpperOrDefault().CompareTo(value);
            return comparison < 0 || (comparison == 0 && !IsUpperIncluded);
        }

        public bool IsAbove(T value)
        {
            if (!HasLowerLimit) return false;
            int comparison = GetLowerOrDefault().CompareTo(value);
            return comparison < 0 || (comparison == 0 && !IsLowerIncluded);
        }

        public int CompareTo(Interval<T> other)
        {
            var result = _upper.CompareTo(other._upper);
            if (result != 0)
                return result;
            if (IsLowerIncluded && !other.IsLowerIncluded)
                return -1;
            if (!IsLowerIncluded && other.IsLowerIncluded)
                return 1;
            return _lower.CompareTo(other._lower);
        }

        public bool Equals(Interval<T> other)
        {
            switch ((IsEmpty() ? 0 : 1) | (other.IsEmpty() ? 0 : 2)) {
            case 0:
                return true;
            case 1:
            case 2:
                return false;
            }

            switch ((IsSingleElement() ? 0 : 1) | (other.IsSingleElement() ? 0 : 2)) {
            case 0:
                return Lower.Equals(other.Lower);
            case 1:
            case 2:
                return false;
            }

            return CompareTo(other) == 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Interval<T>))
                return false;
            return Equals((Interval<T>)obj);
        }

        public override int GetHashCode()
        {
            var h1 = _lower.GetHashCode();
            var h2 = _upper.GetHashCode();
            return h1 ^ h2;
        }

        public override string ToString()
        {
            if (IsEmpty())
                return "{}";
            if (IsSingleElement())
                return "{" + GetLowerOrDefault() + "}";
            var sb = new StringBuilder();
            if (HasLowerLimit) {
                sb.Append(IsLowerIncluded ? "[" : "(");
                sb.Append(GetLowerOrDefault());
            } else {
                sb.Append("(");
            }
            sb.Append(", ");
            if (HasLowerLimit) {
                sb.Append(GetUpperOrDefault());
                sb.Append(IsUpperIncluded ? "]" : ")");
            } else {
                sb.Append(")");
            }
            return sb.ToString();
        }

        public bool IsBelow(T? value)
        {
            if (!HasUpperLimit) return false;
            if (!value.HasValue)
                return true;
            int comparison = _upper.GetValueOrDefault().CompareTo(value.GetValueOrDefault());
            return comparison < 0 || (comparison == 0 && !IsUpperIncluded);
        }

        public bool IsAbove(T? value)
        {
            if (!HasLowerLimit) return false;
            if (!value.HasValue)
                return true;
            int comparison = _lower.GetValueOrDefault().CompareTo(value.GetValueOrDefault());
            return comparison > 0 || (comparison == 0 && !IsLowerIncluded);
        }

        public bool Includes(T? value)
        {
            return !IsBelow(value) && !IsAbove(value);
        }

        public bool Includes(T value)
        {
            return !_lower.IsOutside(value) && !_upper.IsOutside(value);
        }

        public bool Covers(Interval<T> other)
        {
            //var lowerComparison = _lower.CompareTo(other._lower);
            //var lowerPass = Includes(other.lowerLimit()) || (lowerComparison == 0 && !other.includesLowerLimit());
            //var upperComparison = upperLimit().compareTo(other.upperLimit());
            //var upperPass = this.includes(other.upperLimit()) || (upperComparison == 0 && !other.includesUpperLimit());
            //return lowerPass && upperPass;
            throw new NotImplementedException();
        }

        public bool Intersects(Interval<T> other)
        {
            throw new NotImplementedException();
        }

        public Interval<T> Intersect(Interval<T> other)
        {
            throw new NotImplementedException();
        }

        public Interval<T> Gap(Interval<T> other)
        {
            throw new NotImplementedException();
        }

        #region Operators

        public static bool operator ==(Interval<T> a, Interval<T> b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Interval<T> a, Interval<T> b)
        {
            return !a.Equals(b);
        }

        public static bool operator <(Interval<T> x, Interval<T> y)
        {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(Interval<T> x, Interval<T> y)
        {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator >(Interval<T> x, Interval<T> y)
        {
            return x.CompareTo(y) > 0;
        }
        public static bool operator >=(Interval<T> x, Interval<T> y)
        {
            return x.CompareTo(y) >= 0;
        }

        #endregion

        #region Helpers

        private T? LesserOfLowerLimits(Interval<T> other)
        {
            if (!HasLowerLimit)
                return null;
            int comparison = _lower.CompareTo(other._lower);
            if (comparison <= 0)
                return GetLowerOrDefault();
            return (T?)other._lower;
        }

        private T? GreaterOfLowerLimits(Interval<T> other)
        {
            if (!HasLowerLimit)
                return (T?)other._lower;
            int comparison = _lower.CompareTo(other._lower);
            if (comparison >= 0)
                return GetLowerOrDefault();
            return (T?)other._lower;
        }

        private T? LesserOfUpperLimits(Interval<T> other)
        {
            if (!HasUpperLimit)
                return (T?)other._upper;
            int comparison = _upper.CompareTo(other._upper);
            if (comparison <= 0)
                return GetUpperOrDefault();
            return (T?)other._upper;
        }

        private T? GreaterOfUpperLimits(Interval<T> other)
        {
            if (!HasUpperLimit)
                return (T?)other._upper;
            int comparison = _upper.CompareTo(other._upper);
            if (comparison >= 0)
                return GetUpperOrDefault();
            return (T?)other._upper;
        }

        private bool GreaterOfLowerIncludedInIntersection(Interval<T> other)
        {
            var limit = GreaterOfLowerLimits(other);
            return Includes(limit) && other.Includes(limit);
        }

        private bool LesserOfUpperIncludedInIntersection(Interval<T> other)
        {
            var limit = LesserOfUpperLimits(other);
            return Includes(limit) && other.Includes(limit);
        }

        private bool GreaterOfLowerIncludedInUnion(Interval<T> other)
        {
            var limit = GreaterOfLowerLimits(other);
            return Includes(limit) || other.Includes(limit);
        }

        private bool LesserOfUpperIncludedInUnion(Interval<T> other)
        {
            var limit = LesserOfUpperLimits(other);
            return Includes(limit) || other.Includes(limit);
        }

        #endregion
    }

    public static class Interval
    {
        #region Limits

        public static IntervalLimit<T> LowerLimit<T>(T value, bool exclusive = false)
            where T : struct, IComparable<T>
        {
            return new IntervalLimit<T>(value, true, !exclusive);
        }

        public static IntervalLimit<T> LowerLimit<T>(T? value, bool exclusive = false)
            where T : struct, IComparable<T>
        {
            return value.HasValue
                ? LowerLimit(value.GetValueOrDefault(), exclusive)
                : IntervalLimit<T>.UnboundedLower;
        }


        public static IntervalLimit<T> UpperLimit<T>(T value, bool exclusive = false)
            where T : struct, IComparable<T>
        {
            return new IntervalLimit<T>(value, false, !exclusive);
        }

        public static IntervalLimit<T> UpperLimit<T>(T? value, bool exclusive = false)
            where T : struct, IComparable<T>
        {
            return value.HasValue
                ? UpperLimit(value.GetValueOrDefault(), exclusive)
                : IntervalLimit<T>.UnboundedUpper;
        }

        #endregion

        public static Interval<T> Unbounded<T>()
            where T : struct, IComparable<T>
        {
            return new Interval<T>(IntervalLimit<T>.UnboundedLower, IntervalLimit<T>.UnboundedUpper);
        }

        public static Interval<T> Closed<T>(T lower, T upper)
            where T : struct, IComparable<T>
        {
            return new Interval<T>(LowerLimit(lower), UpperLimit(upper));
        }

        public static Interval<T> Closed<T>(T? lower, T? upper)
            where T : struct, IComparable<T>
        {
            return new Interval<T>(LowerLimit(lower), UpperLimit(upper));
        }

        public static Interval<T> Open<T>(T lower, T upper)
            where T : struct, IComparable<T>
        {
            return new Interval<T>(LowerLimit(lower, exclusive: true), UpperLimit(upper, exclusive: true));
        }

        public static Interval<T> Open<T>(T? lower, T? upper)
            where T : struct, IComparable<T>
        {
            return new Interval<T>(LowerLimit(lower, exclusive: true), UpperLimit(upper, exclusive: true));
        }

        public static Interval<T> LeftClosed<T>(T lower, T upper)
            where T : struct, IComparable<T>
        {
            return new Interval<T>(LowerLimit(lower, exclusive: false), UpperLimit(upper, exclusive: true));
        }

        public static Interval<T> LeftClosed<T>(T? lower, T? upper)
            where T : struct, IComparable<T>
        {
            return new Interval<T>(LowerLimit(lower, exclusive: false), UpperLimit(upper, exclusive: true));
        }

        public static Interval<T> RightOpen<T>(T lower, T upper)
            where T : struct, IComparable<T>
        {
            return new Interval<T>(LowerLimit(lower, exclusive: false), UpperLimit(upper, exclusive: true));
        }

        public static Interval<T> RightOpen<T>(T? lower, T? upper)
            where T : struct, IComparable<T>
        {
            return new Interval<T>(LowerLimit(lower, exclusive: false), UpperLimit(upper, exclusive: true));
        }

        public static Interval<T> RightClosed<T>(T lower, T upper)
            where T : struct, IComparable<T>
        {
            return new Interval<T>(LowerLimit(lower, exclusive: true), UpperLimit(upper, exclusive: false));
        }

        public static Interval<T> RightClosed<T>(T? lower, T? upper)
            where T : struct, IComparable<T>
        {
            return new Interval<T>(LowerLimit(lower, exclusive: true), UpperLimit(upper, exclusive: false));
        }

        public static Interval<T> LeftOpen<T>(T lower, T upper)
            where T : struct, IComparable<T>
        {
            return new Interval<T>(LowerLimit(lower, exclusive: true), UpperLimit(upper, exclusive: false));
        }

        public static Interval<T> LeftOpen<T>(T? lower, T? upper)
            where T : struct, IComparable<T>
        {
            return new Interval<T>(LowerLimit(lower, exclusive: true), UpperLimit(upper, exclusive: false));
        }

        public static Interval<T> Over<T>(T lower, bool lowerExcluded, T upper, bool upperExcluded)
            where T : struct, IComparable<T>
        {
            return new Interval<T>(LowerLimit(lower, lowerExcluded), UpperLimit(upper, upperExcluded));
        }

        public static Interval<T> Over<T>(T? lower, bool lowerExcluded, T? upper, bool upperExcluded)
            where T : struct, IComparable<T>
        {
            return new Interval<T>(LowerLimit(lower, lowerExcluded), UpperLimit(upper, upperExcluded));
        }

        public static Interval<T> Above<T>(T lower, bool exclusive = false)
            where T : struct, IComparable<T>
        {
            return new Interval<T>(LowerLimit(lower, exclusive), IntervalLimit<T>.UnboundedUpper);
        }

        public static Interval<T> Below<T>(T upper, bool exclusive = false)
            where T : struct, IComparable<T>
        {
            return new Interval<T>(IntervalLimit<T>.UnboundedLower, UpperLimit(upper, exclusive));
        }
    }

    [Flags]
    public enum TypeOfInterval
    {
        Open = 0,
        Closed = 3,
        LeftClosed = 2,
        RightOpen = 2,
        RightClosed = 1,
        LeftOpen = 1,
        LeftUnbounded = 5,
        RightUnbounded = 6,
        Unbounded = 4,
    }
}
