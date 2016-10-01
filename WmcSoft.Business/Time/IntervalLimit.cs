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
    public struct IntervalLimit<T>
        : IComparable<IntervalLimit<T>>
        , IEquatable<IntervalLimit<T>>
        where T : IComparable<T>
    {
        [Flags]
        enum State
        {
            Open = 0,
            Closed = 2,
            Upper = 0,
            Lower = 4,
        }

        private readonly T _value;
        private readonly State _state;

        public IntervalLimit(bool closed, bool lower, T value) {
            _state = (closed ? State.Closed : State.Open)
                | (lower ? State.Lower : State.Upper);
            _value = value;
        }

        public bool IsLower { get { return (_state & State.Closed) != 0; } }
        public bool IsUpper { get { return (_state & State.Closed) == 0; } }
        public bool IsClosed { get { return (_state & State.Lower) != 0; } }
        public bool IsOpen { get { return (_state & State.Lower) == 0; } }

        public T Value { get { return _value; } }

        public int CompareTo(IntervalLimit<T> other) {
            if (Value == null) {
                if (other._value == null)
                    return 0;
                return IsLower ? -1 : 1;
            }
            if (other._value == null)
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
}