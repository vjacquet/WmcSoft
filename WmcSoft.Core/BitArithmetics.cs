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

namespace WmcSoft
{
    /// <summary>
    /// Provides a set of static methods for bit manipulation. This is a static class.
    /// </summary>
    /// <remarks>Formula taken from Hacker's Delight, second edition, by Henry S. Warren, Jr.</remarks>
    public static class BitArithmetics
    {
        #region Arithmetics on uint

        public static uint TurnOffRightMostOne(uint x) {
            return x & (x - 1);
        }

        public static uint TurnOnRightMostZero(uint x) {
            return x | (x + 1);
        }

        public static uint TurnOffTrailingOnes(uint x) {
            return x & (x + 1);
        }

        public static uint TurnOnTrailingZeroes(uint x) {
            return x | (x - 1);
        }

        public static uint MarkRightMostZero(uint x) {
            return ~x & (x + 1);
        }

        public static uint MarkRightMostOne(uint x) {
            return ~(~x | (x - 1));
        }

        public static bool IsPowerOfTwo(uint x) {
            return 0 == (x & (x - 1));
        }

        public static int CountBits(uint x) {
            int n = 0;
            while (x != 0) {
                ++n;
                x &= (x - 1);
            }
            return n;
        }

        #endregion

        #region Lb

        public static int Lb(int x) {
            if (x < 0) throw new ArgumentOutOfRangeException(nameof(x));

            var lb = 0;
            for (int i = x - 1; i > 0; i /= 2) {
                lb++;
            }
            return lb;
        }

        #endregion
    }
}
