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
using System.Text;

namespace WmcSoft.Collections
{

    /// <summary>
    /// Provides a set of static methods to extend collection related classes or interfaces.
    /// </summary>
    public static class CollectionExtensions
    {
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
        /// <returns></returns>
        public static IList SwapItems(this IList list, int i, int j) {
            object temp = list[i];
            list[i] = list[j];
            list[j] = temp;
            return list;
        }

        /// <summary>
        /// Swaps two items.
        /// </summary>
        /// <typeparam name="T">The type of the element of <paramref name="list"/>.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="i">The i.</param>
        /// <param name="j">The j.</param>
        /// <returns></returns>
        public static IList<T> SwapItems<T>(this IList<T> list, int i, int j) {
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
            return list;
        }

        #endregion

        #region AddRange methods

        /// <summary>
        /// Add a range of items to a collection. 
        /// </summary>
        /// <typeparam name="T">Type of objects within the collection.</typeparam>
        /// <param name="self">The collection to add items to.</param>
        /// <param name="items">The items to add to the collection.</param>
        /// <returns>The collection.</returns>
        public static ICollection<T> AddRange<T>(this ICollection<T> self, IEnumerable<T> items) {
            ICollection collection = self as ICollection;

            if (collection != null && collection.IsSynchronized) {
                lock (collection.SyncRoot) {
                    foreach (var each in items) {
                        self.Add(each);
                    }
                }
            } else {
                foreach (var each in items) {
                    self.Add(each);
                }
            }

            return self;
        }

        /// <summary>
        /// Add a range of items to a list. 
        /// </summary>
        /// <param name="self">The list to add items to.</param>
        /// <param name="items">The items to add to the list.</param>
        /// <returns>The list.</returns>
        public static IList AddRange(this IList self, IEnumerable items) {
            if (self.IsSynchronized) {
                lock (self.SyncRoot) {
                    foreach (var each in items) {
                        self.Add(each);
                    }
                }
            } else {
                foreach (var each in items) {
                    self.Add(each);
                }
            }

            return self;
        }

        #endregion

        #region RemoveRange methods

        /// <summary>
        /// Remove a range of items from a collection. 
        /// </summary>
        /// <typeparam name="T">Type of objects within the collection.</typeparam>
        /// <param name="self">The collection to remove items from.</param>
        /// <param name="items">The items to remove from the collection.</param>
        /// <returns>The collection.</returns>
        public static int RemoveRange<T>(this ICollection<T> self, IEnumerable<T> items) {
            ICollection collection = self as ICollection;
            int count = 0;

            if (collection != null && collection.IsSynchronized) {
                lock (collection.SyncRoot) {
                    foreach (var each in items) {
                        if (self.Remove(each))
                            count++;
                    }
                }
            } else {
                foreach (var each in items) {
                    if (self.Remove(each))
                        count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Remove a range of items from a list. 
        /// </summary>
        /// <param name="self">The list to remove items from.</param>
        /// <param name="items">The items to remove from the list.</param>
        /// <returns>The list.</returns>
        public static void RemoveRange(this IList self, IEnumerable items) {
            if (self.IsSynchronized) {
                lock (self.SyncRoot) {
                    foreach (var each in items) {
                        self.Remove(each);
                    }
                }
            } else {
                foreach (var each in items) {
                    self.Remove(each);
                }
            }
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public static string ToString(this IEnumerable enumerable) {
            string separator = System.Threading.Thread.CurrentThread.CurrentUICulture.TextInfo.ListSeparator;
            StringBuilder sb = new StringBuilder("[");

            IEnumerator it = enumerable.GetEnumerator();
            if (it.MoveNext()) {
                sb.Append(it.Current.ToString());

                while (it.MoveNext()) {
                    sb.Append(separator);
                    sb.Append(it.Current.ToString());
                }
            }

            sb.Append(']');

            return sb.ToString();
        }

        #endregion

        #region Array methods

        /// <summary>
        /// Sorts the elements in an entire Array using the <see cref="IComparable&lt;T&gt;"/> generic interface implementation of each element of the Array.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="array">The one-dimensional, zero-based Array to sort.</param>
        /// <returns>The one-dimensional, zero-based sorted Array</returns>
        public static T[] Sort<T>(this T[] array) {
            Array.Sort(array);
            return array;
        }
        /// <summary>
        /// Sorts the elements in an Array using the specified <see cref="Comparison&lt;T&gt;"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="array">The one-dimensional, zero-based Array to sort.</param>
        /// <param name="comparison">The <see cref="Comparison&lt;T&gt;"/> to use when comparing elements.</param>
        /// <returns>The one-dimensional, zero-based sorted Array</returns>
        public static T[] Sort<T>(this T[] array, Comparison<T> comparison) {
            Array.Sort(array, comparison);
            return array;
        }
        /// <summary>
        /// Sorts the elements in an Array using the specified <see cref="IComparer&lt;T&gt;"/> generic interface.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="array">The one-dimensional, zero-based Array to sort.</param>
        /// <param name="comparer">The <see cref="IComparer&lt;T&gt;"/> generic interface implementation to use when comparing elements, or null to use the IComparable<T> generic interface implementation of each element.</param>
        /// <returns>The one-dimensional, zero-based sorted Array</returns>
        public static T[] Sort<T>(this T[] array, IComparer<T> comparer) {
            Array.Sort(array, comparer);
            return array;
        }

        #endregion
    }
}
