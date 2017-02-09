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

using static WmcSoft.Algorithms;

namespace WmcSoft.Collections.Generic
{
    public static partial class EnumerableExtensions
    {
        /// <summary>
        /// Determines whether at least 'n' of the sequence satisfy a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of the source.</typeparam>
        /// <param name="source">The elements to apply the predicate to.</param>
        /// <param name="quorum">The expected minimal number of occurences.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>true if the quorum is reached; otherwise, false.</returns>
        public static bool Quorum<TSource>(this IEnumerable<TSource> source, int quorum, Predicate<TSource> predicate) {
            if (quorum < 1) throw new ArgumentOutOfRangeException(nameof(quorum));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

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
                return ballot.GetWinner();
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
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (eligible == null) throw new ArgumentNullException(nameof(eligible));

            using (var enumerator = source.GetEnumerator()) {
                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                var ballot = new Ballot<TSource>(equalityComparer);
                do {
                    if (!eligible(enumerator.Current))
                        continue;
                    ballot.Vote(enumerator.Current);
                } while (enumerator.MoveNext());
                return ballot.GetWinner();
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
                return ballot.GetWinner();
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
            if (eligible == null) throw new ArgumentNullException(nameof(eligible));
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
                return ballot.GetWinner();
            }
        }
    }
}