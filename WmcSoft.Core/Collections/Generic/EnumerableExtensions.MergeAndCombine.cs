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
using System.Collections.Generic;
using System.Linq;

namespace WmcSoft.Collections.Generic
{
    public static partial class EnumerableExtensions
    {
        /// <summary>
        ///  Returns distinct elements from a sorted sequence by using a specified <see cref="IEqualityComparer{T}"/> to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sorted sequence to remove duplicate elements from.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to compare values.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains distinct elements from the source sequence.</returns>
        public static IEnumerable<TSource> SortedDistinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
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
        public static IEnumerable<TSource> SortedDistinct<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            return SortedDistinct(source, new EqualityComparerAdapter<TSource>(comparer));
        }

        /// <summary>
        /// Returns distinct elements from a sorted sequence by using the default equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sorted sequence to remove duplicate elements from.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains distinct elements from the source sequence.</returns>
        public static IEnumerable<TSource> SortedDistinct<TSource>(this IEnumerable<TSource> source)
        {
            return SortedDistinct(source, EqualityComparer<TSource>.Default);
        }

        public static IEnumerable<TGroup> SortedCombine<TSource, TGroup>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer, Func<TSource, TGroup, TGroup> accumulator, Func<TSource, TGroup> factory)
        {
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
            where TGroup : new()
        {
            return SortedCombine(source, comparer, accumulator, t => accumulator(t, new TGroup()));
        }
        public static IEnumerable<TGroup> SortedCombine<T, TGroup>(this IEnumerable<T> source, Func<T, TGroup, TGroup> accumulator)
            where TGroup : new()
        {
            return SortedCombine(source, EqualityComparer<T>.Default, accumulator, t => accumulator(t, new TGroup()));
        }

        /// <summary>
        /// Merges two sorted enumerable in another enumerable sorted using the same comparer. When two items are equivalent, items from first come first.
        /// </summary>
        /// <typeparam name="T">The type of enumerated element</typeparam>
        /// <param name="self">The first enumerable</param>
        /// <param name="other">The second enumerable</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing elements, or <c>null</c> to use the default comparer <see cref="Comparer{T}.Default"/>.</param>
        /// <returns>The sorted enumerable</returns>
        /// <remarks>If the two enumerables are not sorted using the comparer, the result is undefined.</remarks>
        public static IEnumerable<T> Merge<T>(this IEnumerable<T> self, IEnumerable<T> other, IComparer<T> comparer)
        {
            comparer = comparer ?? Comparer<T>.Default;
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
        public static IEnumerable<TSource> Merge<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> other)
        {
            return source.Merge(other, Comparer<TSource>.Default);
        }

        /// <summary>
        /// Combines two sorted enumerable of unique element in another enumerable sorted using the same comparer. When two items are equivalent, they are combined with the combiner.
        /// </summary>
        /// <typeparam name="TSource">The type of enumerated element</typeparam>
        /// <param name="source">The first enumerable</param>
        /// <param name="other">The second enumerable</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing elements, or <c>null</c> to use the default comparer <see cref="Comparer{T}.Default"/>.</param>
        /// <returns>The sorted enumerable</returns>
        /// <remarks>If the two enumerables are not sorted using the comparer, the result is undefined.</remarks>
        public static IEnumerable<TSource> Combine<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> other, IComparer<TSource> comparer, Func<TSource, TSource, TSource> combiner)
        {
            comparer = comparer ?? Comparer<TSource>.Default;
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
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing elements, or <c>null</c> to use the default comparer <see cref="Comparer{T}.Default"/>.</param>
        /// <param name="combiner">The combiner</param>
        /// <param name="defaultValue">The default value for the combiner when one value is missing</param>
        /// <returns>The sorted enumerable</returns>
        /// <remarks>If the two enumerables are not sorted using the comparer, the result is undefined.</remarks>
        public static IEnumerable<TOutput> Combine<TInput, TOutput>(this IEnumerable<TInput> source, IEnumerable<TInput> other, IComparer<TInput> comparer, Func<TInput, TInput, TOutput> combiner, TInput defaultValue = default(TInput))
        {
            comparer = comparer ?? Comparer<TInput>.Default;
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
        public static IEnumerable<T> Interleave<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
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
        public static IEnumerable<TSource> Interleave<TSource>(this IEnumerable<IEnumerable<TSource>> enumerables)
        {
            var list = new List<IEnumerator<TSource>>(enumerables.Select(e => e.GetEnumerator()));
            try {
                var i = 0;
                while (list.Count != 0) {
                    i = i % list.Count;

                    if (list[i].MoveNext()) {
                        yield return list[i++].Current;
                    } else {
                        using (var disposable = list[i]) {
                            list.RemoveAt(i);
                        }
                    }
                }
            }
            finally {
                list.ForEach(i => i.Dispose());
            }
        }
    }
}
