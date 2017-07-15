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
    /// Represents a generic read only dictionary for wich the keys are sorted.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    public interface IReadOnlyOrderedDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        /// <summary>
        /// The <see cref="KeyValuePair{TKey, TValue}"/> with the smalled key.
        /// </summary>
        KeyValuePair<TKey, TValue> Min { get; }

        /// <summary>
        /// The <see cref="KeyValuePair{TKey, TValue}"/> with the largest key.
        /// </summary>
        KeyValuePair<TKey, TValue> Max { get; }

        /// <summary>
        /// Largest key less than or equal to <paramref name="key"/>.
        /// </summary>
        KeyValuePair<TKey, TValue> Floor(TKey key);

        /// <summary>
        /// Smallest key greater than or equal to <paramref name="key"/>.
        /// </summary>
        KeyValuePair<TKey, TValue> Ceiling(TKey key);

        /// <summary>
        /// Number of keys less than <paramref name="key"/>.
        /// </summary>
        int Rank(TKey key);

        /// <summary>
        /// Key of rank <paramref name="k"/>.
        /// </summary>
        KeyValuePair<TKey, TValue> Select(int k);

        /// <summary>
        /// The number of values between the <paramref name="lo"/> and <paramref name="hi"/> keys.
        /// </summary>
        /// <param name="lo">The lowest key.</param>
        /// <param name="hi">The highest key</param>
        /// <returns>The number of values.</returns>
        /// <remarks><paramref name="lo"/> is included and <paramref name="hi"/> is excluded.</remarks>
        int CountBetween(TKey lo, TKey hi);

        /// <summary>
        /// The values between the <paramref name="lo"/> and <paramref name="hi"/> keys.
        /// </summary>
        /// <param name="lo">The lowest key.</param>
        /// <param name="hi">The highest key</param>
        /// <returns>The values.</returns>
        /// <remarks><paramref name="lo"/> is included and <paramref name="hi"/> is excluded.</remarks>
        IReadOnlyCollection<KeyValuePair<TKey, TValue>> EnumerateBetween(TKey lo, TKey hi);
    }
}
