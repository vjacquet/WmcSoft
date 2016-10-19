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

namespace WmcSoft.Collections.Generic
{
    public interface IReadOnlyIndex<TKey, TValue> : IReadOnlyCollection<KeyValuePair<TKey, TValue>>
    {
        // <summary>Gets the element that has the specified key in the read-only dictionary.</summary>
        /// <param name="key">The key to locate.</param>
        /// <returns>The element that has the specified key in the read-only dictionary.</returns>
        /// <exception cref="ArgumentNullException">key is null.</exception>
        IReadOnlyList<TValue> this[TKey key] { get; }

        /// <summary>
        ///  Gets an enumerable collection that contains the keys in the read-only index.
        /// </summary>
        /// <returns>An enumerable collection that contains the keys in the read-only index.</returns>
        IEnumerable<TKey> Keys { get; }

        /// <summary>
        /// Determines whether the read-only dictionary contains an element that has the specified key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <returns>true if the read-only index contains an element that has the specified key; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">key is null.</exception>
        bool ContainsKey(TKey key);

        /// <summary>
        /// Gets the values that is associated with the specified key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <exception cref="ArgumentNullException">key is null.</exception>
        IEnumerable<TValue> GetValues(TKey key);
    }
}
