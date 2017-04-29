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

using System.Collections.Generic;

namespace WmcSoft.Collections.Generic.Algorithms
{
    public static class FinderExtensions
    {
        public static int FindFirstOccurence<T>(this IFinder<T> finder, IReadOnlyList<T> t)
        {
            return finder.FindFirstOccurence(t, 0);
        }

        public static IEnumerable<int> FindAllOccurences<T>(this IFinder<T> finder, IReadOnlyList<T> t)
        {
            var found = finder.FindFirstOccurence(t, 0);
            while (found >= 0) {
                yield return found;
                found = finder.FindFirstOccurence(t, found + 1);
            }
        }
    }
}
