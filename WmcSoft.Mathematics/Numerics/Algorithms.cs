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

namespace WmcSoft.Numerics
{
    public static class Algorithms
    {
        public static int EuclidGreatestCommonDivisor(int m, int n)
        {
            if (m < 0) m = -m;
            if (n < 0) n = -n;

            while (n != 0) {
                var t = m % n;
                m = n;
                n = t;
            }
            return m;
        }

        public static int SteinGreatestCommonDivisor(int m, int n)
        {
            if (m < 0) m = -m;
            if (n < 0) n = -n;
            if (m == 0) return n;
            if (n == 0) return m;

            // m > 0 && n > 0
            int dm = 0;
            while ((m & 1) == 0) {
                m >>= 1;
                ++dm;
            }

            int dn = 0;
            while ((n & 1) == 0) {
                n >>= 1;
                ++dn;
            }

            // odd(m) && odd(n)
            while (m != n) {
                if (n > m) {
                    var t = n;
                    n = m;
                    m = t;
                }
                m -= n;
                do {
                    m >>= 1;
                } while ((m & 1) == 0);
            }

            // m == n
            return m << Math.Min(dm, dn);
        }

        public static int GreatestCommonDivisor(params int[] u)
        {
            var n = u.Length;
            switch (n) {
            case 0:
                return 1;
            case 1:
                return u[1];
            case 2:
                return SteinGreatestCommonDivisor(u[0], u[1]);
            default:
                var k = n - 1;
                var d = u[k];
                while (d != 1 && k > 0) {
                    d = SteinGreatestCommonDivisor(d, u[--k]);
                }
                return d;
            }
        }
    }
}
