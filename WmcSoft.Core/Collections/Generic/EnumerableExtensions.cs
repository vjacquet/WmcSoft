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
using System.Linq;
using System.Collections.Generic;

namespace WmcSoft.Collections.Generic
{
    public static class EnumerableExtensions
    {
        #region ToArray

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

        public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source, int length)
            where TSource : new() {
            var array = new TSource[length];
            var i = 0;
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext() && i < length) {
                array[i++] = enumerator.Current;
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

        #region Occurences

        class Votes : IComparable<Votes>
        {
            public int Rank;
            public int Count;

            #region IComparable<Votes> Membres

            public int CompareTo(Votes other) {
                if (Count == other.Count)
                    return other.Rank - Rank;
                return Count - other.Count;
            }

            #endregion
        }

        #endregion

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
            var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new InvalidOperationException();

            var aggregator = new Dictionary<TSource, Votes>(equalityComparer);
            do {
                Votes votes;
                if (aggregator.TryGetValue(enumerator.Current, out votes)) {
                    votes.Count++;
                } else {
                    aggregator.Add(enumerator.Current, new Votes { Rank = aggregator.Count, Count = 1 });
                }
            } while (enumerator.MoveNext());
            return aggregator.OrderByDescending(p => p.Value).First().Key;
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

            var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new InvalidOperationException();

            var aggregator = new Dictionary<TSource, Votes>(equalityComparer);
            do {
                if (!eligible(enumerator.Current))
                    continue;
                Votes votes;
                if (aggregator.TryGetValue(enumerator.Current, out votes)) {
                    votes.Count++;
                } else {
                    aggregator.Add(enumerator.Current, new Votes { Rank = aggregator.Count, Count = 1 });
                }
            } while (enumerator.MoveNext());
            if (aggregator.Count == 0)
                throw new InvalidOperationException();
            return aggregator.OrderByDescending(p => p.Value).First().Key;
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
            var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
                return default(TSource);

            var aggregator = new Dictionary<TSource, Votes>(equalityComparer);
            do {
                Votes votes;
                if (aggregator.TryGetValue(enumerator.Current, out votes)) {
                    votes.Count++;
                } else {
                    aggregator.Add(enumerator.Current, new Votes { Rank = aggregator.Count, Count = 1 });
                }
            } while (enumerator.MoveNext());
            return aggregator.OrderByDescending(p => p.Value).First().Key;
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
            var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
                return default(TSource);

            var aggregator = new Dictionary<TSource, Votes>(equalityComparer);
            do {
                if (!eligible(enumerator.Current))
                    continue;
                Votes votes;
                if (aggregator.TryGetValue(enumerator.Current, out votes)) {
                    votes.Count++;
                } else {
                    aggregator.Add(enumerator.Current, new Votes { Rank = aggregator.Count, Count = 1 });
                }
            } while (enumerator.MoveNext());
            if (aggregator.Count == 0)
                return default(TSource);
            return aggregator.OrderByDescending(p => p.Value).First().Key;
        }

        #endregion
    }
}

