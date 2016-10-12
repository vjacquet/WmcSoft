﻿#region Licence

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
    public static class DecimalExtensions
    {
        private static readonly decimal[] Powers;
        static DecimalExtensions() {
        const int MaxPower = 29; // last power before overflow

            Powers = new decimal[MaxPower];
            Powers[0] = 1m;

            var n = 1m;
            for (int i = 1; i < MaxPower; i++) {
                n *= 10m;
                Powers[i] = n;
            }
        }

        /// <summary>
        /// Returns the number of digits in the unscaled value.
        /// </summary>
        /// <param name="value">The decimal value.</param>
        /// <returns>Returns the <paramref name="value"/>'s precision.</returns>
        public static int Precision(this decimal value) {
            if (value < 0m)
                value = -value;
            var scale = Scale(value);
            value *= Powers[scale];
            var found = Array.BinarySearch(Powers, 1, Powers.Length - 1, value);
            if (found >= 0)
                return found + 1;
            return ~found;
        }

        /// <summary>
        /// Returns the number of digits to the right of the decimal point.
        /// </summary>
        /// <param name="value">The decimal value.</param>
        /// <returns>Returns the <paramref name="value"/>'s scale.</returns>
        public static int Scale(this decimal value) {
            // Scale mask for the flags field. This byte in the flags field contains
            // the power of 10 to divide the Decimal value by. The scale byte must
            // contain a value between 0 and 28 inclusive.
            const int ScaleMask = 0x00FF0000;

            // Number of bits scale is shifted by.
            const int ScaleShift = 16;

            var bits = decimal.GetBits(value);
            return (bits[3] & ScaleMask) >> ScaleShift;
        }
    }
}
