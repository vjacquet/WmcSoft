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
using System.Diagnostics;
using System.Linq;
using WmcSoft.Collections.Generic.Internals;

namespace WmcSoft.Collections.Generic
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
        /// <typeparam name="T">Type of objects within the collection.</typeparam>
        /// <param name="source">The list to add items to.</param>
        /// <param name="value">The value</param>
        /// <returns>True if the value was added, false it was already in the collection.</returns>
        public static bool Ensure<T>(this ICollection<T> source, T value) {
            var collection = source as ICollection;
            if (collection != null && collection.IsSynchronized) {
                lock (collection.SyncRoot) {
                    if (!source.Contains(value)) {
                        source.Add(value);
                        return true;
                    }
                }
            } else {
                if (!source.Contains(value)) {
                    source.Add(value);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add a range of items to a collection. 
        /// </summary>
        /// <typeparam name="T">Type of objects within the collection.</typeparam>
        /// <typeparam name="TCollection">The type of collection</typeparam>
        /// <param name="source">The collection to add items to.</param>
        /// <param name="items">The items to add to the collection.</param>
        /// <returns>The collection.</returns>
        /// <remarks>Does nothing if items is null.</remarks>
        public static ICollection<T> AddRange<T>(this ICollection<T> source, IEnumerable<T> items) {
            if (source == null)
                throw new ArgumentNullException("self");

            if (items == null)
                return source;

            var list = source as List<T>;
            var collection = source as ICollection;
            if (list != null) {
                list.AddRange(items);
            } else if (collection != null && collection.IsSynchronized) {
                lock (collection.SyncRoot) {
                    foreach (var each in items) {
                        source.Add(each);
                    }
                }
            } else {
                foreach (var each in items) {
                    source.Add(each);
                }
            }

            return source;
        }

        public static ICollection<T> AddRange<T>(this ICollection<T> source, params T[] items) {
            return AddRange(source, items.AsEnumerable());
        }

        #endregion

        #region AsCollection

        public static ICollection<T> AsCollection<T>(this IReadOnlyCollection<T> readOnlyCollection) {
            var collection = readOnlyCollection as ICollection<T>;
            if (collection != null)
                return collection;
            return new ReadOnlyCollectionToCollectionAdapter<T>(readOnlyCollection);
        }

        #endregion

        #region AsReadOnly

        /// <summary>
        /// Shields a collection as a readonly collection
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="collection">The collection</param>
        /// <returns>A read only collection</returns>
        public static IReadOnlyCollection<T> AsReadOnly<T>(this ICollection<T> collection) {
            return new ReadOnlyCollectionFacade<T>(collection);
        }

        /// <summary>
        /// Shield a list as a readonly list
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="list">The sorted list</param>
        /// <returns>A read only list</returns>
        public static IReadOnlyList<T> AsReadOnly<T>(this IList<T> list) {
            return new ReadOnlyListFacade<T>(list);
        }

        /// <summary>
        /// Shield a collection as a readonly collection of a different element type
        /// </summary>
        /// <typeparam name="TInput">The source element type</typeparam>
        /// <typeparam name="TOutput">The target element type</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="converter">The converter</param>
        /// <returns>A read only list</returns>
        /// <remarks>Conversion are done "on demand" and are not cached.</remarks>
        public static IReadOnlyCollection<TOutput> AsReadOnly<TInput, TOutput>(this IReadOnlyCollection<TInput> collection, Converter<TInput, TOutput> converter) {
            return new ConvertingCollectionAdapter<TInput, TOutput>(collection, converter);
        }

        /// <summary>
        /// Shield a list as a readonly list of a different element type
        /// </summary>
        /// <typeparam name="TInput">The source element type</typeparam>
        /// <typeparam name="TOutput">The target element type</typeparam>
        /// <param name="list">The sorted list</param>
        /// <param name="converter">The converter</param>
        /// <returns>A read only list</returns>
        /// <remarks>Conversion are done "on demand" and are not cached.</remarks>
        public static IReadOnlyList<TOutput> AsReadOnly<TInput, TOutput>(this IReadOnlyList<TInput> list, Converter<TInput, TOutput> converter) {
            return new ConvertingReadOnlyListAdapter<TInput, TOutput>(list, converter);
        }

        #endregion

        #region BinarySearch methods

        private static int MulDiv(int number, int numerator, int denominator) {
            return (int)(((long)number * numerator) / denominator);
        }

        private static int GetMidpoint(int lo, int hi) {
            Debug.Assert(hi - lo >= 0);
            return lo + (hi - lo) / 2; // cannot overflow when (hi + lo) could
        }

        private static int UnguardedBinarySearch<T>(this IReadOnlyList<T> list, int lo, int hi, T value, IComparer<T> comparer) {
            while (lo <= hi) {
                int mid = GetMidpoint(lo, hi);
                int cmp = comparer.Compare(list[mid], value);
                if (cmp < 0) lo = mid + 1;
                else if (cmp > 0) hi = mid - 1;
                else return mid;
            }
            return ~lo;
        }

        private static int UnguardedBinaryRank<T>(this IReadOnlyList<T> list, int lo, int hi, T value, IComparer<T> comparer) {
            while (lo <= hi) {
                int mid = GetMidpoint(lo, hi);
                int cmp = comparer.Compare(list[mid], value);
                if (cmp < 0) lo = mid + 1;
                else if (cmp > 0) hi = mid - 1;
                else return mid;
            }
            return lo;
        }

        static void Guard<T>(IReadOnlyCollection<T> source, int index, int count) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if ((source.Count - index) < count) throw new ArgumentException("Invalid length");
        }

        static void Guard<T>(ICollection<T> source, int index, int count) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if ((source.Count - index) < count) throw new ArgumentException("Invalid length");
        }

        /// <summary>
        /// Searches a range of elements in the sorted <seealso cref="IReadOnlyList{T}"/> for an element using the specified function and returns the zero-based index of the element.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The zero-based index of item in the sorted <seealso cref="IReadOnlyList{T}"/>, if item is found; 
        /// otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item or, 
        /// if there is no larger element, the bitwise complement of Count.</returns>
        public static int BinarySearch<T>(this IReadOnlyList<T> source, int index, int count, Func<T, int> finder) {
            Guard(source, index, count);

            try {
                int lo = index;
                int hi = lo + count - 1;
                while (lo <= hi) {
                    int mid = GetMidpoint(lo, hi);
                    int cmp = finder(source[mid]);
                    if (cmp < 0) lo = mid + 1;
                    else if (cmp > 0) hi = mid - 1;
                    else return mid;
                }
                return ~lo;
            }
            catch (Exception e) {
                throw new InvalidOperationException("The finder threw an exception", e);
            }
        }

        /// <summary>
        /// Searches a range of elements in the sorted <seealso cref="IReadOnlyList{T}"/> for an element using the specified function and returns the zero-based index of the element.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The zero-based index of item in the sorted <seealso cref="IReadOnlyList{T}"/>, if item is found; 
        /// otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item or, 
        /// if there is no larger element, the bitwise complement of Count.</returns>
        public static int BinarySearch<T>(this IReadOnlyList<T> source, Func<T, int> finder) {
            return BinarySearch(source, 0, source.Count, finder);
        }

        /// <summary>
        /// Searches a range of elements in the sorted <seealso cref="IReadOnlyList{T}"/> for an element using the specified function and returns the element.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static T BinaryFind<T>(this IReadOnlyList<T> source, int index, int count, Func<T, int> finder, T defaultValue = default(T)) {
            var found = BinarySearch(source, index, count, finder);
            if (found >= 0)
                return source[found];
            return defaultValue;
        }

        /// <summary>
        /// Searches a range of elements in the sorted <seealso cref="IReadOnlyList{T}"/> for an element using the specified function and returns the element.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static T BinaryFind<T>(this IReadOnlyList<T> source, Func<T, int> finder, T defaultValue = default(T)) {
            return BinaryFind(source, 0, source.Count, finder, defaultValue);
        }

        /// <summary>
        /// Returns the last element for which the finder returns a negative value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static T LowerBound<T>(this IReadOnlyList<T> source, Func<T, int> finder, T defaultValue = default(T)) {
            return LowerBound(source, 0, source.Count, finder, defaultValue);
        }

        /// <summary>
        /// Returns the last element for which the finder returns a negative value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static T LowerBound<T>(this IReadOnlyList<T> source, int index, int count, Func<T, int> finder, T defaultValue = default(T)) {
            var found = BinarySearch(source, index, count, finder);
            if (found < 0) {
                var lo = ~found;
                if (lo > index)
                    return source[lo - 1];
                return defaultValue;
            }
            return source[found];
        }

        /// <summary>
        /// Returns the first element for which the finder returns a positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static T UpperBound<T>(this IReadOnlyList<T> source, Func<T, int> finder, T defaultValue = default(T)) {
            return UpperBound(source, 0, source.Count, finder, defaultValue);
        }

        /// <summary>
        /// Returns the first element for which the finder returns a positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static T UpperBound<T>(this IReadOnlyList<T> source, int index, int count, Func<T, int> finder, T defaultValue = default(T)) {
            var found = BinarySearch(source, index, count, finder);
            if (found < 0) {
                var lo = ~found;
                if (lo < index + count)
                    return source[lo];
                return defaultValue;
            }
            return source[found];
        }

        /// <summary>
        /// Returns the first element for which the finder returns a positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static Tuple<T, T> Bounds<T>(this IReadOnlyList<T> source, Func<T, int> finder, T defaultValue = default(T)) {
            return Bounds(source, 0, source.Count, finder, defaultValue);
        }

        /// <summary>
        /// Returns the first element for which the finder returns a positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static Tuple<T, T> Bounds<T>(this IReadOnlyList<T> source, int index, int count, Func<T, int> finder, T defaultValue = default(T)) {
            if (count == 0)
                return Tuple.Create(defaultValue, defaultValue);
            var found = BinarySearch(source, index, count, finder);
            if (found < 0) {
                var lo = ~found;
                if (lo <= index)
                    return Tuple.Create(defaultValue, source[lo]);
                if (lo < index + count)
                    return Tuple.Create(source[lo - 1], source[lo]);
                return Tuple.Create(source[lo - 1], defaultValue);
            }
            var equivalent = source[found];
            return Tuple.Create(equivalent, equivalent);
        }

        /// <summary>
        /// Computes the number of elements in the sorted <seealso cref="IReadOnlyList{T}"/> for which the <paramref name="finder"/> returns a negative value.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The number of elements for which the finder returns a negative value.</returns>
        public static int BinaryRank<T>(this IReadOnlyList<T> source, int index, int count, Func<T, int> finder) {
            Guard(source, index, count);

            try {
                int lo = index;
                int hi = lo + count - 1;
                while (lo <= hi) {
                    int mid = GetMidpoint(lo, hi);
                    int cmp = finder(source[mid]);
                    if (cmp < 0) lo = mid + 1;
                    else if (cmp > 0) hi = mid - 1;
                    else return mid;
                }
                return lo;
            }
            catch (Exception e) {
                throw new InvalidOperationException("The finder threw an exception", e);
            }
        }

        /// <summary>
        /// Computes the number of elements in the sorted <seealso cref="IReadOnlyList{T}"/> for which the <paramref name="finder"/> returns a negative value.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The number of elements for which the finder returns a negative value.</returns>
        public static int BinaryRank<T>(this IReadOnlyList<T> source, Func<T, int> finder) {
            return BinaryRank(source, 0, source.Count, finder);
        }

        public static int BinaryRank<T>(this IReadOnlyList<T> source, T value) {
            return BinaryRank(source, value, Comparer<T>.Default);
        }

        public static int BinaryRank<T>(this IReadOnlyList<T> source, T value, IComparer<T> comparer) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return UnguardedBinaryRank(source, 0, source.Count, value, comparer ?? Comparer<T>.Default);
        }

        #endregion

        #region ConvertAll

        /// <summary>
        /// Converts a collection of items of one type to a collection of another type.
        /// </summary>
        /// <typeparam name="TInput">The type of the elements of the source collection.</typeparam>
        /// <typeparam name="TOutput">The type of the elements of the target collection.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="converter">A Converter<TInput, TOutput> that converts each element from one type to another type.</param>
        /// <returns>A collection of the target type containing the converted elements from the source array.</returns>
        public static IList<TOutput> ConvertAll<TInput, TOutput>(this ICollection<TInput> source, Converter<TInput, TOutput> converter) {
            var list = new List<TOutput>(source.Count);
            foreach (var item in source) {
                list.Add(converter(item));
            }
            return list;
        }

        #endregion

        #region ElementsAt

        static int[] Iota(int n) {
            var array = new int[n];
            for (int i = 0; i < n; i++) {
                array[i] = i;
            }
            return array;
        }

        static IEnumerable<TSource> DoElementsAt<TSource>(IEnumerable<TSource> source, params int[] indices) {
            if (indices.Length == 0)
                return Enumerable.Empty<TSource>();

            var length = indices.Length;
            var sorted = Iota(length).Sort(new SourceComparer<int>(indices));
            var buffer = new TSource[length];
            var i = 0;
            var count = 0;
            var m = indices[sorted[i]];
            using (var enumerator = source.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    if (count++ == m) {
                        buffer[i] = enumerator.Current;
                        if (++i == length)
                            return buffer;
                        m = indices[sorted[i]];
                    }
                }
            }

            throw new InvalidOperationException();
        }

        static IEnumerable<TSource> DoElementsAt<TSource>(this IReadOnlyList<TSource> source, params int[] indices) {
            return indices.Select(i => source[i]);
        }

        public static IEnumerable<TSource> ElementsAt<TSource>(this IEnumerable<TSource> source, params int[] indices) {
            var readable = source as IReadOnlyList<TSource>;
            if (readable != null) return DoElementsAt(readable, indices);
            var list = source as IList<TSource>;
            if (list != null) return ElementsAt(list, indices);
            return DoElementsAt(source, indices);
        }

        public static IEnumerable<TSource> ElementsAt<TSource>(this IList<TSource> source, params int[] indices) {
            return indices.Select(i => source[i]);
        }

        static IEnumerable<TSource> DoElementsAtOrDefault<TSource>(IEnumerable<TSource> source, params int[] indices) {
            if (indices.Length == 0)
                return Enumerable.Empty<TSource>();

            var length = indices.Length;
            var sorted = Iota(length).Sort(new SourceComparer<int>(indices));
            var buffer = new TSource[length];
            var i = 0;
            var count = 0;
            var m = indices[sorted[i]];
            using (var enumerator = source.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    if (count++ == m) {
                        buffer[i] = enumerator.Current;
                        if (++i == length)
                            break;
                        m = indices[sorted[i]];
                    }
                }
            }
            return buffer;
        }

        static IEnumerable<TSource> DoElementsAtOrDefault<TSource>(IReadOnlyList<TSource> source, params int[] indices) {
            for (int i = 0; i < indices.Length; i++) {
                var indice = indices[i];
                if (0 < indice || indice >= source.Count)
                    yield return default(TSource);
                else
                    yield return source[indice];
            }
        }

        public static IEnumerable<TSource> ElementsAtOrDefault<TSource>(this IEnumerable<TSource> source, params int[] indices) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var readable = source as IReadOnlyList<TSource>;
            if (readable != null) return DoElementsAtOrDefault(readable, indices);
            var list = source as IList<TSource>;
            if (list != null) return DoElementsAtOrDefault(list, indices);
            return DoElementsAtOrDefault(source, indices);
        }

        public static IEnumerable<TSource> ElementsAtOrDefault<TSource>(this IList<TSource> source, params int[] indices) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            for (int i = 0; i < indices.Length; i++) {
                var indice = indices[i];
                if (0 < indice || indice >= source.Count)
                    yield return default(TSource);
                else
                    yield return source[indice];
            }
        }

        static IEnumerable<TResult> DoElementsAt<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector, params int[] indices) {
            if (indices.Length == 0)
                return Enumerable.Empty<TResult>();

            var length = indices.Length;
            var sorted = Iota(length).Sort(new SourceComparer<int>(indices));
            var buffer = new TResult[length];
            var i = 0;
            var count = 0;
            var m = indices[sorted[i]];
            using (var enumerator = source.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    if (count++ == m) {
                        buffer[i] = selector(enumerator.Current);
                        if (++i == length)
                            return buffer;
                        m = indices[sorted[i]];
                    }
                }
            }

            throw new InvalidOperationException();
        }

        static IEnumerable<TResult> DoElementsAt<TSource, TResult>(this IReadOnlyList<TSource> source, Func<TSource, TResult> selector, params int[] indices) {
            return indices.Select(i => selector(source[i]));
        }

        public static IEnumerable<TResult> ElementsAt<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, params int[] indices) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var readable = source as IReadOnlyList<TSource>;
            if (readable != null) return DoElementsAt(readable, selector, indices);
            var list = source as IList<TSource>;
            if (list != null) return ElementsAt(list, selector, indices);
            return DoElementsAt(source, selector, indices);
        }

        public static IEnumerable<TResult> ElementsAt<TSource, TResult>(this IList<TSource> source, Func<TSource, TResult> selector, params int[] indices) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return indices.Select(i => selector(source[i]));
        }

        static IEnumerable<TResult> DoElementsAtOrDefault<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector, params int[] indices) {
            if (indices.Length == 0)
                return Enumerable.Empty<TResult>();

            var length = indices.Length;
            var sorted = Iota(length).Sort(new SourceComparer<int>(indices));
            var buffer = new TResult[length];
            var i = 0;
            var count = 0;
            var m = indices[sorted[i]];
            using (var enumerator = source.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    if (count++ == m) {
                        buffer[i] = selector(enumerator.Current);
                        if (++i == length)
                            break;
                        m = indices[sorted[i]];
                    }
                }
            }
            return buffer;
        }

        static IEnumerable<TResult> DoElementsAtOrDefault<TSource, TResult>(IReadOnlyList<TSource> source, Func<TSource, TResult> selector, params int[] indices) {
            for (int i = 0; i < indices.Length; i++) {
                var indice = indices[i];
                if (0 < indice || indice >= source.Count)
                    yield return default(TResult);
                else
                    yield return selector(source[indice]);
            }
        }

        public static IEnumerable<TResult> ElementsAtOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, params int[] indices) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var readable = source as IReadOnlyList<TSource>;
            if (readable != null) return DoElementsAtOrDefault(readable, selector, indices);
            var list = source as IList<TSource>;
            if (list != null) return ElementsAtOrDefault(list, selector, indices);
            return DoElementsAtOrDefault(source, selector, indices);
        }

        public static IEnumerable<TResult> ElementsAtOrDefault<TSource, TResult>(this IList<TSource> source, Func<TSource, TResult> selector, params int[] indices) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            for (int i = 0; i < indices.Length; i++) {
                var indice = indices[i];
                if (0 < indice || indice >= source.Count)
                    yield return default(TResult);
                else
                    yield return selector(source[indice]);
            }
        }

        #endregion

        #region InterpolatedSearch

        /// <summary>
        /// Searches a range of elements in the sorted <seealso cref="IReadOnlyList{T}"/> for an element using the specified function and returns the zero-based index of the element.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The zero-based index of item in the sorted <seealso cref="IReadOnlyList{T}"/>, if item is found; 
        /// otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item or, 
        /// if there is no larger element, the bitwise complement of Count.</returns>
        public static int InterpolatedSearch<T>(this IReadOnlyList<T> source, int index, int count, T value, IOrdinal<T> ordinal) {
            Guard(source, index, count);

            try {
                int lo = index;
                int hi = lo + count - 1;
                while (lo <= hi) {
                    int D = ordinal.Compare(source[lo], source[hi]);
                    int d = ordinal.Compare(source[lo], value);
                    if (D == 0) {
                        if (d < 0) return ~lo;
                        else if (d > 0) return ~hi;
                        return lo;
                    }
                    int i = lo + MulDiv(d, hi - lo, D);
                    int cmp = ordinal.Compare(source[i], value);
                    switch (cmp) {
                    case 0:
                        return i;
                    case -1:
                        return UnguardedBinarySearch(source, i + 1, hi, value, ordinal);
                    case 1:
                        return UnguardedBinarySearch(source, lo, i - 1, value, ordinal);
                    default:
                        if (cmp < 0) lo = i + 1;
                        else if (cmp > 0) hi = i - 1;
                        else return i;
                        break;
                    }
                }
                return ~lo;
            }
            catch (Exception e) {
                throw new InvalidOperationException("The ordinal threw an exception", e);
            }
        }

        public static int InterpolatedSearch<T>(this IReadOnlyList<T> source, T value, IOrdinal<T> ordinal) {
            return InterpolatedSearch(source, 0, source.Count, value, ordinal);
        }

        #endregion

        #region MinElement / MaxElement / MinMaxElement

        /// <summary>
        /// Compute the index of the min element in the source list.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of source</typeparam>
        /// <param name="source">The source element</param>
        /// <returns>The zero-based index of the min element or -1 if the range is empty.</returns>
        public static int MinElement<TSource>(this IList<TSource> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Comparison<TSource> comparison = Comparer<TSource>.Default.Compare;
            return MinElement(source, 0, source.Count, comparison);
        }

        /// <summary>
        /// Compute the index of the min element in the source list.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of source</typeparam>
        /// <param name="source">The source element</param>
        /// <param name="comparer">The IComparer<T> implementation to use when comparing elements, or null to use the default comparer Comparer<T>.Default.</param>
        /// <returns>The zero-based index of the min element or -1 if the range is empty.</returns>
        public static int MinElement<TSource>(this IList<TSource> source, IComparer<TSource> comparer) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Comparison<TSource> comparison = (comparer ?? Comparer<TSource>.Default).Compare;
            return MinElement(source, 0, source.Count, comparison);
        }

        /// <summary>
        /// Compute the index of the min element in the source list.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of source</typeparam>
        /// <param name="source">The source element</param>
        /// <param name="index">The zero-based starting index of the range to get the min element from.</param>
        /// <param name="count">The length of the range to get the min element from.</param>
        /// <param name="comparer">The IComparer<T> implementation to use when comparing elements, or null to use the default comparer Comparer<T>.Default.</param>
        /// <returns>The zero-based index of the min element or -1 if the range is empty.</returns>
        public static int MinElement<TSource>(this IList<TSource> source, int index, int count, IComparer<TSource> comparer) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Comparison<TSource> comparison = (comparer ?? Comparer<TSource>.Default).Compare;
            return MinElement(source, index, count, comparison);
        }

        public static int MinElement<TSource>(this IList<TSource> source, Comparison<TSource> comparison) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return MinElement(source, 0, source.Count, comparison);
        }

        public static int MinElement<TSource>(this IList<TSource> source, int index, int length, Comparison<TSource> comparison) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0)
                return -1;
            var min = source[index];
            var p = index;
            length += index;
            for (int i = index + 1; i < length; i++) {
                if (comparison(source[i], min) < 0) {
                    min = source[p = i];
                }
            }
            return p;
        }

        public static int MaxElement<TSource>(this IList<TSource> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Comparison<TSource> comparison = Comparer<TSource>.Default.Compare;
            return MaxElement(source, 0, source.Count, comparison);
        }

        public static int MaxElement<TSource>(this IList<TSource> source, IComparer<TSource> comparer) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Comparison<TSource> comparison = (comparer ?? Comparer<TSource>.Default).Compare;
            return MaxElement(source, 0, source.Count, comparison);
        }

        public static int MaxElement<TSource>(this IList<TSource> source, int index, int length, IComparer<TSource> comparer) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Comparison<TSource> comparison = (comparer ?? Comparer<TSource>.Default).Compare;
            return MaxElement(source, index, length, comparison);
        }

        public static int MaxElement<TSource>(this IList<TSource> source, Comparison<TSource> comparison) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return MaxElement(source, 0, source.Count, comparison);
        }

        public static int MaxElement<TSource>(this IList<TSource> source, int index, int length, Comparison<TSource> comparison) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0)
                return -1;
            var max = source[index];
            var p = index;
            length += index;
            for (int i = index + 1; i < length; i++) {
                if (comparison(source[i], max) >= 0) {
                    max = source[p = i];
                }
            }
            return p;
        }

        public static Tuple<int, int> MinMaxElement<TSource>(this IList<TSource> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Comparison<TSource> comparison = Comparer<TSource>.Default.Compare;
            return MinMaxElement(source, 0, source.Count, comparison);
        }

        public static Tuple<int, int> MinMaxElement<TSource>(this IList<TSource> source, IComparer<TSource> comparer) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Comparison<TSource> comparison = (comparer ?? Comparer<TSource>.Default).Compare;
            return MinMaxElement(source, 0, source.Count, comparison);
        }

        public static Tuple<int, int> MinMaxElement<TSource>(this IList<TSource> source, int index, int length, IComparer<TSource> comparer) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Comparison<TSource> comparison = (comparer ?? Comparer<TSource>.Default).Compare;
            return MinMaxElement(source, index, length, comparison);
        }

        public static Tuple<int, int> MinMaxElement<TSource>(this IList<TSource> source, Comparison<TSource> comparison) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return MinMaxElement(source, 0, source.Count, comparison);
        }

        public static Tuple<int, int> MinMaxElement<TSource>(this IList<TSource> source, int index, int length, Comparison<TSource> comparison) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0)
                return Tuple.Create(-1, -1);
            var min = source[index];
            var max = source[index];
            var p = index;
            var q = index;
            for (int i = index + 1; i < length; i++) {
                if (comparison(source[i], min) < 0) {
                    min = source[p = i];
                } else if (comparison(source[i], max) >= 0) {
                    max = source[q = i];
                }
            }
            return Tuple.Create(p, q);
        }

        #endregion

        #region Partition

        static int UnguardedFindPartitionPoint<T>(IList<T> list, int start, int length, Predicate<T> predicate) {
            int end = start + length - 1;
            while (start < end && !predicate(list[start]))
                start++;
            while (start < end && predicate(list[end]))
                end--;
            if (start == end)
                return start;
            return -1;
        }

        /// <summary>
        /// Returns the partition point of the list if the list is partitionned.
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="source">The list</param>
        /// <param name="predicate">Thre predicate</param>
        /// <returns>The index of the partition point, -1 if the list is not partitionned.</returns>
        public static int FindPartitionPoint<T>(this IList<T> source, Predicate<T> predicate) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException("predicate");

            return UnguardedFindPartitionPoint(source, 0, source.Count, predicate);
        }

        /// <summary>
        /// Returns the partition point of the list if the list is partitionned.
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="index">The start index of the sequence</param>
        /// <param name="count">The length of the sequence</param>
        /// <param name="source">The list</param>
        /// <param name="predicate">Thre predicate</param>
        /// <returns>The index of the partition point, -1 if the list is not partitionned.</returns>
        public static int FindPartitionPoint<T>(this IList<T> source, int index, int count, Predicate<T> predicate) {
            Guard(source, index, count);
            if (predicate == null) throw new ArgumentNullException("predicate");

            return UnguardedFindPartitionPoint(source, index, count, predicate);
        }

        /// <summary>
        /// Returns the partition point of the list if the list is partitionned.
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="source">The list</param>
        /// <param name="predicate">Thre predicate</param>
        /// <returns>The index of the partition point, -1 if the list is not partitionned.</returns>
        public static bool IsPartitioned<T>(this IList<T> source, Predicate<T> predicate) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException("predicate");

            return UnguardedFindPartitionPoint(source, 0, source.Count, predicate) >= 0;
        }

        /// <summary>
        /// Returns the partition point of the list if the list is partitionned.
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="index">The start index of the sequence</param>
        /// <param name="count">The length of the sequence</param>
        /// <param name="source">The list</param>
        /// <param name="predicate">Thre predicate</param>
        /// <returns>The index of the partition point, -1 if the list is not partitionned.</returns>
        public static bool IsPartitioned<T>(this IList<T> source, int index, int count, Predicate<T> predicate) {
            return FindPartitionPoint(source, index, count, predicate) >= 0;
        }

        static int UnguardedPartition<T>(IList<T> list, int start, int length, Predicate<T> predicate) {
            int end = start + length - 1;
            while (true) {
                while (start < end && !predicate(list[start]))
                    start++;
                while (start < end && predicate(list[end]))
                    end--;
                if (start > end)
                    return start;
                list.SwapItems(start++, end--);
            }
        }

        /// <summary>
        /// Implements a partition of the list 
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="source">The list</param>
        /// <param name="predicate">Thre predicate</param>
        /// <returns>The partition point</returns>
        public static int Partition<T>(this IList<T> source, Predicate<T> predicate) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException("predicate");

            return UnguardedPartition(source, 0, source.Count, predicate);
        }

        /// <summary>
        /// Implements a partition of the list 
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="source">The list</param>
        /// <param name="index">The start index of the sequence</param>
        /// <param name="count">The length of the sequence</param>
        /// <param name="predicate">Thre predicate</param>
        /// <returns>The partition point</returns>
        public static int Partition<T>(this IList<T> source, int index, int count, Predicate<T> predicate) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException("predicate");

            return UnguardedPartition(source, index, count, predicate);
        }

        private static int UnguardedStablePartition<T>(this IList<T> list, T[] buffer, int start, int length, Predicate<T> predicate) {
            var p = start;
            var first = 0;
            var last = length;
            while (first < last) {
                var t = list[p++];
                if (!predicate(t))
                    buffer[first++] = t;
                else
                    buffer[--last] = t;
            }
            buffer.CopyTo(list, start, first);
            buffer.CopyBackwardsTo(last, list, start + first, length - last);
            return start + first;
        }

        private static int UnguardedStablePartition<T>(this IList<T> list, int start, int length, Predicate<T> predicate) {
            int offset = start;
            int end = start + length - 1;
            while (start < end && !predicate(list[start]))
                start++;
            while (start < end && predicate(list[end]))
                end--;

            if (start < end) {
                length = end - start + 1;
                var buffer = new T[length];
                offset += UnguardedStablePartition(list, buffer, start, length, predicate);
            }
            return offset;
        }

        /// <summary>
        /// Implements a stable partition of the list 
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="source">The list</param>
        /// <param name="predicate">Thre predicate</param>
        /// <returns>The partition point</returns>
        public static int StablePartition<T>(this IList<T> source, Predicate<T> predicate) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException("predicate");

            return UnguardedStablePartition(source, 0, source.Count, predicate);
        }

        /// <summary>
        /// Implements a stable partition of the list 
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="source">The list</param>
        /// <param name="index">The start index of the sequence</param>
        /// <param name="count">The length of the sequence</param>
        /// <param name="predicate">Thre predicate</param>
        /// <returns>The partition point</returns>
        public static int StablePartition<T>(this IList<T> source, int index, int count, Predicate<T> predicate) {
            Guard(source, index, count);
            if (predicate == null) throw new ArgumentNullException("predicate");

            return UnguardedStablePartition(source, index, count, predicate);
        }

        #endregion

        #region Pop

        /// <summary>
        /// Removes the last element from the list and returns it.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="source">The list.</param>
        /// <exception cref="ArgumentNullException">The list is null.</exception>
        /// <exception cref="InvalidOperationException">The list is empty.</exception>
        /// <returns>The last element from the list.</returns>
        public static T Pop<T>(this IList<T> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.Count == 0) throw new InvalidOperationException();

            var last = source.Count - 1;
            var t = source[last];
            source.RemoveAt(last);
            return t;
        }

        /// <summary>
        /// Removes the element at the specified <param name="index"/> from the list and returns it.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="source">The list.</param>
        /// <param name="index">The index of the element to remove.</param>
        /// <exception cref="ArgumentNullException">The list is null.</exception>
        /// <exception cref="InvalidOperationException">The list is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><param name="index"/> is not a valid index in the <paramref name="source"/>.</exception>
        /// <returns>The last element from the list.</returns>
        public static T Pop<T>(this IList<T> source, int index) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.Count == 0) throw new InvalidOperationException();

            var t = source[index];
            source.RemoveAt(index);
            return t;
        }

        #endregion

        #region RemoveIf methods

        public static int RemoveIf<T>(this ICollection<T> source, Func<T, bool> predicate) {
            var list = source as IList<T>;
            if (list != null)
                return RemoveIf(list, predicate);

            int count = 0;
            var recycler = new List<T>();
            recycler.AddRange(source.Where(predicate));
            foreach (var item in recycler) {
                if (source.Remove(item))
                    count++;
            }
            return count;
        }

        public static int RemoveIf<T>(this IList<T> source, Func<T, bool> predicate) {
            var list = source as List<T>;
            if (list != null)
                return list.RemoveAll(x => predicate(x));

            int count = 0;
            for (int i = source.Count - 1; i >= 0; i--) {
                var item = source[i];
                if (predicate(item)) {
                    source.RemoveAt(i);
                    count++;
                }
            }
            return count;
        }

        public static int RemoveIf<T>(this IList<T> source, Func<T, int, bool> predicate) {
            int count = 0;
            for (int i = 0; i < source.Count;) {
                var item = source[i];
                if (predicate(item, i)) {
                    source.RemoveAt(i);
                    count++;
                } else {
                    i++;
                }
            }
            return count;
        }

        #endregion

        #region RemoveRange methods

        /// <summary>
        /// Remove a range of items from a collection. 
        /// </summary>
        /// <typeparam name="T">Type of objects within the collection.</typeparam>
        /// <param name="source">The collection to remove items from.</param>
        /// <param name="items">The items to remove from the collection.</param>
        /// <returns>The count of items removed from the collection.</returns>
        /// <remarks>Does nothing if items is null.</remarks>
        public static int RemoveRange<T>(this ICollection<T> source, IEnumerable<T> items) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (items == null)
                return 0;

            ICollection collection = source as ICollection;
            int count = 0;

            if (collection != null && collection.IsSynchronized) {
                lock (collection.SyncRoot) {
                    foreach (var each in items) {
                        if (source.Remove(each))
                            count++;
                    }
                }
            } else {
                foreach (var each in items) {
                    if (source.Remove(each))
                        count++;
                }
            }

            return count;
        }

        #endregion

        #region Reserve

        public static List<T> Reserve<T>(this List<T> source, int extraSpace) {
            var required = source.Count + extraSpace;
            if (source.Capacity < required)
                source.Capacity = required;
            return source;
        }

        #endregion

        #region Slice

        class SliceList<T> : IList<T>
        {
            readonly IList<T> _underlying;
            readonly int _startIndex;
            readonly int _length;

            public SliceList(IList<T> underlying, int startIndex, int length) {
                _underlying = underlying;
                _startIndex = startIndex;
                _length = length;
            }

            public T this[int index] {
                get { return _underlying[_startIndex + index]; }
                set { _underlying[_startIndex + index] = value; }
            }

            public int Count {
                get { return _length; }
            }

            public bool IsReadOnly {
                get { return true; }
            }

            public void Add(T item) {
                throw new NotSupportedException();
            }

            public void Clear() {
                throw new NotSupportedException();
            }

            public bool Contains(T item) {
                return IndexOf(item) >= 0;
            }

            public void CopyTo(T[] array, int arrayIndex) {
                if (array == null) throw new ArgumentNullException("array");
                if (arrayIndex < 0) throw new ArgumentOutOfRangeException("arrayIndex");
                if ((array.Length - arrayIndex) > _length) throw new ArgumentException();

                var count = _length;
                var s = _startIndex;
                var t = arrayIndex;
                while (count-- > 0)
                    array[t++] = _underlying[s++];
            }

            public IEnumerator<T> GetEnumerator() {
                return _underlying.Skip(_startIndex).Take(_length).GetEnumerator();
            }

            public int IndexOf(T item) {
                var endIndex = _startIndex + _length;
                for (int i = _startIndex; i < endIndex; i++) {
                    if (Equals(_underlying[i], item))
                        return i - _startIndex;
                }
                return -1;
            }

            public void Insert(int index, T item) {
                throw new NotSupportedException();
            }

            public bool Remove(T item) {
                throw new NotSupportedException();
            }

            public void RemoveAt(int index) {
                throw new NotSupportedException();
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }

        public static IList<T> Slice<T>(this IList<T> list, int startIndex, int length) {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (startIndex < 0 || startIndex > list.Count) throw new ArgumentOutOfRangeException("startIndex");
            if (length < 0 || startIndex > (list.Count - length)) throw new ArgumentOutOfRangeException("length");

            return new SliceList<T>(list, startIndex, length);
        }

        #endregion

        #region Stack & Queue

        public static void Push<T>(this Stack<T> stack, IEnumerable<T> items) {
            foreach (var item in items)
                stack.Push(item);
        }

        public static void Enqueue<T>(this Queue<T> queue, IEnumerable<T> items) {
            foreach (var item in items)
                queue.Enqueue(item);
        }

        #endregion

        #region Reverse

        public static void Reverse<T>(this IList<T> list, int startIndex, int length) {
            var start = startIndex;
            var end = startIndex + length - 1;
            while (start < end) {
                SwapItems(list, start, end);
            }
        }

        public static void Reverse<T>(this IList<T> list) {
            Reverse(list, 0, list.Count);
        }

        #endregion

        #region Shuffle methods

        static void UnguardedShuffle<T>(IList<T> list, int index, int count, Random random) {
            int end = index + count;
            int j;
            for (int i = index; i < end; i++) {
                j = random.Next(i, end);
                SwapItems(list, i, j);
            }
        }

        /// <summary>
        /// Suffles in place items of the list.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="source">The list.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when list is null</exception>
        /// <exception cref="System.ArgumentException">Thrown when list is read only</exception>
        /// <remarks>Implements Fisher-Yates suffle, https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle </remarks>
        public static void Shuffle<T>(this IList<T> source) {
            Shuffle(source, new Random());
        }

        /// <summary>
        /// Suffles in place items of the list.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="source">The list.</param>
        /// <param name="random">The random object to use to perfom the suffle.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when list or random is null</exception>
        /// <exception cref="System.ArgumentException">Thrown when list is read only</exception>
        /// <remarks>Implements Fisher-Yates suffle, https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle </remarks>
        public static void Shuffle<T>(this IList<T> source, Random random) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (random == null) throw new ArgumentNullException(nameof(random));

            UnguardedShuffle(source, 0, source.Count, random);
        }

        /// <summary>
        /// Splits the items in half (a bottom half and a top half) and 
        /// then interweaves each half of the deck such that every-other item came 
        /// from the same half of the list.
        /// The first item will move to second place.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="source">The list.</param>
        /// <remarks>See https://en.wikipedia.org/wiki/Faro_shuffle and https://en.wikipedia.org/wiki/In_shuffle for more information.</remarks>
        public static void PerfectInShuffle<T>(this IList<T> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if ((source.Count % 2) == 1) throw new ArgumentException(nameof(source));

            UnguardedPerfectInShuffle(source, 0, source.Count);
        }

        public static void PerfectInShuffle<T>(this IList<T> source, int index, int count) {
            Guard(source, index, count);
            if ((count % 2) == 1) throw new ArgumentException(nameof(count));

            UnguardedPerfectInShuffle(source, index, count);
        }

        static void UnguardedPerfectInShuffle<T>(IList<T> list, int index, int count) {
            var aux = new T[count];
            int m = index + count / 2;
            int r = index + count;
            for (int i = index, j = 0; i < r; i += 2, j++) {
                aux[i] = list[m + j];
                aux[i + 1] = list[index + j];
            }
            aux.CopyTo(list, index, count);
        }

        public static void PerfectInUnshuffle<T>(this IList<T> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if ((source.Count % 2) == 1) throw new ArgumentException(nameof(source));

            UnguardedPerfectInUnshuffle(source, 0, source.Count);
        }

        public static void PerfectInUnshuffle<T>(this IList<T> source, int index, int count) {
            Guard(source, index, count);
            if ((count % 2) == 1) throw new ArgumentException(nameof(count));

            UnguardedPerfectInUnshuffle(source, index, count);
        }

        static void UnguardedPerfectInUnshuffle<T>(IList<T> list, int index, int count) {
            var aux = new T[count];
            int m = index + count / 2;
            int r = index + count;
            for (int i = index, j = 0; i < r; i += 2, j++) {
                aux[index + j] = list[i + 1];
                aux[m + j] = list[i];
            }
            aux.CopyTo(list, index, count);
        }

        /// <summary>
        /// Splits the items in half (a bottom half and a top half) and 
        /// then interweaves each half such that every-other item came 
        /// from the same half of the list.
        /// The first item remain first.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="source">The list.</param>
        /// <remarks>See https://en.wikipedia.org/wiki/Faro_shuffle and https://en.wikipedia.org/wiki/Out_shuffle for more information.</remarks>
        public static void PerfectOutShuffle<T>(this IList<T> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if ((source.Count % 2) == 1) throw new ArgumentException(nameof(source));

            UnguardedPerfectOutShuffle(source, 0, source.Count);
        }

        public static void PerfectOutShuffle<T>(this IList<T> source, int index, int count) {
            Guard(source, index, count);
            if ((count % 2) == 1) throw new ArgumentException(nameof(count));

            UnguardedPerfectOutShuffle(source, index, count);
        }

        static void UnguardedPerfectOutShuffle<T>(IList<T> list, int index, int count) {
            var aux = new T[count];
            int m = index + count / 2;
            int r = index + count;
            for (int i = index, j = 0; i < r; i += 2, j++) {
                aux[i] = list[index + j];
                aux[i + 1] = list[m + j];
            }
            aux.CopyTo(list, index, count);
        }

        public static void PerfectOutUnshuffle<T>(this IList<T> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if ((source.Count % 2) == 1) throw new ArgumentException(nameof(source));

            UnguardedPerfectOutUnshuffle(source, 0, source.Count);
        }

        public static void PerfectOutUnshuffle<T>(this IList<T> source, int index, int count) {
            Guard(source, index, count);
            if ((count % 2) == 1) throw new ArgumentException(nameof(count));

            UnguardedPerfectOutUnshuffle(source, index, count);
        }

        static void UnguardedPerfectOutUnshuffle<T>(IList<T> list, int index, int count) {
            var aux = new T[count];
            int m = index + count / 2;
            int r = index + count;
            for (int i = index, j = 0; i < r; i += 2, j++) {
                aux[index + j] = list[i];
                aux[m + j] = list[i + 1];
            }
            aux.CopyTo(list, index, count);
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
        /// <remarks>This function does not guard against null list or out of bound indices for performance reasons.</remarks>
        public static IList<T> SwapItems<T>(this IList<T> source, int i, int j) {
            T temp = source[i];
            source[i] = source[j];
            source[j] = temp;
            return source;
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
            if (convert == null) throw new ArgumentNullException(nameof(convert));
            if (collection == null)
                return null;

            var output = new TOutput[collection.Count];
            var i = 0;
            using (var enumerator = collection.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    output[i++] = convert(enumerator.Current);
                }
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
            if (convert == null) throw new ArgumentNullException(nameof(convert));
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

        #region Toggle

        /// <summary>
        /// Removes an item if found in the collection or adds it if missing.
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="item">The item</param>
        /// <returns>false if the item was removed, true if it was added.</returns>
        public static bool Toggle<T>(this ICollection<T> collection, T item) {
            if (collection.Remove(item))
                return false;
            collection.Add(item);
            return true;
        }

        #endregion
    }
}