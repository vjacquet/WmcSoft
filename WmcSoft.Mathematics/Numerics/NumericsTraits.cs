using System;
using System.Numerics;

namespace WmcSoft.Numerics
{
    // TODO: Should it be a static class or a generic interface?
    //       The former allow using the traits as free functions 
    //       whereas the later can be used to declare traits for other types.
    public static class NumericsTraits
    {
        #region IsInteger

        public static bool IsInteger(int i) { return true; }

        public static bool IsInteger(BigInteger i) { return true; }

        public static bool IsInteger(Complex i) {
            return i.Imaginary == 0d && IsInteger(i.Real);
        }

        public static bool IsInteger(double i) {
            var j = Math.Abs(i);
            return (j - Math.Truncate(j)) == 0d;
        }

        public static bool IsInteger(decimal i) {
            var j = Math.Abs(i);
            return (j - Decimal.Truncate(j)) == 0m;
        }

        public static bool IsInteger(Fraction i) {
            return i.Denominator == 1;
        }

        public static bool IsInteger(BigFraction i) {
            return i.Denominator == BigInteger.One;
        }

        #endregion
    }
}
