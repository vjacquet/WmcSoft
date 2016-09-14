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
using WmcSoft.Collections.Generic;

namespace WmcSoft
{
    public static class Algorithms
    {
        #region Distances

        /// <summary>
        /// Computes the number of positions at which the chars of the two string of equal length are different.
        /// </summary>
        /// <param name="x">The first string.</param>
        /// <param name="y">The second string.</param>
        /// <returns>The number of positions at which the chars of <paramref name="x"/> and <paramref name="y"/> are different.</returns>
        /// <remarks>The chars are compared bytewise.</remarks>
        /// <exception cref="ArgumentNullException">Either <paramref name="x"/> pr <paramref name="y"/> is null.</exception>
        /// <exception cref="ArgumentException">The two strings have different length.</exception>
        public static int Hamming(string x, string y) {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));
            if (x.Length != y.Length) throw new ArgumentException();

            var length = x.Length;
            var dist = 0;
            for (int i = 0; i < length; i++) {
                if (string.CompareOrdinal(x, i, y, i, 1) != 0)
                    dist++;
            }
            return dist;
        }

        public static int Hamming<TComparer>(string x, string y, TComparer comparer)
            where TComparer : IEqualityComparer<char> {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));
            if (x.Length != y.Length) throw new ArgumentException();
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            using (var ex = x.GetEnumerator())
            using (var ey = y.GetEnumerator()) {
                var dist = 0;
                while (ex.MoveNext() & ey.MoveNext()) {
                    if (!comparer.Equals(ex.Current, ey.Current))
                        dist++;
                }
                return dist;
            }
        }

        public static int Hamming<T>(IReadOnlyCollection<T> x, IReadOnlyCollection<T> y)
            where T : IEquatable<T> {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));
            if (x.Count != y.Count) throw new ArgumentException();

            using (var ex = x.GetEnumerator())
            using (var ey = y.GetEnumerator()) {
                var dist = 0;
                while (ex.MoveNext() & ey.MoveNext()) {
                    if (!ex.Current.Equals(ey.Current))
                        dist++;
                }
                return dist;
            }
        }

        public static int Hamming<T, TComparer>(IReadOnlyCollection<T> x, IReadOnlyCollection<T> y, TComparer comparer)
            where TComparer : IEqualityComparer<T> {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));
            if (x.Count != y.Count) throw new ArgumentException();
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            using (var ex = x.GetEnumerator())
            using (var ey = y.GetEnumerator()) {
                var dist = 0;
                while (ex.MoveNext() & ey.MoveNext()) {
                    if (!comparer.Equals(ex.Current, ey.Current))
                        dist++;
                }
                return dist;
            }
        }

        /// <summary>
        /// Computes the Levenshtein distance of two strings.
        /// </summary>
        /// <param name="s">The first string</param>
        /// <param name="t">The second string</param>
        /// <returns>From <https://en.wikipedia.org/wiki/Levenshtein_distance></returns>
        /// <remarks>To use different weight for insertions, deletions and transposition, check <https://en.wikipedia.org/wiki/Edit_distance>.</remarks>
        public static int Levenshtein(string s, string t) {
            // degenerate cases
            if (s == t)
                return 0;
            if (string.IsNullOrEmpty(s))
                return t == null ? 0 : t.Length;
            if (string.IsNullOrEmpty(t))
                return s.Length;

            var m = s.Length;
            var n = t.Length;
            var d = new int[2, n + 1];
            for (int j = 1; j <= n; j++) {
                d[0, j] = j;
            }

            var r = 0;
            for (int i = 1; i <= m; i++) {
                var pr = r;
                r = (r + 1) % 2;
                d[r, 0] = i;

                var cost = (s[0] == t[0]) ? 0 : 1;
                d[r, 1] = Min(d[pr, 1] + 1, d[r, 0] + 1, d[pr, 0] + cost);
                for (int j = 2; j <= n; j++) {
                    cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                    var min = Min(d[pr, j] + 1, d[r, j - 1] + 1, d[pr, j - 1] + cost);
                    d[r, j] = min;
                }
            }

            return d[r, n];
        }

        /// <summary>
        /// Computes the Damerau-Levenshtein distance of two strings.
        /// </summary>
        /// <param name="s">The first string</param>
        /// <param name="t">The second string</param>
        /// <returns>From <https://en.wikipedia.org/wiki/Damerau%E2%80%93Levenshtein_distance></returns>
        public static int DamerauLevenshtein(string s, string t) {
            // degenerate cases
            if (s == t)
                return 0;
            if (string.IsNullOrEmpty(s))
                return t == null ? 0 : t.Length;
            if (string.IsNullOrEmpty(t))
                return s.Length;

            var m = s.Length;
            var n = t.Length;
            var d = new int[3, n + 1];
            for (int j = 1; j <= n; j++) {
                d[0, j] = j;
            }

            var r = 0;
            for (int i = 1; i <= m; i++) {
                var pr = r;
                r = (r + 1) % 3;
                d[r, 0] = i;

                var cost = (s[0] == t[0]) ? 0 : 1;
                d[r, 1] = Min(d[pr, 1] + 1, d[r, 0] + 1, d[pr, 0] + cost);
                for (int j = 2; j <= n; j++) {
                    cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                    var min = Min(d[pr, j] + 1, d[r, j - 1] + 1, d[pr, j - 1] + cost);
                    if (i > 1 && s[i - 1] == t[j - 2] && s[i - 2] == t[j - 1])
                        min = Min(min, d[(r + 1) % 3, j - 2] + 1);
                    d[r, j] = min;
                }
            }

            return d[r, n];
        }

        /// <summary>
        /// Assigns a new value to an element and returns the old one..
        /// </summary>
        /// <typeparam name="T">The type of element to assign.</typeparam>
        /// <param name="obj">The element.</param>
        /// <param name="value">The new value.</param>
        /// <returns>The old value.</returns>
        public static T Exchange<T>(ref T obj, T value) {
            var t = obj;
            obj = value;
            return t;
        }

        /// <summary>
        /// Swaps two elements.
        /// </summary>
        /// <typeparam name="T">The type of element to swap.</typeparam>
        /// <param name="x">The first parameter.</param>
        /// <param name="y">The second parameter.</param>
        public static void Swap<T>(ref T x, ref T y) {
            var t = x;
            x = y;
            y = t;
        }

        #endregion

        #region Coprimes

        /// <summary>
        /// Returns the coprimes elements.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The coprimes.</returns>
        public static IReadOnlyList<int> Coprimes(params int[] values) {
            var list = new List<int>(values);
            list.Sort();
            CollectionExtensions.RemoveIf(list, (x, index) => list.Take(index).Any(e => x % e == 0));
            return list;
        }

        #endregion

        #region Factorial

        /// <summary>
        /// Compute n!
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static long Factorial(byte n) {
            long result = 1;
            if (n <= 1)
                return 1;
            while (n > 1)
                result *= n--;
            return result;
        }

        #endregion

        #region GreatestCommonDivisor

        /// <summary>
        /// Implementation of the Euclide algorithm to compute the Greatest Common Divisor.
        /// </summary>
        /// <param name="first">First parameter.</param>
        /// <param name="second">Second parameter.</param>
        /// <returns>Returns the Greatest Common Divisor of both parameters</returns>
        public static long GreatestCommonDivisor(long first, long second) {
            long tmp;
            while (first > 0) {
                if (first < second) { // swap the values
                    tmp = first;
                    first = second;
                    second = tmp;
                }
                first = first - second;
            }
            return second;
        }

        #endregion

        ///// <summary>
        ///// /
        ///// </summary>
        ///// <param name="n"></param>
        ///// <returns></returns>
        //public static long Fibonacci(short n) {
        //    long Fn = 0;
        //    long Fn1 = 1;
        //    long Fn2;
        //    while (n-- != 0) {
        //        Fn2 = Fn + Fn1;
        //        Fn = Fn1;
        //        Fn1 = Fn2;
        //    }
        //    return Fn;
        //}

        #region Midpoint

        /// <summary>
        /// Gets the mid point of 2 values without overflowing.
        /// </summary>
        /// <param name="x">The first value</param>
        /// <param name="y">The second value.</param>
        /// <returns>The midpoint value.</returns>
        public static int Midpoint(int x, int y) {
            var delta = y - x;
            if (delta > 0)
                return x + delta / 2;
            return y - delta / 2;
        }

        /// <summary>
        /// Gets the mid point of 2 values without overflowing.
        /// </summary>
        /// <param name="x">The first value</param>
        /// <param name="y">The second value.</param>
        /// <returns>The midpoint value.</returns>
        public static long Midpoint(long x, long y) {
            var delta = y - x;
            if (delta > 0)
                return x + delta / 2;
            return y - delta / 2;
        }

        #endregion

        #region Min/Max

        /// <summary>
        /// Returns the smaller of n 32-bit signed integers, without checking the arguments.
        /// </summary>
        /// <param name="values">The values</param>
        /// <returns>The smaller of the n 32-bit signed integers.</returns>
        public static int UnguardedMin(params int[] values) {
            var min = values[0];
            for (int i = 1; i != values.Length; ++i) {
                min = Math.Min(min, values[i]);
            }
            return min;
        }

        /// <summary>
        /// Returns the smaller of n 64-bit signed integers, without checking the arguments.
        /// </summary>
        /// <param name="values">The values</param>
        /// <returns>The smaller of the n 64-bit signed integers.</returns>
        public static long UnguardedMin(params long[] values) {
            var min = values[0];
            for (int i = 1; i != values.Length; ++i) {
                min = Math.Min(min, values[i]);
            }
            return min;
        }

        /// <summary>
        /// Returns the larger of n 32-bit signed integers, without checking the arguments.
        /// </summary>
        /// <param name="values">The values</param>
        /// <returns>The larger of the n 32-bit signed integers.</returns>
        public static int UnguardedMax(params int[] values) {
            var max = values[0];
            for (int i = 1; i != values.Length; ++i) {
                max = Math.Max(max, values[i]);
            }
            return max;
        }

        /// <summary>
        /// Returns the larger of n 64-bit signed integers, without checking the arguments.
        /// </summary>
        /// <param name="values">The values</param>
        /// <returns>The larger of the n 64-bit signed integers.</returns>
        public static long UnguardedMax(params long[] values) {
            var max = values[0];
            for (int i = 1; i != values.Length; ++i) {
                max = System.Math.Max(max, values[i]);
            }
            return max;
        }

        /// <summary>
        /// Returns the smaller of n 32-bit signed integers.
        /// </summary>
        /// <param name="values">The values</param>
        /// <returns>The smaller of the n 32-bit signed integers.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="values"/> is empty.</exception>
        public static int Min(params int[] values) {
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (values.Length == 0) throw new ArgumentOutOfRangeException(nameof(values));

            return UnguardedMin(values);
        }

        /// <summary>
        /// Returns the smaller of n 64-bit signed integers.
        /// </summary>
        /// <param name="values">The values</param>
        /// <returns>The smaller of the n 64-bit signed integers.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="values"/> is empty.</exception>
        public static long Min(params long[] values) {
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (values.Length == 0) throw new ArgumentOutOfRangeException(nameof(values));

            return UnguardedMin(values);
        }

        /// <summary>
        /// Returns the larger of n 32-bit signed integers.
        /// </summary>
        /// <param name="values">The values</param>
        /// <returns>The larger of the n 32-bit signed integers.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="values"/> is empty.</exception>
        public static int Max(params int[] values) {
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (values.Length == 0) throw new ArgumentOutOfRangeException(nameof(values));

            return UnguardedMax(values);
        }

        /// <summary>
        /// Returns the larger of n 64-bit signed integers.
        /// </summary>
        /// <param name="values">The values</param>
        /// <returns>The larger of the n 64-bit signed integers.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="values"/> is empty.</exception>
        public static long Max(params long[] values) {
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (values.Length == 0) throw new ArgumentOutOfRangeException(nameof(values));

            return UnguardedMax(values);
        }

        /// <summary>
        /// Returns the smaller of two instances of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance</param>
        /// <returns>The smaller of two instances.</returns>
        public static T Min<T>(T x, T y) where T : IComparable<T> {
            return y.CompareTo(x) < 0 ? y : x;
        }

        /// <summary>
        /// Returns the smaller of n instances of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance</param>
        /// <returns>The smaller of n instances.</returns>
        public static T Min<T>(params T[] args) where T : IComparable<T> {
            return args.Min();
        }

        /// <summary>
        /// Returns the smaller of two instances of <typeparamref name="T"/>, using a <see cref="Comparison{T}"/> .
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="comparison">The comparison.</param>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance</param>
        /// <returns>The smaller of two instances.</returns>
        public static T Min<T>(Comparison<T> comparison, T x, T y) {
            return comparison(y, x) < 0 ? y : x;
        }

        /// <summary>
        /// Returns the smaller of n instances of <typeparamref name="T"/>, using a <see cref="Comparison{T}"/> .
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="comparison">The comparison.</param>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance</param>
        /// <returns>The smaller of n instances.</returns>
        public static T Min<T>(Comparison<T> comparison, params T[] args) {
            if (args == null || args.Length == 0)
                throw new ArgumentException("args");

            var min = args[0];
            for (int i = 1; i < args.Length; i++) {
                if (comparison(args[i], min) < 0)
                    min = args[i];
            }
            return min;
        }

        /// <summary>
        /// Returns the smaller of two instances of <typeparamref name="T"/>, using a <see cref="Relation{T}"/> .
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="relation">The relation.</param>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance</param>
        /// <returns>The smaller of two instances.</returns>
        public static T Min<T>(Relation<T> relation, T x, T y) {
            return relation(y, x) ? y : x;
        }

        /// <summary>
        /// Returns the smaller of n instances of <typeparamref name="T"/>, using a <see cref="Relation{T}"/> .
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="relation">The relation.</param>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance</param>
        /// <returns>The smaller of n instances.</returns>
        public static T Min<T>(Relation<T> relation, params T[] args) {
            if (args == null || args.Length == 0)
                throw new ArgumentException("args");

            var min = args[0];
            for (int i = 1; i < args.Length; i++) {
                if (relation(args[i], min))
                    min = args[i];
            }
            return min;
        }

        /// <summary>
        /// Returns the smaller of two instances of <typeparamref name="T"/>, using a <see cref="IComparer{T}"/> .
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="comparer">The comparer.</param>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance</param>
        /// <returns>The smaller of two instances.</returns>
        public static T Min<T>(IComparer<T> comparer, T x, T y) {
            Comparison<T> comparison = comparer.Compare;
            return Min(comparison, x, y);
        }

        /// <summary>
        /// Returns the smaller of n instances of <typeparamref name="T"/>, using a <see cref="IComparer{T}"/> .
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="comparer">The comparer.</param>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance</param>
        /// <returns>The smaller of n instances.</returns>
        public static T Min<T>(IComparer<T> comparer, params T[] args) {
            Comparison<T> comparison = comparer.Compare;
            return Min(comparison, args);
        }

        /// <summary>
        /// Returns the larger of two instances of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance</param>
        /// <returns>The larger of two instances.</returns>
        public static T Max<T>(T x, T y) where T : IComparable<T> {
            return y.CompareTo(x) < 0 ? x : y;
        }

        /// <summary>
        /// Returns the larger of n instances of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance</param>
        /// <returns>The larger of n instances.</returns>
        public static T Max<T>(params T[] args) where T : IComparable<T> {
            return args.Max();
        }

        /// <summary>
        /// Returns the larger of two instances of <typeparamref name="T"/>, using a <see cref="Comparison{T}"/> .
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="comparison">The comparison.</param>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance</param>
        /// <returns>The larger of two instances.</returns>
        public static T Max<T>(Comparison<T> comparison, T x, T y) {
            return comparison(y, x) < 0 ? x : y;
        }

        /// <summary>
        /// Returns the larger of n instances of <typeparamref name="T"/>, using a <see cref="Comparison{T}"/> .
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="comparison">The comparison.</param>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance</param>
        /// <returns>The larger of n instances.</returns>
        public static T Max<T>(Comparison<T> comparison, params T[] args) {
            if (args == null || args.Length == 0)
                throw new ArgumentException("args");

            var max = args[0];
            for (int i = 1; i < args.Length; i++) {
                if (comparison(args[i], max) >= 0)
                    max = args[i];
            }
            return max;
        }

        /// <summary>
        /// Returns the larger of two instances of <typeparamref name="T"/>, using a <see cref="Relation{T}"/> .
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="relation">The relation.</param>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance</param>
        /// <returns>The larger of two instances.</returns>
        public static T Max<T>(Relation<T> relation, T x, T y) {
            return relation(y, x) ? x : y;
        }

        /// <summary>
        /// Returns the larger of n instances of <typeparamref name="T"/>, using a <see cref="Relation{T}"/> .
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="relation">The relation.</param>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance</param>
        /// <returns>The larger of n instances.</returns>
        public static T Max<T>(Relation<T> relation, params T[] args) {
            if (args == null || args.Length == 0)
                throw new ArgumentException("args");

            var max = args[0];
            for (int i = 1; i < args.Length; i++) {
                if (relation(max, args[i]))
                    max = args[i];
            }
            return max;
        }

        /// <summary>
        /// Returns the larger of two instances of <typeparamref name="T"/>, using a <see cref="IComparer{T}"/> .
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="comparer">The comparer.</param>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance</param>
        /// <returns>The larger of two instances.</returns>
        public static T Max<T>(IComparer<T> comparer, T x, T y) {
            Comparison<T> comparison = comparer.Compare;
            return Max(comparison, x, y);
        }

        /// <summary>
        /// Returns the larger of n instances of <typeparamref name="T"/>, using a <see cref="IComparer{T}"/> .
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="comparer">The comparer.</param>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance</param>
        /// <returns>The larger of n instances.</returns>
        public static T Max<T>(IComparer<T> comparer, params T[] args) {
            Comparison<T> comparison = comparer.Compare;
            return Max(comparison, args);
        }

        /// <summary>
        /// Returns the smaller and larger of n instances of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance</param>
        /// <returns>The pair of the smaller and larger of n instances.</returns>
        public static Pair<T> MinMax<T>(params T[] args) where T : IComparable<T> {
            if (args == null || args.Length == 0)
                throw new ArgumentException("args");
            if (args.Length == 1)
                return Pair.Create(args[0], args[0]);

            var min = args[0];
            var max = args[0];
            for (int i = 1; i < args.Length; i++) {
                if (args[i].CompareTo(min) < 0)
                    min = args[i];
                else if (args[i].CompareTo(max) >= 0)
                    max = args[i];
            }
            return Pair.Create(min, max);
        }

        /// <summary>
        /// Returns the smaller and larger of n instances of <typeparamref name="T"/>, using a <see cref="Comparison{T}"/> .
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="comparison">The comparison.</param>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance</param>
        /// <returns>The pair of the smaller and larger of n instances.</returns>
        public static Pair<T> MinMax<T>(Comparison<T> comparison, params T[] args) {
            if (args == null || args.Length == 0)
                throw new ArgumentException("args");
            if (args.Length == 1)
                return Pair.Create(args[0], args[0]);

            var min = args[0];
            var max = args[0];
            for (int i = 1; i < args.Length; i++) {
                if (comparison(args[i], min) < 0)
                    min = args[i];
                else if (comparison(args[i], max) >= 0)
                    max = args[i];
            }
            return Pair.Create(min, max);
        }

        /// <summary>
        /// Returns the smaller and larger of n instances of <typeparamref name="T"/>, using a <see cref="Relation{T}"/> .
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="relation">The relation.</param>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance</param>
        /// <returns>The pair of the smaller and larger of n instances.</returns>
        public static Pair<T> MinMax<T>(Relation<T> relation, params T[] args) {
            if (args == null || args.Length == 0)
                throw new ArgumentException("args");
            if (args.Length == 1)
                return Pair.Create(args[0], args[0]);

            var min = args[0];
            var max = args[0];
            for (int i = 1; i < args.Length; i++) {
                if (relation(args[i], min))
                    min = args[i];
                else if (relation(max, args[i]))
                    max = args[i];
            }
            return Pair.Create(min, max);
        }

        /// <summary>
        /// Returns the smaller and larger of n instances of <typeparamref name="T"/>, using a <see cref="IComparer{T}"/> .
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="comparer">The comparer.</param>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance</param>
        /// <returns>The pair of the smaller and larger of n instances.</returns>
        public static Pair<T> MinMax<T>(IComparer<T> comparer, params T[] args) {
            Comparison<T> comparison = comparer.Compare;
            return MinMax(comparison, args);
        }

        #endregion

        #region Mismatch

        /// <summary>
        /// Returns items from the primary enumerator only when the relation with the secondary enumerator returns false.
        /// </summary>
        /// <typeparam name="T">The primary type.</typeparam>
        /// <typeparam name="U">The secondary type</typeparam>
        /// <param name="primary">The primary enumerable.</param>
        /// <param name="secondary">The secondary enumerable.</param>
        /// <param name="relation">The relation</param>
        /// <returns>Items from the primary enumerator for which the relation with the secondary enumerator returns false.</returns>
        /// <remarks>The enumeration stops when any enumerator cannot move to the next item.</remarks>
        public static IEnumerable<T> Mismatch<T, U>(IEnumerable<T> primary, IEnumerable<U> secondary, Func<T, U, bool> relation) {
            using (var p = primary.GetEnumerator())
            using (var s = secondary.GetEnumerator()) {
                while (p.MoveNext() & s.MoveNext()) {
                    if (!relation(p.Current, s.Current))
                        yield return p.Current;
                }
            }
        }

        #endregion
    }
}