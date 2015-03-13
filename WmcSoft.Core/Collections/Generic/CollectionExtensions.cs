﻿#region Licence

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
using System.Globalization;
using System.Linq;
using System.Text;

namespace WmcSoft.Collections.Generic
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
        public static void Suffle<T>(this IList<T> list) {
            Suffle(list, new Random());
        }

        /// <summary>
        /// Suffles in place items of the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="random">The random object to use to perfom the suffle.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when list or random is null</exception>
        /// <exception cref="System.ArgumentException">Thrown when list is read only</exception>
        public static void Suffle<T>(this IList<T> list, Random random) {
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
        /// <typeparam name="T">The type of the element of <paramref name="list"/>.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="i">The i.</param>
        /// <param name="j">The j.</param>
        /// <returns>The list</returns>
        /// <remarks>This function does not guard against null list or out of bound indices.</remarks>
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
        /// <remarks>Does nothing if items is null.</remarks>
        public static ICollection<T> AddRange<T>(this ICollection<T> self, IEnumerable<T> items) {
            if (self == null)
                throw new ArgumentNullException("self");

            if (items == null)
                return self;

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

        #endregion

        #region RemoveRange methods

        /// <summary>
        /// Remove a range of items from a collection. 
        /// </summary>
        /// <typeparam name="T">Type of objects within the collection.</typeparam>
        /// <param name="self">The collection to remove items from.</param>
        /// <param name="items">The items to remove from the collection.</param>
        /// <returns>The count of items removed from the collection.</returns>
        /// <remarks>Does nothing if items is null.</remarks>
        public static int RemoveRange<T>(this ICollection<T> self, IEnumerable<T> items) {
            if (self == null)
                throw new ArgumentNullException("self");

            if (items == null)
                return 0;

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

        #endregion

        #region ToString

        static TextInfo GetTextInfo(IFormatProvider formatProvider) {
            TextInfo info;
            CultureInfo cultureProvider = formatProvider as CultureInfo;
            if (cultureProvider != null) {
                return cultureProvider.TextInfo;
            }

            info = formatProvider as TextInfo;
            if (info != null) {
                return info;
            }

            if (formatProvider != null) {
                info = formatProvider.GetFormat(typeof(TextInfo)) as TextInfo;
                if (info != null) {
                    return info;
                }
            }

            return CultureInfo.CurrentCulture.TextInfo;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public static string ToString<T>(this IEnumerable<T> enumerable, string format, IFormatProvider formatProvider = null)
            where T : IFormattable {
            if (enumerable == null)
                throw new ArgumentNullException("enumerable");

            TextInfo textInfo = GetTextInfo(formatProvider);
            var separator = textInfo.ListSeparator;
            var it = enumerable.GetEnumerator();
            if (it.MoveNext()) {
                var sb = new StringBuilder(it.Current.ToString(format, formatProvider));
                while (it.MoveNext()) {
                    sb.Append(separator);
                    sb.Append(it.Current.ToString(format, formatProvider));
                }
                return sb.ToString();
            }
            return "";
        }

        public static string ToString<T>(this IEnumerable<T> enumerable, IFormatProvider formatProvider)
            where T : IFormattable {
            return enumerable.ToString(null, formatProvider);
        }

        #endregion

        #region ToArray methods

        /// <summary>
        /// Convert a collection to an array.
        /// </summary>
        /// <typeparam name="TInput">Type of the collection items</typeparam>
        /// <typeparam name="TOutput">Type of the array items.</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="convert">The converter from the input type to the output type.</param>
        /// <returns>An array or null if collection is null</returns>
        /// <remarks>Uses the Count of items of the collection to avoid amortizing reallocations.</remarks>
        public static TOutput[] ToArray<TInput, TOutput>(this IReadOnlyCollection<TInput> collection, Converter<TInput, TOutput> convert) {
            if (convert == null)
                throw new ArgumentNullException("convert");
            if (collection == null)
                return null;

            var output = new TOutput[collection.Count];
            var i = 0;
            var enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext()) {
                output[i++] = convert(enumerator.Current);
            }
            return output;
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
        public static TOutput[] ToArray<TInput, TOutput>(this IReadOnlyList<TInput> list, Converter<TInput, TOutput> convert) {
            if (convert == null)
                throw new ArgumentNullException("convert");
            if (list == null)
                return null;

            var length = list.Count;
            var output = new TOutput[length];
            // for List implementation, for loops are slightly faster than foreach loops.
            for (int i = 0; i < length; i++) {
                output[i] = convert(list[i]);
            }
            return output;
        }

        #endregion
    }
}
