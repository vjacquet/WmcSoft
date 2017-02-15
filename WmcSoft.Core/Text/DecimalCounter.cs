#region Licence

/****************************************************************************
          Copyright 1999-2017 Vincent J. Jacquet.  All rights reserved.

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
using System.Globalization;
using System.Text.RegularExpressions;

namespace WmcSoft.Text
{
    public struct DecimalCounter : IComparable<DecimalCounter>, IEquatable<DecimalCounter>
    {
        const char FirstLetter = 'A';

        const int MinStoredValue = 0;
        const int MaxStoredValue = Int32.MaxValue - 1;

        static readonly Regex Validator = new Regex("^[1-9][0-9]*$", RegexOptions.CultureInvariant | RegexOptions.Singleline);

        public static readonly DecimalCounter MinValue = new DecimalCounter(MinStoredValue);
        public static readonly DecimalCounter MaxValue = new DecimalCounter(MaxStoredValue);

        readonly int _storage;

        public DecimalCounter(int value) {
            if (value > MaxStoredValue || value < MinStoredValue)
                throw new OverflowException();
            _storage = value;
        }

        public DecimalCounter(string value) {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (!Validator.IsMatch(value)) throw new ArgumentException(nameof(value));

            var x = Int32.Parse(value, CultureInfo.InvariantCulture);
            if (x > MaxStoredValue || x < MinStoredValue)
                throw new OverflowException();
            _storage = x - 1;
        }

        public int CompareTo(DecimalCounter other) {
            return _storage - other._storage;
        }

        public DecimalCounter Increment(int n) {
            return new DecimalCounter(_storage + n);
        }

        public override int GetHashCode() {
            return _storage;
        }

        public override string ToString() {
            return (_storage + 1).ToString(CultureInfo.InvariantCulture);
        }

        public bool Equals(DecimalCounter other) {
            return _storage == other._storage;
        }

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != typeof(DecimalCounter))
                return false;
            return Equals((DecimalCounter)obj);
        }

        #region Conversion operators

        public static implicit operator int(DecimalCounter x) {
            return x._storage;
        }

        public static implicit operator string(DecimalCounter x) {
            return x.ToString();
        }

        public static implicit operator DecimalCounter(int x) {
            return new DecimalCounter(x);
        }

        public static implicit operator DecimalCounter(string x) {
            return new DecimalCounter(x);
        }

        #endregion

        #region Relational operators

        public static bool operator ==(DecimalCounter a, DecimalCounter b) {
            return a.Equals(b);
        }
        public static bool operator !=(DecimalCounter a, DecimalCounter b) {
            return !a.Equals(b);
        }

        public static bool operator <(DecimalCounter x, DecimalCounter y) {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(DecimalCounter x, DecimalCounter y) {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator >(DecimalCounter x, DecimalCounter y) {
            return x.CompareTo(y) > 0;
        }
        public static bool operator >=(DecimalCounter x, DecimalCounter y) {
            return x.CompareTo(y) >= 0;
        }

        #endregion

        #region Arithmetic operators

        public static DecimalCounter operator +(DecimalCounter x, int n) {
            return x.Increment(n);
        }

        public static DecimalCounter operator -(DecimalCounter x, int n) {
            return x.Increment(-n);
        }

        public static DecimalCounter operator ++(DecimalCounter x) {
            return x.Increment(1);
        }

        public static DecimalCounter operator --(DecimalCounter x) {
            return x.Increment(-1);
        }

        #endregion
    }
}
