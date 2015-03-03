using System;

namespace WmcSoft.Numerics
{
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

        #endregion

        #region Operators

        public static implicit operator Rational(int x) {
            return new Rational(x, 1);
        }
        public static Rational FromInt32(int x) {
            return (Rational)x;
        }

        public static Rational operator +(Rational x, Rational y) {
            return new Rational(x._numerator * y._denominator + y._numerator * x._denominator, x._denominator * y._denominator);
        }
        public static Rational Add(Rational x, Rational y) {
            return x + y;
        }

        public static Rational operator -(Rational x, Rational y) {
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

        public string ToString(string format, IFormatProvider formatProvider) {
            return String.Format("{0}/{1}", _numerator, _denominator);
        }

        #endregion
    }
}
