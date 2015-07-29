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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft
{
    public static class OrdinalExtensions
    {
        public static T Next<T>(this IOrdinal<T> ordinal, T x, int n = 1) {
            return ordinal.Advance(x, n);
        }

        public static T Previous<T>(this IOrdinal<T> ordinal, T x, int n = 1) {
            return ordinal.Advance(x, -n);
        }

        public static IEnumerable<T> Sequence<T>(this IOrdinal<T> ordinal, T first, T last, int stride = 1) {
            if (stride > 0) {
                while (ordinal.Compare(first, last) <= 0) {
                    yield return first;
                    first = ordinal.Advance(first, stride);
                }
            } else if (stride < 0) {
                while (ordinal.Compare(first, last) >= 0) {
                    yield return first;
                    first = ordinal.Advance(first, stride);
                }
            } else {
                yield return first;
                if (ordinal.Compare(first, last) != 0)
                    yield return last;
            }
        }
    }
}
