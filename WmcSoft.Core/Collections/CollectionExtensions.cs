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
using System.Linq;
using System.Text;

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
        public static bool Ensure(this IList list, object value) {
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

        /// <summary>
        /// Add a range of items to a list. 
        /// </summary>
        /// <typeparam name="TList">The type of list</typeparam>
        /// <param name="list">The list to add items to.</param>
        /// <param name="items">The items to add to the list.</param>
        /// <returns>The list.</returns>
        /// <remarks>Does nothing if items is null.</remarks>
        public static TList AddRange<TList>(this TList list, IEnumerable items)
          where TList : IList {
            if (list == null)
                throw new ArgumentNullException("list");

            if (items == null)
                return list;

            if (list.IsSynchronized) {
                lock (list.SyncRoot) {
                    foreach (var each in items) {
                        list.Add(each);
                    }
                }
            } else {
                foreach (var each in items) {
                    list.Add(each);
                }
            }

            return list;
        }

        #endregion

        #region RemoveRange methods

        /// <summary>
        /// Remove a range of items from a list. 
        /// </summary>
        /// <param name="source">The list to remove items from.</param>
        /// <param name="items">The items to remove from the list.</param>
        /// <returns>The count of items removed from the collection.</returns>
        /// <remarks>Does nothing if items is null.</remarks>
        public static int RemoveRange(this IList source, IEnumerable items) {
            if (source == null)
                throw new ArgumentNullException("source");

            if (items == null)
                return 0;

            if (source.IsSynchronized) {
                lock (source.SyncRoot) {
                    var count = source.Count;
                    foreach (var each in items) {
                        source.Remove(each);
                    }
                    return count - source.Count;
                }
            } else {
                var count = source.Count;
                foreach (var each in items) {
                    source.Remove(each);
                }
                return count - source.Count;
            }
        }

        #endregion

        #region ReplaceAll

        /// <summary>
        /// Removes all existing items and adds the given items.
        /// </summary>
        /// <param name="list">The list</param>
        /// <param name="items">The items to add</param>
        public static void ReplaceAll(this IList list, IEnumerable items) {
            if (list == null)
                throw new ArgumentNullException("list");

            if (list.IsSynchronized) {
                lock (list.SyncRoot) {
                    list.Clear();
                    if (items == null)
                        return;
                    foreach (var each in items) {
                        list.Add(each);
                    }
                }
            } else {
                list.Clear();
                if (items == null)
                    return;
                foreach (var each in items) {
                    list.Add(each);
                }
            }
        }

        #endregion

        #region ToArray

        /// <summary>
        /// Converts a list to an array.
        /// </summary>
        /// <param name="source">The list</param>
        /// <returns>An array of objets containing all the items in the collection.</returns>
        public static object[] ToArray(this IList source) {
            if (source == null)
                throw new ArgumentNullException("source");

            var items = new object[source.Count];
            source.CopyTo(items, 0);
            return items;
        }

        /// <summary>
        /// Convert a list to an array.
        /// </summary>
        /// <typeparam name="TInput">Type of the list items</typeparam>
        /// <typeparam name="TOutput">Type of the array items.</typeparam>
        /// <param name="collection">The list</param>
        /// <param name="convert">The converter from the input type to the output type.</param>
        /// <returns>An array</returns>
        /// <remarks>Uses the Count of items of the list to avoid amortizing reallocations.</remarks>
        public static TOutput[] ToArray<TInput, TOutput>(this ICollection collection, Converter<TInput, TOutput> convert) {
            if (convert == null)
                throw new ArgumentNullException("convert");
            if (collection == null)
                return null;

            var length = collection.Count;
            var output = new TOutput[length];
            // for List implementation, for loops are slightly faster than foreach loops.
            var i = 0;
            foreach (var item in collection) {
                output[i++] = convert((TInput)item);
            }
            return output;
        }

        #endregion

        #region Suffle methods

        /// <summary>
        /// Suffles in place items of the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when list is null</exception>
        /// <exception cref="System.ArgumentException">Thrown when list is read only</exception>
        public static void Suffle(this IList list) {
            Suffle(list, new Random());
        }

        /// <summary>
        /// Suffles in place items of the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="random">The random object to use to perfom the suffle.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when list or random is null</exception>
        /// <exception cref="System.ArgumentException">Thrown when list is read only</exception>
        public static void Suffle(this IList list, Random random) {
            if (list == null)
                throw new ArgumentNullException("list");
            if (random == null)
                throw new ArgumentNullException("random");
            if (list.IsReadOnly)
                throw new ArgumentException();

            int j;

            for (int i = 0; i < list.Count; i++) {
                j = random.Next(i, list.Count);
                list.SwapItems(i, j);
            }
        }

        #endregion

        #region SwapItems methods

        /// <summary>
        /// Swaps two items.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="i">The item at the <paramref name="i"/> index.</param>
        /// <param name="j">The item at the <paramref name="j"/> index.</param>
        /// <returns>The list</returns>
        /// <remarks>This function does not guard against null list or out of bound indices.</remarks>
        public static TList SwapItems<TList>(this TList list, int i, int j)
            where TList : IList {
            object temp = list[i];
            list[i] = list[j];
            list[j] = temp;
            return list;
        }

        #endregion
    }
}
