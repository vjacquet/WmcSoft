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
    public struct BigRational : IEquatable<BigRational>, IComparable<BigRational>, IFormattable
    {
        private readonly BigInteger _numerator;
        private readonly BigInteger _denominator;

        #region Lifecycle

        public BigRational(BigInteger numerator, BigInteger denominator) {
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

        public BigInteger Numerator { get { return _numerator; } }
        public BigInteger Denominator { get { return _denominator; } }

        #endregion

        #region Operators

        public static implicit operator BigRational(int x) {
            return new BigRational(x, 1);
        }
        public static BigRational FromInt32(int x) {
            return (BigRational)x;
        }

        public static BigRational operator +(BigRational x, BigRational y) {
            return new BigRational(x._numerator * y._denominator + y._numerator * x._denominator, x._denominator * y._denominator);
        }
        public static BigRational Add(BigRational x, BigRational y) {
            return x + y;
        }

        public static BigRational operator -(BigRational x, BigRational y) {
            return new BigRational(x._numerator * y._denominator - y._numerator * x._denominator, x._denominator * y._denominator);
        }
        public static BigRational Subtract(BigRational x, BigRational y) {
            return x - y;
        }

        public static BigRational operator *(BigRational x, BigRational y) {
            return new BigRational(x._numerator * y._numerator, x._denominator * y._denominator);
        }
        public static BigRational Multiply(BigRational x, BigRational y) {
            return x * y;
        }

        public static BigRational operator /(BigRational x, BigRational y) {
            return new BigRational(x._numerator * y._denominator, x._denominator * y._numerator);
        }
        public static BigRational Divide(BigRational x, BigRational y) {
            return x / y;
        }

        public static BigRational operator -(BigRational x) {
            return new BigRational(-x._numerator, x._denominator);
        }
        public static BigRational Negate(BigRational x) {
            return -x;
        }

        public static BigRational operator +(BigRational x) {
            return x;
        }
        public static BigRational Plus(BigRational x) {
            return x;
        }

        #endregion

        #region Methods

        public BigRational Simplify() {
            var gcd = BigInteger.GreatestCommonDivisor(_numerator, _denominator);
            return new BigRational(_numerator / gcd, _denominator / gcd);
        }

        #endregion

        #region IEquatable<BigRational> Membres

        public bool Equals(BigRational other) {
            return CompareTo(other) == 0;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return CompareTo((BigRational)obj) == 0;
        }

        public override int GetHashCode() {
            return _numerator.GetHashCode() ^ _denominator.GetHashCode();
        }

        #endregion

        #region IComparable<BigRational> Membres

        public int CompareTo(BigRational other) {
            // should be optimized
            var result = (_numerator * other._denominator - _denominator * other._numerator);
            if (result < 0)
                return -1;
            if (result > 0)
                return 1;
            return 0;
        }

        #endregion

        #region IFormattable Membres

        public string ToString(string format, IFormatProvider formatProvider) {
            return String.Format("{0}/{1}", _numerator, _denominator);
        }

        #endregion
    }
}
