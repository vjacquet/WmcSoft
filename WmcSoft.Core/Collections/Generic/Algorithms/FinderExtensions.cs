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
    /// <summary>
    /// Defines the extension methods in the <see cref="IFinder{T}"/> interface.
    /// This is a static class.
    /// </summary>
    public static class FinderExtensions
    {
        /// <summary>
        /// Finds the first occurence of the pattern in the <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source list.</typeparam>
        /// <param name="finder">The finder.</param>
        /// <param name="list">The list.</param>
        /// <returns>The index of the next occurence; - or - <c>-1</c> if not found.</returns>
        public static int FindFirstOccurence<T>(this IFinder<T> finder, IReadOnlyList<T> list)
        {
            return finder.FindNextOccurence(list, 0);
        }

        /// <summary>
        /// Enumerates all occurrences of the pattern in the <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source list.</typeparam>
        /// <param name="finder">The finder.</param>
        /// <param name="list">The list.</param>
        /// <returns>The index of all occurences of the pattern in the list.</returns>
        public static IEnumerable<int> FindAllOccurences<T>(this IFinder<T> finder, IReadOnlyList<T> list)
        {
            var found = finder.FindNextOccurence(list, 0);
            while (found >= 0) {
                yield return found;
                found = finder.FindNextOccurence(list, found + 1);
            }
        }
    }
}
