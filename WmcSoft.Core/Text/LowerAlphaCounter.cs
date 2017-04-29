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
using System.Text.RegularExpressions;

namespace WmcSoft.Text
{
    public struct LowerAlphaCounter : IComparable<LowerAlphaCounter>, IEquatable<LowerAlphaCounter>
    {
        const char FirstLetter = 'a';

        const int MinStoredValue = 0;
        const int MaxStoredValue = 26 * 26 * 26 - 1;

        static readonly Regex Validator = new Regex("^[a-z]{1,3}$", RegexOptions.CultureInvariant | RegexOptions.Singleline);

        public static readonly LowerAlphaCounter MinValue = new LowerAlphaCounter(MinStoredValue);
        public static readonly LowerAlphaCounter MaxValue = new LowerAlphaCounter(MaxStoredValue);

        readonly int _storage;

        public LowerAlphaCounter(int value)
        {
            if (value > MaxStoredValue || value < MinStoredValue)
                throw new OverflowException();
            _storage = value;
        }

        public LowerAlphaCounter(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (!Validator.IsMatch(value)) throw new ArgumentException(nameof(value));

            var x = 0;
            foreach (var c in value) {
                x *= 26;
                x += c - FirstLetter + 1;
            }
            _storage = x - 1;
        }

        public int CompareTo(LowerAlphaCounter other)
        {
            return _storage - other._storage;
        }

        public LowerAlphaCounter Increment(int n)
        {
            return new LowerAlphaCounter(_storage + n);
        }

        public override int GetHashCode()
        {
            return _storage;
        }

        public override string ToString()
        {
            if (_storage < 26)
                return ((char)(FirstLetter + _storage)).ToString();

            var x = _storage;
            var chars = new char[3];
            var startIndex = 3;
            while (x != 0) {
                var lo = x % 26;
                x /= 26;
                chars[--startIndex] = (char)(FirstLetter + _storage);
            }
            return new string(chars, startIndex, 3 - startIndex);
        }

        public bool Equals(LowerAlphaCounter other)
        {
            return _storage == other._storage;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(LowerAlphaCounter))
                return false;
            return Equals((LowerAlphaCounter)obj);
        }

        #region Conversion operators

        public static implicit operator int(LowerAlphaCounter x)
        {
            return x._storage;
        }

        public static implicit operator string(LowerAlphaCounter x)
        {
            return x.ToString();
        }

        public static implicit operator LowerAlphaCounter(int x)
        {
            return new LowerAlphaCounter(x);
        }

        public static implicit operator LowerAlphaCounter(string x)
        {
            return new LowerAlphaCounter(x);
        }

        #endregion

        #region Relational operators

        public static bool operator ==(LowerAlphaCounter a, LowerAlphaCounter b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(LowerAlphaCounter a, LowerAlphaCounter b)
        {
            return !a.Equals(b);
        }

        public static bool operator <(LowerAlphaCounter x, LowerAlphaCounter y)
        {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(LowerAlphaCounter x, LowerAlphaCounter y)
        {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator >(LowerAlphaCounter x, LowerAlphaCounter y)
        {
            return x.CompareTo(y) > 0;
        }
        public static bool operator >=(LowerAlphaCounter x, LowerAlphaCounter y)
        {
            return x.CompareTo(y) >= 0;
        }

        #endregion

        #region Arithmetic operators

        public static LowerAlphaCounter operator +(LowerAlphaCounter x, int n)
        {
            return x.Increment(n);
        }

        public static LowerAlphaCounter operator -(LowerAlphaCounter x, int n)
        {
            return x.Increment(-n);
        }

        public static LowerAlphaCounter operator ++(LowerAlphaCounter x)
        {
            return x.Increment(1);
        }

        public static LowerAlphaCounter operator --(LowerAlphaCounter x)
        {
            return x.Increment(-1);
        }

        #endregion
    }
}
