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
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace WmcSoft
{
    /// <summary>
    /// Defines an order on elements with means to jump from one to another.
    /// </summary>
    /// <typeparam name="T">The type of items to order.</typeparam>
    /// <remarks>The Compare method returns the distance between two elements.</remarks>
    public interface IOrdinal<T> : IComparer<T>
    {
        T Advance(T x, int n);
    }

    public struct Int32Ordinal : IOrdinal<int>
    {
        #region IOrdinal<int> Members

        public int Advance(int x, int n) {
            return checked(x + n);
        }

        #endregion

        #region IComparer<int> Members

        public int Compare(int x, int y) {
            return checked(x - y);
        }

        #endregion
    }

    public struct Int64Ordinal : IOrdinal<long>
    {
        #region IOrdinal<long> Members

        public long Advance(long x, int n) {
            return checked(x + n);
        }

        #endregion

        #region IComparer<int> Members

        public int Compare(long x, long y) {
            return checked((int)(x - y));
        }

        #endregion
    }

    public struct DateTimeOrdinal : IOrdinal<DateTime>
    {
        #region IOrdinal<DateTime> Members

        public DateTime Advance(DateTime x, int n) {
            return x.AddDays(n);
        }

        #endregion

        #region IComparer<DateTime> Members

        public int Compare(DateTime x, DateTime y) {
            return (int)Math.Truncate((x - y).TotalDays);
        }

        #endregion
    }

    public struct YearOrdinal : IOrdinal<DateTime>
    {
        #region IOrdinal<DateTime> Members

        public DateTime Advance(DateTime x, int n) {
            return x.AddYears(n);
        }

        #endregion

        #region IComparer<DateTime> Members

        public int Compare(DateTime x, DateTime y) {
            return (x.Year - y.Year);
        }

        #endregion
    }

    public struct SequenceOrdinal<T> : IOrdinal<T>, IEquatable<SequenceOrdinal<T>>, IReadOnlyList<T>
        where T : IEquatable<T>
    {
        readonly T[] _values;

        public SequenceOrdinal(params T[] values) {
            _values = values;
        }

        private int IndexOf(T value) {
            for (int i = 0; i < _values.Length; i++) {
                if (_values[i].Equals(value))
                    return i;
            }
            throw new IndexOutOfRangeException();
        }

        #region IOrdinal<DateTime> Members

        public T Advance(T x, int n) {
            var ix = IndexOf(x);
            return _values[ix + n];
        }

        #endregion

        #region IComparer<T> Members

        public int Compare(T x, T y) {
            var ix = IndexOf(x);
            var iy = IndexOf(y);
            return ix - iy;
        }

        #endregion

        public int Count {
            get { return _values != null ? _values.Length : 0; }
        }

        public T this[int index] {
            get {
                return ((IReadOnlyList<T>)_values)[index];
            }
        }

        public bool Equals(SequenceOrdinal<T> other) {
            if (ReferenceEquals(_values, other._values))
                return true;
            if (Count != other.Count)
                return false;
            for (int i = 0; i < _values.Length; i++) {
                if (!_values[i].Equals(other._values[i]))
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != typeof(SequenceOrdinal<T>))
                return false;
            return Equals((SequenceOrdinal<T>)obj);
        }

        public override int GetHashCode() {
            if (Count == 0)
                return 0;
            return _values.GetHashCode();
        }

        public override string ToString() {
            var sb = new StringBuilder();
            sb.Append('{');
            var length = Count;
            if (length > 0) {
                sb.Append(_values[0]);
                for (int i = 1; i < length; i++) {
                    sb.Append(',');
                    sb.Append(_values[i]);
                }
            }
            sb.Append('}');
            return sb.ToString();
        }

        public IEnumerator<T> GetEnumerator() {
            return ((IReadOnlyList<T>)_values).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IReadOnlyList<T>)_values).GetEnumerator();
        }
    }
}
