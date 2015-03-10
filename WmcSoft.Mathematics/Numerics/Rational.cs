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

namespace WmcSoft.Numerics
{
    /// <summary>
    /// 
    /// </summary>
    [SerializableAttribute]
    public struct Rational : IEquatable<Rational>, IComparable<Rational>, IFormattable
    {
        private readonly int _numerator;
        private readonly int _denominator;

        #region Lifecycle

        public Rational(int numerator, int denominator) {
            if (denominator == 0)
                throw new DivideByZeroException();
            else if (denominator > 0) {
                _numerator = numerator;
                _denominator = denominator;
            } else {
                _numerator = -numerator;
                _denominator = -denominator;
            }
        }

        #endregion

        #region Properties

        public int Numerator { get { return _numerator; } }
        public int Denominator { get { return _denominator; } }

        public bool IsInteger { get { return _denominator == 1 || _numerator % _denominator == 0; } }

        #endregion

        #region Operators

        public static implicit operator Rational(int x) {
            return new Rational(x, 1);
        }
        public static Rational FromInt32(int x) {
            return (Rational)x;
        }

        public static explicit operator int(Rational q) {
            if (q._denominator == 1)
                return q._numerator;
            if (q._numerator % q._denominator == 0)
                return q._numerator / q._denominator;

            throw new InvalidCastException();
        }
        public static int FromInt32(Rational q) {
            return (int)q;
        }

        public static Rational operator +(Rational x, Rational y) {
            if (x._denominator == y._denominator)
                return new Rational(x._numerator + y._numerator, x._denominator);
            return new Rational(x._numerator * y._denominator + y._numerator * x._denominator, x._denominator * y._denominator);
        }
        public static Rational Add(Rational x, Rational y) {
            return x + y;
        }

        public static Rational operator -(Rational x, Rational y) {
            if (x._denominator == y._denominator)
                return new Rational(x._numerator - y._numerator, x._denominator);
            return new Rational(x._numerator * y._denominator - y._numerator * x._denominator, x._denominator * y._denominator);
        }
        public static Rational Subtract(Rational x, Rational y) {
            return x - y;
        }

        public static Rational operator *(Rational x, Rational y) {
            return new Rational(x._numerator * y._numerator, x._denominator * y._denominator);
        }
        public static Rational Multiply(Rational x, Rational y) {
            return x * y;
        }

        public static Rational operator /(Rational x, Rational y) {
            return new Rational(x._numerator * y._denominator, x._denominator * y._numerator);
        }
        public static Rational Divide(Rational x, Rational y) {
            return x / y;
        }

        public static Rational operator -(Rational x) {
            return new Rational(-x._numerator, x._denominator);
        }
        public static Rational Negate(Rational x) {
            return -x;
        }

        public static Rational operator +(Rational x) {
            return x;
        }
        public static Rational Plus(Rational x) {
            return x;
        }

        #endregion

        #region Methods

        public Rational Simplify() {
            var gcd = GreatestCommonDivisor(_numerator, _denominator);
            return new Rational(_numerator / gcd, _denominator / gcd);
        }

        public static int GreatestCommonDivisor(int m, int n) {
            if (m == 0)
                throw new ArgumentOutOfRangeException("m");
            else if (m < 0)
                m = -m;

            if (n == 0)
                throw new ArgumentOutOfRangeException("n");
            else if (n < 0)
                n = -n;

            while (n != 0) {
                var t = m % n;
                m = n;
                n = t;
            }

            return m;
        }

        #endregion

        #region IEquatable<Rational> Membres

        public bool Equals(Rational other) {
            return CompareTo(other) == 0;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return CompareTo((Rational)obj) == 0;
        }

        public override int GetHashCode() {
            return _numerator ^ _denominator;
        }

        #endregion

        #region IComparable<Rational> Membres

        public int CompareTo(Rational other) {
            long numerator = _numerator;
            long denominator = _denominator;
            return (numerator * other._denominator - denominator * other._numerator).Clamp();
        }

        #endregion

        #region IFormattable Membres

        public override string ToString() {
            return ToString(null, null);
        }
        public string ToString(IFormatProvider formatProvider) {
            return ToString(null, formatProvider);
        }
        public string ToString(string format, IFormatProvider formatProvider = null) {
            if (_denominator == 1 || _numerator == 0)
                return _numerator.ToString(format, formatProvider);
            return _numerator.ToString(format, formatProvider)
                + '/'
                + _denominator.ToString(format, formatProvider);
        }

        #endregion
    }
}
