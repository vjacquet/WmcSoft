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
    public struct DecimalLeadingZeroCounter : IComparable<DecimalLeadingZeroCounter>, IEquatable<DecimalLeadingZeroCounter>
    {
        const int MinStoredValue = 0;
        const int MaxStoredValue = Int32.MaxValue - 1;

        static readonly Regex Validator = new Regex("^(0[1-9])|([1-9][0-9]+)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);

        public static readonly DecimalLeadingZeroCounter MinValue = new DecimalLeadingZeroCounter(MinStoredValue);
        public static readonly DecimalLeadingZeroCounter MaxValue = new DecimalLeadingZeroCounter(MaxStoredValue);

        readonly int _storage;

        public DecimalLeadingZeroCounter(int value) {
            if (value > MaxStoredValue || value < MinStoredValue)
                throw new OverflowException();
            _storage = value;
        }

        public DecimalLeadingZeroCounter(string value) {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (!Validator.IsMatch(value)) throw new ArgumentException(nameof(value));

            var x = Int32.Parse(value, CultureInfo.InvariantCulture);
            if (x > MaxStoredValue || x < MinStoredValue)
                throw new OverflowException();
            _storage = x - 1;
        }

        public int CompareTo(DecimalLeadingZeroCounter other) {
            return _storage - other._storage;
        }

        public DecimalLeadingZeroCounter Increment(int n) {
            return new DecimalLeadingZeroCounter(_storage + n);
        }

        public override int GetHashCode() {
            return _storage;
        }

        public override string ToString() {
            return (_storage + 1).ToString(CultureInfo.InvariantCulture);
        }

        public bool Equals(DecimalLeadingZeroCounter other) {
            return _storage == other._storage;
        }

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != typeof(DecimalLeadingZeroCounter))
                return false;
            return Equals((DecimalLeadingZeroCounter)obj);
        }

        #region Conversion operators

        public static implicit operator int(DecimalLeadingZeroCounter x) {
            return x._storage;
        }

        public static implicit operator string(DecimalLeadingZeroCounter x) {
            return x.ToString();
        }

        public static implicit operator DecimalLeadingZeroCounter(int x) {
            return new DecimalLeadingZeroCounter(x);
        }

        public static implicit operator DecimalLeadingZeroCounter(string x) {
            return new DecimalLeadingZeroCounter(x);
        }

        #endregion

        #region Relational operators

        public static bool operator ==(DecimalLeadingZeroCounter a, DecimalLeadingZeroCounter b) {
            return a.Equals(b);
        }
        public static bool operator !=(DecimalLeadingZeroCounter a, DecimalLeadingZeroCounter b) {
            return !a.Equals(b);
        }

        public static bool operator <(DecimalLeadingZeroCounter x, DecimalLeadingZeroCounter y) {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(DecimalLeadingZeroCounter x, DecimalLeadingZeroCounter y) {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator >(DecimalLeadingZeroCounter x, DecimalLeadingZeroCounter y) {
            return x.CompareTo(y) > 0;
        }
        public static bool operator >=(DecimalLeadingZeroCounter x, DecimalLeadingZeroCounter y) {
            return x.CompareTo(y) >= 0;
        }

        #endregion

        #region Arithmetic operators

        public static DecimalLeadingZeroCounter operator +(DecimalLeadingZeroCounter x, int n) {
            return x.Increment(n);
        }

        public static DecimalLeadingZeroCounter operator -(DecimalLeadingZeroCounter x, int n) {
            return x.Increment(-n);
        }

        public static DecimalLeadingZeroCounter operator ++(DecimalLeadingZeroCounter x) {
            return x.Increment(1);
        }

        public static DecimalLeadingZeroCounter operator --(DecimalLeadingZeroCounter x) {
            return x.Increment(-1);
        }

        #endregion
    }
}
