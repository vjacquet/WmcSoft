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
using System.Collections.Generic;
using System.Numerics;
using WmcSoft.Collections.Generic;

namespace WmcSoft
{
    public static class Generators
    {
        #region Iota

        public static IEnumerable<int> Iota(int value = 0) {
            checked {
                while (true) {
                    yield return value;
                    ++value;
                }
            }
        }

        #endregion

        #region Factorial

        public static IEnumerable<BigInteger> Factorial() {
            yield return BigInteger.One;
            yield return BigInteger.One;

            var i = 2;
            var result = BigInteger.One;
            while (true) {
                result *= i;
                yield return result;
                i++;
            }
        }

        #endregion

        #region Fibonacci

        public static IEnumerable<BigInteger> Fibonacci() {
            BigInteger Fn = 0;
            BigInteger Fn1 = 1;

            yield return Fn;
            yield return Fn1;

            BigInteger Fn2;
            while (true) {
                Fn2 = Fn + Fn1;
                yield return Fn2;
                Fn = Fn1;
                Fn1 = Fn2;
            }
        }

        #endregion

        #region Sequence

        /// <summary>
        /// Generates a sequence.
        /// </summary>
        /// <typeparam name="T">The type of values generated by the sequence.</typeparam>
        /// <param name="func">The function to obtain the next element from the previous one.</param>
        /// <param name="v0">The initial value.</param>
        /// <returns>The sequence as an enumerable of values.</returns>
        public static IEnumerable<T> Sequence<T>(Func<T, T> func, T v0 = default(T)) {
            yield return v0;

            var vn = v0;
            while (true) {
                vn = func(vn);
                yield return vn;
            }
        }
        /// <summary>
        /// Generates a sequence.
        /// </summary>
        /// <typeparam name="T">The type of values generated by the sequence.</typeparam>
        /// <param name="func">The function to obtain the next element from the previous one, with the indice of the element as first parameter.</param>
        /// <param name="v0">The initial value.</param>
        /// <returns>The sequence as an enumerable of values.</returns>
        public static IEnumerable<T> Sequence<T>(Func<int, T, T> func, T v0 = default(T)) {
            yield return v0;

            var vn = v0;
            var n = 0;
            while (true) {
                vn = func(++n, vn);
                yield return vn;
            }
        }

        /// <summary>
        /// Generates a sequence.
        /// </summary>
        /// <typeparam name="T">The type of values generated by the sequence.</typeparam>
        /// <param name="func">The function to obtain the next element from the two previous one.</param>
        /// <param name="v0">The first value.</param>
        /// <param name="v1">The second value.</param>
        /// <returns>The sequence as an enumerable of values.</returns>
        public static IEnumerable<T> Sequence<T>(Func<T, T, T> func, T v0 = default(T), T v1 = default(T)) {
            yield return v0;
            yield return v1;

            var v = new[] { v0, v1 };
            var i = 0;
            while (true) {
                v[i] = func(v[i], v[1 - i]);
                yield return v[i];
                i = 1 - i;
            }
        }

        /// <summary>
        /// Generates a sequence.
        /// </summary>
        /// <typeparam name="T">The type of values generated by the sequence.</typeparam>
        /// <param name="func">The function to obtain the next element from the two previous one, with the indice of the element as first parameter.</param>
        /// <param name="v0">The first value.</param>
        /// <param name="v1">The second value.</param>
        /// <returns>The sequence as an enumerable of values.</returns>
        public static IEnumerable<T> Sequence<T>(Func<int, T, T, T> func, T v0 = default(T), T v1 = default(T)) {
            yield return v0;
            yield return v1;

            var v = new[] { v0, v1 };
            var i = 0;
            var n = 1;
            while (true) {
                v[i] = func(++n, v[i], v[1 - i]);
                yield return v[i];
                i = 1 - i;
            }
        }

        #endregion

        #region Random

        /// <summary>
        /// Returns a nonnegative random number. 
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <returns>A 32-bit signed integer greater than or equal to zero and less than <see cref="Int32.MaxValue"/>.</returns>
        public static IEnumerable<int> Random(Random random) {
            if (random == null)
                throw new ArgumentNullException("random");

            while (true) {
                yield return random.Next();
            }
        }
        /// <summary>
        /// Returns a nonnegative random number, using the default <see cref="Random"/> generator. 
        /// </summary>
        /// <returns>A 32-bit signed integer greater than or equal to zero and less than <see cref="Int32.MaxValue"/>.</returns>
        public static IEnumerable<int> Random() {
            return Random(new Random());
        }

        /// <summary>
        /// Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. <see cref="maxValue"/> must be greater than or equal to zero. </param>
        /// <returns>A 32-bit signed integer greater than or equal to zero and less than <see cref="maxValue"/>; that is, 
        /// the range of return values includes zero but not <see cref="maxValue"/>.
        ///  If <see cref="maxValue"/> equals zero, <see cref="maxValue"/> is returned. </returns>
        public static IEnumerable<int> Random(Random random, int maxValue) {
            if (random == null)
                throw new ArgumentNullException("random");

            while (true) {
                yield return random.Next(maxValue);
            }
        }
        /// <summary>
        /// Returns a nonnegative random number less than the specified maximum, using the default <see cref="Random"/> generator.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. <see cref="maxValue"/> must be greater than or equal to zero. </param>
        /// <returns>A 32-bit signed integer greater than or equal to zero and less than <see cref="maxValue"/>; that is, 
        /// the range of return values includes zero but not <see cref="maxValue"/>.
        ///  If <see cref="maxValue"/> equals zero, <see cref="maxValue"/> is returned. </returns>
        public static IEnumerable<int> Random(int maxValue) {
            return Random(new Random(), maxValue);
        }

        /// <summary>
        /// Yields random numbers within a specified range.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. <see cref="maxValue"/> must be greater than or equal to <see cref="minValue"/>. </param>
        /// <returns>A 32-bit signed integer greater than or equal to <see cref="minValue"/> and less than <see cref="maxValue"/>; that is, 
        /// the range of return values includes <see cref="minValue"/> but not <see cref="maxValue"/>.
        ///  If <see cref="minValue"/> equals <see cref="maxValue"/>, <see cref="minValue"/> is returned. </returns>
        public static IEnumerable<int> Random(Random random, int minValue, int maxValue) {
            if (random == null)
                throw new ArgumentNullException("random");

            while (true) {
                yield return random.Next(minValue, maxValue);
            }
        }
        /// <summary>
        /// Yields random numbers within a specified range, using the default <see cref="Random"/> generator.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. <see cref="maxValue"/> must be greater than or equal to <see cref="minValue"/>. </param>
        /// <returns>A 32-bit signed integer greater than or equal to <see cref="minValue"/> and less than <see cref="maxValue"/>; that is, 
        /// the range of return values includes <see cref="minValue"/> but not <see cref="maxValue"/>.
        ///  If <see cref="minValue"/> equals <see cref="maxValue"/>, <see cref="minValue"/> is returned. </returns>
        public static IEnumerable<int> Random(int minValue, int maxValue) {
            return Random(new Random(), minValue, maxValue);
        }

        /// <summary>
        /// Returns a random number between 0.0 and 1.0. 
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <returns>A double-precision floating point number greater than or equal to 0.0, and less than 1.0.</returns>
        public static IEnumerable<double> RandomDouble(Random random) {
            if (random == null)
                throw new ArgumentNullException("random");

            while (true) {
                yield return random.Next();
            }
        }
        /// <summary>
        /// Returns a random number between 0.0 and 1.0, using the default <see cref="Random"/> generator. 
        /// </summary>
        /// <returns>A double-precision floating point number greater than or equal to 0.0, and less than 1.0.</returns>
        public static IEnumerable<double> RandomDouble() {
            var random = new Random();
            while (true) {
                yield return random.NextDouble();
            }
        }

        #endregion

        #region Permutations

        class SourceReadOnlyList<T> : IReadOnlyList<T>
        {
            readonly T[] _list;
            readonly int[] _indices;

            public SourceReadOnlyList(T[] list, int[] indices) {
                _list = list;
                _indices = indices;
            }

            #region IReadOnlyList<T> Members

            public T this[int index] {
                get { return _list[_indices[index]]; }
            }

            #endregion

            #region IReadOnlyCollection<T> Members

            public int Count {
                get { return _indices.Length; }
            }

            #endregion

            #region IEnumerable<T> Members

            public IEnumerator<T> GetEnumerator() {
                var length = _indices.Length;
                for (int i = 0; i < length; i++) {
                    yield return _list[_indices[i]];
                }
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }

            #endregion
        }

        static bool IsOdd(int i) {
            return (i & 1) == 0;
        }

        public static IEnumerable<IReadOnlyList<T>> Permutations<T>(params T[] values) {
            // Adapted from Knuth, TAoCP, Vol 4A, Page 329, Algorithm G
            var n = values.Length;
            var c = new int[n + 1];//(int[])Array.CreateInstance(typeof(int), new int[] { n }, new int[] { 1 });
            var a = Iota().ToArray(n);
            var list = new SourceReadOnlyList<T>(values, a);
            int k;
            while (true) {
                yield return list;

                k = 1;
                while (c[k] == k) {
                    c[k] = 0;
                    k++;
                    if (k == n)
                        yield break;
                }
                c[k] = c[k] + 1;

                if (IsOdd(k))
                    a.SwapItems(k, 0);
                else
                    a.SwapItems(k, c[k] - 1);
            }
        }

        #endregion
    }
}
