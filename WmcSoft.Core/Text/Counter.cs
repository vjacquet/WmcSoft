﻿#region Licence

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

namespace WmcSoft.Text
{
    public struct Counter : IComparable<Counter>, IEquatable<Counter>, IFormattable
    {
        readonly int _storage;

        public Counter(int value)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
            _storage = value;
        }
        public int CompareTo(Counter other)
        {
            return _storage - other._storage;
        }

        public Counter Increment(int n)
        {
            return new Counter(_storage + n);
        }

        public override int GetHashCode()
        {
            return _storage;
        }

        public override string ToString()
        {
            return ToString("1");
        }

        public bool Equals(Counter other)
        {
            return _storage == other._storage;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Counter))
                return false;
            return Equals((Counter)obj);
        }

        public static Counter Parse(string s)
        {
            if (TryParse(s, out Counter counter))
                return counter;
            throw new FormatException();
        }

        public static bool TryParse(string s, out Counter result)
        {
            if (int.TryParse(s, out int value)) {
                result = new Counter(value - 1);
                return true;
            }
            result = default;
            return false;
        }

        #region Conversion operators

        public static implicit operator int(Counter x)
        {
            return x._storage;
        }

        public static implicit operator Counter(int x)
        {
            return new Counter(x);
        }

        #endregion

        #region Relational operators

        public static bool operator ==(Counter a, Counter b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Counter a, Counter b)
        {
            return !a.Equals(b);
        }

        public static bool operator <(Counter x, Counter y)
        {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(Counter x, Counter y)
        {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator >(Counter x, Counter y)
        {
            return x.CompareTo(y) > 0;
        }
        public static bool operator >=(Counter x, Counter y)
        {
            return x.CompareTo(y) >= 0;
        }

        #endregion

        #region Arithmetic operators

        public static Counter operator +(Counter x, int n)
        {
            return x.Increment(n);
        }

        public static Counter operator -(Counter x, int n)
        {
            return x.Increment(-n);
        }

        public static Counter operator ++(Counter x)
        {
            return x.Increment(1);
        }

        public static Counter operator --(Counter x)
        {
            return x.Increment(-1);
        }

        #endregion

        #region IFormattable members

        /// <summary>
        /// Formats the value of the current instance using the specified format.
        /// </summary>
        /// <param name="format">
        ///   The format to use.
        ///   -or- 
        ///   A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="IFormattable"/> implementation.
        /// </param>
        /// <returns>The value of the current instance in the specified format.</returns>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <summary>
        /// Formats the value of the current instance using the specified format.
        /// </summary>
        /// <param name="format">
        ///   The format to use.
        ///   -or- 
        ///   A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="IFormattable"/> implementation.
        /// </param>
        /// <param name="formatProvider">
        ///   The provider to use to format the value.
        ///   -or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting
        ///   of the operating system.
        /// </param>
        /// <returns>The value of the current instance in the specified format.</returns>
        /// <remarks>Supports <see cref="Guid"/> five format in addition to "S" for the 22 chars short version.</remarks>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format) {
            case "0":
                return ((DecimalLeadingZeroCounter)_storage).ToString();
            case "1":
                return ((DecimalCounter)_storage).ToString();
            case "a":
                return ((LowerAlphaCounter)_storage).ToString();
            case "A":
                return ((UpperAlphaCounter)_storage).ToString();
            }
            throw new FormatException();
        }

        #endregion
    }
}
