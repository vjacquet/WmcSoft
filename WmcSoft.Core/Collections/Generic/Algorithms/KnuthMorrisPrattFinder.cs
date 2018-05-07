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

namespace WmcSoft.Collections.Generic.Algorithms
{
    /// <summary>
    /// Implements the finder using the Knuth-Morris-Pratt alforithm.
    /// </summary>
    /// <typeparam name="T">The type of item in the sequence.</typeparam>
    public class KnuthMorrisPrattFinder<T> : IFinder<T>
    {
        readonly IReadOnlyList<T> _pattern;
        readonly IEqualityComparer<T> _comparer;
        readonly int[] _next;

        /// <summary>
        /// Creates a new instance of the <see cref="KnuthMorrisPrattFinder{T}"/> for the given <paramref name="pattern"/> and <paramref name="comparer"/>.
        /// </summary>
        /// <param name="pattern">The pattern to match.</param>
        /// <param name="comparer">The elemnt comparer.</param>
        public KnuthMorrisPrattFinder(IReadOnlyList<T> pattern, IEqualityComparer<T> comparer = null)
        {
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            _pattern = pattern;
            _comparer = comparer ?? EqualityComparer<T>.Default;

            var length = pattern.Count;
            _next = new int[length + 1];
            _next[0] = -1;
            for (int i = 0, j = -1; i < length; _next[++i] = ++j) {
                while (j >= 0 && !_comparer.Equals(pattern[i], pattern[j]))
                    j = _next[j];
            }
        }

        /// <inheritdoc/>
        public int FindNextOccurence(IReadOnlyList<T> list, int startIndex)
        {
            int i = startIndex, j = 0, m = _pattern.Count, n = list.Count;
            for (; j < m && i < n; i++, j++) {
                while (j >= 0 && !_comparer.Equals(list[i], _pattern[j]))
                    j = _next[j];
            }
            if (j == m)
                return i - m;
            return -1;
        }
    }
}
