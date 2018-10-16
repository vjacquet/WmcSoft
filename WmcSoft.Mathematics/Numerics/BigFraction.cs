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
    /// <summary>
    /// Represents a fraction of <see cref="BigInteger"/>, where the numerator and denominator are prime to each other.
    /// </summary>
    [Serializable]
    public struct BigFraction : IEquatable<BigFraction>, IComparable<BigFraction>, IFormattable
    {
        #region Constants

        public static BigFraction Zero = new BigFraction(NumericsUtilities.Uninitialized, BigInteger.Zero);

        #endregion

        #region Fields

        private readonly BigInteger numerator;
        private readonly BigInteger denominator;

        #endregion

        #region Lifecycle

        internal BigFraction(NumericsUtilities.UninitializedTag tag, BigInteger numerator)
        {
            this.numerator = numerator;
            denominator = BigInteger.One;
        }

        internal BigFraction(NumericsUtilities.UninitializedTag tag, BigInteger numerator, BigInteger denominator)
        {
            this.numerator = numerator;
            this.denominator = denominator;
        }

        public BigFraction(BigInteger numerator, BigInteger denominator)
        {
            if (denominator == 0)
                throw new DivideByZeroException();
            else if (denominator > 0) {
                var gcd = BigInteger.GreatestCommonDivisor(numerator, denominator);
                this.numerator = numerator / gcd;
                this.denominator = denominator / gcd;
            } else {
                var gcd = BigInteger.GreatestCommonDivisor(numerator, denominator);
                this.numerator = -numerator;
                this.denominator = -denominator;
            }
        }

        public void Deconstruct(out BigInteger numerator, out BigInteger denominator)
        {
            numerator = this.numerator;
            denominator = this.denominator;
        }

        #endregion

        #region Properties

        public BigInteger Numerator => numerator;
        public BigInteger Denominator => denominator;

        public int Sign => numerator.Sign;

        public bool IsInteger => denominator == BigInteger.One;
        public bool IsProper => numerator < denominator;
        public bool IsImproper => !IsProper;

        #endregion

        #region Operators

        public static implicit operator BigFraction(int x)
        {
            return new BigFraction(x, 1);
        }
        public static BigFraction FromInt32(int x)
        {
            return x;
        }

        public static implicit operator BigFraction(BigInteger x)
        {
            return new BigFraction(x, 1);
        }
        public static BigFraction FromBigInteger(BigInteger x)
        {
            return x;
        }

        public static explicit operator BigInteger(BigFraction q)
        {
            if (q.denominator == 1)
                return q.numerator;
            if (q.numerator % q.denominator == 0)
                return q.numerator / q.denominator;

            throw new InvalidCastException();
        }
        public static BigInteger FromBigInteger(BigFraction q)
        {
            return (BigInteger)q;
        }

        public static BigFraction operator +(BigFraction x, BigFraction y)
        {
            if (x.denominator == y.denominator)
                return new BigFraction(x.numerator + y.numerator, x.denominator);
            return new BigFraction(x.numerator * y.denominator + y.numerator * x.denominator, x.denominator * y.denominator);
        }
        public static BigFraction Add(BigFraction x, BigFraction y)
        {
            return x + y;
        }

        public static BigFraction operator -(BigFraction x, BigFraction y)
        {
            if (x.denominator == y.denominator)
                return new BigFraction(x.numerator + y.numerator, x.denominator);
            return new BigFraction(x.numerator * y.denominator - y.numerator * x.denominator, x.denominator * y.denominator);
        }
        public static BigFraction Subtract(BigFraction x, BigFraction y)
        {
            return x - y;
        }

        public static BigFraction operator *(BigFraction x, BigFraction y)
        {
            return new BigFraction(x.numerator * y.numerator, x.denominator * y.denominator);
        }
        public static BigFraction Multiply(BigFraction x, BigFraction y)
        {
            return x * y;
        }

        public static BigFraction operator /(BigFraction x, BigFraction y)
        {
            return new BigFraction(x.numerator * y.denominator, x.denominator * y.numerator);
        }
        public static BigFraction Divide(BigFraction x, BigFraction y)
        {
            return x / y;
        }

        public static BigFraction operator -(BigFraction x)
        {
            return new BigFraction(-x.numerator, x.denominator);
        }
        public static BigFraction Negate(BigFraction x)
        {
            return -x;
        }

        public static BigFraction operator +(BigFraction x)
        {
            return x;
        }
        public static BigFraction Plus(BigFraction x)
        {
            return x;
        }

        public BigFraction Reciprocal()
        {
            if (numerator > 0)
                return new BigFraction(NumericsUtilities.Uninitialized, denominator, numerator);
            if (numerator < 0)
                return new BigFraction(NumericsUtilities.Uninitialized, -denominator, -numerator);
            throw new DivideByZeroException();
        }
        public static BigFraction Reciprocal(BigFraction x)
        {
            return x.Reciprocal();
        }
        public static BigFraction Reciprocal(BigInteger x)
        {
            return new BigFraction(NumericsUtilities.Uninitialized, BigInteger.One, x);
        }

        public static bool operator ==(BigFraction x, BigFraction y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(BigFraction x, BigFraction y)
        {
            return !x.Equals(y);
        }

        public static bool operator <(BigFraction x, BigFraction y)
        {
            return x.CompareTo(y) < 0;
        }

        public static bool operator <=(BigFraction x, BigFraction y)
        {
            return x.CompareTo(y) <= 0;
        }

        public static bool operator >=(BigFraction x, BigFraction y)
        {
            return x.CompareTo(y) >= 0;
        }

        public static bool operator >(BigFraction x, BigFraction y)
        {
            return x.CompareTo(y) > 0;
        }

        #endregion

        #region IEquatable<BigRational> Membres

        public bool Equals(BigFraction other)
        {
            return CompareTo(other) == 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return CompareTo((BigFraction)obj) == 0;
        }

        public override int GetHashCode()
        {
            return numerator.GetHashCode() ^ denominator.GetHashCode();
        }

        #endregion

        #region IComparable<BigRational> Membres

        static int ComparePositives(BigInteger xn, BigInteger xd, BigInteger yn, BigInteger yd)
        {
            var result = (xn * yd - yn * xd);
            if (result < 0)
                return -1;
            if (result > 0)
                return 1;
            return 0;
        }

        public int CompareTo(BigFraction other)
        {
            var config = 4 * (Sign + 1) + (other.Sign + 1);
            switch (config) {
            case 0b0000: // -1 | -1
                return ComparePositives(-other.numerator, other.denominator, -numerator, denominator);
            case 0b1010: //  1 |  1
                return ComparePositives(numerator, denominator, other.numerator, other.denominator);
            case 0b0100: //  0 | -1
            case 0b1000: //  1 | -1
            case 0b1001: //  1 |  0
                return 1;
            case 0b0001: // -1 |  0
            case 0b0010: // -1 |  1
            case 0b0110: //  0 |  1
                return -1;
            case 0b0101: //  0 |  0
                return 0;
            }
            throw new InvalidOperationException();
        }

        #endregion

        #region IFormattable Membres

        public override string ToString()
        {
            return ToString(null, null);
        }
        public string ToString(IFormatProvider formatProvider)
        {
            return ToString(null, formatProvider);
        }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (denominator == 1 || numerator == 0)
                return numerator.ToString(format, formatProvider);
            return numerator.ToString(format, formatProvider)
                + '/'
                + denominator.ToString(format, formatProvider);
        }

        #endregion
    }
}
