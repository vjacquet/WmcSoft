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
    /// <summary>
    /// Represents a generic mutable collection of key/value pairs for wich the key can have multiple values.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the mutable index.</typeparam>
    /// <typeparam name="TValue">The type of values in the mutable index.</typeparam>
    public interface IIndex<TKey, TValue> : IReadOnlyIndex<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// Adds the <paramref name="key"/>/<paramref name="value"/> pair to the index.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if a new entry with the specified <paramref name="key"/> was created; otherwise, <c>false</c>.</returns>
        new bool Add(KeyValuePair<TKey, TValue> item);

        /// <summary>
        /// Adds the <paramref name="key"/>/<paramref name="value"/> pair to the index.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if a new entry with the specified <paramref name="key"/> was created; otherwise, <c>false</c>.</returns>
        bool Add(TKey key, TValue value);

        /// <summary>
        /// Removes the values associated with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The count of values removed.</returns>
        int Remove(TKey key);

        /// <summary>
        /// Removes the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if <paramref name="key"/>/<paramref name="value"/> pair was successfully removed from the <see cref="IIndex<TKey, TValue>"/>; otherwise, <c>false</c>. 
        /// This method also returns <c>false</c> if <paramref name="key"/>/<paramref name="value"/> pair is not found in the original <see cref="IIndex<TKey, TValue>"/>.</returns>
        bool Remove(TKey key, TValue value);
    }
}