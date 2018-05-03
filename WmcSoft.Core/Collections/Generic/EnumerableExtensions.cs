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
using System.Linq;
using System.Text;
using WmcSoft.Collections.Generic.Internals;
using WmcSoft.Collections.Specialized;
using WmcSoft.Diagnostics;
using static WmcSoft.Algorithms;

namespace WmcSoft.Collections.Generic
{
    public static partial class EnumerableExtensions
    {
        #region AsEnumerable

        /// <summary>
        /// Lift the source element as en enumerable.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of source</typeparam>
        /// <param name="source">the source element</param>
        /// <returns>An enumerable on the source</returns>
        public static IEnumerable<TSource> AsEnumerable<TSource>(TSource source)
        {
            return new SingleItemReadOnlyList<TSource>(source);
        }

        /// <summary>
        /// Lift the source element as en enumerable.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of source</typeparam>
        /// <param name="source">the source element</param>
        /// <returns>An enumerable on the source, or an empty enumerable when the source is null</returns>
        public static IEnumerable<TSource> AsEnumerable<TSource>(TSource? source) where TSource : struct
        {
            if (source.HasValue)
                yield return source.GetValueOrDefault();
        }

        #endregion

        #region AsReadOnlyCollection

        /// <summary>
        /// Returns an enumerable optimized for functions requiring a Count
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="source">The sequence of items</param>
        /// <param name="count">The count of items.</param>
        /// <returns>The decorated enumerable</returns>
        /// <remarks>For optimization, the function does not guard against wrong count.</remarks>
        public static IReadOnlyCollection<T> AsReadOnlyCollection<T>(this IEnumerable<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            return new CollectionAdapter<T>(count, source);
        }

        #endregion

        #region AnyOrEmpty

        /// <summary>
        /// Determines if a sequence is empty or if any of its elements match the specified predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> whose elements to apply the predicate to.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns><c>true</c> if the source is empty or if any element match the predicate; otherwise, <c>false</c>.</returns>
        public static bool AnyOrEmpty<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            using (var enumerator = source.GetEnumerator()) {
                if (!enumerator.MoveNext())
                    return true;

                do {
                    if (predicate(enumerator.Current))
                        return true;
                } while (enumerator.MoveNext());
                return false;
            }
        }

        #endregion

        #region AtLeast / AtMost

        /// <summary>
        /// Determines whether a sequence contains at least <paramref name="n"/> elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to check.</param>
        /// <param name="n">The expected number of elements.</param>
        /// <returns><c>true</c> if the sequence contains at least <paramref name="n"/> elements; otherwise, <c>false</c>.</returns>
        public static bool AtLeast<TSource>(this IEnumerable<TSource> source, int n)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (n == int.MaxValue) throw new ArgumentOutOfRangeException(nameof(n));

            var traits = new EnumerableTraits<TSource>(source);
            if (traits.HasCount)
                return traits.Count >= n;
            var count = 0;
            foreach (var item in source) {
                if (count++ > n)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether at least <paramref name="n"/> elements of a sequence satisfy a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> whose elements to apply the predicate to.</param>
        /// <param name="n">The expected number of elements.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns><c>true</c> if at least <paramref name="n"/> elements in the source sequence pass the test in the specified predicate; otherwise, <c>false</c>.</returns>
        public static bool AtLeast<TSource>(this IEnumerable<TSource> source, int n, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (n == int.MaxValue) throw new ArgumentOutOfRangeException(nameof(n));

            var count = 0;
            foreach (var item in source.Where(predicate)) {
                if (count++ > n)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether a sequence contains at most <paramref name="n"/> elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to check.</param>
        /// <param name="n">The expected number of elements.</param>
        /// <returns><c>true</c> if the sequence contains at most <paramref name="n"/> elements; otherwise, <c>false</c>.</returns>
        public static bool AtMost<TSource>(this IEnumerable<TSource> source, int n)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (n == int.MaxValue) throw new ArgumentOutOfRangeException(nameof(n));

            var traits = new EnumerableTraits<TSource>(source);
            if (traits.HasCount)
                return traits.Count <= n;
            var count = 0;
            foreach (var item in source) {
                if (count++ > n)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determines whether at most <paramref name="n"/> elements of a sequence satisfy a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> whose elements to apply the predicate to.</param>
        /// <param name="n">The expected number of elements.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns><c>true</c> if at most <paramref name="n"/> elements in the source sequence pass the test in the specified predicate; otherwise, <c>false</c>.</returns>
        public static bool AtMost<TSource>(this IEnumerable<TSource> source, int n, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (n == int.MaxValue) throw new ArgumentOutOfRangeException(nameof(n));

            var count = 0;
            foreach (var item in source.Where(predicate)) {
                if (count++ > n)
                    return false;
            }
            return true;
        }

        #endregion

        #region Backwards

        public static IEnumerable<T> UnguardedBackwards<T>(IReadOnlyList<T> source)
        {
            for (int i = source.Count - 1; i >= 0; i--) {
                yield return source[i];
            }
        }

        public static IReadOnlyList<T> Backwards<T>(this IReadOnlyList<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new BackwardsReadOnlyList<T>(source);
        }

        /// <summary>
        /// Inverts the order of the elements in a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <see cref="source"/>.</typeparam>
        /// <param name="source">A sequence of values to reverse.</param>
        /// <returns>A sequence whose elements correspond to those of the input sequence in reverse order.</returns>
        /// <remarks>Similar to <see cref="Enumerable.Reverse{TSource}(IEnumerable{TSource})"/> except that a copy 
        /// is avoided for classes implementing <see cref="IList{TSource}"/>.</remarks>
        public static IEnumerable<TSource> Backwards<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            switch (source) {
            case IReadOnlyList<TSource> readOnlyList:
                return Backwards(readOnlyList);
            case IList<TSource> list:
                return Backwards(list.AsReadOnly());
            default:
                return source.Reverse();
            }
        }

        #endregion

        #region Crawl

        static IEnumerable<T> UnguardedCrawlWhile<T>(T start, Func<T, T> next, Predicate<T> predicate)
        {
            for (T item = start; predicate(item); item = next(item))
                yield return item;
        }

        public static IEnumerable<T> Crawl<T>(this T start, Func<T, T> next, T sentinel = default)
        {
            if (next == null) throw new ArgumentNullException(nameof(next));

            var comparer = EqualityComparer<T>.Default;
            return UnguardedCrawlWhile(start, next, i => !comparer.Equals(i, sentinel));
        }

        public static IEnumerable<T> CrawlWhile<T>(this T start, Func<T, T> next, Predicate<T> predicate)
        {
            if (next == null) throw new ArgumentNullException(nameof(next));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return UnguardedCrawlWhile(start, next, i => predicate(i));
        }

        public static IEnumerable<T> CrawlUntil<T>(this T start, Func<T, T> next, Predicate<T> predicate)
        {
            if (next == null) throw new ArgumentNullException(nameof(next));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return UnguardedCrawlWhile(start, next, i => !predicate(i));
        }

        #endregion

        #region Choose

        static IEnumerable<TSource> UnguardedFlush<TSource>(IEnumerator<TSource> enumerator)
        {
            using (enumerator) {
                do {
                    yield return enumerator.Current;
                } while (enumerator.MoveNext());
            }
        }

        static IEnumerable<TSource> UnguardedFlush<TSource>(IEnumerator<TSource> enumerator, Func<TSource, bool> predicate)
        {
            using (enumerator) {
                yield return enumerator.Current;
                while (enumerator.MoveNext()) {
                    var current = enumerator.Current;
                    if (predicate(current))
                        yield return current;
                }
            }
        }

        static IEnumerable<TSource> UnguardedChoose<TSource>(IEnumerable<TSource> source, Func<TSource, bool>[] predicates)
        {
            var length = predicates.Length;
            var slots = new List<TSource>[length];
            for (int i = 1; i < length; i++) {
                slots[i] = new List<TSource>();
            }
            var enumerator = source.GetEnumerator();
            try {
                var predicate = predicates[0];
                while (enumerator.MoveNext()) {
                    var item = enumerator.Current;
                    if (predicate(item)) {
                        return UnguardedFlush(Move(ref enumerator), predicate);
                    }
                    for (int i = 1; i < length; i++) {
                        if (predicates[i](item)) {
                            slots[i].Add(item);
                            i++;
                            while (length > i) {
                                length--;
                                slots[length] = null; // not needed anymore
                            }
                        }
                    }
                }

                // exhausted items, return the first not empty slot.
                for (int i = 1; i < length; i++) {
                    if (slots[i].Count > 0) {
                        return slots[i];
                    }
                }
                return Enumerable.Empty<TSource>();
            } finally {
                if (enumerator != null)
                    enumerator.Dispose();
            }
        }

        /// <summary>
        /// Returns the elements matching the first predicate. 
        /// If none match, then returns the element matching the second, etc.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the sequence.</typeparam>
        /// <param name="source">The sequence of elements.</param>
        /// <param name="predicates">The prioritized sequence of predicates.</param>
        /// <returns>The elements matching the first predicate to match any element.</returns>
        public static IEnumerable<TSource> Choose<TSource>(this IEnumerable<TSource> source, params Func<TSource, bool>[] predicates)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var length = predicates == null ? 0 : predicates.Length;
            switch (length) {
            case 0:
                return source;
            case 1:
                return source.Where(predicates[0]);
            default:
                return UnguardedChoose(source, predicates);
            }
        }

        #endregion

        #region Discretize

        /// <summary>
        /// Categorize the source's elements in buckets delimited by the specified <paramref name="bounds"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="source">The sequence of items</param>
        /// <param name="bounds">The buckets boundaries.</param>
        /// <returns>The corresponding sequence of buckets index.</returns>
        /// <remarks>The bounds are left inclusive and right exclusive. Items less than bounds[0] are in the bucket 0, items equals to bounds[i-1] and less than bounds[i] are in bucket <c>i</c>.</remarks>
        public static IEnumerable<int> Discretize<T>(this IEnumerable<T> source, params T[] bounds)
        {
            return Discretize(source, Comparer<T>.Default, bounds);
        }

        /// <summary>
        /// Categorize the source's elements in buckets delimited by the specified <paramref name="bounds"/>, comparing using <paramref name="comparer"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="source">The sequence of items</param>
        /// <param name="bounds">The buckets boundaries.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>The corresponding sequence of buckets index.</returns>
        public static IEnumerable<int> Discretize<T>(this IEnumerable<T> source, IComparer<T> comparer, params T[] bounds)
        {
            foreach (var item in source) {
                var index = Array.BinarySearch(bounds, item, comparer);
                if (index < 0)
                    yield return ~index;
                else
                    yield return index + 1;
            }
        }

        #endregion

        #region DrawLots

        static T UnguardedDrawLots<T>(IEnumerable<T> source, Random random)
        {
            switch (source) {
            case IReadOnlyList<T> list:
                return list[random.Next(list.Count)];
            case IList<T> list:
                return list[random.Next(list.Count)];
            case IReadOnlyCollection<T> collection:
                return source.Skip(random.Next(collection.Count)).First();
            case ICollection<T> collection:
                return source.Skip(random.Next(collection.Count)).First();
            case ICollection collection:
                return source.Skip(random.Next(collection.Count)).First();
            }

            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var value = enumerator.Current;
                    var count = 1;
                    while (enumerator.MoveNext()) {
                        if (random.Next(++count) == 0) {
                            value = enumerator.Current;
                        }
                    }
                    return value;
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Picks an element at random from the source sequence.
        /// </summary>
        /// <typeparam name="T">The type of the element of the source</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to return the element from.</param>
        /// <param name="random">The random.</param>
        /// <returns>Returns an item from the <see cref="source"/> picked at random.</returns>
        /// <remarks>Jon Skeet proposed a similar solution at stackoverflow http://stackoverflow.com/a/648240 </remarks>
        public static T DrawLots<T>(this IEnumerable<T> source, Random random)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (random == null) throw new ArgumentNullException(nameof(random));

            return UnguardedDrawLots(source, random);
        }

        /// <summary>
        /// Picks an element at random from the source sequence.
        /// </summary>
        /// <typeparam name="T">The type of the element of the source</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to return the element from.</param>
        /// <param name="random">The random.</param>
        /// <returns>Returns an item from the <see cref="source"/> picked at random.</returns>
        /// <remarks>Jon Skeet proposed a similar solution at stackoverflow http://stackoverflow.com/a/648240 </remarks>
        public static T DrawLots<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return UnguardedDrawLots(source, new Random());
        }

        #endregion

        #region ElementAt

        /// <summary>
        /// Returns the element at a specified index in a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to return an element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <param name="selector">A transform function to apply to the element.</param>
        /// <returns>The element at the specified position in the source sequence.</returns>
        public static TResult ElementAt<TSource, TResult>(this IEnumerable<TSource> source, int index, Func<TSource, TResult> selector)
        {
            // I would normally return default(TResult) but here I should be consistent with the framework.
            if (source == null) throw new ArgumentNullException(nameof(source));

            var result = source.ElementAt(index);
            return selector(result);
        }

        /// <summary>
        /// Returns the element at a specified index in a sequence, or a default value if the sequence is shorter.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to return an element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <param name="selector">A transform function to apply to the element.</param>
        /// <returns>The element at the specified position in the source sequence, or <code>default(TSource)</code> if the sequence contains less elements.</returns>
        public static TResult ElementAtOrDefault<TSource, TResult>(this IEnumerable<TSource> source, int index, Func<TSource, TResult> selector)
        {
            // I would normally return default(TResult) but here I should be consistent with the framework.
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (index >= 0) {
                var list = source as IList<TSource>;
                if (list == null) {
                    using (var enumerator = source.GetEnumerator()) {
                        while (enumerator.MoveNext()) {
                            if (index == 0)
                                return selector(enumerator.Current);
                            index--;
                        }
                    }
                } else if (index < list.Count) {
                    return selector(list[index]);
                }
            }
            return default;
        }

        #endregion

        #region Equivalent

        public static bool Equivalent<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            var t1 = new EnumerableTraits<TSource>(first);
            var t2 = new EnumerableTraits<TSource>(second);

            if (t1.HasCount && t2.HasCount && t1.Count != t2.Count)
                return false;

            var bag = new Bag<TSource>(first);
            if (t2.HasCount && bag.Count != t2.Count)
                return false;

            foreach (var item in second) {
                if (!bag.Remove(item))
                    return false;
            }
            return bag.Count == 0;
        }

        #endregion

        #region Except

        /// <summary>
        /// Produces the set difference of two sequences by using the default equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <param name="first">An <see cref="IEnumerable{T}"/> whose elements that are not also in <paramref name="second"/> will be returned.</param>
        /// <param name="second">Elements that also occur in the <paramref name="second"/> sequence will be removed from the returned sequence.</param>
        /// <returns>A sequence that contains the set difference of the elements of two sequences.</returns>
        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, params TSource[] second)
        {
            return Enumerable.Except(first, second);
        }

        /// <summary>
        /// Produces the set difference of two sequences by using the specified <see cref="IEqualityComparer{T}"/> to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <param name="first">An <see cref="IEnumerable{T}"/> whose elements that are not also in <paramref name="second"/> will be returned.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to compare values.</param>
        /// <param name="second">Elements that also occur in the <paramref name="second"/> sequence will be removed from the returned sequence.</param>
        /// <returns>A sequence that contains the set difference of the elements of two sequences.</returns>
        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEqualityComparer<TSource> comparer, params TSource[] second)
        {
            return Enumerable.Except(first, second, comparer);
        }

        #endregion

        #region Extract

        public static int Extract<TSource>(this IEnumerable<TSource> source, out TSource value)
        {
            using (var enumerator = new CountingEnumerator<TSource>(source)) {
                value = (enumerator.MoveNext()) ? enumerator.Current : default;
                return enumerator.Count;
            }
        }
        public static int Extract<TSource>(this IEnumerable<TSource> source, out TSource value1, out TSource value2)
        {
            using (var enumerator = new CountingEnumerator<TSource>(source)) {
                value1 = (enumerator.MoveNext()) ? enumerator.Current : default;
                value2 = (enumerator.MoveNext()) ? enumerator.Current : default;
                return enumerator.Count;
            }
        }
        public static int Extract<TSource>(this IEnumerable<TSource> source, out TSource value1, out TSource value2, out TSource value3)
        {
            using (var enumerator = new CountingEnumerator<TSource>(source)) {
                value1 = (enumerator.MoveNext()) ? enumerator.Current : default;
                value2 = (enumerator.MoveNext()) ? enumerator.Current : default;
                value3 = (enumerator.MoveNext()) ? enumerator.Current : default;
                return enumerator.Count;
            }
        }
        public static int Extract<TSource>(this IEnumerable<TSource> source, out TSource value1, out TSource value2, out TSource value3, out TSource value4)
        {
            using (var enumerator = new CountingEnumerator<TSource>(source)) {
                value1 = (enumerator.MoveNext()) ? enumerator.Current : default;
                value2 = (enumerator.MoveNext()) ? enumerator.Current : default;
                value3 = (enumerator.MoveNext()) ? enumerator.Current : default;
                value4 = (enumerator.MoveNext()) ? enumerator.Current : default;
                return enumerator.Count;
            }
        }

        #endregion

        #region XxxIfXxx

        /// <summary>
        /// Returns the elements of the specified sequence or the type parameter's default value in a singleton collection if the sequence is null or empty.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to return a default value for if it is null or empty.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> object that contains the default value for the TSource type if source is null or empty; otherwise, source.</returns>
        public static IEnumerable<TSource> DefaultIfNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return DefaultIfNullOrEmpty(source, default);
        }

        /// <summary>
        /// Returns the elements of the specified sequence or the specified value in a singleton collection if the sequence is empty.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to return a default value for if it is null or empty.</param>
        /// <param name="defaultValue">The value to return if the sequence is null or empty.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> object that contains the default value for the TSource type if source is null or empty; otherwise, source.</returns>
        public static IEnumerable<TSource> DefaultIfNullOrEmpty<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
        {
            if (source == null)
                return new SingleItemReadOnlyList<TSource>(defaultValue);
            return source.DefaultIfEmpty();
        }

        /// <summary>
        /// Returns the elements of the specified sequence or an empty sequence if <paramref name="source"/> is null.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to return if it is not null.</param>
        /// <returns>An empty sequence if ource is null; otherwise, source.</returns>
        public static IEnumerable<TSource> EmptyIfNull<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                return Enumerable.Empty<TSource>();
            return source;
        }

        #endregion

        #region LexicographicalCompare

        public static int LexicographicalCompare<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IComparer<TSource> comparer)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));

            var lexicographical = new LexicographicalComparer<TSource>(comparer);
            return lexicographical.Compare(first, second);
        }

        public static int LexicographicalCompare<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return LexicographicalCompare(first, second, null);
        }

        #endregion

        #region Match/Mismatch

        /// <summary>
        /// Returns items from the primary enumerator only when the relation with the secondary enumerator returns <c>true</c>.
        /// </summary>
        /// <typeparam name="T">The primary type.</typeparam>
        /// <typeparam name="U">The secondary type</typeparam>
        /// <param name="first">The primary enumerable.</param>
        /// <param name="second">The secondary enumerable.</param>
        /// <param name="relation">The relation</param>
        /// <returns>Items from the primary enumerator for which the relation with the secondary enumerator returns <c>true</c>.</returns>
        /// <remarks>The enumeration stops when any enumerator cannot move to the next item.</remarks>
        public static IEnumerable<T> Match<T, U>(this IEnumerable<T> first, IEnumerable<U> second, Func<T, U, bool> relation)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (relation == null) throw new ArgumentNullException(nameof(relation));

            return UnguardedMatch(first, second, relation);
        }

        /// <summary>
        /// Returns items from the primary enumerator only when the relation with the secondary enumerator returns <c>false</c>.
        /// </summary>
        /// <typeparam name="T">The primary type.</typeparam>
        /// <typeparam name="U">The secondary type</typeparam>
        /// <param name="first">The primary enumerable.</param>
        /// <param name="second">The secondary enumerable.</param>
        /// <param name="relation">The relation</param>
        /// <returns>Items from the primary enumerator for which the relation with the secondary enumerator returns <c>false</c>.</returns>
        /// <remarks>The enumeration stops when any enumerator cannot move to the next item.</remarks>
        public static IEnumerable<T> Mismatch<T, U>(this IEnumerable<T> first, IEnumerable<U> second, Func<T, U, bool> relation)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (relation == null) throw new ArgumentNullException(nameof(relation));

            return UnguardedMismatch(first, second, relation);
        }

        #endregion

        #region NGrams

        public static IEnumerable<NGram<T>> NGrams<T>(this IEnumerable<T> source, int n)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (n < 2) throw new ArgumentOutOfRangeException(nameof(n));

            var ring = new Ring<T>(n);
            using (var enumerator = source.GetEnumerator()) {
                var remaning = n;
                while (remaning-- > 0) {
                    if (!enumerator.MoveNext())
                        yield break;
                    ring.Enqueue(enumerator.Current);
                }

                yield return new NGram<T>(ring.ToArray());
                while (enumerator.MoveNext()) {
                    ring.Enqueue(enumerator.Current);
                    yield return new NGram<T>(ring.ToArray());
                }
            }
        }

        #endregion

        #region NthElements

        public static IEnumerable<TSource> TakeEveryNthElements<TSource>(this IEnumerable<TSource> source, int n)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var i = 0;
            foreach (var element in source) {
                if (++i % n == 0)
                    yield return element;
            }
        }

        public static IEnumerable<TSource> TakeEveryNthElements<TSource>(this IEnumerable<TSource> source, int n1, int n2)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var i = 0;
            foreach (var element in source) {
                ++i;
                if (i % n1 == 0 | i % n2 == 0)
                    yield return element;
            }
        }

        public static IEnumerable<TSource> TakeEveryNthElements<TSource>(this IEnumerable<TSource> source, params int[] n)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (n == null) throw new ArgumentNullException(nameof(n));

            if (n.Length == 0)
                yield break;

            var i = 0;
            foreach (var element in source) {
                ++i;
                if (n.Any(x => i % x == 0))
                    yield return element;
            }
        }

        #endregion

        #region None

        /// <summary>
        /// Determines whether a sequence contains no element.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{TSource}"/> to check for emptiness.</param>
        /// <returns>true if the source sequence is empty; otherwise, false.</returns>
        public static bool None<TSource>(this IEnumerable<TSource> source)
        {
            return !source.Any();
        }

        /// <summary>
        /// Determines whether no element of a sequence satisfies a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{TSource}"/> whose elements to apply the predicate to.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>true if no element in the source sequence pass the test in the specified predicate, or if the sequence is empty; otherwise, false.</returns>
        public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return !source.Any(predicate);
        }

        #endregion

        #region CountOccurences

        public static Dictionary<TSource, int> CountOccurences<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer = null)
        {
            if (source == null) throw new ArgumentNullException();

            var dictionary = new Dictionary<TSource, int>(comparer);
            foreach (var item in source) {
                if (item == null) {
                    continue;
                } else if (dictionary.TryGetValue(item, out int count)) {
                    dictionary[item] = count + 1;
                } else {
                    dictionary.Add(item, 1);
                }
            }
            return dictionary;
        }

        public static Dictionary<TSource, int> CountOccurences<TSource>(this IEnumerable<TSource> source, out int nullCount)
            where TSource : class
        {
            return CountOccurences(source, null, out nullCount);
        }

        public static Dictionary<TSource, int> CountOccurences<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer, out int nullCount)
            where TSource : class
        {
            if (source == null) throw new ArgumentNullException();

            var dictionary = new Dictionary<TSource, int>(comparer);
            nullCount = 0;
            foreach (var item in source) {
                if (item == null) {
                    nullCount++;
                } else if (dictionary.TryGetValue(item, out int count)) {
                    dictionary[item] = count + 1;
                } else {
                    dictionary.Add(item, 1);
                }
            }
            return dictionary;
        }

        #endregion

        #region Read

        [DebuggerStepThrough]
        public static bool TryRead<T>(this IEnumerator<T> enumerator, out T value)
        {
            if (enumerator.MoveNext()) {
                value = enumerator.Current;
                return true;
            }
            value = default;
            return false;
        }

        [DebuggerStepThrough]
        public static T Read<T>(this IEnumerator<T> enumerator)
        {
            if (enumerator.MoveNext()) {
                return enumerator.Current;
            }
            throw new InvalidOperationException();
        }

        [DebuggerStepThrough]
        public static T ReadOrDefault<T>(this IEnumerator<T> enumerator, T defaultValue = default)
        {
            if (enumerator.MoveNext()) {
                return enumerator.Current;
            }
            return defaultValue;
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
                T _current;

                public Enumerator(IList<T> list, int repeat)
                {
                    _list = list;
                    _repeat = repeat;
                    _offset = -1;
                    _countdown = repeat;
                    _current = default;
                }

                #region IEnumerator<T> Membres

                public T Current => _current;

                #endregion

                #region IDisposable Membres

                public void Dispose()
                {
                }

                #endregion

                #region IEnumerator Membres

                object IEnumerator.Current => Current;

                public bool MoveNext()
                {
                    _offset++;
                    if (_offset >= _list.Count) {
                        if (_countdown == 1) {
                            _offset--; // to allow multiple call.
                            _current = default;
                            return false;
                        }
                        _offset = 0;
                        _countdown--;
                    }
                    _current = _list[_offset];
                    return true;
                }

                public void Reset()
                {
                    _offset = -1;
                    _current = default;
                    _countdown = _repeat;
                }

                #endregion
            }

            #region fields

            readonly IList<T> _list;
            readonly int _repeat;

            #endregion

            #region Lifecycle

            public CollateRepeat(IEnumerable<T> enumerable, int repeat)
            {
                _list = new List<T>(enumerable);
                _repeat = repeat;
            }

            #endregion

            #region IReadOnlyCollection<T> Membres

            public int Count => _list.Count * _repeat;

            #endregion

            #region IEnumerable<T> Membres

            public IEnumerator<T> GetEnumerator()
            {
                return new Enumerator(_list, _repeat);
            }

            #endregion

            #region IEnumerable Membres

            IEnumerator IEnumerable.GetEnumerator()
            {
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
                T _current;

                public Enumerator(IList<T> list, int repeat)
                {
                    _list = list;
                    _repeat = repeat;
                    _countdown = repeat + 1;
                    _current = default;
                }

                #region IEnumerator<T> Membres

                public T Current => _current;

                #endregion

                #region IDisposable Membres

                public void Dispose()
                {
                }

                #endregion

                #region IEnumerator Membres

                object IEnumerator.Current => Current;

                public bool MoveNext()
                {
                    if (_offset == _list.Count)
                        return false;// to allow multiple call.

                    _countdown--;
                    if (_countdown == 0) {
                        _countdown = _repeat;
                        _offset++;
                        if (_offset == _list.Count) {
                            _current = default;
                            return false;
                        }
                    }
                    _current = _list[_offset];
                    return true;
                }

                public void Reset()
                {
                    _offset = -1;
                    _countdown = _repeat;
                    _current = default;
                }

                #endregion
            }

            #region fields

            readonly IList<T> _list;
            readonly int _repeat;

            #endregion

            #region Lifecycle

            public GroupedRepeat(IEnumerable<T> enumerable, int repeat)
            {
                _list = new List<T>(enumerable);
                _repeat = repeat;
            }

            #endregion

            #region IReadOnlyCollection<T> Membres

            public int Count => _list.Count * _repeat;

            #endregion

            #region IEnumerable<T> Membres

            public IEnumerator<T> GetEnumerator()
            {
                return new Enumerator(_list, _repeat);
            }

            #endregion

            #region IEnumerable Membres

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }

        /// <summary>
        /// Repeats the sequence count times.
        /// </summary>
        /// <typeparam name="T">The item type</typeparam>
        /// <param name="source">The enumerator</param>
        /// <param name="count">The number of time to repeat the sequence</param>
        /// <param name="collate">Collate the items</param>
        /// <returns>The list with the repeated sequence.</returns>
        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> source, int count, bool collate = true)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            switch (count) {
            case 0:
                return Enumerable.Empty<T>();
            case 1:
                return source;
            default:
                // most optimized implementation tries to cast as ICollection<T>, not IReadOnlyCollection, to get the count of items.
                return collate
                    ? new CollateRepeat<T>(source, count).AsCollection()
                    : new GroupedRepeat<T>(source, count).AsCollection();
            }
        }

        #endregion

        #region Scan

        static IEnumerable<T> UnguardedScan<T>(IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source) {
                action(item);
                yield return item;
            }
        }

        /// <summary>
        /// Executes the <paramref name="action"/> on the items before returning them.
        /// </summary>
        /// <typeparam name="T">The type of the element of the source</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to return the element from.</param>
        /// <param name="action">The action.</param>
        /// <returns>The items in source.</returns>
        public static IEnumerable<T> Scan<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            return UnguardedScan(source, action);
        }

        static IEnumerable<T> UnguardedScan<T>(IEnumerable<T> source, Action<T, int> action)
        {
            var i = 0;
            foreach (var item in source) {
                action(item, ++i);
                yield return item;
            }
        }

        /// <summary>
        /// Executes the <paramref name="action"/> on the items before returning them.
        /// </summary>
        /// <typeparam name="T">The type of the element of the source</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to return the element from.</param>
        /// <param name="action">The action.</param>
        /// <returns>The items in source.</returns>
        public static IEnumerable<T> Scan<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            return UnguardedScan(source, action);
        }

        #endregion

        #region Stride

        static IEnumerable<T> UnguardedStride<T>(IEnumerable<T> source, int step)
        {
            var i = step - 1;
            foreach (var item in source) {
                if (++i == step) {
                    i = 0;
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Returns every Nth element of the source, starting with the first one.
        /// </summary>
        /// <typeparam name="T">The type of the element of the source</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to return the element from.</param>
        /// <param name="step">The step.</param>
        /// <returns>Every Nth element of the source, starting with the first one.</returns>
        public static IEnumerable<T> Stride<T>(this IEnumerable<T> source, int step)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (step < 1) throw new ArgumentOutOfRangeException(nameof(step));

            return UnguardedStride(source, step);
        }

        #endregion

        #region Tail

        /// <summary>
        /// Returns the count last elements from the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <see cref="source"/>.</typeparam>
        /// <param name="source">The sequence to return elements from.</param>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>A sequence that contains at most the specified number elements at the end of the input sequence.</returns>
        public static IEnumerable<TSource> Tail<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (count <= 0)
                return Enumerable.Empty<TSource>();
            if (count == 1)
                return TailIterator1(source);

            var traits = new EnumerableTraits<TSource>(source);
            if (traits.HasCount) {
                if (traits.Count <= count)
                    return source;
                return source.Skip(traits.Count - count);
            }

            return TailIterator(source, count);
        }

        static IEnumerable<TSource> TailIterator1<TSource>(IEnumerable<TSource> source)
        {
            var last = source.Last();
            yield return last;
        }

        static IEnumerable<TSource> TailIterator<TSource>(IEnumerable<TSource> source, int count)
        {
            var buffer = new TSource[count];
            var n = 0;
            var full = false;
            foreach (var e in source) {
                buffer[n++] = e;
                if (n == count) {
                    n = 0;
                    full = true;
                }
            }
            if (full) {
                for (int i = n; i < count; i++)
                    yield return buffer[i];
                for (int i = 0; i < n; i++)
                    yield return buffer[i];
            } else {
                for (int i = 0; i < n; i++)
                    yield return buffer[i];
            }
        }

        #endregion

        #region TailUnless

        /// <summary>
        /// Returns the count last element that does not match the predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <see cref="source"/>.</typeparam>
        /// <param name="source">The sequence to return elements from.</param>
        /// <param name="predicate">The predicate</param>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>A sequence that contains at most the specified number elements not matching the predicate, at the end of the input sequence.</returns>
        public static IEnumerable<TSource> TailUnless<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (count <= 0)
                return Enumerable.Empty<TSource>();
            bool unless(TSource x) => !predicate(x);
            if (count == 1)
                return TailUnless1(source, unless);
            return TailIterator(source.Where(unless), count);
        }

        static IEnumerable<TSource> TailUnless1<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            var last = source.Last(predicate);
            yield return last;
        }

        #endregion

        #region TakeUnless

        /// <summary>
        /// Returns the count first elements that does not match the predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <see cref="source"/>.</typeparam>
        /// <param name="source">The sequence to return elements from.</param>
        /// <param name="predicate">The predicate</param>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>A sequence that contains at most the specified number elements not matching the predicate, from the start of the input sequence.</returns>
        public static IEnumerable<TSource> TakeUnless<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            bool unless(TSource x) => !predicate(x);
            return source.Where(unless).Take(count);
        }

        #endregion

        #region ToArray

        public static BitArray ToBitArray<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var traits = new EnumerableTraits<TSource>(source);
            if (!traits.HasCount)
                return new BitArray(source.Select(_ => predicate(_)).ToArray());

            var result = new bool[traits.Count];
            using (var enumerator = source.GetEnumerator()) {
                var i = 0;
                while (enumerator.MoveNext()) {
                    result[i++] = predicate(enumerator.Current);
                }
            }
            return new BitArray(result);
        }

        private static List<TSource> MakeList<TSource>(this IEnumerable<TSource> source)
        {
            if (source is IReadOnlyCollection<TSource> collection) {
                var list = new List<TSource>(collection.Count);
                list.AddRange(source);
                return list;
            }
            return new List<TSource>(source);
        }

        public static TOutput[,] ToTwoDimensionalArray<TInput, TOutput>(this IEnumerable<TInput> source, params Converter<TInput, TOutput>[] converters)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var list = MakeList(source);

            var columns = converters.Length;
            var rows = list.Count;
            var array = new TOutput[rows, columns];
            for (var i = 0; i < rows; i++) {
                var item = list[i];
                for (var j = 0; j < columns; j++) {
                    array[i, j] = converters[j](item);
                }
            }
            return array;
        }

        /// <summary>
        /// Creates an array of the specified length from an IEnumerable.
        /// </summary>
        /// <typeparam name="TSource">The type of element of the source.</typeparam>
        /// <param name="source">An IEnumerable to create an array from.</param>
        /// <param name="length">The required length of the array</param>
        /// <returns>An array of size length that contains the elements from the input sequence.</returns>
        /// <remarks>If the input sequence as less than length elements, the array is padded with default(TSource) values. 
        /// If the input sequence contains more elements, only the first length items are copied in the array.</remarks>
        public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source, int length)
            where TSource : new()
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var array = new TSource[length];
            var i = 0;
            using (var enumerator = source.GetEnumerator()) {
                while (enumerator.MoveNext() && i < length) {
                    array[i++] = enumerator.Current;
                }
            }
            return array;
        }

        public static T[] AsArray<T>(this IEnumerable<T> values)
        {
            if (values == null)
                return null;

            if (values is T[] array)
                return array;

            return values.ToArray();
        }

        #endregion

        #region ToEnumerable

        class ShieldEnumerable<TSource> : IEnumerable<TSource>
        {
            readonly IEnumerable<TSource> _source;

            public ShieldEnumerable(IEnumerable<TSource> source)
            {
                _source = source;
            }

            public IEnumerator<TSource> GetEnumerator()
            {
                return _source.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _source.GetEnumerator();
            }
        }

        /// <summary>
        /// Hides a enumerable.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> ToEnumerable<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new ShieldEnumerable<TSource>(source);
        }

        #endregion

        #region ToDictionary

        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, DuplicatePolicy policy, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer = null)
        {
            return ToDictionary(source, policy, keySelector, SpecialFunctions<TSource>.Identity, comparer);
        }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, DuplicatePolicy policy, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null) throw new ArgumentNullException(nameof(elementSelector));

            var d = new Dictionary<TKey, TElement>(comparer);
            switch (policy) {
            case DuplicatePolicy.ThrowException:
                foreach (var item in source) {
                    var key = keySelector(item);
                    var value = elementSelector(item);
                    try {
                        d.Add(key, value);
                    } catch (ArgumentException e) {
                        e.CaptureContext(new { key });
                        throw;
                    }
                }
                break;
            case DuplicatePolicy.KeepFirst:
                foreach (var item in source) {
                    var selector = keySelector(item);
                    if (!d.ContainsKey(selector))
                        d.Add(selector, elementSelector(item));
                }
                break;
            case DuplicatePolicy.KeepLast:
                foreach (var item in source) {
                    d[keySelector(item)] = elementSelector(item);
                }
                break;
            }
            return d;
        }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TElement, TElement, TElement> merger, IEqualityComparer<TKey> comparer = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null) throw new ArgumentNullException(nameof(elementSelector));
            if (merger == null) throw new ArgumentNullException(nameof(merger));

            return ToDictionary(source, keySelector, elementSelector, (x, y) => merger(x, elementSelector(y)), comparer);
        }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TElement, TSource, TElement> merger, IEqualityComparer<TKey> comparer = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null) throw new ArgumentNullException(nameof(elementSelector));
            if (merger == null) throw new ArgumentNullException(nameof(merger));

            var d = new Dictionary<TKey, TElement>(comparer);
            foreach (TSource item in source) {
                var selector = keySelector(item);
                if (!d.TryGetValue(selector, out var element)) {
                    d.Add(selector, elementSelector(item));
                } else {
                    d[selector] = merger(element, item);
                }
            }
            return d;
        }

        #endregion

        #region ToHashSet

        /// <summary>
        /// Creates a <see cref="HashSet{T}"/> from an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to create a <see cref="HashSet{T}"/> from.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values in the set, or null to use the default <see cref="EqualityComparer{T}"/> implementation for the set type.</param>
        /// <returns>A <see cref="HashSet{T}"/> that contains element from the input sequence.</returns>
        public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new HashSet<TSource>(source, comparer);
        }

        #endregion

        #region ToRanges

        public static IEnumerable<R> ToRanges<T, R>(this IEnumerable<T> values, Func<T, T, bool> isSuccessor, Func<T, T, R> factory)
        {
            using (var enumerator = values.GetEnumerator()) {
                if (!enumerator.MoveNext())
                    yield break;
                var start = enumerator.Current;
                var end = start;
                while (enumerator.MoveNext()) {
                    if (isSuccessor(end, enumerator.Current)) {
                        end = enumerator.Current;
                    } else {
                        yield return factory(start, end);
                        end = start = enumerator.Current;
                    }
                }
                yield return factory(start, end);
            }
        }

        public static IEnumerable<Tuple<T, T>> ToRanges<T>(this IEnumerable<T> values, Func<T, T, bool> isSuccessor)
        {
            return values.ToRanges(isSuccessor, Tuple.Create);
        }

        #endregion

        #region ToString

        static TextInfo GetTextInfo(IFormatProvider formatProvider)
        {
            return formatProvider.GetFormat<TextInfo>();
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <param name="source">The enumerable.</param>
        /// <param name="format">
        ///   The format to use.
        ///   -or- 
        ///   A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="IFormattable"/> implementation.
        /// </param>
        /// <param name="formatProvider">
        ///   The provider to use to format the value.
        ///   -or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting
        ///   of the operating system.
        /// </param>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public static string ToString<T>(this IEnumerable<T> source, string format, IFormatProvider formatProvider = null)
            where T : IFormattable
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var textInfo = GetTextInfo(formatProvider);
            var separator = textInfo.ListSeparator;
            using (var it = source.GetEnumerator()) {
                if (it.MoveNext()) {
                    var sb = new StringBuilder(it.Current.ToString(format, formatProvider));
                    while (it.MoveNext()) {
                        sb.Append(separator);
                        sb.Append(it.Current.ToString(format, formatProvider));
                    }
                    return sb.ToString();
                }
            }
            return "";
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <param name="source">The enumerable.</param>
        /// <param name="formatProvider">
        ///   The provider to use to format the value.
        ///   -or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting
        ///   of the operating system.
        /// </param>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public static string ToString<T>(this IEnumerable<T> source, IFormatProvider formatProvider)
            where T : IFormattable
        {
            return source.ToString(null, formatProvider);
        }

        #endregion

        #region Unless

        /// <summary>
        /// Filters a sequence of values based on a predicate evaluating to false.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to filter.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains elements from the input sequence that does not satisfy the condition.</returns>
        public static IEnumerable<TSource> Unless<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return source.Where(x => !predicate(x));
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate evaluating to false. Each element's index is used in the logic of the predicate function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to filter.</param>
        /// <param name="predicate">A function to test each element for a condition; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains elements from the input sequence that does not satisfy the condition.</returns>
        public static IEnumerable<TSource> Unless<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return source.Where((x, i) => !predicate(x, i));
        }

        #endregion

        #region ZipAll methods

        public static IEnumerable<TResult> UnguardedZipAll<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector, TFirst defaultFirst = default, TSecond defaultSecond = default)
        {
            using (var enumerator1 = first.GetEnumerator())
            using (var enumerator2 = second.GetEnumerator()) {
                while (true) {
                    if (!enumerator1.MoveNext()) {
                        while (enumerator2.MoveNext()) {
                            yield return resultSelector(defaultFirst, enumerator2.Current);
                        }
                        yield break;
                    }
                    if (!enumerator2.MoveNext()) {
                        do {
                            yield return resultSelector(enumerator1.Current, defaultSecond);
                        } while (enumerator1.MoveNext());
                        yield break;
                    }
                    yield return resultSelector(enumerator1.Current, enumerator2.Current);
                }
            }
        }

        public static IEnumerable<TResult> ZipAll<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector, TFirst defaultFirst = default, TSecond defaultSecond = default)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return UnguardedZipAll(first, second, resultSelector, defaultFirst, defaultSecond);
        }

        #endregion
    }
}
