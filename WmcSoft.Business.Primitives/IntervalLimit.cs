﻿#region Licence

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
 * Adapted from IntervalLimit.java
 * -------------------------------
 * Copyright (c) 2005 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System;
using System.ComponentModel;

namespace WmcSoft
{
    /// <summary>
    /// Represents a limit for an interval.
    /// </summary>
    /// <typeparam name="T">The type of the limit value.</typeparam>
    [Serializable]
    [ImmutableObject(true)]
    public struct IntervalLimit<T> : IComparable<IntervalLimit<T>>, IEquatable<IntervalLimit<T>>
        where T : struct, IComparable<T>
    {
        public static readonly IntervalLimit<T> Undefined;
        public static readonly IntervalLimit<T> Unbounded;
        public static readonly IntervalLimit<T> UnboundedLower = new IntervalLimit<T>(State.Lower, default(T));
        public static readonly IntervalLimit<T> UnboundedUpper = new IntervalLimit<T>(State.Upper, default(T));

        [Flags]
        internal enum State
        {
            None = 0,
            Closed = 1,
            Open = 2,
            Lower = 4,
            Upper = 8,
        }

        private readonly T _value;
        private readonly State _state;

        private IntervalLimit(State state, T value)
        {
            _value = value;
            _state = state;
        }

        public IntervalLimit(T value, bool lower, bool closed)
            : this((closed ? State.Closed : State.Open) | (lower ? State.Lower : State.Upper), value)
        {
        }

        public bool IsLower => (_state & State.Lower) != 0;
        public bool IsUpper => (_state & State.Upper) != 0;
        public bool IsClosed => (_state & State.Closed) != 0;
        public bool IsOpen => (_state & State.Open) != 0;

        /// <summary>
        /// Gets a value indicating whether the current <see cref="IntervalLimit{T}"/> object has a valid value of its underlying type.
        /// </summary>
        public bool HasValue => (_state & (State.Closed | State.Open)) != State.None;

        /// <summary>
        /// Gets the value of the current <see cref="IntervalLimit{T}"/> object if it has been assigned a valid underlying value.
        /// </summary>
        public T Value {
            get {
                if (!HasValue)
                    throw new InvalidOperationException();
                return _value;
            }
        }

        /// <summary>
        /// Retrieves the value of the current <see cref="IntervalLimit{T}"/> object, or the object's default value.
        /// </summary>
        /// <returns>The value of the <see cref="Value"/> property if the <see cref="HasValue"/> property is <c>true</c>;
        /// otherwise, the default value of the current <see cref="IntervalLimit{T}"/> object.
        /// The type of the default value is the type argument of the current <see cref="IntervalLimit{T}"/> object, and the value of the default value consists solely of binary zeroes.</returns>
        public T GetValueOrDefault()
        {
            return _value;
        }

        /// <summary>
        /// Retrieves the value of the current <see cref="IntervalLimit{T}"/> object, or the specified default value.
        /// </summary>
        /// <param name="defaultValue">A value to return if the <see cref="HasValue"/> property is <c>false</c>.</param>
        /// <returns>The value of the <see cref="Value"/> property if the <see cref="HasValue"/> property is <c>true</c>; otherwise, the <paramref name="defaultValue"/> parameter.</returns>
        public T GetValueOrDefault(T defaultValue)
        {
            return HasValue ? _value: defaultValue;
        }

        internal bool IsOutside(T value)
        {
            switch (_state) {
            case State.Lower | State.Closed:
                return value.CompareTo(_value) < 0;
            case State.Lower | State.Open:
                return value.CompareTo(_value) <= 0;
            case State.Upper | State.Open:
                return value.CompareTo(_value) >= 0;
            case State.Upper | State.Closed:
                return value.CompareTo(_value) > 0;
            default:
                throw new InvalidOperationException();
            }
        }

        public int CompareTo(IntervalLimit<T> other)
        {
            // `null` is lesser than lower bounds, but greater than upper bounds
            // to mimic -∞ < limit < ∞
#if WORKINPROGRESS
            if (_state == other._state) {
                if (HasValue)
                    return _value.CompareTo(other._value);
                return 0;
            }
            if (!HasValue)
                return other.IsLower ? -1 : 1;
            if (!other.HasValue)
                return IsLower ? 1 : -1;

            var comparison = _value.CompareTo(other._value);
            if (comparison != 0)
                return comparison;
            if (IsLower) {
                if (other.IsLower)
                    return IsClosed ? -1 : 1;
                if (IsClosed)
                    return other.IsClosed ? 0 : -1;
                return other.IsClosed ? 1 : 0;
            } else {
                if (other.IsUpper)
                    return IsClosed ? 1 : -1;
                if (IsClosed)
                    return other.IsClosed ? 0 : 1;
                return other.IsClosed ? -1 : 0;
            }
#else
            if (!HasValue) {
                if (!other.HasValue)
                    return 0;
                if (IsLower)
                    return -1;
                return other.IsUpper ? 1 : -1;
            }
            if (!other.HasValue) {
                if (other.IsUpper)
                    return 1;
                return IsUpper ? -1 : 1;
            }
            // should the limit be equal when only the value are equal?
            return _value.CompareTo(other._value);
#endif
        }

        public bool Equals(IntervalLimit<T> other)
        {
#if WORKINPROGRESS
            return _state == other._state && _value.Equals(other._state);
#else
            return CompareTo(other) == 0;
#endif
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(IntervalLimit<T>))
                return false;
            var other = (IntervalLimit<T>)obj;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked {
                var h1 = _state.GetHashCode();
                var h2 = _value.GetHashCode();
                return (((h1 << 5) + h1) ^ h2);
            }
        }

        #region Operators

        public static explicit operator T? (IntervalLimit<T> x)
        {
            if (x.HasValue)
                return x.Value;
            return null;
        }

        public static bool operator ==(IntervalLimit<T> a, IntervalLimit<T> b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(IntervalLimit<T> a, IntervalLimit<T> b)
        {
            return !a.Equals(b);
        }

        public static bool operator <(IntervalLimit<T> x, IntervalLimit<T> y)
        {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(IntervalLimit<T> x, IntervalLimit<T> y)
        {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator >(IntervalLimit<T> x, IntervalLimit<T> y)
        {
            return x.CompareTo(y) > 0;
        }
        public static bool operator >=(IntervalLimit<T> x, IntervalLimit<T> y)
        {
            return x.CompareTo(y) >= 0;
        }

        #endregion
    }
}
