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
        public static IList SwapItems(this IList list, int i, int j) {
            object temp = list[i];
            list[i] = list[j];
            list[j] = temp;
            return list;
        }

        #endregion

        #region AddRange methods

        /// <summary>
        /// Add a range of items to a list. 
        /// </summary>
        /// <param name="self">The list to add items to.</param>
        /// <param name="items">The items to add to the list.</param>
        /// <returns>The list.</returns>
        /// <remarks>Does nothing if items is null.</remarks>
        public static IList AddRange(this IList self, IEnumerable items) {
            if (self == null)
                throw new ArgumentNullException("self");

            if (items == null)
                return self;

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
        /// Remove a range of items from a list. 
        /// </summary>
        /// <param name="self">The list to remove items from.</param>
        /// <param name="items">The items to remove from the list.</param>
        /// <returns>The count of items removed from the collection.</returns>
        /// <remarks>Does nothing if items is null.</remarks>
        public static int RemoveRange(this IList self, IEnumerable items) {
            if (self == null)
                throw new ArgumentNullException("self");

            if (items == null)
                return 0;

            if (self.IsSynchronized) {
                lock (self.SyncRoot) {
                    var count = self.Count;
                    foreach (var each in items) {
                        self.Remove(each);
                    }
                    return count - self.Count;
                }
            } else {
                var count = self.Count;
                foreach (var each in items) {
                    self.Remove(each);
                }
                return count - self.Count;
            }
        }

        #endregion
    }
}
