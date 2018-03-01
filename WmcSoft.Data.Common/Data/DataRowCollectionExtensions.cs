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
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace WmcSoft.Data
{
    public static class DataRowCollectionExtensions
    {
        public static IEnumerable<DataRow> Where(this DataRowCollection collection, Func<DataRow, bool> predicate)
        {
            Debug.Assert(collection != null);
            return collection.Cast<DataRow>().Where(predicate);
        }

        public static IEnumerable<DataRow> Where(this DataRowCollection collection, Func<DataRow, int, bool> predicate)
        {
            Debug.Assert(collection != null);
            return collection.Cast<DataRow>().Where(predicate);
        }

        public static IEnumerable<U> Select<U>(this DataRowCollection collection, Func<DataRow, U> selector)
        {
            Debug.Assert(collection != null);
            return collection.Cast<DataRow>().Select(selector);
        }
        public static IEnumerable<V> SelectMany<U, V>(this DataRowCollection collection, Func<DataRow, IEnumerable<U>> selector, Func<DataRow, U, V> resultSelector)
        {
            Debug.Assert(collection != null);
            return collection.Cast<DataRow>().SelectMany(selector, resultSelector);
        }

        public static IEnumerable<V> Join<U, K, V>(this DataRowCollection collection, IEnumerable<U> inner, Func<DataRow, K> outerKeySelector, Func<U, K> innerKeySelector, Func<DataRow, U, V> resultSelector)
        {
            Debug.Assert(collection != null);
            return collection.Cast<DataRow>().Join(inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static IEnumerable<V> GroupJoin<U, K, V>(this DataRowCollection collection, IEnumerable<U> inner, Func<DataRow, K> outerKeySelector, Func<U, K> innerKeySelector, Func<DataRow, IEnumerable<U>, V> resultSelector)
        {
            Debug.Assert(collection != null);
            return collection.Cast<DataRow>().GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static IOrderedEnumerable<DataRow> OrderBy<K>(this DataRowCollection collection, Func<DataRow, K> keySelector)
        {
            Debug.Assert(collection != null);
            return collection.Cast<DataRow>().OrderBy(keySelector);
        }

        public static IOrderedEnumerable<DataRow> OrderByDescending<K>(this DataRowCollection collection, Func<DataRow, K> keySelector)
        {
            Debug.Assert(collection != null);
            return collection.Cast<DataRow>().OrderByDescending(keySelector);
        }

        public static IEnumerable<IGrouping<K, DataRow>> GroupBy<K>(this DataRowCollection collection, Func<DataRow, K> keySelector)
        {
            Debug.Assert(collection != null);
            return collection.Cast<DataRow>().GroupBy(keySelector);
        }

        public static IEnumerable<IGrouping<K, E>> GroupBy<K, E>(this DataRowCollection collection, Func<DataRow, K> keySelector, Func<DataRow, E> elementSelector)
        {
            Debug.Assert(collection != null);
            return collection.Cast<DataRow>().GroupBy(keySelector, elementSelector);
        }
    }
}
