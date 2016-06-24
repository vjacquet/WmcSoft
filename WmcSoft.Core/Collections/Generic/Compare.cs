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

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Defines static methods to create comparer builders.
    /// This is a static class. 
    /// </summary>
    public static class Compare
    {
        internal static int CombineHashCodes(int h1, int h2) {
            return (((h1 << 5) + h1) ^ h2);
        }

        public static ComparerBuilder<T> OrderOf<T>() {
            return ComparerBuilder<T>.Default;
        }
    }
}
