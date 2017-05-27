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
using System.Linq.Expressions;

namespace WmcSoft.Linq
{
    public static class QueryableExtensions
    {
        #region Conversions

        public static IList<TResult> ToList<TInput, TResult>(this IQueryable<TInput> query, Converter<TInput, TResult> convert)
        {
            var list = new List<TResult>();
            foreach (var record in query) {
                list.Add(convert(record));
            }
            return list;
        }

        public static TResult[] ToArray<TInput, TResult>(this IQueryable<TInput> query, Converter<TInput, TResult> convert)
        {
            return query.ToList(convert).ToArray();
        }

        #endregion

        #region None

        /// <summary>
        /// Determines whether a sequence contains no element.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The <see cref="IQueryable{TSource}"/> to check for emptiness.</param>
        /// <returns>true if the source sequence is empty; otherwise, false.</returns>
        public static bool None<TSource>(this IQueryable<TSource> source)
        {
            return !Queryable.Any(source);
        }

        /// <summary>
        /// Determines whether no element of a sequence satisfies a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">An <see cref="IQueryable{TSource}"/> whose elements to apply the predicate to.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>true if no element in the source sequence pass the test in the specified predicate, or if the sequence is empty; otherwise, false.</returns>
        public static bool None<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return !Queryable.Any(source, predicate);
        }

        #endregion
    }
}
