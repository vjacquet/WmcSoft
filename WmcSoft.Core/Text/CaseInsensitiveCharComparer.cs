#region Licence

/****************************************************************************
          Copyright 1999-2017 Vincent J. Jacquet.  All rights reserved.

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

using System.Collections.Generic;
using System.Globalization;

namespace WmcSoft.Text
{
    public struct CaseInsensitiveCharComparer : IComparer<char>, IEqualityComparer<char>
    {
        private readonly CultureInfo _culture;

        public CaseInsensitiveCharComparer(CultureInfo ci)
        {
            _culture = ci;
        }

        public int Compare(char x, char y)
        {
            return char.ToUpper(x, _culture) - char.ToUpper(y, _culture);
        }

        public bool Equals(char x, char y)
        {
            return char.ToUpper(x, _culture) == char.ToUpper(y, _culture);
        }

        public int GetHashCode(char obj)
        {
            return char.ToUpper(obj, _culture).GetHashCode();
        }

        #region Factories

        public static CaseInsensitiveCharComparer CurrentCulture => new CaseInsensitiveCharComparer(CultureInfo.CurrentCulture);
        public static CaseInsensitiveCharComparer InvariantCulture => new CaseInsensitiveCharComparer(CultureInfo.InvariantCulture);

        #endregion
    }
}
