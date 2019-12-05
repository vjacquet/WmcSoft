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
using WmcSoft.Collections.Generic.Internals;

using static WmcSoft.Algorithms;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Provides a set of static methods to extend collection related classes or interfaces.
    /// This is a static class.
    /// </summary>
    public static partial class CollectionExtensions
    {
        #region Adapters

        interface IListAdapter<T>
        {
            T this[int index] { get; }
            int Count { get; }
        }

        struct ReadOnlyListAdapter<T> : IListAdapter<T>
        {
            private readonly IReadOnlyList<T> _underlying;

            public ReadOnlyListAdapter(IReadOnlyList<T> list)
            {
                _underlying = list;
            }

            public T this[int index] => _underlying[index];
            public int Count => _underlying.Count;
        }

        struct ProjectionReadOnlyListAdapter<TSource, T> : IListAdapter<T>
        {
            private readonly IReadOnlyList<TSource> _underlying;
            private readonly Func<TSource, T> _selector;

            public ProjectionReadOnlyListAdapter(IReadOnlyList<TSource> list, Func<TSource, T> selector)
            {
                _underlying = list;
                _selector = selector;
            }

            public T this[int index] => _selector(_underlying[index]);
            public int Count => _underlying.Count;
        }

        struct ListAdapter<T> : IListAdapter<T>
        {
            private readonly IList<T> _underlying;

            public ListAdapter(IList<T> list)
            {
                _underlying = list;
            }

            public T this[int index] => _underlying[index];
            public int Count => _underlying.Count;
        }

        struct ProjectionListAdapter<TSource, T> : IListAdapter<T>
        {
            private readonly IList<TSource> _underlying;
            private readonly Func<TSource, T> _selector;

            public ProjectionListAdapter(IList<TSource> list, Func<TSource, T> selector)
            {
                _underlying = list;
                _selector = selector;
            }

            public T this[int index] => _selector(_underlying[index]);
            public int Count => _underlying.Count;
        }

        interface ISelector<in TSource, out TResult>
        {
            TResult Select(TSource source);
        }

        struct IdentitySelector<T> : ISelector<T, T>
        {
            public T Select(T source) => source;
        }

        struct FuncSelector<TSource, TResult> : ISelector<TSource, TResult>
        {
            private readonly Func<TSource, TResult> _selector;

            public FuncSelector(Func<TSource, TResult> selector)
            {
                _selector = selector;
            }

            public TResult Select(TSource source) => _selector(source);
        }

        #endregion

        #region AddRange methods

        /// <summary>
        /// Ensure the value is present by adding it when missing.
        /// </summary>
        /// <typeparam name="T">Type of objects within the collection.</typeparam>
        /// <param name="source">The list to add items to.</param>
        /// <param name="value">The value</param>
        /// <returns>True if the value was added, false it was already in the collection.</returns>
        public static bool Ensure<T>(this ICollection<T> source, T value)
        {
            if (source is ICollection collection && collection.IsSynchronized) {
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
        public static TCollection AddRange<T, TCollection>(this TCollection source, IEnumerable<T> items)
            where TCollection : ICollection<T>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (items == null)
                return source;

            switch (source) {
            case List<T> list:
                list.AddRange(items);
                break;
            case ICollection collection when collection.IsSynchronized:
                lock (collection.SyncRoot) {
                    foreach (var each in items) {
                        source.Add(each);
                    }
                }
                break;
            default:
                foreach (var each in items) {
                    source.Add(each);
                }
                break;
            }

            return source;
        }

        public static TCollection AddRange<T, TCollection>(this TCollection source, params T[] items)
            where TCollection : ICollection<T>
        {
            return AddRange(source, (IEnumerable<T>)items);
        }

        #endregion

        #region AsCollection

        public static ICollection<T> AsCollection<T>(this IReadOnlyCollection<T> readOnlyCollection)
        {
            if (readOnlyCollection is ICollection<T> collection)
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
        /// <remarks>Unlike <see cref="System.Collections.ObjectModel.ReadOnlyCollection{T}"/>, the returned type implements only <see cref="IReadOnlyCollection{T}"/>.</remarks>
        public static IReadOnlyCollection<T> AsReadOnly<T>(this ICollection<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return new ReadOnlyCollectionFacade<T>(collection);
        }

        /// <summary>
        /// Shields a list as a read only list
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="list">The list</param>
        /// <returns>A read only list.</returns>
        /// <remarks>Unlike <see cref="System.Collections.ObjectModel.ReadOnlyCollection{T}"/>, the returned type implements only <see cref="IReadOnlyList{T}"/>.</remarks>
        public static IReadOnlyList<T> AsReadOnly<T>(this IList<T> list)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));

            return new ReadOnlyListFacade<T>(list);
        }

        /// <summary>
        /// Shield a dictionary as a read only dictionary
        /// </summary>
        /// <typeparam name="TKey">The type of the keys</typeparam>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="dictionary">The dictionary</param>
        /// <returns>A read only dictionary</returns>
        /// <remarks>Unlike <see cref="System.Collections.ObjectModel.ReadOnlyDictionary{TKey, TValue}"/>, the returned type implements only <see cref="IReadOnlyDictionary{TKey, TValue}"/>.</remarks>
        public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            return new ReadOnlyDictionaryFacade<TKey, TValue>(dictionary);
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
        public static IReadOnlyCollection<TOutput> AsReadOnly<TInput, TOutput>(this IReadOnlyCollection<TInput> collection, Converter<TInput, TOutput> converter)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (converter == null) throw new ArgumentNullException(nameof(converter));

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
        public static IReadOnlyList<TOutput> AsReadOnly<TInput, TOutput>(this IReadOnlyList<TInput> list, Converter<TInput, TOutput> converter)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (converter == null) throw new ArgumentNullException(nameof(converter));

            return new ConvertingReadOnlyListAdapter<TInput, TOutput>(list, converter);
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
        public static IList<TOutput> ConvertAll<TInput, TOutput>(this ICollection<TInput> source, Converter<TInput, TOutput> converter)
        {
            var list = new List<TOutput>(source.Count);
            foreach (var item in source) {
                list.Add(converter(item));
            }
            return list;
        }

        #endregion

        #region ElementsAt

        static IEnumerable<TResult> DoElementsAt<TSource, TResult, TSelector>(IEnumerable<TSource> source, int[] indices, TSelector selector, TResult tag, bool supportsDefault)
            where TSelector : struct, ISelector<TSource, TResult>
        {
            if (indices.Length == 0)
                return Enumerable.Empty<TResult>();

            // Get the indices in order and to store the value at the correct indice in the buffer
            // sorted contains the position of the indices in increasing order
            var length = indices.Length;
            var sorted = Iota(length).Sort(new SourceComparer<int>(indices));
            var buffer = new TResult[length];
            var i = 0;
            var count = 0;
            var m = indices[sorted[i]];
            using (var enumerator = source.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    if (count++ == m) {
                        buffer[sorted[i]] = selector.Select(enumerator.Current);
                        if (++i == length)
                            return buffer;
                        m = indices[sorted[i]];
                    }
                }
            }

            if (supportsDefault)
                return buffer;
            throw new InvalidOperationException();
        }

        static IEnumerable<TSource> DoElementsAt<TList, TSource>(TList source, TSource tag, params int[] indices)
            where TList : struct, IListAdapter<TSource>
        {
            return indices.Select(i => source[i]);
        }

        public static IEnumerable<TSource> ElementsAt<TSource>(this IList<TSource> list, params int[] indices)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));

            return DoElementsAt(new ListAdapter<TSource>(list), default(TSource), indices);
        }

        public static IEnumerable<TSource> ElementsAt<TSource>(this IEnumerable<TSource> source, params int[] indices)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            switch (source) {
            case IReadOnlyList<TSource> readable:
                return DoElementsAt(new ReadOnlyListAdapter<TSource>(readable), default(TSource), indices);
            case IList<TSource> list:
                return DoElementsAt(new ListAdapter<TSource>(list), default(TSource), indices);
            default:
                return DoElementsAt(source, indices, new IdentitySelector<TSource>(), default(TSource), supportsDefault: false);
            }
        }

        static IEnumerable<TSource> DoElementsAtOrDefault<TList, TSource>(TList source, TSource tag, params int[] indices)
            where TList : struct, IListAdapter<TSource>
        {
            for (var i = 0; i < indices.Length; i++) {
                var indice = indices[i];
                if (0 < indice || indice >= source.Count)
                    yield return tag;
                else
                    yield return source[indice];
            }
        }

        public static IEnumerable<TSource> ElementsAtOrDefault<TSource>(this IList<TSource> list, params int[] indices)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));

            return DoElementsAtOrDefault(new ListAdapter<TSource>(list), default(TSource), indices);
        }

        public static IEnumerable<TSource> ElementsAtOrDefault<TSource>(this IEnumerable<TSource> source, params int[] indices)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            switch (source) {
            case IReadOnlyList<TSource> readable:
                return DoElementsAtOrDefault(new ReadOnlyListAdapter<TSource>(readable), default(TSource), indices);
            case IList<TSource> list:
                return DoElementsAtOrDefault(new ListAdapter<TSource>(list), default(TSource), indices);
            default:
                return DoElementsAt(source, indices, new IdentitySelector<TSource>(), default(TSource), supportsDefault: true);
            }
        }

        public static IEnumerable<TResult> ElementsAt<TSource, TResult>(this IList<TSource> list, Func<TSource, TResult> selector, params int[] indices)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return DoElementsAt(new ProjectionListAdapter<TSource, TResult>(list, selector), default(TResult), indices);
        }

        public static IEnumerable<TResult> ElementsAt<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, params int[] indices)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            switch (source) {
            case IReadOnlyList<TSource> readable:
                return DoElementsAt(new ProjectionReadOnlyListAdapter<TSource, TResult>(readable, selector), default(TResult), indices);
            case IList<TSource> list:
                return DoElementsAt(new ProjectionListAdapter<TSource, TResult>(list, selector), default(TResult), indices);
            default:
                return DoElementsAt(source, indices, new FuncSelector<TSource, TResult>(selector), default(TResult), supportsDefault: false);
            }
        }

        public static IEnumerable<TResult> ElementsAtOrDefault<TSource, TResult>(this IList<TSource> list, Func<TSource, TResult> selector, params int[] indices)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return DoElementsAtOrDefault(new ProjectionListAdapter<TSource, TResult>(list, selector), default(TResult), indices);
        }

        public static IEnumerable<TResult> ElementsAtOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, params int[] indices)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            switch (source) {
            case IReadOnlyList<TSource> readable:
                return DoElementsAtOrDefault(new ProjectionReadOnlyListAdapter<TSource, TResult>(readable, selector), default(TResult), indices);
            case IList<TSource> list:
                return DoElementsAtOrDefault(new ProjectionListAdapter<TSource, TResult>(list, selector), default(TResult), indices);
            default:
                return DoElementsAt(source, indices, new FuncSelector<TSource, TResult>(selector), default(TResult), supportsDefault: true);
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
        public static int InterpolatedSearch<T>(this IReadOnlyList<T> source, int index, int count, T value, IOrdinal<T> ordinal)
        {
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
            } catch (Exception e) {
                var message = string.Format(Properties.Resources.InvalidOperationExceptionRethrow, nameof(ordinal), e.GetType().FullName);
                throw new InvalidOperationException(message, e);
            }
        }

        public static int InterpolatedSearch<T>(this IReadOnlyList<T> source, T value, IOrdinal<T> ordinal)
        {
            return InterpolatedSearch(source, 0, source.Count, value, ordinal);
        }

        #endregion

        #region Partition

        static int UnguardedFindPartitionPoint<T>(IList<T> list, int start, int length, Predicate<T> predicate)
        {
            var end = start + length - 1;
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
        public static int FindPartitionPoint<T>(this IList<T> source, Predicate<T> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

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
        public static int FindPartitionPoint<T>(this IList<T> source, int index, int count, Predicate<T> predicate)
        {
            Guard(source, index, count);
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return UnguardedFindPartitionPoint(source, index, count, predicate);
        }

        /// <summary>
        /// Returns the partition point of the list if the list is partitionned.
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="source">The list</param>
        /// <param name="predicate">Thre predicate</param>
        /// <returns>The index of the partition point, -1 if the list is not partitionned.</returns>
        public static bool IsPartitioned<T>(this IList<T> source, Predicate<T> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

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
        public static bool IsPartitioned<T>(this IList<T> source, int index, int count, Predicate<T> predicate)
        {
            return FindPartitionPoint(source, index, count, predicate) >= 0;
        }

        static int UnguardedPartition<T>(IList<T> list, int start, int length, Predicate<T> predicate)
        {
            var end = start + length - 1;
            while (true) {
                while (start < end && !predicate(list[start]))
                    start++;
                while (start < end && predicate(list[end]))
                    end--;
                if (start > end)
                    return start;
                SwapItems(list, start++, end--);
            }
        }

        /// <summary>
        /// Implements a partition of the list 
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="source">The list</param>
        /// <param name="predicate">Thre predicate</param>
        /// <returns>The partition point</returns>
        public static int Partition<T>(this IList<T> source, Predicate<T> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

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
        public static int Partition<T>(this IList<T> source, int index, int count, Predicate<T> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return UnguardedPartition(source, index, count, predicate);
        }

        private static int UnguardedStablePartition<T>(this IList<T> list, T[] buffer, int start, int length, Predicate<T> predicate)
        {
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
            buffer.ReverseCopyTo(last, list, start + first, length - last);
            return start + first;
        }

        private static int UnguardedStablePartition<T>(IList<T> list, int start, int length, Predicate<T> predicate)
        {
            var offset = start;
            var end = start + length - 1;
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
        public static int StablePartition<T>(this IList<T> source, Predicate<T> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

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
        public static int StablePartition<T>(this IList<T> source, int index, int count, Predicate<T> predicate)
        {
            Guard(source, index, count);
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

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
        public static T Pop<T>(this IList<T> source)
        {
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
        public static T Pop<T>(this IList<T> source, int index)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.Count == 0) throw new InvalidOperationException();

            var t = source[index];
            source.RemoveAt(index);
            return t;
        }

        #endregion

        #region RemoveIf methods

        public static int RemoveIf<T>(this ICollection<T> source, Func<T, bool> predicate)
        {
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

        public static int RemoveIf<T>(this IList<T> source, Func<T, bool> predicate)
        {
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

        public static int RemoveIf<T>(this IList<T> source, Func<T, int, bool> predicate)
        {
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
        public static int RemoveRange<T>(this ICollection<T> source, IEnumerable<T> items)
        {
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

        /// <summary>
        /// Ensures the list has enough space for <paramref name="extraSpace"/> more items.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="extraSpace">the number of items to make room for.</param>
        /// <returns>The <paramref name="list"/>.</returns>
        public static List<T> Reserve<T>(this List<T> list, int extraSpace)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));

            var required = list.Count + extraSpace;
            if (list.Capacity < required)
                list.Capacity = required;
            return list;
        }

        #endregion

        #region Stack & Queue

        public static void Push<T>(this Stack<T> stack, IEnumerable<T> items)
        {
            foreach (var item in items)
                stack.Push(item);
        }

        public static void Enqueue<T>(this Queue<T> queue, IEnumerable<T> items)
        {
            foreach (var item in items)
                queue.Enqueue(item);
        }

        #endregion

        #region Reverse

        public static void Reverse<T>(this IList<T> list, int startIndex, int length)
        {
            var start = startIndex;
            var end = startIndex + length - 1;
            while (start < end) {
                SwapItems(list, start, end);
            }
        }

        public static void Reverse<T>(this IList<T> list)
        {
            Reverse(list, 0, list.Count);
        }

        #endregion

        #region Shuffle methods

        internal static void UnguardedShuffle<T>(IList<T> list, int index, int count, Random random)
        {
            var end = index + count;
            int j;
            for (var i = index; i < end; i++) {
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
        public static void Shuffle<T>(this IList<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            UnguardedShuffle(source, 0, source.Count, new Random());
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
        public static void Shuffle<T>(this IList<T> source, Random random)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (random == null) throw new ArgumentNullException(nameof(random));

            UnguardedShuffle(source, 0, source.Count, random);
        }

        static void UnguardedPartialShuffle<T>(IList<T> source, int count, Random random)
        {
            int j;
            for (var i = 0; i < count; i++) {
                j = random.Next(i, source.Count);
                SwapItems(source, i, j);
            }
        }

        public static void PartialShuffle<T>(this IList<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count > source.Count) throw new ArgumentOutOfRangeException(nameof(count));

            UnguardedPartialShuffle(source, count, new Random());
        }

        public static void PartialShuffle<T>(this IList<T> source, int count, Random random)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count > source.Count) throw new ArgumentOutOfRangeException(nameof(count));
            if (random == null) throw new ArgumentNullException(nameof(random));

            UnguardedPartialShuffle(source, count, random);
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
        public static void PerfectInShuffle<T>(this IList<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if ((source.Count % 2) == 1) throw new ArgumentException(nameof(source));

            UnguardedPerfectInShuffle(source, 0, source.Count);
        }

        public static void PerfectInShuffle<T>(this IList<T> source, int index, int count)
        {
            Guard(source, index, count);
            if ((count % 2) == 1) throw new ArgumentException(nameof(count));

            UnguardedPerfectInShuffle(source, index, count);
        }

        static void UnguardedPerfectInShuffle<T>(IList<T> list, int index, int count)
        {
            var aux = new T[count];
            var m = index + count / 2;
            var r = index + count;
            for (int i = index, j = 0; i < r; i += 2, j++) {
                aux[i] = list[m + j];
                aux[i + 1] = list[index + j];
            }
            aux.CopyTo(list, index, count);
        }

        public static void PerfectInUnshuffle<T>(this IList<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if ((source.Count % 2) == 1) throw new ArgumentException(nameof(source));

            UnguardedPerfectInUnshuffle(source, 0, source.Count);
        }

        public static void PerfectInUnshuffle<T>(this IList<T> source, int index, int count)
        {
            Guard(source, index, count);
            if ((count % 2) == 1) throw new ArgumentException(nameof(count));

            UnguardedPerfectInUnshuffle(source, index, count);
        }

        static void UnguardedPerfectInUnshuffle<T>(IList<T> list, int index, int count)
        {
            var aux = new T[count];
            var m = index + count / 2;
            var r = index + count;
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
        public static void PerfectOutShuffle<T>(this IList<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if ((source.Count % 2) == 1) throw new ArgumentException(nameof(source));

            UnguardedPerfectOutShuffle(source, 0, source.Count);
        }

        public static void PerfectOutShuffle<T>(this IList<T> source, int index, int count)
        {
            Guard(source, index, count);
            if ((count % 2) == 1) throw new ArgumentException(nameof(count));

            UnguardedPerfectOutShuffle(source, index, count);
        }

        static void UnguardedPerfectOutShuffle<T>(IList<T> list, int index, int count)
        {
            var aux = new T[count];
            int m = index + count / 2;
            int r = index + count;
            for (int i = index, j = 0; i < r; i += 2, j++) {
                aux[i] = list[index + j];
                aux[i + 1] = list[m + j];
            }
            aux.CopyTo(list, index, count);
        }

        public static void PerfectOutUnshuffle<T>(this IList<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if ((source.Count % 2) == 1) throw new ArgumentException(nameof(source));

            UnguardedPerfectOutUnshuffle(source, 0, source.Count);
        }

        public static void PerfectOutUnshuffle<T>(this IList<T> source, int index, int count)
        {
            Guard(source, index, count);
            if ((count % 2) == 1) throw new ArgumentException(nameof(count));

            UnguardedPerfectOutUnshuffle(source, index, count);
        }

        static void UnguardedPerfectOutUnshuffle<T>(IList<T> list, int index, int count)
        {
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
        public static IList<T> SwapItems<T>(this IList<T> source, int i, int j)
        {
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
        public static TOutput[] ToArray<TInput, TOutput>(this IReadOnlyCollection<TInput> collection, Converter<TInput, TOutput> convert)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (convert == null) throw new ArgumentNullException(nameof(convert));

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
        public static TOutput[] ToArray<TInput, TOutput>(this IReadOnlyList<TInput> list, Converter<TInput, TOutput> convert)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (convert == null) throw new ArgumentNullException(nameof(convert));

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
        public static bool Toggle<T>(this ICollection<T> collection, T item)
        {
            if (collection.Remove(item))
                return false;
            collection.Add(item);
            return true;
        }

        #endregion
    }
}
