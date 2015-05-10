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

namespace WmcSoft.Arithmetics
{
    public struct DoubleArithmetics : IArithmetics<double>
    {
        #region IArithmetics<double> Membres

        public double Add(double x, double y) {
            return x + y;
        }

        public double Subtract(double x, double y) {
            return x - y;
        }

        public double Multiply(double x, double y) {
            return x * y;
        }

        public double Divide(double x, double y) {
            return x / y;
        }

        public double Remainder(double x, double y) {
            return x % y;
        }

        public double DivideRemainder(double x, double y, out double remainder) {
            remainder = x % y;
            return x / y;
        }

        public double Negate(double x) {
            return -x;
        }

        public double Reciprocal(double x) {
            return 1d / x;
        }

        public double Zero {
            get { return 0d; }
        }

        public double One {
            get { return 1d; }
        }

        public bool SupportReciprocal {
            get { return true; }
        }

        #endregion
    }
}
