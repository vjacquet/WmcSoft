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
using System.Numerics;

namespace WmcSoft.Numerics
{
    [SerializableAttribute]
    public struct BigFraction : IEquatable<BigFraction>, IComparable<BigFraction>, IFormattable
    {
        private readonly BigInteger _numerator;
        private readonly BigInteger _denominator;

        #region Lifecycle

        internal BigFraction(NumericsUtilities.UninitializedTag tag, BigInteger numerator) {
            _numerator = numerator;
            _denominator = BigInteger.One;
        }

        internal BigFraction(NumericsUtilities.UninitializedTag tag, BigInteger numerator, BigInteger denominator) {
            _numerator = numerator;
            _denominator = denominator;
        }

        public BigFraction(BigInteger numerator, BigInteger denominator) {
            if (denominator == 0)
                throw new DivideByZeroException();
            else if (denominator > 0) {
                var gcd = BigInteger.GreatestCommonDivisor(numerator, denominator);
                _numerator = numerator / gcd;
                _denominator = denominator / gcd;
            } else {
                var gcd = BigInteger.GreatestCommonDivisor(numerator, denominator);
                _numerator = -numerator;
                _denominator = -denominator;
            }
        }

        #endregion

        #region Properties

        public BigInteger Numerator => _numerator;
        public BigInteger Denominator => _denominator;

        public bool IsInteger => _denominator == BigInteger.One;
        public bool IsProper => _numerator < _denominator;
        public bool IsImproper => !IsProper;

        #endregion

        #region Operators

        public static implicit operator BigFraction(int x) {
            return new BigFraction(x, 1);
        }
        public static BigFraction FromInt32(int x) {
            return x;
        }

        public static implicit operator BigFraction(BigInteger x) {
            return new BigFraction(x, 1);
        }
        public static BigFraction FromBigInteger(BigInteger x) {
            return x;
        }

        public static explicit operator BigInteger(BigFraction q) {
            if (q._denominator == 1)
                return q._numerator;
            if (q._numerator % q._denominator == 0)
                return q._numerator / q._denominator;

            throw new InvalidCastException();
        }
        public static BigInteger FromBigInteger(BigFraction q) {
            return (BigInteger)q;
        }

        public static BigFraction operator +(BigFraction x, BigFraction y) {
            if (x._denominator == y._denominator)
                return new BigFraction(x._numerator + y._numerator, x._denominator);
            return new BigFraction(x._numerator * y._denominator + y._numerator * x._denominator, x._denominator * y._denominator);
        }
        public static BigFraction Add(BigFraction x, BigFraction y) {
            return x + y;
        }

        public static BigFraction operator -(BigFraction x, BigFraction y) {
            if (x._denominator == y._denominator)
                return new BigFraction(x._numerator + y._numerator, x._denominator);
            return new BigFraction(x._numerator * y._denominator - y._numerator * x._denominator, x._denominator * y._denominator);
        }
        public static BigFraction Subtract(BigFraction x, BigFraction y) {
            return x - y;
        }

        public static BigFraction operator *(BigFraction x, BigFraction y) {
            return new BigFraction(x._numerator * y._numerator, x._denominator * y._denominator);
        }
        public static BigFraction Multiply(BigFraction x, BigFraction y) {
            return x * y;
        }

        public static BigFraction operator /(BigFraction x, BigFraction y) {
            return new BigFraction(x._numerator * y._denominator, x._denominator * y._numerator);
        }
        public static BigFraction Divide(BigFraction x, BigFraction y) {
            return x / y;
        }

        public static BigFraction operator -(BigFraction x) {
            return new BigFraction(-x._numerator, x._denominator);
        }
        public static BigFraction Negate(BigFraction x) {
            return -x;
        }

        public static BigFraction operator +(BigFraction x) {
            return x;
        }
        public static BigFraction Plus(BigFraction x) {
            return x;
        }

        #endregion

        #region IEquatable<BigRational> Membres

        public bool Equals(BigFraction other) {
            return CompareTo(other) == 0;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return CompareTo((BigFraction)obj) == 0;
        }

        public override int GetHashCode() {
            return _numerator.GetHashCode() ^ _denominator.GetHashCode();
        }

        #endregion

        #region IComparable<BigRational> Membres

        public int CompareTo(BigFraction other) {
            // TODO: Try to optimize. It is correct but certainly very slow.
            var result = (_numerator * other._denominator - _denominator * other._numerator);
            if (result < 0)
                return -1;
            if (result > 0)
                return 1;
            return 0;
        }

        #endregion

        #region IFormattable Membres

        public override string ToString() {
            return ToString(null, null);
        }
        public string ToString(IFormatProvider formatProvider) {
            return ToString(null, formatProvider);
        }
        public string ToString(string format, IFormatProvider formatProvider) {
            if (_denominator == 1 || _numerator == 0)
                return _numerator.ToString(format, formatProvider);
            return _numerator.ToString(format, formatProvider)
                + '/'
                + _denominator.ToString(format, formatProvider);
        }

        #endregion
    }
}
