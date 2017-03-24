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

namespace WmcSoft.Text
{
    /// <summary>
    /// Represents a strongly-typed, read-only trie of elements.
    /// </summary>
    /// <typeparam name="TLetter">The type of letters that compose the key.</typeparam>
    /// <typeparam name="TValue">The type of the values.</typeparam>
    public interface ITrie<TLetter, TValue> : IDictionary<IReadOnlyList<TLetter>, TValue>
            where TLetter : struct, IEquatable<TLetter>
    {
        /// <summary>
        /// returns the length of the longest sequence of letters trie.
        /// </summary>
        /// <param name="query">The query sequence.</param>
        /// <returns>The length of the longest sequence; or -1 if no such sequence exists.</returns>
        int GetLengthLongestPrefixOf(IReadOnlyList<TLetter> query);

        /// <summary>
        /// Returns all of the keys in the set that start with <see cref="prefix"/>.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns>An enumerable of all the keys in the set that start with <see cref="prefix"/>.</returns>
        IEnumerable<IReadOnlyList<TLetter>> GetKeysWithPrefix(IReadOnlyList<TLetter> prefix);

        /// <summary>
        /// Returns all of the keys in the symbol table that match <see cref="pattern"/>, where the null letter is treated as a wildcard letter.
        /// </summary>
        /// <param name="pattern">The pattern</param>
        /// <returns>An enumerable of all the keys in the set that start with <see cref="prefix"/>, where the null letter is treated as a wildcard letter.</returns>
        IEnumerable<IReadOnlyList<TLetter>> Match(IReadOnlyList<TLetter?> pattern);
    }
}
