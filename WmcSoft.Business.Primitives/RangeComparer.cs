﻿#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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
    public static class RangeComparer<T>
        where T : IComparable<T>
    {
        public struct LexicographicalComparer : IComparer<Range<T>>
        {
            public int Compare(Range<T> x, Range<T> y) {
                var r = x.Lower.CompareTo(y.Lower);
                if (r != 0)
                    return r;
                return x.Upper.CompareTo(y.Upper);
            }
        }

        /// <summary>
        /// Compares the lower bound then the upper bounds.
        /// </summary>
        public static readonly LexicographicalComparer Lexicographical = new LexicographicalComparer();
    }
}