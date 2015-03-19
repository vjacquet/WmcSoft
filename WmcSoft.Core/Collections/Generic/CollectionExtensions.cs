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
using System.Globalization;
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

        #region AsCollection

        public static ICollection<T> AsCollection<T>(this IReadOnlyCollection<T> readOnlyCollection) {
            var collection = readOnlyCollection as ICollection<T>;
            if (collection != null)
                return collection;
            return new ReadOnlyCollectionToCollectionAdapter<T>(readOnlyCollection);
        }

        #endregion

        #region AsReadOnly

        public static IReadOnlyCollection<T> AsReadOnly<T>(this ICollection<T> collection) {
            return new ReadOnlyCollection<T>(collection);
        }

        public static IReadOnlyList<TOutput> AsReadOnly<TInput, TOutput>(this IReadOnlyList<TInput> list, Converter<TInput, TOutput> convert) {
            return new ConvertingListAdapter<TInput, TOutput>(list, convert);
        }

        #endregion

        #region Repeat

        class CollateRepeat<T> : IReadOnlyCollection<T>
        {
            class Enumerator : IEnumerator<T>
            {
                readonly IList<T> _list;
                readonly int _repeat;
                int _offset;
                int _countdown;

                public Enumerator(IList<T> list, int repeat) {
                    _list = list;
                    _repeat = repeat;
                    _offset = -1;
                    _countdown = repeat;
                }

                #region IEnumerator<T> Membres

                public T Current {
                    get { return _list[_offset]; }
                }

                #endregion

                #region IDisposable Membres

                public void Dispose() {
                }

                #endregion

                #region IEnumerator Membres

                object IEnumerator.Current {
                    get { return Current; }
                }

                public bool MoveNext() {
                    _offset++;
                    if (_offset >= _list.Count) {
                        if (_countdown == 1) {
                            _offset--; // to allow multiple call.
                            return false;
                        }
                        _offset = 0;
                        _countdown--;
                        return true;
                    }
                    return true;
                }

                public void Reset() {
                    _offset = -1;
                    _countdown = _repeat;
                }

                #endregion
            }

            #region fields

            IList<T> _list;
            int _repeat;

            #endregion

            #region Lifecycle

            public CollateRepeat(IEnumerable<T> enumerable, int repeat) {
                _list = new List<T>(enumerable);
                _repeat = repeat;
            }

            #endregion

            #region IReadOnlyCollection<T> Membres

            public int Count {
                get { return _list.Count * _repeat; }
            }

            #endregion

            #region IEnumerable<T> Membres

            public IEnumerator<T> GetEnumerator() {
                return new Enumerator(_list, _repeat);
            }

            #endregion

            #region IEnumerable Membres

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }

            #endregion
        }

        class GroupedRepeat<T> : IReadOnlyCollection<T>
        {
            class Enumerator : IEnumerator<T>
            {
                readonly IList<T> _list;
                readonly int _repeat;
                int _offset;
                int _countdown;

                public Enumerator(IList<T> list, int repeat) {
                    _list = list;
                    _repeat = repeat;
                    _countdown = repeat + 1;
                }

                #region IEnumerator<T> Membres

                public T Current {
                    get { return _list[_offset]; }
                }

                #endregion

                #region IDisposable Membres

                public void Dispose() {
                }

                #endregion

                #region IEnumerator Membres

                object IEnumerator.Current {
                    get { return Current; }
                }

                public bool MoveNext() {
                    if (_offset == _list.Count)
                        return false;
                    _countdown--;
                    if (_countdown == 0) {
                        _countdown = _repeat;
                        _offset++;
                        if (_offset == _list.Count)
                            return false;
                    }
                    return true;
                }

                public void Reset() {
                    _offset = -1;
                    _countdown = _repeat;
                }

                #endregion
            }

            #region fields

            IList<T> _list;
            int _repeat;

            #endregion

            #region Lifecycle

            public GroupedRepeat(IEnumerable<T> enumerable, int repeat) {
                _list = new List<T>(enumerable);
                _repeat = repeat;
            }

            #endregion

            #region IReadOnlyCollection<T> Membres

            public int Count {
                get { return _list.Count * _repeat; }
            }

            #endregion

            #region IEnumerable<T> Membres

            public IEnumerator<T> GetEnumerator() {
                return new Enumerator(_list, _repeat);
            }

            #endregion

            #region IEnumerable Membres

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }

            #endregion
        }

        /// <summary>
        /// Repeats the sequence count times.
        /// </summary>
        /// <typeparam name="T">The item type</typeparam>
        /// <param name="self">The enumerator</param>
        /// <param name="count">The number of time to repeat the sequence</param>
        /// <param name="collate">Collate the items</param>
        /// <returns>The list with the repeated sequence</returns>
        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> self, int count, bool collate = true) {
            if (collate)
                return new CollateRepeat<T>(self, count).AsCollection();
            return new GroupedRepeat<T>(self, count).AsCollection();
        }

        #endregion

        #region BinarySearch methods

        private static int GetMedian(int lo, int hi) {
            Debug.Assert(hi - lo >= 0);
            return lo + (hi - lo) / 2; // reduce the chance of overflow
        }

        /// <summary>
        /// Searches a range of elements in the sorted IReadOnlyList<T> for an element using the specified comparer and returns the zero-based index of the element.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="list">The list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="comparer"></param>
        /// <returns>The zero-based index of item in the sorted IReadOnlyList<T>, if item is found; 
        /// otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item or, 
        /// if there is no larger element, the bitwise complement of Count.</returns>
        public static int BinarySearch<T>(this IReadOnlyList<T> list, int index, int count, Func<T, int> comparer) {
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
                    int i = GetMedian(lo, hi);
                    int c = comparer(list[i]);
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
                throw new InvalidOperationException("The comparer threw an exception", e);
            }
        }

        public static int BinarySearch<T>(this IReadOnlyList<T> list, Func<T, int> comparer) {
            return list.BinarySearch(0, list.Count, comparer);
        }

        public static T BinaryFind<T>(this T[] array, int index, int length, Func<T, int> comparer) {
            var found = array.BinarySearch(index, length, comparer);
            if (found >= 0)
                return array[found];
            return default(T);
        }
        public static T BinaryFind<T>(this T[] array, Func<T, int> comparer) {
            int lb = array.GetLowerBound(0);
            return array.BinaryFind(lb, array.Length, comparer);
        }

        #endregion

        #region Combine & Merge

        /// <summary>
        /// Merges two sorted enumerable in another enumerable sorted using the same comparer. When two items are equivalent, items from first come first.
        /// </summary>
        /// <typeparam name="T">The type of enumerated element</typeparam>
        /// <param name="self">The first enumerable</param>
        /// <param name="other">The second enumerable</param>
        /// <param name="comparer">The comparer</param>
        /// <returns>The sorted enumerable</returns>
        /// <remarks>If the two enumerables are not sorted using the comparer, the result is undefined.</remarks>
        public static IEnumerable<T> Merge<T>(this IEnumerable<T> self, IEnumerable<T> other, IComparer<T> comparer) {
            using(var enumerator1 = self.GetEnumerator())
            using (var enumerator2 = other.GetEnumerator()) {
                var hasValue1 = enumerator1.MoveNext();
                var hasValue2 = enumerator2.MoveNext();

                if (!hasValue1) {
                    // first is empty, consume all second
                    while (hasValue2) {
                        yield return enumerator2.Current;
                        hasValue2 = enumerator2.MoveNext();
                    }
                    yield break;
                }
                if (!hasValue2) {
                    // second is empty, consume all first
                    while (hasValue1) {
                        yield return enumerator1.Current;
                        hasValue1 = enumerator1.MoveNext();
                    }
                    yield break;
                }
                while (true) {
                    if (comparer.Compare(enumerator2.Current, enumerator1.Current) < 0) {
                        yield return enumerator2.Current;
                        hasValue2 = enumerator2.MoveNext();
                        if (!hasValue2) {
                            // shortcut: second is now empty
                            do {
                                yield return enumerator1.Current;
                            } while (enumerator1.MoveNext());
                            yield break;
                        }
                    } else {
                        yield return enumerator1.Current;
                        hasValue1 = enumerator1.MoveNext();
                        if (!hasValue1) {
                            // shortcut: first is now empty
                            do {
                                yield return enumerator2.Current;
                            } while (enumerator2.MoveNext());
                            yield break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Merges two sorted enumerable in another enumerable sorted using the default comparer. When two items are equivalent, items from first come first.
        /// </summary>
        /// <typeparam name="T">The type of enumerated element</typeparam>
        /// <param name="self">The first enumerable</param>
        /// <param name="other">The second enumerable</param>
        /// <param name="comparer">The comparer</param>
        /// <returns>The sorted enumerable</returns>
        /// <remarks>If the two enumerables are not sorted using the default comparer, the result is undefined.</remarks>
        public static IEnumerable<T> Merge<T>(this IEnumerable<T> self, IEnumerable<T> other) {
            return self.Merge(other, Comparer<T>.Default);
        }

        /// <summary>
        /// Combines two sorted enumerable of unique element in another enumerable sorted using the same comparer. When two items are equivalent, they are combined with the combiner.
        /// </summary>
        /// <typeparam name="T">The type of enumerated element</typeparam>
        /// <param name="self">The first enumerable</param>
        /// <param name="other">The second enumerable</param>
        /// <param name="comparer">The comparer</param>
        /// <returns>The sorted enumerable</returns>
        /// <remarks>If the tw enumerables are not sorted using the comparer, the result is undefined.</remarks>
        public static IEnumerable<T> Combine<T>(this IEnumerable<T> self, IEnumerable<T> other, IComparer<T> comparer, Func<T, T, T> combiner) {
            using (var enumerator1 = self.GetEnumerator())
            using (var enumerator2 = other.GetEnumerator()) {
                var hasValue1 = enumerator1.MoveNext();
                var hasValue2 = enumerator2.MoveNext();

                if (!hasValue1) {
                    // first is empty, consume all second
                    while (hasValue2) {
                        yield return enumerator2.Current;
                        hasValue2 = enumerator2.MoveNext();
                    }
                    yield break;
                }
                if (!hasValue2) {
                    // second is empty, consume all first
                    while (hasValue1) {
                        yield return enumerator1.Current;
                        hasValue1 = enumerator1.MoveNext();
                    }
                    yield break;
                }
                while (true) {
                    int comparison = comparer.Compare(enumerator2.Current, enumerator1.Current);
                    if (comparison < 0) {
                        yield return enumerator2.Current;
                        hasValue2 = enumerator2.MoveNext();
                        if (!hasValue2) {
                            // shortcut: second is now empty
                            do {
                                yield return enumerator1.Current;
                            } while (enumerator1.MoveNext());
                            yield break;
                        }
                    } else if (comparison > 0) {
                        yield return enumerator1.Current;
                        hasValue1 = enumerator1.MoveNext();
                        if (!hasValue1) {
                            // shortcut: first is now empty
                            do {
                                yield return enumerator2.Current;
                            } while (enumerator2.MoveNext());
                            yield break;
                        }
                    } else {
                        yield return combiner(enumerator1.Current, enumerator2.Current);
                        hasValue1 = enumerator1.MoveNext();
                        hasValue2 = enumerator2.MoveNext();
                    }
                }
            }
        }

        #endregion

        #region Traits

        public static bool IsSorted<T>(this IEnumerable<T> enumerable, IComparer<T> comparer) {
            using (var enumerator = enumerable.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var previous = enumerator.Current;
                    while (enumerator.MoveNext()) {
                        if (comparer.Compare(previous, enumerator.Current) > 0)
                            return false;
                        previous = enumerator.Current;
                    }
                }
                return true;
            }
        }
        public static bool IsSorted<T>(this IEnumerable<T> enumerable) {
            return enumerable.IsSorted(Comparer<T>.Default);
        }

        public static bool IsSortedAndDistinct<T>(this IEnumerable<T> enumerable, IComparer<T> comparer) {
            using (var enumerator = enumerable.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var previous = enumerator.Current;
                    while (enumerator.MoveNext()) {
                        if (comparer.Compare(previous, enumerator.Current) >= 0)
                            return false;
                        previous = enumerator.Current;
                    }
                }
                return true;
            }
        }
        public static bool IsSortedAndDistinct<T>(this IEnumerable<T> enumerable) {
            return enumerable.IsSortedAndDistinct(Comparer<T>.Default);
        }

        #endregion
    }
}
