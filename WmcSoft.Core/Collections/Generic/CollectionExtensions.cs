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
using System.Diagnostics;
using System.Linq;

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
        /// <param name="self">The list to add items to.</param>
        /// <param name="value">The value</param>
        /// <returns>True if the value was added, false it was already in the collection.</returns>
        public static bool Ensure<T>(this ICollection<T> self, T value) {
            var collection = self as ICollection;
            if (collection != null && collection.IsSynchronized) {
                lock (collection.SyncRoot) {
                    if (!self.Contains(value)) {
                        self.Add(value);
                        return true;
                    }
                }
            } else {
                if (!self.Contains(value)) {
                    self.Add(value);
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
        /// <param name="self">The collection to add items to.</param>
        /// <param name="items">The items to add to the collection.</param>
        /// <returns>The collection.</returns>
        /// <remarks>Does nothing if items is null.</remarks>
        public static ICollection<T> AddRange<T>(this ICollection<T> self, IEnumerable<T> items) {
            if (self == null)
                throw new ArgumentNullException("self");

            if (items == null)
                return self;

            var list = self as List<T>;
            var collection = self as ICollection;
            if (list != null) {
                list.AddRange(items);
            } else if (collection != null && collection.IsSynchronized) {
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

        public static ICollection<T> AddRange<T>(this ICollection<T> self, params T[] items) {
            return AddRange(self, items.AsEnumerable());
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

        #region AsEnumerable

        /// <summary>
        /// Lift the source element as en enumerable.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of source</typeparam>
        /// <param name="source">the source element</param>
        /// <returns>An enumerable on the source</returns>
        public static IEnumerable<TSource> AsEnumerable<TSource>(TSource source) {
            yield return source;
        }

        /// <summary>
        /// Lift the source element as en enumerable.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of source</typeparam>
        /// <param name="source">the source element</param>
        /// <returns>An enumerable on the source, or an empty enumerable when the source is null</returns>
        public static IEnumerable<TSource> AsEnumerable<TSource>(TSource? source) where TSource : struct {
            if (source.HasValue)
                yield return source.GetValueOrDefault();
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
            return new ConvertingListAdapter<TInput, TOutput>(list, converter);
        }

        #endregion

        #region BinarySearch methods

        public static int MulDiv(int number, int numerator, int denominator) {
            return (int)(((long)number * numerator) / denominator);
        }

        private static int GetMidpoint(int lo, int hi) {
            Debug.Assert(hi - lo >= 0);
            return lo + (hi - lo) / 2; // cannot overflow when (hi + lo) could
        }

        private static int DoBinarySearch<T>(this IReadOnlyList<T> list, int lo, int hi, T value, IComparer<T> comparer) {
            while (lo <= hi) {
                int i = GetMidpoint(lo, hi);
                int c = comparer.Compare(list[i], value);
                if (c == 0)
                    return i;
                if (c < 0) {
                    lo = i + 1;
                } else {
                    hi = i - 1;
                }
            }
            return ~lo;
        }

        /// <summary>
        /// Searches a range of elements in the sorted <seealso cref="IReadOnlyList{T}"/> for an element using the specified function and returns the zero-based index of the element.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="list">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The zero-based index of item in the sorted <seealso cref="IReadOnlyList{T}"/>, if item is found; 
        /// otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item or, 
        /// if there is no larger element, the bitwise complement of Count.</returns>
        public static int BinarySearch<T>(this IReadOnlyList<T> list, int index, int count, Func<T, int> finder) {
            if (list == null)
                throw new ArgumentNullException("list");
            if (index < 0)
                throw new ArgumentOutOfRangeException("index");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");
            if ((list.Count - index) < count)
                throw new ArgumentException("Invalid length");

            try {
                int lo = index;
                int hi = lo + count - 1;
                while (lo <= hi) {
                    int i = GetMidpoint(lo, hi);
                    int c = finder(list[i]);
                    if (c == 0)
                        return i;
                    if (c < 0) {
                        lo = i + 1;
                    } else {
                        hi = i - 1;
                    }
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
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="list">The sorted list</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The zero-based index of item in the sorted <seealso cref="IReadOnlyList{T}"/>, if item is found; 
        /// otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item or, 
        /// if there is no larger element, the bitwise complement of Count.</returns>
        public static int BinarySearch<T>(this IReadOnlyList<T> list, Func<T, int> finder) {
            return BinarySearch(list, 0, list.Count, finder);
        }

        /// <summary>
        /// Searches a range of elements in the sorted <seealso cref="IReadOnlyList{T}"/> for an element using the specified function and returns the element.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="list">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static T BinaryFind<T>(this IReadOnlyList<T> list, int index, int count, Func<T, int> finder, T defaultValue = default(T)) {
            var found = BinarySearch(list, index, count, finder);
            if (found >= 0)
                return list[found];
            return defaultValue;
        }

        /// <summary>
        /// Searches a range of elements in the sorted <seealso cref="IReadOnlyList{T}"/> for an element using the specified function and returns the element.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="list">The sorted list</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static T BinaryFind<T>(this IReadOnlyList<T> list, Func<T, int> finder, T defaultValue = default(T)) {
            return BinaryFind(list, 0, list.Count, finder, defaultValue);
        }

        /// <summary>
        /// Returns the last element for which the finder returns a negative value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="list">The sorted list</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static T LowerBound<T>(this IReadOnlyList<T> list, Func<T, int> finder, T defaultValue = default(T)) {
            return LowerBound(list, 0, list.Count, finder, defaultValue);
        }

        /// <summary>
        /// Returns the last element for which the finder returns a negative value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="list">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static T LowerBound<T>(this IReadOnlyList<T> list, int index, int count, Func<T, int> finder, T defaultValue = default(T)) {
            var found = BinarySearch(list, index, count, finder);
            if (found < 0) {
                var lo = ~found;
                if (lo > index)
                    return list[lo - 1];
                return defaultValue;
            }
            return list[found];
        }

        /// <summary>
        /// Returns the first element for which the finder returns a positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="list">The sorted list</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static T UpperBound<T>(this IReadOnlyList<T> list, Func<T, int> finder, T defaultValue = default(T)) {
            return UpperBound(list, 0, list.Count, finder, defaultValue);
        }

        /// <summary>
        /// Returns the first element for which the finder returns a positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="list">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static T UpperBound<T>(this IReadOnlyList<T> list, int index, int count, Func<T, int> finder, T defaultValue = default(T)) {
            var found = BinarySearch(list, index, count, finder);
            if (found < 0) {
                var lo = ~found;
                if (lo < index + count)
                    return list[lo];
                return defaultValue;
            }
            return list[found];
        }

        /// <summary>
        /// Returns the first element for which the finder returns a positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="list">The sorted list</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static Tuple<T, T> Bounds<T>(this IReadOnlyList<T> list, Func<T, int> finder, T defaultValue = default(T)) {
            return Bounds(list, 0, list.Count, finder, defaultValue);
        }

        /// <summary>
        /// Returns the first element for which the finder returns a positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="list">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static Tuple<T, T> Bounds<T>(this IReadOnlyList<T> list, int index, int count, Func<T, int> finder, T defaultValue = default(T)) {
            if (count == 0)
                return Tuple.Create(defaultValue, defaultValue);
            var found = BinarySearch(list, index, count, finder);
            if (found < 0) {
                var lo = ~found;
                if (lo <= index)
                    return Tuple.Create(defaultValue, list[lo]);
                if (lo < index + count)
                    return Tuple.Create(list[lo - 1], list[lo]);
                return Tuple.Create(list[lo - 1], defaultValue);
            }
            var equivalent = list[found];
            return Tuple.Create(equivalent, equivalent);
        }

        #endregion

        #region ConvertAll

        /// <summary>
        /// Converts a collection of one type to a collection of another type.
        /// </summary>
        /// <typeparam name="TInput">The type of the elements of the source collection.</typeparam>
        /// <typeparam name="TOutput">The type of the elements of the target collection.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="converter">A Converter<TInput, TOutput> that converts each element from one type to another type.</param>
        /// <returns>A collection of the target type containing the converted elements from the source array.</returns>
        public static ICollection<TOutput> ConvertAll<TInput, TOutput>(this ICollection<TInput> source, Converter<TInput, TOutput> converter) {
            var list = new List<TOutput>(source.Count);
            foreach (var item in source) {
                list.Add(converter(item));
            }
            return list;
        }

        /// <summary>
        /// Converts a list of one type to a list of another type.
        /// </summary>
        /// <typeparam name="TInput">The type of the elements of the source list.</typeparam>
        /// <typeparam name="TOutput">The type of the elements of the target list.</typeparam>
        /// <param name="source">The source list.</param>
        /// <param name="converter">A Converter<TInput, TOutput> that converts each element from one type to another type.</param>
        /// <returns>A list of the target type containing the converted elements from the source array.</returns>
        public static IList<TOutput> ConvertAll<TInput, TOutput>(this IList<TInput> source, Converter<TInput, TOutput> converter) {
            var list = new List<TOutput>(source.Count);
            foreach (var item in source) {
                list.Add(converter(item));
            }
            return list;
        }
        #endregion

        #region ElementsAt

        public static IEnumerable<TSource> ElementsAt<TSource>(this IReadOnlyList<TSource> source, params int[] indices) {
            return indices.Select(i => source[i]);
        }
        public static IEnumerable<TSource> ElementsAt<TSource>(this IList<TSource> source, params int[] indices) {
            return indices.Select(i => source[i]);
        }

        public static IEnumerable<TSource> ElementsAtOrDefault<TSource>(this IReadOnlyList<TSource> source, params int[] indices) {
            for (int i = 0; i < indices.Length; i++) {
                var indice = indices[i];
                if (0 < indice || indice >= source.Count)
                    yield return default(TSource);
                else
                    yield return source[indice];
            }
        }
        public static IEnumerable<TSource> ElementsAtOrDefault<TSource>(this IList<TSource> source, params int[] indices) {
            for (int i = 0; i < indices.Length; i++) {
                var indice = indices[i];
                if (0 < indice || indice >= source.Count)
                    yield return default(TSource);
                else
                    yield return source[indice];
            }
        }

        #endregion

        #region InterpolatedSearch

        /// <summary>
        /// Searches a range of elements in the sorted <seealso cref="IReadOnlyList{T}"/> for an element using the specified function and returns the zero-based index of the element.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="list">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The zero-based index of item in the sorted <seealso cref="IReadOnlyList{T}"/>, if item is found; 
        /// otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item or, 
        /// if there is no larger element, the bitwise complement of Count.</returns>
        public static int InterpolatedSearch<T>(this IReadOnlyList<T> list, int index, int count, T value, IOrdinal<T> ordinal) {
            if (list == null)
                throw new ArgumentNullException("list");
            if (index < 0)
                throw new ArgumentOutOfRangeException("index");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");
            if ((list.Count - index) < count)
                throw new ArgumentException("Invalid length");

            try {
                int lo = index;
                int hi = lo + count - 1;
                while (lo <= hi) {
                    int D = ordinal.Compare(list[lo], list[hi]);
                    int d = ordinal.Compare(list[lo], value);
                    if (D == 0) {
                        if (d < 0)
                            return ~lo;
                        if (d > 0)
                            return ~hi;
                        return lo;
                    }
                    int i = lo + MulDiv(d, hi - lo, D);
                    int c = ordinal.Compare(list[i], value);
                    switch (c) {
                    case 0:
                        return i;
                    case -1:
                        return DoBinarySearch(list, i + 1, hi, value, ordinal);
                    case 1:
                        return DoBinarySearch(list, lo, i - 1, value, ordinal);
                    default:
                        if (c == 0)
                            return i;
                        if (c < 0) {
                            lo = i + 1;
                        } else {
                            hi = i - 1;
                        }
                        break;
                    }
                }
                return ~lo;
            }
            catch (Exception e) {
                throw new InvalidOperationException("The ordinal threw an exception", e);
            }
        }

        public static int InterpolatedSearch<T>(this IReadOnlyList<T> list, T value, IOrdinal<T> ordinal) {
            return InterpolatedSearch(list, 0, list.Count, value, ordinal);
        }

        #endregion

        #region MinElement / MaxElement / MinMaxElement

        public static int MinElement<TSource>(this IList<TSource> source, IComparer<TSource> comparer) {
            if (source == null) throw new ArgumentNullException("source");

            Comparison<TSource> comparison = comparer.Compare;
            return MinElement(source, 0, source.Count, comparison);
        }

        public static int MinElement<TSource>(this IList<TSource> source, int index, int length, IComparer<TSource> comparer) {
            if (source == null) throw new ArgumentNullException("source");

            Comparison<TSource> comparison = comparer.Compare;
            return MinElement(source, index, length, comparison);
        }

        public static int MinElement<TSource>(this IList<TSource> source, Comparison<TSource> comparison) {
            if (source == null) throw new ArgumentNullException("source");

            return MinElement(source, 0, source.Count, comparison);
        }

        public static int MinElement<TSource>(this IList<TSource> source, int index, int length, Comparison<TSource> comparison) {
            if (source == null) throw new ArgumentNullException("source");

            if (source.Count == 0)
                return index - 1;
            var min = source[0];
            var p = 0;
            length += index;
            for (int i = index + 1; i < length; i++) {
                if (comparison(source[i], min) < 0) {
                    min = source[p = i];
                }
            }
            return p;
        }

        public static int MaxElement<TSource>(this IList<TSource> source, IComparer<TSource> comparer) {
            if (source == null) throw new ArgumentNullException("source");

            Comparison<TSource> comparison = comparer.Compare;
            return MaxElement(source, 0, source.Count, comparison);
        }

        public static int MaxElement<TSource>(this IList<TSource> source, int index, int length, IComparer<TSource> comparer) {
            if (source == null) throw new ArgumentNullException("source");

            Comparison<TSource> comparison = comparer.Compare;
            return MaxElement(source, index, length, comparison);
        }

        public static int MaxElement<TSource>(this IList<TSource> source, Comparison<TSource> comparison) {
            if (source == null) throw new ArgumentNullException("source");

            return MaxElement(source, 0, source.Count, comparison);
        }

        public static int MaxElement<TSource>(this IList<TSource> source, int index, int length, Comparison<TSource> comparison) {
            if (source == null) throw new ArgumentNullException("source");

            if (source.Count == 0)
                return index - 1;
            var max = source[0];
            var p = 0;
            length += index;
            for (int i = index + 1; i < source.Count; i++) {
                if (comparison(source[i], max) >= 0) {
                    max = source[p = i];
                }
            }
            return p;
        }

        public static Tuple<int, int> MinMaxElement<TSource>(this IList<TSource> source, IComparer<TSource> comparer) {
            Comparison<TSource> comparison = comparer.Compare;
            return MinMaxElement(source, comparison);
        }

        public static Tuple<int, int> MinMaxElement<TSource>(this IList<TSource> source, Comparison<TSource> comparison) {
            if (source == null) throw new ArgumentNullException("source");

            if (source.Count == 0)
                return Tuple.Create(-1, -1);
            var min = source[0];
            var max = source[0];
            var p = 0;
            var q = 0;
            for (int i = 1; i < source.Count; i++) {
                if (comparison(source[i], min) < 0) {
                    min = source[p = i];
                } else if (comparison(source[i], max) >= 0) {
                    max = source[q = i];
                }
            }
            return Tuple.Create(p, q);
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

        #region Reserve

        public static void Reserve<T>(this List<T> list, int extraSpace) {
            var required = list.Count + extraSpace;
            if (list.Capacity < required)
                list.Capacity = required;
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
                list.SwapItems(start, end);
            }
        }

        public static void Reverse<T>(this IList<T> list) {
            Reverse(list, 0, list.Count);
        }

        #endregion

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
        /// <param name="list">The list.</param>
        /// <param name="i">The item at the <paramref name="i"/> index.</param>
        /// <param name="j">The item at the <paramref name="j"/> index.</param>
        /// <returns>The list</returns>
        /// <remarks>This function does not guard against null list or out of bound indices.</remarks>
        public static IList<T> SwapItems<T>(this IList<T> list, int i, int j) {
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
            return list;
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
