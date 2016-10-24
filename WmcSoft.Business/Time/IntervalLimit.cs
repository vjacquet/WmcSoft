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
 * Adapted from DateSpecification.java
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
    public struct IntervalLimit<T> : IComparable<IntervalLimit<T>>, IEquatable<IntervalLimit<T>>
        where T : IComparable<T>
    {
        public static readonly IntervalLimit<T> Undefined;

        [Flags]
        internal enum State
        {
            None = 0,
            Open = 1,
            Closed = 2,
            Upper = 4,
            Lower = 8,
        }

        private readonly T _value;
        private readonly State _state;

        public IntervalLimit(bool closed, bool lower, T value) {
            _state = value == null
                ? State.None
                : (closed ? State.Closed : State.Open) | (lower ? State.Lower : State.Upper);
            _value = value;
        }

        public bool IsLower { get { return (_state & State.Lower) != 0; } }
        public bool IsUpper { get { return (_state & State.Upper) != 0; } }
        public bool IsClosed { get { return (_state & State.Closed) != 0; } }
        public bool IsOpen { get { return (_state & State.Open) != 0; } }

        public T Value { get { return _value; } }
        public bool HasValue { get { return _state != State.None; } }

        public int CompareTo(IntervalLimit<T> other) {
            if (!HasValue) {
                if (!other.HasValue)
                    return 0;
                return IsLower ? -1 : 1;
            }
            if (!other.HasValue)
                return other.IsLower ? 1 : -1;
            return _value.CompareTo(other._value);
        }

        public bool Equals(IntervalLimit<T> other) {
            return CompareTo(other) == 0;
        }

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != typeof(IntervalLimit<T>))
                return false;
            var other = (IntervalLimit<T>)obj;
            return Equals(other);
        }

        public override int GetHashCode() {
            var h1 = _state.GetHashCode();
            var h2 = _value.GetHashCode();
            return (((h1 << 5) + h1) ^ h2);
        }

        #region Operators

        public static bool operator ==(IntervalLimit<T> a, IntervalLimit<T> b) {
            return a.Equals(b);
        }
        public static bool operator !=(IntervalLimit<T> a, IntervalLimit<T> b) {
            return !a.Equals(b);
        }

        public static bool operator <(IntervalLimit<T> x, IntervalLimit<T> y) {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(IntervalLimit<T> x, IntervalLimit<T> y) {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator >(IntervalLimit<T> x, IntervalLimit<T> y) {
            return x.CompareTo(y) > 0;
        }
        public static bool operator >=(IntervalLimit<T> x, IntervalLimit<T> y) {
            return x.CompareTo(y) >= 0;
        }

        #endregion
    }

    public static class IntervalLimit
    {
        public static IntervalLimit<T> Lower<T>(bool closed, T value)
            where T : IComparable<T> {
            return new IntervalLimit<T>(closed, true, value);
        }

        public static IntervalLimit<T> Upper<T>(bool closed, T value)
            where T : IComparable<T> {
            return new IntervalLimit<T>(closed, false, value);
        }
    }
}