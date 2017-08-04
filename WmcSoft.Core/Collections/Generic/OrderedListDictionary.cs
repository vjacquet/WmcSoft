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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WmcSoft.Collections.Generic.Internals;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Implements a generic collection of key/value pairs using a list.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    public class OrderedListDictionary<TKey, TValue> : SortedList<TKey, TValue>, IOrderedDictionary<TKey, TValue>
    {
        #region Lifecycle

        public OrderedListDictionary() : base()
        {
        }

        public OrderedListDictionary(int capacity) : base(capacity)
        {
        }

        public OrderedListDictionary(IComparer<TKey> comparer) : base(comparer)
        {
        }

        public OrderedListDictionary(int capacity, IComparer<TKey> comparer) : base(capacity, comparer)
        {
        }

        public OrderedListDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
        }

        public OrderedListDictionary(IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer) : base(dictionary, comparer)
        {
        }

        #endregion

        #region IReadOnlyOrderedDictionary<TKey, TValue> members

        ICollection<KeyValuePair<TKey, TValue>> Storage => this;

        /// <summary>
        /// The <see cref="KeyValuePair{TKey, TValue}"/> with the smalled key.
        /// </summary>
        public KeyValuePair<TKey, TValue> Min {
            get {
                return Storage.First();
            }
        }

        /// <summary>
        /// The <see cref="KeyValuePair{TKey, TValue}"/> with the largest key.
        /// </summary>
        public KeyValuePair<TKey, TValue> Max {
            get {
                return Storage.Last();
            }
        }

        /// <summary>
        /// Largest key less than or equal to <paramref name="key"/>.
        /// </summary>
        public KeyValuePair<TKey, TValue> Floor(TKey key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Smallest key greater than or equal to <paramref name="key"/>.
        /// </summary>
        public KeyValuePair<TKey, TValue> Ceiling(TKey key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Number of keys less than <paramref name="key"/>.
        /// </summary>
        public int Rank(TKey key)
        {
            return Keys.AsReadOnly().BinaryRank(key, Comparer);
        }

        /// <summary>
        /// Key of rank <paramref name="k"/>.
        /// </summary>
        public KeyValuePair<TKey, TValue> Select(int k)
        {
            if (k < 0 | k > Count) throw new ArgumentOutOfRangeException(nameof(k));

            return new KeyValuePair<TKey, TValue>(Keys[k], Values[k]);
        }

        /// <summary>
        /// The number of values between the <paramref name="lo"/> and <paramref name="hi"/> keys.
        /// </summary>
        /// <param name="lo">The lowest key.</param>
        /// <param name="hi">The highest key</param>
        /// <returns>The number of values.</returns>
        /// <remarks><paramref name="lo"/> is included and <paramref name="hi"/> is excluded.</remarks>
        public int CountBetween(TKey lo, TKey hi)
        {
            return Rank(hi) - Rank(lo);
        }

        /// <summary>
        /// The values between the <paramref name="lo"/> and <paramref name="hi"/> keys.
        /// </summary>
        /// <param name="lo">The lowest key.</param>
        /// <param name="hi">The highest key</param>
        /// <returns>The values.</returns>
        /// <remarks><paramref name="lo"/> is included and <paramref name="hi"/> is excluded.</remarks>
        public IReadOnlyCollection<KeyValuePair<TKey, TValue>> EnumerateBetween(TKey lo, TKey hi)
        {
            var l = Rank(lo);
            var h = Rank(hi);
            var count = h - l;
            var enumerable = EnumerateRange(l, count);
            return new ReadOnlyCollectionAdapter<KeyValuePair<TKey, TValue>>(count, enumerable);
        }

        IEnumerable<KeyValuePair<TKey, TValue>> EnumerateRange(int startIndex, int count)
        {
            while (startIndex != count) {
                yield return new KeyValuePair<TKey, TValue>(Keys[startIndex], Values[startIndex]);
                startIndex++;
            }
        }

        #endregion

        #region IOrderedDictionary<TKey, TValue> members

        /// <summary>
        /// Removes the minimum value.
        /// </summary>
        public void RemoveMin()
        {
            RemoveAt(0);
        }

        /// <summary>
        /// Removes the maximum value.
        /// </summary>
        public void RemoveMax()
        {
            RemoveAt(Count - 1);
        }

        #endregion
    }
}