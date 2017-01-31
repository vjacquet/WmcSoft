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
using System.Linq;

namespace WmcSoft.Data
{
    public static class DataViewExtensions
    {
        public static IEnumerable<DataRow> Where(this DataView view, Func<DataRow, bool> predicate) {
            return view.Cast<DataRow>().Where(predicate);
        }

        public static IEnumerable<U> Select<U>(this DataView view, Func<DataRow, U> selector) {
            return view.Cast<DataRow>().Select(selector);
        }
        public static IEnumerable<V> SelectMany<U, V>(this DataView view, Func<DataRow, IEnumerable<U>> selector, Func<DataRow, U, V> resultSelector) {
            return view.Cast<DataRow>().SelectMany(selector, resultSelector);
        }

        public static IEnumerable<V> Join<U, K, V>(this DataView view, IEnumerable<U> inner, Func<DataRow, K> outerKeySelector, Func<U, K> innerKeySelector, Func<DataRow, U, V> resultSelector) {
            return view.Cast<DataRow>().Join(inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static IEnumerable<V> GroupJoin<U, K, V>(this DataView view, IEnumerable<U> inner, Func<DataRow, K> outerKeySelector, Func<U, K> innerKeySelector, Func<DataRow, IEnumerable<U>, V> resultSelector) {
            return view.Cast<DataRow>().GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static IOrderedEnumerable<DataRow> OrderBy<K>(this DataView view, Func<DataRow, K> keySelector) {
            return view.Cast<DataRow>().OrderBy(keySelector);
        }

        public static IOrderedEnumerable<DataRow> OrderByDescending<K>(this DataView view, Func<DataRow, K> keySelector) {
            return view.Cast<DataRow>().OrderByDescending(keySelector);
        }

        public static IEnumerable<IGrouping<K, DataRow>> GroupBy<K>(this DataView view, Func<DataRow, K> keySelector) {
            return view.Cast<DataRow>().GroupBy(keySelector);
        }

        public static IEnumerable<IGrouping<K, E>> GroupBy<K, E>(this DataView view, Func<DataRow, K> keySelector, Func<DataRow, E> elementSelector) {
            return view.Cast<DataRow>().GroupBy(keySelector, elementSelector);
        }

        public static IEnumerable<DataRow> Where(this DataView view, Func<DataRow, int, bool> predicate) {
            return view.Cast<DataRow>().Where(predicate);
        }
    }
}
