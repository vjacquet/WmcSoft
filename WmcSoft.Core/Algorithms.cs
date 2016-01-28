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

namespace WmcSoft
{
    public static class Algorithms
    {
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

        public static int Min(params int[] values) {
            if (values.Length == 0)
                throw new ArgumentOutOfRangeException("values");

            var min = values[0];
            for (int i = 1; i != values.Length; ++i) {
                min = System.Math.Min(min, values[i]);
            }
            return min;
        }
        public static long Min(params long[] values) {
            if (values.Length == 0)
                throw new ArgumentOutOfRangeException("values");

            var min = values[0];
            for (int i = 1; i != values.Length; ++i) {
                min = System.Math.Min(min, values[i]);
            }
            return min;
        }

        public static int Max(params int[] values) {
            if (values.Length == 0)
                throw new ArgumentOutOfRangeException("values");

            var max = values[0];
            for (int i = 1; i != values.Length; ++i) {
                max = System.Math.Max(max, values[i]);
            }
            return max;
        }
        public static long Max(params long[] values) {
            if (values.Length == 0)
                throw new ArgumentOutOfRangeException("values");

            var max = values[0];
            for (int i = 1; i != values.Length; ++i) {
                max = System.Math.Max(max, values[i]);
            }
            return max;
        }

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

        public static T Min<T>(T x, T y) where T : IComparable<T> {
            return y.CompareTo(x) < 0 ? y : x;
        }

        public static T Min<T>(params T[] args) where T : IComparable<T> {
            return args.Min();
        }

        public static T Min<T>(Comparison<T> comparison, T x, T y) {
            return comparison(y, x) < 0 ? y : x;
        }

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

        public static T Min<T>(Relation<T> relation, T x, T y) {
            return relation(y, x) ? y : x;
        }

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

        public static T Min<T>(IComparer<T> comparer, T x, T y) {
            Comparison<T> comparison = comparer.Compare;
            return Min(comparison, x, y);
        }

        public static T Min<T>(IComparer<T> comparer, params T[] args) {
            Comparison<T> comparison = comparer.Compare;
            return Min(comparison, args);
        }

        public static T Max<T>(params T[] args) where T : IComparable<T> {
            return args.Max();
        }

        public static T Max<T>(T x, T y) where T : IComparable<T> {
            return y.CompareTo(x) < 0 ? x : y;
        }

        public static T Max<T>(Comparison<T> comparison, T x, T y) {
            return comparison(y, x) < 0 ? x : y;
        }

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

        public static T Max<T>(Relation<T> relation, T x, T y) {
            return relation(y, x) ? x : y;
        }

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

        public static T Max<T>(IComparer<T> comparer, T x, T y) {
            Comparison<T> comparison = comparer.Compare;
            return Max(comparison, x, y);
        }

        public static T Max<T>(IComparer<T> comparer, params T[] args) {
            Comparison<T> comparison = comparer.Compare;
            return Max(comparison, args);
        }

        public static Tuple<T, T> MinMax<T>(params T[] args) where T : IComparable<T> {
            if (args == null || args.Length == 0)
                throw new ArgumentException("args");
            if (args.Length == 1)
                return Tuple.Create(args[0], args[0]);

            var min = args[0];
            var max = args[0];
            for (int i = 1; i < args.Length; i++) {
                if (args[i].CompareTo(min) < 0)
                    min = args[i];
                else if (args[i].CompareTo(max) >= 0)
                    max = args[i];
            }
            return Tuple.Create(min, max);
        }

        public static Tuple<T, T> MinMax<T>(Comparison<T> comparison, params T[] args) {
            if (args == null || args.Length == 0)
                throw new ArgumentException("args");
            if (args.Length == 1)
                return Tuple.Create(args[0], args[0]);

            var min = args[0];
            var max = args[0];
            for (int i = 1; i < args.Length; i++) {
                if (comparison(args[i], min) < 0)
                    min = args[i];
                else if (comparison(args[i], max) >= 0)
                    max = args[i];
            }
            return Tuple.Create(min, max);
        }

        public static Tuple<T, T> MinMax<T>(IComparer<T> comparer, params T[] args) {
            Comparison<T> comparison = comparer.Compare;
            return MinMax(comparison, args);
        }

        public static Tuple<T, T> MinMax<T>(Relation<T> relation, params T[] args) {
            if (args == null || args.Length == 0)
                throw new ArgumentException("args");
            if (args.Length == 1)
                return Tuple.Create(args[0], args[0]);

            var min = args[0];
            var max = args[0];
            for (int i = 1; i < args.Length; i++) {
                if (relation(args[i], min))
                    min = args[i];
                else if (relation(max, args[i]))
                    max = args[i];
            }
            return Tuple.Create(min, max);
        }

        #endregion
    }
}