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
    public struct DecimalArithmetics : IArithmetics<decimal>
    {
        #region IArithmetics<decimal> Membres

        public decimal Add(decimal x, decimal y)
        {
            return x + y;
        }

        public decimal Subtract(decimal x, decimal y)
        {
            return x - y;
        }

        public decimal Multiply(decimal x, decimal y)
        {
            return x * y;
        }

        public decimal Divide(decimal x, decimal y)
        {
            return x / y;
        }

        public decimal Remainder(decimal x, decimal y)
        {
            return x % y;
        }

        public decimal DivideRemainder(decimal x, decimal y, out decimal remainder)
        {
            remainder = x % y;
            return x / y;
        }

        public decimal Negate(decimal x)
        {
            return -x;
        }

        public decimal Reciprocal(decimal x)
        {
            return 1m / x;
        }

        public decimal Zero {
            get { return 0m; }
        }

        public decimal One {
            get { return 1m; }
        }

        public bool SupportReciprocal {
            get { return true; }
        }

        #endregion
    }
}
