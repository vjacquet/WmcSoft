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
    [Serializable]
    public struct Fraction : IEquatable<Fraction>, IComparable<Fraction>, IFormattable
    {
        private readonly int _numerator;
        private readonly int _denominator;

        #region Lifecycle

        internal Fraction(NumericsUtilities.UninitializedTag tag, int numerator) {
            _numerator = numerator;
            _denominator = 1;
        }

        internal Fraction(NumericsUtilities.UninitializedTag tag, int numerator, int denominator) {
            _numerator = numerator;
            _denominator = denominator;
        }

        public Fraction(int numerator, int denominator) {
            if (denominator == 0)
                throw new DivideByZeroException();
            else if (denominator > 0) {
                var gcd = GreatestCommonDivisor(Math.Abs(numerator), denominator);
                _numerator = numerator / gcd;
                _denominator = denominator / gcd;
            } else {
                var gcd = GreatestCommonDivisor(Math.Abs(numerator), -denominator);
                _numerator = -numerator;
                _denominator = -denominator;
            }
        }

        #endregion

        #region Properties

        public int Numerator { get { return _numerator; } }
        public int Denominator { get { return _denominator; } }

        public bool IsInteger { get { return _denominator == 1; } }

        #endregion

        #region Operators

        public static implicit operator Fraction(int x) {
            return new Fraction(NumericsUtilities.Uninitialized, x);
        }
        public static Fraction FromInt32(int x) {
            return x;
        }

        public static explicit operator int (Fraction q) {
            if (q._denominator == 1)
                return q._numerator;
            int rem;
            var n = Math.DivRem(q._numerator, q._denominator, out rem);
            if (rem == 0)
                return n;
            throw new InvalidCastException();
        }
        public static int FromInt32(Fraction q) {
            return (int)q;
        }

        public static Fraction operator +(Fraction x, Fraction y) {
            if (x._denominator == y._denominator)
                return new Fraction(x._numerator + y._numerator, x._denominator);
            return new Fraction(x._numerator * y._denominator + y._numerator * x._denominator, x._denominator * y._denominator);
        }
        public static Fraction Add(Fraction x, Fraction y) {
            return x + y;
        }

        public static Fraction operator -(Fraction x, Fraction y) {
            if (x._denominator == y._denominator)
                return new Fraction(x._numerator - y._numerator, x._denominator);
            return new Fraction(x._numerator * y._denominator - y._numerator * x._denominator, x._denominator * y._denominator);
        }
        public static Fraction Subtract(Fraction x, Fraction y) {
            return x - y;
        }

        public static Fraction operator *(Fraction x, Fraction y) {
            return new Fraction(x._numerator * y._numerator, x._denominator * y._denominator);
        }
        public static Fraction Multiply(Fraction x, Fraction y) {
            return x * y;
        }

        public static Fraction operator /(Fraction x, Fraction y) {
            return new Fraction(x._numerator * y._denominator, x._denominator * y._numerator);
        }
        public static Fraction Divide(Fraction x, Fraction y) {
            return x / y;
        }

        public static Fraction operator -(Fraction x) {
            return new Fraction(-x._numerator, x._denominator);
        }
        public static Fraction Negate(Fraction x) {
            return -x;
        }

        public static Fraction operator +(Fraction x) {
            return x;
        }
        public static Fraction Plus(Fraction x) {
            return x;
        }

        #endregion

        #region Methods

        static int GreatestCommonDivisor(int m, int n) {
            // assumes m & n are striclty positive numbers
            while (n != 0) {
                var t = m % n;
                m = n;
                n = t;
            }
            return m;
        }

        #endregion

        #region IEquatable<Rational> Membres

        public bool Equals(Fraction other) {
            return CompareTo(other) == 0;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return CompareTo((Fraction)obj) == 0;
        }

        public override int GetHashCode() {
            return _numerator ^ _denominator;
        }

        #endregion

        #region IComparable<Rational> Membres

        public int CompareTo(Fraction other) {
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
