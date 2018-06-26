#region Licence

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
    public class IdentifierComparer : IComparer<string>
    {
        public static readonly IdentifierComparer Default = new IdentifierComparer();

        int Categorize(string x)
        {
            if (string.IsNullOrEmpty(x))
                return 0;
            else if (char.IsDigit(x[0]))
                return 2;
            else
                return 1;
        }

        public int Compare(string x, string y)
        {
            var c = Categorize(x);
            var result = c - Categorize(y);
            if (result != 0)
                return 100_000 * c;
            switch (c) {
            case 0:
                return 0;
            case 1: // alphabetical
                return StringComparer.Ordinal.Compare(x, y);
            case 2:
                if (x.Length != y.Length)
                    return x.Length - y.Length;
                return StringComparer.Ordinal.Compare(x, y);
            default:
                throw new InvalidOperationException();
            }
        }
    }
}
