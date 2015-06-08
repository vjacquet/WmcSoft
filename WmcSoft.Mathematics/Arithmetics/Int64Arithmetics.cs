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

namespace WmcSoft.Arithmetics
{
    public struct Int64Arithmetics : IArithmetics<long>
    {
        #region IArithmetics<long> Membres

        public long Add(long x, long y) {
            return x + y;
        }

        public long Subtract(long x, long y) {
            return x - y;
        }

        public long Multiply(long x, long y) {
            return x * y;
        }

        public long Divide(long x, long y) {
            return x / y;
        }

        public long Remainder(long x, long y) {
            return x % y;
        }

        public long DivideRemainder(long x, long y, out long remainder) {
            return Math.DivRem(x, y, out remainder);
        }

        public long Negate(long x) {
            return -x;
        }

        public long Reciprocal(long x) {
            throw new NotSupportedException();
        }

        public long Zero {
            get { return 0; }
        }

        public long One {
            get { return 1; }
        }

        public bool SupportReciprocal {
            get { return false; }
        }

        #endregion
    }
}
