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
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using WmcSoft.Collections.Generic.Internals;
using System.Collections;
using System.Globalization;
using System.Text;

namespace WmcSoft.Collections.Generic
{
    public static class EnumerableExtensions
    {
        #region AsReadOnlyCollection

        /// <summary>
        /// Returns an enumerable optimized for functions requiring a Count
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="source">The count of items</param>
        /// <param name="count">The count of items.</param>
        /// <returns>The decorated enumerable</returns>
        /// <remarks>For optimization, the function does not guard against wrong count.</remarks>
        public static IReadOnlyCollection<T> AsReadOnlyCollection<T>(this IEnumerable<T> source, int count) {
            return new ReadOnlyCollectionAdapter<T>(source, count);
        }

        #endregion

        #region Backwards

        #region BackwardsEnumerableAdapter

        class BackwardsEnumerableAdapter<T> : IEnumerable<T>
        {
            private readonly IEnumerable<T> _enumerable;

            public BackwardsEnumerableAdapter(IEnumerable<T> source) {
                _enumerable = source;
            }

            #region IEnumerable<T> Membres

            public IEnumerator<T> GetEnumerator() {
                var stack = new Stack<T>(_enumerable);
                while (stack.Count > 0) {
                    yield return stack.Pop();
                }
            }

            #endregion

            #region IEnumerable Membres

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }

            #endregion
        }

        #endregion

        public static IEnumerable<T> Backwards<T>(this IReadOnlyList<T> source) {
            for (int i = source.Count - 1; i >= 0; i--) {
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
        /// is avoided for classes implementing <see cref="IList{TSource}"/>.</remarks>
        public static IEnumerable<TSource> Backwards<TSource>(this IEnumerable<TSource> source) {
            var readOnlyList = source as IReadOnlyList<TSource>;
            if (readOnlyList != null)
                return Backwards(readOnlyList);

            var list = source as IList<TSource>;
            if (list != null)
                return Backwards(list.AsReadOnly());

            return new BackwardsEnumerableAdapter<TSource>(source);
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
        public static TResult ElementAt<TSource, TResult>(this IEnumerable<TSource> source, int index, Func<TSource, TResult> selector) {
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
        public static TResult ElementAtOrDefault<TSource, TResult>(this IEnumerable<TSource> source, int index, Func<TSource, TResult> selector) {
            if (source == null) {
                // I would normally return default(TResult) but here I should be consistent with the framework.
                throw new ArgumentNullException("source");
            }

            if (index >= 0) {
                var list = source as IList<TSource>;
                if (list == null) {
                    using (IEnumerator<TSource> enumerator = source.GetEnumerator()) {
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
            return default(TResult);
        }

        #endregion

        #region Extract

        public static int Extract<TSource>(this IEnumerable<TSource> source, out TSource value) {
            using (var enumerator = new CountingEnumerator<TSource>(source)) {
                value = (enumerator.MoveNext()) ? enumerator.Current : default(TSource);
                return enumerator.Count;
            }
        }
        public static int Extract<TSource>(this IEnumerable<TSource> source, out TSource value1, out TSource value2) {
            using (var enumerator = new CountingEnumerator<TSource>(source)) {
                value1 = (enumerator.MoveNext()) ? enumerator.Current : default(TSource);
                value2 = (enumerator.MoveNext()) ? enumerator.Current : default(TSource);
                return enumerator.Count;
            }
        }
        public static int Extract<TSource>(this IEnumerable<TSource> source, out TSource value1, out TSource value2, out TSource value3) {
            using (var enumerator = new CountingEnumerator<TSource>(source)) {
                value1 = (enumerator.MoveNext()) ? enumerator.Current : default(TSource);
                value2 = (enumerator.MoveNext()) ? enumerator.Current : default(TSource);
                value3 = (enumerator.MoveNext()) ? enumerator.Current : default(TSource);
                return enumerator.Count;
            }
        }
        public static int Extract<TSource>(this IEnumerable<TSource> source, out TSource value1, out TSource value2, out TSource value3, out TSource value4) {
            using (var enumerator = new CountingEnumerator<TSource>(source)) {
                value1 = (enumerator.MoveNext()) ? enumerator.Current : default(TSource);
                value2 = (enumerator.MoveNext()) ? enumerator.Current : default(TSource);
                value3 = (enumerator.MoveNext()) ? enumerator.Current : default(TSource);
                value4 = (enumerator.MoveNext()) ? enumerator.Current : default(TSource);
                return enumerator.Count;
            }
        }

        #endregion

        #region ForEach

        /// <summary>
        /// Performs the specified action on each element of the sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of the source.</typeparam>
        /// <param name="source">The elements to apply the predicate to.</param>
        /// <param name="action">A function to apply on each element.</param>
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action) {
            if (source == null)
                return;
            foreach (var item in source)
                action(item);
        }

        /// <summary>
        /// Determines whether each element matches the condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of the source.</typeparam>
        /// <param name="source">The elements to apply the predicate to.</param>
        /// <param name="predicate">The delegate that defines the conditions to check against the elements.</param>
        /// <returns>true if every element in the sequence matches the conditions defined by the specified predicate; 
        /// otherwise, false. If there are no elements, the return value is true.</returns>
        /// <remarks>The predicate is applied on each element of the sequence.</remarks>
        public static bool TrueForEach<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate) {
            if (source == null)
                return true;

            bool b = true;
            foreach (var item in source)
                b &= predicate(item);
            return b;
        }

        /// <summary>
        /// Determines whether some element matches the condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of the source.</typeparam>
        /// <param name="source">The elements to apply the predicate to.</param>
        /// <param name="predicate">The delegate that defines the conditions to check against the elements.</param>
        /// <returns>true if some element in the sequence matches the conditions defined by the specified predicate; 
        /// otherwise, false. If there are no elements, the return value is true.</returns>
        /// <remarks>The predicate is applied on each element of the sequence.</remarks>
        public static bool TrueForSome<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate) {
            if (source == null)
                return false;

            bool b = false;
            foreach (var item in source)
                b |= predicate(item);
            return b;
        }

        #endregion

        #region XxxIfXxx

        /// <summary>
        /// Returns the elements of the specified sequence or the type parameter's default value in a singleton collection if the sequence is null or empty.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to return a default value for if it is null or empty.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> object that contains the default value for the TSource type if source is null or empty; otherwise, source.</returns>
        public static IEnumerable<TSource> DefaultIfNullOrEmpty<TSource>(this IEnumerable<TSource> source) {
            return DefaultIfNullOrEmpty(source, default(TSource));
        }

        /// <summary>
        /// Returns the elements of the specified sequence or the specified value in a singleton collection if the sequence is empty.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to return a default value for if it is null or empty.</param>
        /// <param name="defaultValue">The value to return if the sequence is null or empty.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> object that contains the default value for the TSource type if source is null or empty; otherwise, source.</returns>
        public static IEnumerable<TSource> DefaultIfNullOrEmpty<TSource>(this IEnumerable<TSource> source, TSource defaultValue) {
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
        public static IEnumerable<TSource> EmptyIfNull<TSource>(this IEnumerable<TSource> source) {
            if (source == null)
                return Enumerable.Empty<TSource>();
            return source;
        }

        #endregion

        #region Merge, Combine, etc.

        /// <summary>
        ///  Returns distinct elements from a sorted sequence by using a specified <see cref="IEqualityComparer{T}"/> to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sorted sequence to remove duplicate elements from.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to compare values.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains distinct elements from the source sequence.</returns>
        public static IEnumerable<TSource> SortedDistinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer) {
            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var current = enumerator.Current;
                    while (enumerator.MoveNext()) {
                        if (!comparer.Equals(current, enumerator.Current)) {
                            yield return current;
                            current = enumerator.Current;
                        }
                    }
                    yield return current;
                }
            }
        }

        /// <summary>
        ///  Returns distinct elements from a sorted sequence by using a specified <see cref="IComparer{T}"/> to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sorted sequence to remove duplicate elements from.</param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare values.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains distinct elements from the source sequence.</returns>
        public static IEnumerable<TSource> SortedDistinct<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer) {
            return SortedDistinct(source, new EqualityComparerAdapter<TSource>(comparer));
        }

        /// <summary>
        /// Returns distinct elements from a sorted sequence by using the default equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sorted sequence to remove duplicate elements from.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains distinct elements from the source sequence.</returns>
        public static IEnumerable<TSource> SortedDistinct<TSource>(this IEnumerable<TSource> source) {
            return SortedDistinct(source, EqualityComparer<TSource>.Default);
        }

        public static IEnumerable<TGroup> SortedCombine<TSource, TGroup>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer, Func<TSource, TGroup, TGroup> accumulator, Func<TSource, TGroup> factory) {
            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var current = enumerator.Current;
                    var group = factory(current);
                    while (enumerator.MoveNext()) {
                        if (!comparer.Equals(current, enumerator.Current)) {
                            yield return group;
                            current = enumerator.Current;
                            group = factory(current);
                        } else {
                            group = accumulator(current, group);
                        }
                    }
                    yield return group;
                }
            }
        }
        public static IEnumerable<TGroup> SortedCombine<TSource, TGroup>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer, Func<TSource, TGroup, TGroup> accumulator)
            where TGroup : new() {
            return SortedCombine(source, comparer, accumulator, t => accumulator(t, new TGroup()));
        }
        public static IEnumerable<TGroup> SortedCombine<T, TGroup>(this IEnumerable<T> source, Func<T, TGroup, TGroup> accumulator)
            where TGroup : new() {
            return SortedCombine(source, EqualityComparer<T>.Default, accumulator, t => accumulator(t, new TGroup()));
        }

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
        /// <typeparam name="TSource">The type of enumerated element</typeparam>
        /// <param name="source">The first enumerable</param>
        /// <param name="other">The second enumerable</param>
        /// <param name="comparer">The comparer</param>
        /// <returns>The sorted enumerable</returns>
        /// <remarks>If the two enumerables are not sorted using the default comparer, the result is undefined.</remarks>
        public static IEnumerable<TSource> Merge<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> other) {
            return source.Merge(other, Comparer<TSource>.Default);
        }

        /// <summary>
        /// Combines two sorted enumerable of unique element in another enumerable sorted using the same comparer. When two items are equivalent, they are combined with the combiner.
        /// </summary>
        /// <typeparam name="TSource">The type of enumerated element</typeparam>
        /// <param name="source">The first enumerable</param>
        /// <param name="other">The second enumerable</param>
        /// <param name="comparer">The comparer</param>
        /// <returns>The sorted enumerable</returns>
        /// <remarks>If the two enumerables are not sorted using the comparer, the result is undefined.</remarks>
        public static IEnumerable<TSource> Combine<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> other, IComparer<TSource> comparer, Func<TSource, TSource, TSource> combiner) {
            using (var enumerator1 = source.GetEnumerator())
            using (var enumerator2 = other.GetEnumerator()) {
                var hasValue1 = enumerator1.MoveNext();
                var hasValue2 = enumerator2.MoveNext();

                if (!hasValue1)
                    goto NoMoreValue1;

                if (!hasValue2)
                    goto NoMoreValue2;

                while (true) {
                    int comparison = comparer.Compare(enumerator2.Current, enumerator1.Current);
                    if (comparison < 0) {
                        yield return enumerator2.Current;
                        hasValue2 = enumerator2.MoveNext();
                        if (!hasValue2)
                            goto NoMoreValue2;
                    } else if (comparison > 0) {
                        yield return enumerator1.Current;
                        hasValue1 = enumerator1.MoveNext();
                        if (!hasValue1)
                            goto NoMoreValue1;
                    } else {
                        yield return combiner(enumerator1.Current, enumerator2.Current);
                        hasValue1 = enumerator1.MoveNext();
                        hasValue2 = enumerator2.MoveNext();
                        if (!hasValue1)
                            goto NoMoreValue1;
                        if (!hasValue2)
                            goto NoMoreValue2;
                    }
                }

            NoMoreValue1:
                // first is empty, consume all second
                while (hasValue2) {
                    yield return enumerator2.Current;
                    hasValue2 = enumerator2.MoveNext();
                }
                yield break;

            NoMoreValue2:
                // second is empty, consume all first
                while (hasValue1) {
                    yield return enumerator1.Current;
                    hasValue1 = enumerator1.MoveNext();
                }
                yield break;
            }
        }

        /// <summary>
        /// Combines two sorted enumerable of unique element in another enumerable sorted using the same comparer. When two items are equivalent, they are combined with the combiner.
        /// </summary>
        /// <typeparam name="TInput">The type of enumerated element</typeparam>
        /// <typeparam name="TOutput">The type of enumerated element</typeparam>
        /// <param name="source">The first enumerable</param>
        /// <param name="other">The second enumerable</param>
        /// <param name="comparer">The comparer</param>
        /// <param name="combiner">The combiner</param>
        /// <param name="defaultValue">The default value for the combiner when one value is missing</param>
        /// <returns>The sorted enumerable</returns>
        /// <remarks>If the two enumerables are not sorted using the comparer, the result is undefined.</remarks>
        public static IEnumerable<TOutput> Combine<TInput, TOutput>(this IEnumerable<TInput> source, IEnumerable<TInput> other, IComparer<TInput> comparer, Func<TInput, TInput, TOutput> combiner, TInput defaultValue = default(TInput)) {
            using (var enumerator1 = source.GetEnumerator())
            using (var enumerator2 = other.GetEnumerator()) {
                var hasValue1 = enumerator1.MoveNext();
                var hasValue2 = enumerator2.MoveNext();

                if (!hasValue1)
                    goto NoMoreValue1;

                if (!hasValue2)
                    goto NoMoreValue2;

                while (true) {
                    int comparison = comparer.Compare(enumerator2.Current, enumerator1.Current);
                    if (comparison < 0) {
                        yield return combiner(defaultValue, enumerator2.Current);
                        hasValue2 = enumerator2.MoveNext();
                        if (!hasValue2)
                            goto NoMoreValue2;
                    } else if (comparison > 0) {
                        yield return combiner(enumerator1.Current, defaultValue);
                        hasValue1 = enumerator1.MoveNext();
                        if (!hasValue1)
                            goto NoMoreValue1;
                    } else {
                        yield return combiner(enumerator1.Current, enumerator2.Current);
                        hasValue1 = enumerator1.MoveNext();
                        hasValue2 = enumerator2.MoveNext();
                        if (!hasValue1)
                            goto NoMoreValue1;
                        if (!hasValue2)
                            goto NoMoreValue2;
                    }
                }

            NoMoreValue1:
                // first is empty, consume all second
                while (hasValue2) {
                    yield return combiner(defaultValue, enumerator2.Current);
                    hasValue2 = enumerator2.MoveNext();
                }
                yield break;

            NoMoreValue2:
                // second is empty, consume all first
                while (hasValue1) {
                    yield return combiner(enumerator1.Current, defaultValue);
                    hasValue1 = enumerator1.MoveNext();
                }
                yield break;
            }
        }

        /// <summary>
        /// Concatenates two sequences by alternating their items.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
        /// <param name="first">The first sequence to concatenate</param>
        /// <param name="second">The second sequence to concatenate</param>
        /// <returns>An IEnumerable<T> that contains the alternated elements of the two input sequences.</returns>
        public static IEnumerable<T> Interleave<T>(this IEnumerable<T> first, IEnumerable<T> second) {
            using (var enumerator1 = first.GetEnumerator())
            using (var enumerator2 = second.GetEnumerator()) {
                while (true) {
                    if (!enumerator1.MoveNext()) {
                        // first is empty, consume all second
                        while (enumerator2.MoveNext())
                            yield return enumerator2.Current;
                        yield break;
                    }
                    yield return enumerator1.Current;

                    if (!enumerator2.MoveNext()) {
                        // second is empty, consume all first
                        while (enumerator1.MoveNext())
                            yield return enumerator1.Current;
                        yield break;
                    }
                    yield return enumerator2.Current;
                }
            }
        }

        /// <summary>
        /// Concatenates sequences by alternating their items.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <param name="enumerables">The sequence of sequences</param>
        /// <returns>An IEnumerable<T> that contains the alternated elements of the input sequences.</returns>
        public static IEnumerable<TSource> Interleave<TSource>(this IEnumerable<IEnumerable<TSource>> enumerables) {
            var list = new List<IEnumerator<TSource>>(enumerables.Select(e => e.GetEnumerator()));
            var i = 0;
            while (list.Count != 0) {
                i = i % list.Count;

                if (list[i].MoveNext()) {
                    yield return list[i].Current;
                    i++;
                } else {
                    list.RemoveAt(i);
                }
            }
        }

        #endregion

        #region Min / Max / MinMax

        public static TSource Min<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer) {
            Comparison<TSource> comparison = comparer.Compare;
            return Min(source, comparison);
        }

        public static TSource Min<TSource>(this IEnumerable<TSource> source, Comparison<TSource> comparison) {
            if (source == null) throw new ArgumentNullException("source");

            using (var enumerator = source.GetEnumerator()) {
                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                var min = enumerator.Current;
                while (enumerator.MoveNext()) {
                    if (comparison(enumerator.Current, min) < 0)
                        min = enumerator.Current;
                }
                return min;
            }
        }

        public static TSource Max<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer) {
            Comparison<TSource> comparison = comparer.Compare;
            return Max(source, comparison);
        }

        public static TSource Max<TSource>(this IEnumerable<TSource> source, Comparison<TSource> comparison) {
            if (source == null) throw new ArgumentNullException("source");

            using (var enumerator = source.GetEnumerator()) {
                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                var max = enumerator.Current;
                while (enumerator.MoveNext()) {
                    if (comparison(enumerator.Current, max) >= 0)
                        max = enumerator.Current;
                }
                return max;
            }
        }

        public static Tuple<TSource, TSource> MinMax<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer) {
            Comparison<TSource> comparison = comparer.Compare;
            return MinMax(source, comparison);
        }

        public static Tuple<TSource, TSource> MinMax<TSource>(this IEnumerable<TSource> source, Comparison<TSource> comparison) {
            if (source == null) throw new ArgumentNullException("source");

            using (var enumerator = source.GetEnumerator()) {
                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                var min = enumerator.Current;
                var max = enumerator.Current;
                while (enumerator.MoveNext()) {
                    if (comparison(enumerator.Current, min) < 0)
                        min = enumerator.Current;
                    else if (comparison(enumerator.Current, max) >= 0)
                        max = enumerator.Current;
                }
                return Tuple.Create(min, max);
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
        public static bool None<TSource>(this IEnumerable<TSource> source) {
            return !source.Any();
        }

        /// <summary>
        /// Determines whether no element of a sequence satisfies a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{TSource}"/> whose elements to apply the predicate to.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>true if no element in the source sequence pass the test in the specified predicate, or if the sequence is empty; otherwise, false.</returns>
        public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) {
            return !source.Any(predicate);
        }

        #endregion

        #region Quorum & Elected

        /// <summary>
        /// Determines whether at least 'n' of the sequence satisfy a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of the source.</typeparam>
        /// <param name="source">The elements to apply the predicate to.</param>
        /// <param name="quorum">The expected minimal number of occurences.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>true if the quorum is reached; otherwise, false.</returns>
        public static bool Quorum<TSource>(this IEnumerable<TSource> source, int quorum, Predicate<TSource> predicate) {
            if (quorum < 1)
                throw new ArgumentOutOfRangeException("quorum");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            if (source == null)
                return false;

            using (var enumerator = source.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    if (predicate(enumerator.Current)) {
                        if (quorum == 1)
                            return true;
                        quorum--;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the element with the most occurences.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of the source.</typeparam>
        /// <param name="source">The elements.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns>The element with the most occurences.</returns>
        /// <remarks>In case of ties, the element that appeared first in the sequence is returned.</remarks>
        public static TSource Elected<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> equalityComparer = null)
            where TSource : IEquatable<TSource> {
            using (var enumerator = source.GetEnumerator()) {
                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                var ballot = new Ballot<TSource>(equalityComparer);
                do {
                    ballot.Vote(enumerator.Current);
                } while (enumerator.MoveNext());
                return ballot.First().Key;
            }
        }

        /// <summary>
        /// Returns the element with the most occurences. Elements that are not eligible are discarded.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of the source.</typeparam>
        /// <param name="source">The elements.</param>
        /// <param name="eligible">The predicate to indicate whether an element is eligible or not.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns>The element with the most occurences.</returns>
        /// <remarks>In case of ties, the element that appeared first in the sequence is returned.</remarks>
        public static TSource Elected<TSource>(this IEnumerable<TSource> source, Predicate<TSource> eligible, IEqualityComparer<TSource> equalityComparer = null)
            where TSource : IEquatable<TSource> {
            if (source == null)
                throw new ArgumentNullException("source");
            if (eligible == null)
                throw new ArgumentNullException("eligible");

            using (var enumerator = source.GetEnumerator()) {
                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                var ballot = new Ballot<TSource>(equalityComparer);
                do {
                    if (!eligible(enumerator.Current))
                        continue;
                    ballot.Vote(enumerator.Current);
                } while (enumerator.MoveNext());
                return ballot.First().Key;
            }
        }

        /// <summary>
        /// Returns the element with the most occurences, or a default value if the sequence is empty.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of the source.</typeparam>
        /// <param name="source">The elements.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns>default(TSource) if the source is empty; otherwise the element with the most occurences.</returns>
        /// <remarks>In case of ties, the element that appeared first in the sequence is returned.</remarks>
        public static TSource ElectedOrDefault<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> equalityComparer = null)
            where TSource : IEquatable<TSource> {
            if (source == null)
                return default(TSource);
            using (var enumerator = source.GetEnumerator()) {
                if (!enumerator.MoveNext())
                    return default(TSource);

                var ballot = new Ballot<TSource>(equalityComparer);
                do {
                    ballot.Vote(enumerator.Current);
                } while (enumerator.MoveNext());
                if (!ballot.HasVotes)
                    return default(TSource);
                return ballot.First().Key;
            }
        }

        /// <summary>
        /// Returns the element with the most occurences, or a default value if the sequence contains no eligible elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of the source.</typeparam>
        /// <param name="source">The elements.</param>
        /// <param name="eligible">The predicate to indicate whether an element is eligible or not.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns>default(TSource) if the source has no eligible elements; otherwise the element with the most occurences.</returns>
        /// <remarks>In case of ties, the element that appeared first in the sequence is returned.</remarks>
        public static TSource ElectedOrDefault<TSource>(this IEnumerable<TSource> source, Predicate<TSource> eligible, IEqualityComparer<TSource> equalityComparer = null)
            where TSource : IEquatable<TSource> {
            if (eligible == null)
                throw new ArgumentNullException("eligible");
            if (source == null)
                return default(TSource);

            using (var enumerator = source.GetEnumerator()) {
                if (!enumerator.MoveNext())
                    return default(TSource);

                var ballot = new Ballot<TSource>(equalityComparer);
                do {
                    if (!eligible(enumerator.Current))
                        continue;
                    ballot.Vote(enumerator.Current);
                } while (enumerator.MoveNext());
                if (!ballot.HasVotes)
                    return default(TSource);
                return ballot.First().Key;
            }
        }

        #endregion

        #region Read

        [DebuggerStepThrough]
        public static bool Read<T>(this IEnumerator<T> enumerator, out T value) {
            if (enumerator.MoveNext()) {
                value = enumerator.Current;
                return true;
            }
            value = default(T);
            return false;
        }

        [DebuggerStepThrough]
        public static T Read<T>(this IEnumerator<T> enumerator) {
            if (enumerator.MoveNext()) {
                return enumerator.Current;
            }
            throw new InvalidOperationException();
        }

        [DebuggerStepThrough]
        public static T ReadOrDefault<T>(this IEnumerator<T> enumerator) {
            if (enumerator.MoveNext()) {
                return enumerator.Current;
            }
            return default(T);
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

        #region Stride

        /// <summary>
        /// Returns every Nth element of the source, starting with the first one
        /// </summary>
        /// <typeparam name="T">The type of the element of the source</typeparam>
        /// <param name="source">An IEnumerable&lt;T> to return the element from.</param>
        /// <param name="step">The step</param>
        /// <returns></returns>
        public static IEnumerable<T> Stride<T>(this IEnumerable<T> source, int step) {
            if (source == null)
                throw new ArgumentNullException("source");
            if (step < 1)
                throw new ArgumentOutOfRangeException("step");
            var i = step - 1;
            foreach (var item in source) {
                if (++i == step) {
                    i = 0;
                    yield return item;
                }
            }
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
        public static IEnumerable<TSource> Tail<TSource>(this IEnumerable<TSource> source, int count) {
            if (source == null) throw new ArgumentNullException("source");
            if (count <= 0)
                return System.Linq.Enumerable.Empty<TSource>();
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

        static IEnumerable<TSource> TailIterator1<TSource>(IEnumerable<TSource> source) {
            var last = source.Last();
            yield return last;
        }

        static IEnumerable<TSource> TailIterator<TSource>(IEnumerable<TSource> source, int count) {
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
        public static IEnumerable<TSource> TailUnless<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, int count) {
            if (source == null) throw new ArgumentNullException("source");
            if (count <= 0)
                return System.Linq.Enumerable.Empty<TSource>();
            Func<TSource, bool> unless = x => !predicate(x);
            if (count == 1)
                return TailUnless1(source, unless);
            return TailIterator(source.Where(unless), count);
        }

        static IEnumerable<TSource> TailUnless1<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate) {
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
        public static IEnumerable<TSource> TakeUnless<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, int count) {
            if (source == null) throw new ArgumentNullException("source");
            Func<TSource, bool> unless = x => !predicate(x);
            return source.Where(unless).Take(count);
        }

        #endregion

        #region ToArray

        public static BitArray ToBitArray<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate) {
            var traits = new EnumerableTraits<TSource>(source);
            if (!traits.HasCount)
                return new BitArray(source.Select(_ => predicate(_)).ToArray());

            var result = new bool[traits.Count];
            using (var enumerator = source.GetEnumerator()) {
                int i = 0;
                while (enumerator.MoveNext()) {
                    result[i++] = predicate(enumerator.Current);
                }
            }
            return new BitArray(result);
        }

        private static List<TSource> MakeList<TSource>(this IEnumerable<TSource> source) {
            var collection = source as IReadOnlyCollection<TSource>;
            if (collection != null) {
                var list = new List<TSource>(collection.Count);
                list.AddRange(source);
                return list;
            }
            return new List<TSource>(source);
        }

        public static TOutput[,] ToTwoDimensionalArray<TInput, TOutput>(this IEnumerable<TInput> source, params Converter<TInput, TOutput>[] converters) {
            var list = MakeList(source);

            var columns = converters.Length;
            var rows = list.Count;
            var array = new TOutput[rows, columns];
            for (int i = 0; i < rows; i++) {
                var item = list[i];
                for (int j = 0; j < columns; j++) {
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
            where TSource : new() {
            var array = new TSource[length];
            var i = 0;
            using (var enumerator = source.GetEnumerator()) {
                while (enumerator.MoveNext() && i < length) {
                    array[i++] = enumerator.Current;
                }
            }
            return array;
        }

        public static T[] AsArray<T>(this IEnumerable<T> values) {
            if (values == null)
                return null;

            var array = values as T[];
            if (array != null)
                return array;

            return values.ToArray();
        }

        #endregion

        #region ToEnumerable

        class ShieldEnumerable<TSource> : IEnumerable<TSource>
        {
            readonly IEnumerable<TSource> _source;

            public ShieldEnumerable(IEnumerable<TSource> source) {
                _source = source;
            }

            public IEnumerator<TSource> GetEnumerator() {
                return _source.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return _source.GetEnumerator();
            }
        }

        /// <summary>
        /// Hides a enumerable.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> ToEnumerable<TSource>(this IEnumerable<TSource> source) {
            return new ShieldEnumerable<TSource>(source);
        }

        #endregion

        #region ToDictionary

        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, DuplicatePolicy policy) {
            return ToDictionary(source, keySelector, SpecialFunctions<TSource>.Identity, null, policy);
        }

        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, DuplicatePolicy policy) {
            return ToDictionary(source, keySelector, SpecialFunctions<TSource>.Identity, comparer, policy);
        }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, DuplicatePolicy policy) {
            return ToDictionary(source, keySelector, elementSelector, null, policy);
        }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, DuplicatePolicy policy) {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");
            if (elementSelector == null) throw new ArgumentNullException("elementSelector");

            var d = new Dictionary<TKey, TElement>(comparer);
            switch (policy) {
            case DuplicatePolicy.ThrowException:
                foreach (TSource item in source) {
                    d.Add(keySelector(item), elementSelector(item));
                }
                break;
            case DuplicatePolicy.KeepFirst:
                foreach (TSource item in source) {
                    var selector = keySelector(item);
                    if (!d.ContainsKey(selector))
                        d.Add(selector, elementSelector(item));
                }
                break;
            case DuplicatePolicy.KeepLast:
                foreach (TSource item in source) {
                    d[keySelector(item)] = elementSelector(item);
                }
                break;
            }
            return d;
        }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, Func<TElement, TSource, TElement> merger) {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");
            if (elementSelector == null) throw new ArgumentNullException("elementSelector");
            if (merger == null) throw new ArgumentNullException("merger");

            var d = new Dictionary<TKey, TElement>(comparer);
            foreach (TSource item in source) {
                var selector = keySelector(item);
                TElement element;
                if (!d.TryGetValue(selector, out element)) {
                    d.Add(selector, elementSelector(item));
                } else {
                    d[selector] = merger(element, item);
                }
            }
            return d;
        }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, Func<TElement, TElement, TElement> merger) {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");
            if (elementSelector == null) throw new ArgumentNullException("elementSelector");
            if (merger == null) throw new ArgumentNullException("merger");

            var d = new Dictionary<TKey, TElement>(comparer);
            foreach (TSource item in source) {
                var selector = keySelector(item);
                TElement element;
                if (!d.TryGetValue(selector, out element)) {
                    d.Add(selector, elementSelector(item));
                } else {
                    d[selector] = merger(element, elementSelector(item));
                }
            }
            return d;
        }

        #endregion

        #region ToRanges

        public static IEnumerable<R> ToRanges<T, R>(this IEnumerable<T> values, Func<T, T, bool> isSuccessor, Func<T, T, R> factory) {
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

        public static IEnumerable<Tuple<T, T>> ToRanges<T>(this IEnumerable<T> values, Func<T, T, bool> isSuccessor) {
            return values.ToRanges(isSuccessor, Tuple.Create);
        }

        #endregion

        #region ToString

        static TextInfo GetTextInfo(IFormatProvider formatProvider) {
            return formatProvider.GetFormat<TextInfo>();
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

            var textInfo = GetTextInfo(formatProvider);
            var separator = textInfo.ListSeparator;
            using (var it = enumerable.GetEnumerator()) {
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

        public static string ToString<T>(this IEnumerable<T> enumerable, IFormatProvider formatProvider)
            where T : IFormattable {
            return enumerable.ToString(null, formatProvider);
        }

        #endregion

        #region Traits

        /// <summary>
        /// Check if a sequence of elements is sorted.
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="enumerable">The sequence.</param>
        /// <param name="comparer">The comparer</param>
        /// <returns>Returns true if each element in the sequence is less than or equal to its successors; otherwise, false.</returns>
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

        /// <summary>
        /// Check if a sequence of elements is sorted, using the default comparer.
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="enumerable">The sequence.</param>
        /// <returns>Returns true if each element in the sequence is less than or equal to its successors; otherwise, false.</returns>
        public static bool IsSorted<T>(this IEnumerable<T> enumerable) {
            return enumerable.IsSorted(Comparer<T>.Default);
        }

        /// <summary>
        /// Check if a sequence of elements is sorted.
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="enumerable">The sequence.</param>
        /// <returns>Returns true if each element in the sequence is less than to its successors; otherwise, false.</returns>
        /// <remarks>If true, then the elements are unique in the sequence.</remarks>
        public static bool IsSortedSet<T>(this IEnumerable<T> enumerable, IComparer<T> comparer) {
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
        /// <summary>
        /// Check if a sequence of elements is sorted, using the default comparer.
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="enumerable">The sequence.</param>
        /// <param name="comparer">The comparer</param>
        /// <returns>Returns true if each element in the sequence is less than to its successors; otherwise, false.</returns>
        /// <remarks>If true, then the elements are unique in the sequence.</remarks>
        public static bool IsSortedSet<T>(this IEnumerable<T> enumerable) {
            return enumerable.IsSortedSet(Comparer<T>.Default);
        }

        #endregion

        #region ZipAll methods

        public static IEnumerable<TResult> ZipAll<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector) {
            using (var enumerator1 = first.GetEnumerator())
            using (var enumerator2 = second.GetEnumerator()) {
                while (true) {
                    if (!enumerator1.MoveNext()) {
                        while (enumerator2.MoveNext()) {
                            yield return resultSelector(default(TFirst), enumerator2.Current);
                        }
                        yield break;
                    }
                    if (!enumerator2.MoveNext()) {
                        do {
                            yield return resultSelector(enumerator1.Current, default(TSecond));
                        } while (enumerator1.MoveNext());
                        yield break;
                    }
                    yield return resultSelector(enumerator1.Current, enumerator2.Current);
                }
            }
        }

        #endregion
    }
}

