#region Licence

/****************************************************************************
          Copyright 1999-2015 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft.Collections
{
    /// <summary>
    /// Provides a set of static methods to extend collection related classes or interfaces.
    /// </summary>
    public static class CollectionExtensions
    {
        #region AddRange methods

        /// <summary>
        /// Ensure the value is present by adding it when missing.
        /// </summary>
        /// <param name="list">The list to add items to.</param>
        /// <param name="value">The value</param>
        /// <returns>True if the value was added, false it was already in the list</returns>
        public static bool Ensure(this IList list, object value)
        {
            if (list.IsSynchronized) {
                lock (list.SyncRoot) {
                    if (!list.Contains(value)) {
                        list.Add(value);
                        return true;
                    }
                }
            } else {
                if (!list.Contains(value)) {
                    list.Add(value);
                    return true;
                }
            }
            return false;
        }

        static void UnguardedAddRange(IList list, IEnumerable items)
        {
            foreach (var each in items) {
                list.Add(each);
            }
        }

        /// <summary>
        /// Add a range of items to a list. 
        /// </summary>
        /// <typeparam name="TList">The type of list</typeparam>
        /// <param name="list">The list to add items to.</param>
        /// <param name="items">The items to add to the list.</param>
        /// <returns>The list.</returns>
        /// <remarks>Does nothing if items is null.</remarks>
        public static TList AddRange<TList>(this TList list, IEnumerable items)
          where TList : IList
        {
            if (list == null) throw new ArgumentNullException(nameof(list));

            if (items == null)
                return list;

            if (list.IsSynchronized) {
                lock (list.SyncRoot) {
                    UnguardedAddRange(list, items);
                }
            } else {
                UnguardedAddRange(list, items);
            }

            return list;
        }

        #endregion

        #region Backwards

        public static IEnumerable Backwards(this IList source)
        {
            for (var i = source.Count - 1; i >= 0; i--) {
                yield return source[i];
            }
        }

        /// <summary>
        /// Inverts the order of the elements in a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <see cref="source"/>.</typeparam>
        /// <param name="source">A sequence of values to reverse.</param>
        /// <returns>A sequence whose elements correspond to those of the input sequence in reverse order.</returns>
        /// <remarks>Similar to <see cref="Enumerable.Reverse{TSource}(IEnumerable{TSource})"/> except that a copy 
        /// is avoided for classes implementing <see cref="IList"/>.</remarks>
        public static IEnumerable Backwards(this IEnumerable source)
        {
            if (source is IList list)
                return Backwards(list);

            return source.Cast<object>().Reverse();
        }

        #endregion

        #region RemoveRange methods

        static int UnguardedRemoveRange(IList source, IEnumerable items)
        {
            var count = source.Count;
            foreach (var each in items) {
                source.Remove(each);
            }
            return count - source.Count;
        }

        /// <summary>
        /// Remove a range of items from a list. 
        /// </summary>
        /// <param name="source">The list to remove items from.</param>
        /// <param name="items">The items to remove from the list.</param>
        /// <returns>The count of items removed from the collection.</returns>
        /// <remarks>Does nothing if items is null.</remarks>
        public static int RemoveRange(this IList source, IEnumerable items)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (items == null)
                return 0;

            if (source.IsSynchronized) {
                lock (source.SyncRoot) {
                    return UnguardedRemoveRange(source, items);
                }
            } else {
                return UnguardedRemoveRange(source, items);
            }
        }

        #endregion

        #region ReplaceAll

        /// <summary>
        /// Removes all existing items and adds the given items.
        /// </summary>
        /// <param name="source">The list</param>
        /// <param name="items">The items to add</param>
        public static void ReplaceAll(this IList source, IEnumerable items)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (items == null) {
                source.Clear();
            } else if (source.IsSynchronized) {
                lock (source.SyncRoot) {
                    source.Clear();
                    UnguardedAddRange(source, items);
                }
            } else {
                source.Clear();
                UnguardedAddRange(source, items);
            }
        }

        #endregion

        #region Shuffle methods

        static void UnguardedShuffle(this IList source, int startIndex, int count, Random random)
        {
            for (var i = startIndex; i < count; i++) {
                SwapItems(source, i, random.Next(i, count));
            }
        }

        /// <summary>
        /// Suffles in place items of the list.
        /// </summary>
        /// <param name="source">The list.</param>
        /// <exception cref="ArgumentNullException">Thrown when list is null</exception>
        /// <exception cref="ArgumentException">Thrown when list is read only</exception>
        /// <remarks>Implements Fisher-Yates suffle, https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle </remarks>
        public static void Shuffle(this IList source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.IsReadOnly) throw new ArgumentException();

            UnguardedShuffle(source, 0, source.Count, new Random());
        }

        /// <summary>
        /// Suffles in place items of the list.
        /// </summary>
        /// <param name="source">The list.</param>
        /// <param name="random">The random object to use to perfom the suffle.</param>
        /// <exception cref="ArgumentNullException">Thrown when list or random is null</exception>
        /// <exception cref="ArgumentException">Thrown when list is read only</exception>
        /// <remarks>Implements Fisher-Yates suffle, https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle </remarks>
        public static void Shuffle(this IList source, Random random)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.IsReadOnly) throw new ArgumentException();
            if (random == null) throw new ArgumentNullException(nameof(random));

            UnguardedShuffle(source, 0, source.Count, random);
        }

        public static void PartialShuffle(this IList source, int count)
        {
            PartialShuffle(source, count, new Random());
        }

        public static void PartialShuffle(this IList source, int count, Random random)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (random == null) throw new ArgumentNullException(nameof(random));
            if (source.IsReadOnly) throw new ArgumentException();
            if (count > source.Count) throw new ArgumentOutOfRangeException(nameof(count));

            int j;
            for (var i = 0; i < count; i++) {
                j = random.Next(i, source.Count);
                SwapItems(source, i, j);
            }
        }

        #endregion

        #region SwapItems methods

        /// <summary>
        /// Swaps two items.
        /// </summary>
        /// <param name="source">The list.</param>
        /// <param name="i">The item at the <paramref name="i"/> index.</param>
        /// <param name="j">The item at the <paramref name="j"/> index.</param>
        /// <returns>The list</returns>
        /// <remarks>This function does not guard against null list or out of bound indices.</remarks>
        public static TList SwapItems<TList>(this TList source, int i, int j)
            where TList : IList
        {
            var temp = source[i];
            source[i] = source[j];
            source[j] = temp;
            return source;
        }

        #endregion

        #region ToArray

        /// <summary>
        /// Converts a list to an array.
        /// </summary>
        /// <param name="source">The list</param>
        /// <returns>An array of objets containing all the items in the collection.</returns>
        public static object[] ToArray(this IList source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var items = new object[source.Count];
            source.CopyTo(items, 0);
            return items;
        }

        /// <summary>
        /// Convert a list to an array.
        /// </summary>
        /// <typeparam name="TInput">Type of the list items</typeparam>
        /// <typeparam name="TOutput">Type of the array items.</typeparam>
        /// <param name="source">The list</param>
        /// <param name="convert">The converter from the input type to the output type.</param>
        /// <returns>An array</returns>
        /// <remarks>Uses the Count of items of the list to avoid amortizing reallocations.</remarks>
        public static TOutput[] ToArray<TInput, TOutput>(this ICollection source, Converter<TInput, TOutput> convert)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (convert == null) throw new ArgumentNullException(nameof(convert));

            var length = source.Count;
            var output = new TOutput[length];
            // for List implementation, for loops are slightly faster than foreach loops.
            var i = 0;
            foreach (var item in source) {
                output[i++] = convert((TInput)item);
            }
            return output;
        }

        #endregion

        #region ToArrayList

        /// <summary>
        /// Creates an <see cref="ArrayList"/> from an <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the collection's items.</typeparam>
        /// <param name="source">The collection.</param>
        /// <returns>The array list</returns>
        public static ArrayList ToArrayList<T>(this ICollection<T> source)
        {
            var result = new ArrayList(source.Count);
            UnguardedAddRange(result, source);
            return result;
        }

        #endregion
    }
}