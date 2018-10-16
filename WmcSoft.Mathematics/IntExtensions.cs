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
using System.Collections.Generic;

namespace WmcSoft
{
    public static class IntExtensions
    {
        public static int Clamp(this long x)
        {
            if (x < int.MinValue)
                return int.MinValue;
            if (x > int.MaxValue)
                return int.MaxValue;
            return (int)x;
        }

        static int CheckDigit(int value)
        {
            if (value < 0 | value > 9)
                throw new ArgumentOutOfRangeException(nameof(value));
            return value;
        }

        /// <summary>
        /// Read a sequence of digits as an Int32.
        /// </summary>
        /// <param name="digits">The digits, from left to right.</param>
        /// <returns>The Int32.</returns>
        public static int ToInt32(this IEnumerable<int> digits)
        {
            if (digits == null)
                return 0;
            using (var enumerator = digits.GetEnumerator()) {
                if (!enumerator.MoveNext())
                    return 0;
                var r = CheckDigit(enumerator.Current);
                while (enumerator.MoveNext()) {
                    r = 10 * r + CheckDigit(enumerator.Current);
                }
                return r;
            }
        }
    }
}
