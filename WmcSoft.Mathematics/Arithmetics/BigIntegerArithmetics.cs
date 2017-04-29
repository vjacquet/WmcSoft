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

namespace WmcSoft.Arithmetics
{
    public struct BigIntegerArithmetics : IArithmetics<BigInteger>
    {
        #region IArithmetics<BigInteger> Membres

        public BigInteger Add(BigInteger x, BigInteger y)
        {
            return x + y;
        }

        public BigInteger Subtract(BigInteger x, BigInteger y)
        {
            return x - y;
        }

        public BigInteger Multiply(BigInteger x, BigInteger y)
        {
            return x * y;
        }

        public BigInteger Divide(BigInteger x, BigInteger y)
        {
            return x / y;
        }

        public BigInteger Remainder(BigInteger x, BigInteger y)
        {
            return x % y;
        }

        public BigInteger DivideRemainder(BigInteger x, BigInteger y, out BigInteger remainder)
        {
            return BigInteger.DivRem(x, y, out remainder);
        }

        public BigInteger Negate(BigInteger x)
        {
            return -x;
        }

        public BigInteger Reciprocal(BigInteger x)
        {
            throw new NotSupportedException();
        }

        public BigInteger Zero {
            get { return 0; }
        }

        public BigInteger One {
            get { return 1; }
        }

        public bool SupportReciprocal {
            get { return false; }
        }

        #endregion
    }
}
