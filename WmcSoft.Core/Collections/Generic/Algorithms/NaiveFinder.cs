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
    /// Implements the finder by comparing the pattern item by item.
    /// </summary>
    /// <typeparam name="T">The type of item in the sequence.</typeparam>
    public class NaiveFinder<T> : IFinder<T>
    {
        readonly IReadOnlyList<T> _pattern;
        readonly IEqualityComparer<T> _comparer;

        /// <summary>
        /// Creates a new instance of the <see cref="NaiveFinder{T}"/> for the given <paramref name="pattern"/> and <paramref name="comparer"/>.
        /// </summary>
        /// <param name="pattern">The pattern to match.</param>
        /// <param name="comparer">The elemnt comparer.</param>
        public NaiveFinder(IReadOnlyList<T> pattern, IEqualityComparer<T> comparer = null)
        {
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            _pattern = pattern;
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        /// <inheritdoc/>
        public int FindNextOccurence(IReadOnlyList<T> list, int startIndex)
        {
            var length = list.Count - _pattern.Count;
            for (int i = startIndex; i < length; i++) {
                if (EqualsPattern(list, i))
                    return i;
            }
            return -1;
        }

        private bool EqualsPattern(IReadOnlyList<T> list, int startIndex)
        {
            var length = _pattern.Count;
            for (int i = 0, j = startIndex; i < length; i++, j++) {
                if (!_comparer.Equals(list[j], _pattern[i]))
                    return false;
            }
            return true;
        }
    }
}
