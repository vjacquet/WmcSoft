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
using WmcSoft.Collections.Generic;

namespace WmcSoft.Numerics
{
    /// <summary>
    /// Represents a fraction, where the numerator and denominator are prime to each other.
    /// </summary>
    [Serializable]
    public struct Fraction : IEquatable<Fraction>, IComparable<Fraction>, IFormattable, IStructuralEquatable
    {
        #region Constants

        public static Fraction Zero = new Fraction(NumericsUtilities.Uninitialized, 0);

        #endregion

        #region Fields

        private readonly int _numerator;
        private readonly int _denominator;

        #endregion

        #region Lifecycle

        internal Fraction(NumericsUtilities.UninitializedTag tag, int numerator, int denominator = 1)
        {
            _numerator = numerator;
            _denominator = denominator;
        }

        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0) {
                throw new DivideByZeroException();
            } else if (denominator > 0) {
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

        public int Numerator => _numerator;
        public int Denominator => _denominator;

        public bool IsInteger => _denominator == 1;
        public bool IsProper => _numerator < _denominator;
        public bool IsImproper => !IsProper;

        #endregion

        #region Operators

        public static implicit operator Fraction(int x)
        {
            return new Fraction(NumericsUtilities.Uninitialized, x);
        }
        public static Fraction FromInt32(int x)
        {
            return x;
        }

        public static explicit operator int(Fraction q)
        {
            if (q._denominator == 1)
                return q._numerator;
            int rem;
            var n = Math.DivRem(q._numerator, q._denominator, out rem);
            if (rem == 0)
                return n;
            throw new InvalidCastException();
        }
        public static int FromInt32(Fraction q)
        {
            return (int)q;
        }

        static Fraction Add(int un, int ud, int vn, int vd)
        {
            if (ud == vd)
                return new Fraction(un + vn, ud);

            // Algorithm to avoid potential overflow - it might be faster to simply convert to long then back to int.
            var d1 = GreatestCommonDivisor(ud, vd);
            if (d1 == 1)
                return new Fraction(NumericsUtilities.Uninitialized, un * vd + ud * vn, ud * vd);
            var t = un * (vd / d1) + vn * (ud / d1);
            var d2 = GreatestCommonDivisor(t, d1);
            return new Fraction(NumericsUtilities.Uninitialized, t / d2, (ud / d1) * (vd / d2));
        }

        public static Fraction operator +(Fraction x, Fraction y)
        {
            return Add(x._numerator, x._denominator, y._numerator, y._denominator);
        }
        public static Fraction Add(Fraction x, Fraction y)
        {
            return x + y;
        }

        public static Fraction operator -(Fraction x, Fraction y)
        {
            return Add(x._numerator, x._denominator, -y._numerator, y._denominator);
        }
        public static Fraction Subtract(Fraction x, Fraction y)
        {
            return x - y;
        }

        static Fraction Multiply(int un, int ud, int vn, int vd)
        {
            // assumes all parameters are stricly positive
            var d1 = GreatestCommonDivisor(un, vd);
            var d2 = GreatestCommonDivisor(ud, vn);
            return new Fraction(NumericsUtilities.Uninitialized, (un / d1) * (vn / d2), (ud / d2) * (vd / d1));
        }

        public static Fraction operator *(Fraction x, Fraction y)
        {
            if (x._numerator == 0 || y._numerator == 0)
                return Zero;

            if (x._numerator > 0 && y._numerator > 0)
                return Multiply(x._numerator, x._denominator, y._numerator, y._denominator);
            if (x._numerator > 0 && y._numerator < 0)
                return -Multiply(x._numerator, x._denominator, -y._numerator, y._denominator);
            if (x._numerator < 0 && y._numerator < 0)
                return Multiply(-x._numerator, x._denominator, -y._numerator, y._denominator);
            return -Multiply(-x._numerator, x._denominator, y._numerator, y._denominator);
        }

        public static Fraction Multiply(Fraction x, Fraction y)
        {
            return x * y;
        }

        public static Fraction operator /(Fraction x, Fraction y)
        {
            if (x._numerator == 0)
                return Zero;
            if (y._numerator == 0)
                throw new DivideByZeroException();

            if (x._numerator > 0 && y._numerator > 0)
                return Multiply(x._numerator, x._denominator, y._denominator, y._numerator);
            if (x._numerator > 0 && y._numerator < 0)
                return -Multiply(x._numerator, x._denominator, -y._denominator, -y._numerator);
            if (x._numerator < 0 && y._numerator < 0)
                return Multiply(-x._numerator, x._denominator, -y._denominator, -y._numerator);
            return -Multiply(-x._numerator, x._denominator, y._denominator, y._numerator);
        }
        public static Fraction Divide(Fraction x, Fraction y)
        {
            return x / y;
        }

        public static Fraction operator -(Fraction x)
        {
            return new Fraction(-x._numerator, x._denominator);
        }
        public static Fraction Negate(Fraction x)
        {
            return -x;
        }

        public static Fraction operator +(Fraction x)
        {
            return x;
        }
        public static Fraction Plus(Fraction x)
        {
            return x;
        }

        public Fraction Inverse()
        {
            if (_numerator > 0)
                return new Fraction(NumericsUtilities.Uninitialized, _denominator, _numerator);
            if (_numerator < 0)
                return new Fraction(NumericsUtilities.Uninitialized, -_denominator, -_numerator);
            throw new DivideByZeroException();
        }
        public static Fraction Inverse(Fraction x)
        {
            return x.Inverse();
        }

        public static bool operator ==(Fraction x, Fraction y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Fraction x, Fraction y)
        {
            return !x.Equals(y);
        }

        public static bool operator <(Fraction x, Fraction y)
        {
            return x.CompareTo(y) < 0;
        }

        public static bool operator <=(Fraction x, Fraction y)
        {
            return x.CompareTo(y) <= 0;
        }

        public static bool operator >=(Fraction x, Fraction y)
        {
            return x.CompareTo(y) >= 0;
        }

        public static bool operator >(Fraction x, Fraction y)
        {
            return x.CompareTo(y) > 0;
        }

        #endregion

        #region Methods

        static int GreatestCommonDivisor(int m, int n)
        {
            // assumes m & n are striclty positive numbers
            while (n != 0) {
                var t = m % n;
                m = n;
                n = t;
            }
            return m;
        }

        #endregion

        #region IEquatable<Fraction> Membres

        public bool Equals(Fraction other)
        {
            return CompareTo(other) == 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return CompareTo((Fraction)obj) == 0;
        }

        public override int GetHashCode()
        {
            return _numerator ^ _denominator;
        }

        #endregion

        #region IComparable<Fraction> Membres

        public int CompareTo(Fraction other)
        {
            long numerator = _numerator;
            long denominator = _denominator;
            return (numerator * other._denominator - denominator * other._numerator).Clamp();
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
        public string ToString(string format, IFormatProvider formatProvider = null)
        {
            if (_denominator == 1 || _numerator == 0)
                return _numerator.ToString(format, formatProvider);
            return _numerator.ToString(format, formatProvider)
                + '/'
                + _denominator.ToString(format, formatProvider);
        }

        #endregion

        #region IStructuralEquatable

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            if (other == null || other.GetType() != GetType())
                return false;

            var that = (Fraction)other;
            return comparer.Equals(_numerator, that._numerator) && comparer.Equals(_denominator, that._denominator);
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            return comparer.CombineHashCodes(_numerator, _denominator);
        }

        #endregion
    }
}
