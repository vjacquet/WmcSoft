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

namespace WmcSoft.Text
{
    /// <summary>
    /// Compares numerical strings in ascending order, without parsing the values.
    /// </summary>
    /// <remarks>2 is before 10.</remarks>
    public struct NumericalComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == null)
                return y != null ? 1 : 0;
            if (y == null)
                return -1;
            var result = x.Length - y.Length;
            if (result != 0)
                return result;
            return StringComparer.CurrentCulture.Compare(x, y);
        }
    }
}
