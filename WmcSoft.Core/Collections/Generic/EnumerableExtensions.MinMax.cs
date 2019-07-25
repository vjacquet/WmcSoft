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
        #region Max

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the element that have maximum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="comparer">A transform function to apply to each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence is empty.</exception>
        /// <returns>The maximum value in the sequence.</returns>
        public static TSource Max<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                return source.Max();

            return UnguardedMax(source, comparer.Compare);
        }

        public static TSource Max<TSource>(this IEnumerable<TSource> source, Comparison<TSource> comparison)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (comparison == null)
                return source.Max();

            return UnguardedMax(source, comparison);
        }

        static TSource UnguardedMax<TSource>(this IEnumerable<TSource> source, Comparison<TSource> comparison)
        {
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

        #endregion

        #region MaxBy

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the element that have maximum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence is empty.</exception>
        /// <returns>The element in the sequence for which <paramref name="selector"/> returned the maximum value.</returns>
        public static TSource MaxBy<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var element = enumerator.Current;
                    var value = selector(element);
                    while (enumerator.MoveNext()) {
                        var e = enumerator.Current;
                        var x = selector(e);
                        if (x >= value) {
                            element = e;
                            value = x;
                        }
                    }
                    return element;
                }
            }

            return Enumerable.Empty<TSource>().Single(); // forces throw of exception.
        }

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the element that have maximum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence is empty.</exception>
        /// <returns>The element in the sequence for which <paramref name="selector"/> returned the maximum value.</returns>
        public static TSource MaxBy<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var element = enumerator.Current;
                    var value = selector(element);
                    while (value == null && enumerator.MoveNext()) {
                        element = enumerator.Current;
                        value = selector(element);
                    }

                    while (enumerator.MoveNext()) {
                        var e = enumerator.Current;
                        var x = selector(e);
                        if (x.HasValue && x.GetValueOrDefault() >= value) {
                            element = e;
                            value = x;
                        }
                    }
                    return element;
                }
            }

            return Enumerable.Empty<TSource>().Single(); // forces throw of exception.
        }

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the element that have maximum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence is empty.</exception>
        /// <returns>The element in the sequence for which <paramref name="selector"/> returned the maximum value.</returns>
        public static TSource MaxBy<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var element = enumerator.Current;
                    var value = selector(element);
                    while (enumerator.MoveNext()) {
                        var e = enumerator.Current;
                        var x = selector(e);
                        if (x >= value) {
                            element = e;
                            value = x;
                        }
                    }
                    return element;
                }
            }

            return Enumerable.Empty<TSource>().Single(); // forces throw of exception.
        }

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the element that have maximum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence is empty.</exception>
        /// <returns>The element in the sequence for which <paramref name="selector"/> returned the maximum value.</returns>
        public static TSource MaxBy<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var element = enumerator.Current;
                    var value = selector(element);
                    while (value == null && enumerator.MoveNext()) {
                        element = enumerator.Current;
                        value = selector(element);
                    }

                    while (enumerator.MoveNext()) {
                        var e = enumerator.Current;
                        var x = selector(e);
                        if (x.HasValue && x.GetValueOrDefault() >= value) {
                            element = e;
                            value = x;
                        }
                    }
                    return element;
                }
            }

            return Enumerable.Empty<TSource>().Single(); // forces throw of exception.
        }

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the element that have maximum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence is empty.</exception>
        /// <returns>The element in the sequence for which <paramref name="selector"/> returned the maximum value.</returns>
        public static TSource MaxBy<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var element = enumerator.Current;
                    var value = selector(element);
                    while (double.IsNaN(value) && enumerator.MoveNext()) {
                        element = enumerator.Current;
                        value = selector(element);
                    }

                    while (enumerator.MoveNext()) {
                        var e = enumerator.Current;
                        var x = selector(e);
                        if (!double.IsNaN(x) && x >= value) {
                            element = e;
                            value = x;
                        }
                    }
                    return element;
                }
            }

            return Enumerable.Empty<TSource>().Single(); // forces throw of exception.
        }

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the element that have maximum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence is empty.</exception>
        /// <returns>The element in the sequence for which <paramref name="selector"/> returned the maximum value.</returns>
        public static TSource MaxBy<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var element = enumerator.Current;
                    var value = selector(element);
                    while ((!value.HasValue || double.IsNaN(value.GetValueOrDefault())) && enumerator.MoveNext()) {
                        element = enumerator.Current;
                        value = selector(element);
                    }

                    while (enumerator.MoveNext()) {
                        var e = enumerator.Current;
                        var x = selector(e);
                        if (x.HasValue && !double.IsNaN(x.GetValueOrDefault()) && x.GetValueOrDefault() >= value) {
                            element = e;
                            value = x;
                        }
                    }
                    return element;
                }
            }

            return Enumerable.Empty<TSource>().Single(); // forces throw of exception.
        }

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the element that have maximum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence is empty.</exception>
        /// <returns>The element in the sequence for which <paramref name="selector"/> returned the maximum value.</returns>
        public static TSource MaxBy<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var element = enumerator.Current;
                    var value = selector(element);
                    while (float.IsNaN(value) && enumerator.MoveNext()) {
                        element = enumerator.Current;
                        value = selector(element);
                    }

                    while (enumerator.MoveNext()) {
                        var e = enumerator.Current;
                        var x = selector(e);
                        if (!float.IsNaN(x) && x >= value) {
                            element = e;
                            value = x;
                        }
                    }
                    return element;
                }
            }

            return Enumerable.Empty<TSource>().Single(); // forces throw of exception.
        }

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the element that have maximum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence is empty.</exception>
        /// <returns>The element in the sequence for which <paramref name="selector"/> returned the maximum value.</returns>
        public static TSource MaxBy<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var element = enumerator.Current;
                    var value = selector(element);
                    while ((!value.HasValue || float.IsNaN(value.GetValueOrDefault())) && enumerator.MoveNext()) {
                        element = enumerator.Current;
                        value = selector(element);
                    }

                    while (enumerator.MoveNext()) {
                        var e = enumerator.Current;
                        var x = selector(e);
                        if (x.HasValue && !float.IsNaN(x.GetValueOrDefault()) && x.GetValueOrDefault() >= value) {
                            element = e;
                            value = x;
                        }
                    }
                    return element;
                }
            }

            return Enumerable.Empty<TSource>().Single(); // forces throw of exception.
        }

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the element that have maximum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence is empty.</exception>
        /// <returns>The element in the sequence for which <paramref name="selector"/> returned the maximum value.</returns>
        public static TSource MaxBy<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var element = enumerator.Current;
                    var value = selector(element);
                    while (enumerator.MoveNext()) {
                        var e = enumerator.Current;
                        var x = selector(e);
                        if (x >= value) {
                            element = e;
                            value = x;
                        }
                    }
                    return element;
                }
            }

            return Enumerable.Empty<TSource>().Single(); // forces throw of exception.
        }

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the element that have maximum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence is empty.</exception>
        /// <returns>The element in the sequence for which <paramref name="selector"/> returned the maximum value.</returns>
        public static TSource MaxBy<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var element = enumerator.Current;
                    var value = selector(element);
                    while (value == null && enumerator.MoveNext()) {
                        element = enumerator.Current;
                        value = selector(element);
                    }

                    while (enumerator.MoveNext()) {
                        var e = enumerator.Current;
                        var x = selector(e);
                        if (x.HasValue && x.GetValueOrDefault() >= value) {
                            element = e;
                            value = x;
                        }
                    }
                    return element;
                }
            }

            return Enumerable.Empty<TSource>().Single(); // forces throw of exception.
        }

        #endregion

        #region Min

        public static TSource Min<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                return source.Min();

            return UnguardedMin(source, comparer.Compare);
        }

        public static TSource Min<TSource>(this IEnumerable<TSource> source, Comparison<TSource> comparison)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (comparison == null)
                return source.Max();

            return UnguardedMin(source, comparison);
        }

        static TSource UnguardedMin<TSource>(this IEnumerable<TSource> source, Comparison<TSource> comparison)
        {
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

        #endregion

        #region MinBy

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the element that have minimum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence is empty.</exception>
        /// <returns>The element in the sequence for which <paramref name="selector"/> returned the minimum value.</returns>
        public static TSource MinBy<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var element = enumerator.Current;
                    var value = selector(element);
                    while (enumerator.MoveNext()) {
                        var e = enumerator.Current;
                        var x = selector(e);
                        if (x < value) {
                            element = e;
                            value = x;
                        }
                    }
                    return element;
                }
            }

            return Enumerable.Empty<TSource>().Single(); // forces throw of exception.
        }

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the element that have minimum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence is empty.</exception>
        /// <returns>The element in the sequence for which <paramref name="selector"/> returned the minimum value.</returns>
        public static TSource MinBy<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var element = enumerator.Current;
                    var value = selector(element);
                    while (value == null && enumerator.MoveNext()) {
                        element = enumerator.Current;
                        value = selector(element);
                    }

                    while (enumerator.MoveNext()) {
                        var e = enumerator.Current;
                        var x = selector(e);
                        if (x.HasValue && x.GetValueOrDefault() < value) {
                            element = e;
                            value = x;
                        }
                    }
                    return element;
                }
            }

            return Enumerable.Empty<TSource>().Single(); // forces throw of exception.
        }

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the element that have minimum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence is empty.</exception>
        /// <returns>The element in the sequence for which <paramref name="selector"/> returned the minimum value.</returns>
        public static TSource MinBy<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var element = enumerator.Current;
                    var value = selector(element);
                    while (enumerator.MoveNext()) {
                        var e = enumerator.Current;
                        var x = selector(e);
                        if (x < value) {
                            element = e;
                            value = x;
                        }
                    }
                    return element;
                }
            }

            return Enumerable.Empty<TSource>().Single(); // forces throw of exception.
        }

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the element that have minimum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence is empty.</exception>
        /// <returns>The element in the sequence for which <paramref name="selector"/> returned the minimum value.</returns>
        public static TSource MinBy<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var element = enumerator.Current;
                    var value = selector(element);
                    while (value == null && enumerator.MoveNext()) {
                        element = enumerator.Current;
                        value = selector(element);
                    }

                    while (enumerator.MoveNext()) {
                        var e = enumerator.Current;
                        var x = selector(e);
                        if (x.HasValue && x.GetValueOrDefault() < value) {
                            element = e;
                            value = x;
                        }
                    }
                    return element;
                }
            }

            return Enumerable.Empty<TSource>().Single(); // forces throw of exception.
        }

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the element that have minimum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence is empty.</exception>
        /// <returns>The element in the sequence for which <paramref name="selector"/> returned the minimum value.</returns>
        public static TSource MinBy<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var element = enumerator.Current;
                    var value = selector(element);
                    while (double.IsNaN(value) && enumerator.MoveNext()) {
                        element = enumerator.Current;
                        value = selector(element);
                    }

                    while (enumerator.MoveNext()) {
                        var e = enumerator.Current;
                        var x = selector(e);
                        if (!double.IsNaN(x) && x < value) {
                            element = e;
                            value = x;
                        }
                    }
                    return element;
                }
            }

            return Enumerable.Empty<TSource>().Single(); // forces throw of exception.
        }

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the element that have minimum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence is empty.</exception>
        /// <returns>The element in the sequence for which <paramref name="selector"/> returned the minimum value.</returns>
        public static TSource MinBy<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var element = enumerator.Current;
                    var value = selector(element);
                    while ((!value.HasValue || double.IsNaN(value.GetValueOrDefault())) && enumerator.MoveNext()) {
                        element = enumerator.Current;
                        value = selector(element);
                    }

                    while (enumerator.MoveNext()) {
                        var e = enumerator.Current;
                        var x = selector(e);
                        if (x.HasValue && !double.IsNaN(x.GetValueOrDefault()) && x.GetValueOrDefault() < value) {
                            element = e;
                            value = x;
                        }
                    }
                    return element;
                }
            }

            return Enumerable.Empty<TSource>().Single(); // forces throw of exception.
        }

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the element that have minimum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence is empty.</exception>
        /// <returns>The element in the sequence for which <paramref name="selector"/> returned the minimum value.</returns>
        public static TSource MinBy<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var element = enumerator.Current;
                    var value = selector(element);
                    while (float.IsNaN(value) && enumerator.MoveNext()) {
                        element = enumerator.Current;
                        value = selector(element);
                    }

                    while (enumerator.MoveNext()) {
                        var e = enumerator.Current;
                        var x = selector(e);
                        if (!float.IsNaN(x) && x < value) {
                            element = e;
                            value = x;
                        }
                    }
                    return element;
                }
            }

            return Enumerable.Empty<TSource>().Single(); // forces throw of exception.
        }

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the element that have minimum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence is empty.</exception>
        /// <returns>The element in the sequence for which <paramref name="selector"/> returned the minimum value.</returns>
        public static TSource MinBy<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var element = enumerator.Current;
                    var value = selector(element);
                    while ((!value.HasValue || float.IsNaN(value.GetValueOrDefault())) && enumerator.MoveNext()) {
                        element = enumerator.Current;
                        value = selector(element);
                    }

                    while (enumerator.MoveNext()) {
                        var e = enumerator.Current;
                        var x = selector(e);
                        if (x.HasValue && !float.IsNaN(x.GetValueOrDefault()) && x.GetValueOrDefault() < value) {
                            element = e;
                            value = x;
                        }
                    }
                    return element;
                }
            }

            return Enumerable.Empty<TSource>().Single(); // forces throw of exception.
        }

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the element that have minimum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence is empty.</exception>
        /// <returns>The element in the sequence for which <paramref name="selector"/> returned the minimum value.</returns>
        public static TSource MinBy<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var element = enumerator.Current;
                    var value = selector(element);
                    while (enumerator.MoveNext()) {
                        var e = enumerator.Current;
                        var x = selector(e);
                        if (x < value) {
                            element = e;
                            value = x;
                        }
                    }
                    return element;
                }
            }

            return Enumerable.Empty<TSource>().Single(); // forces throw of exception.
        }

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the element that have minimum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence is empty.</exception>
        /// <returns>The element in the sequence for which <paramref name="selector"/> returned the minimum value.</returns>
        public static TSource MinBy<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var enumerator = source.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var element = enumerator.Current;
                    var value = selector(element);
                    while (value == null && enumerator.MoveNext()) {
                        element = enumerator.Current;
                        value = selector(element);
                    }

                    while (enumerator.MoveNext()) {
                        var e = enumerator.Current;
                        var x = selector(e);
                        if (x.HasValue && x.GetValueOrDefault() < value) {
                            element = e;
                            value = x;
                        }
                    }
                    return element;
                }
            }

            return Enumerable.Empty<TSource>().Single(); // forces throw of exception.
        }

        #endregion

        #region MinMax

        static (T? min, T? max) ValueTypeMinMax<T>(this IEnumerable<T?> source)
            where T : struct, IComparable<T>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                var hasData = false;
                while ((hasData = enumerator.MoveNext()) && !enumerator.Current.HasValue)
                    ;
                if (!hasData)
                    return (default(T?), default(T?));

                var min = enumerator.Current.GetValueOrDefault();
                var max = min;
                while (enumerator.MoveNext()) {
                    if (!enumerator.Current.HasValue)
                        continue;
                    if (enumerator.Current.GetValueOrDefault().CompareTo(min) < 0)
                        min = enumerator.Current.GetValueOrDefault();
                    else if (max.CompareTo(enumerator.Current.GetValueOrDefault()) <= 0) // Ensure stability
                        max = enumerator.Current.GetValueOrDefault();
                }
                return (min, max);
            }
        }

        static (T min, T max) ValueTypeMinMax<T>(this IEnumerable<T> source)
            where T : struct, IComparable<T>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                var min = enumerator.Current;
                var max = min;
                while (enumerator.MoveNext()) {
                    if (enumerator.Current.CompareTo(min) < 0)
                        min = enumerator.Current;
                    else if (max.CompareTo(enumerator.Current) <= 0) // Ensure stability
                        max = enumerator.Current;
                }
                return (min, max);
            }
        }

        public static (double? min, double? max) MinMax(this IEnumerable<double?> source)
        {
            return ValueTypeMinMax(source);
        }

        public static (double min, double max) MinMax(this IEnumerable<double> source)
        {
            return ValueTypeMinMax(source);
        }

        public static (long? min, long? max) MinMax(this IEnumerable<long?> source)
        {
            return ValueTypeMinMax(source);
        }

        public static (long min, long max) MinMax(this IEnumerable<long> source)
        {
            return ValueTypeMinMax(source);
        }

        public static (int? min, int? max) MinMax(this IEnumerable<int?> source)
        {
            return ValueTypeMinMax(source);
        }

        public static (int min, int max) MinMax(this IEnumerable<int> source)
        {
            return ValueTypeMinMax(source);
        }

        public static (float? min, float? max) MinMax(this IEnumerable<float?> source)
        {
            return ValueTypeMinMax(source);
        }

        public static (float min, float max) MinMax(this IEnumerable<float> source)
        {
            return ValueTypeMinMax(source);
        }

        public static (decimal? min, decimal? max) MinMax(this IEnumerable<decimal?> source)
        {
            return ValueTypeMinMax(source);
        }

        public static (decimal min, decimal max) MinMax(this IEnumerable<decimal> source)
        {
            return ValueTypeMinMax(source);
        }

        public static (TSource min, TSource max) MinMax<TSource>(this IEnumerable<TSource> source)
        {
            return MinMax(source, Comparer<TSource>.Default);
        }

        public static (int min, int max) MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            return source.Select(selector).MinMax();
        }
        public static (int? min, int? max) MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            return source.Select(selector).MinMax();
        }

        public static (long min, long max) MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            return source.Select(selector).MinMax();
        }
        public static (long? min, long? max) MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            return source.Select(selector).MinMax();
        }

        public static (float min, float max) MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            return source.Select(selector).MinMax();
        }
        public static (float? min, float? max) MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            return source.Select(selector).MinMax();
        }

        public static (double min, double max) MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            return source.Select(selector).MinMax();
        }
        public static (double? min, double? max) MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            return source.Select(selector).MinMax();
        }

        public static (decimal min, decimal max) MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            return source.Select(selector).MinMax();
        }
        public static (decimal? min, decimal? max) MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            return source.Select(selector).MinMax();
        }

        public static (TResult min, TResult max) MinMax<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return source.Select(selector).MinMax();
        }

        public static (TSource min, TSource max) MinMax<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            Comparison<TSource> comparison = comparer.Compare;
            return MinMax(source, comparison);
        }

        static (TSource min, TSource max) UnguardedMinMaxNeverNull<TSource>(IEnumerable<TSource> source, Comparison<TSource> comparison)
        {
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
                return (min, max);
            }
        }

        static (TSource min, TSource max) UnguardedMinMaxMaybeNull<TSource>(IEnumerable<TSource> source, Comparison<TSource> comparison)
        {
            using (var enumerator = source.GetEnumerator()) {
                var hasData = false;
                while ((hasData = enumerator.MoveNext()) && enumerator.Current == null)
                    ;
                if (!hasData)
                    return (default(TSource), default(TSource));

                var min = enumerator.Current;
                var max = enumerator.Current;
                while (enumerator.MoveNext()) {
                    if (enumerator.Current == null)
                        continue;
                    if (comparison(enumerator.Current, min) < 0)
                        min = enumerator.Current;
                    else if (comparison(enumerator.Current, max) >= 0)
                        max = enumerator.Current;
                }
                return (min, max);
            }
        }

        public static (TSource min, TSource max) MinMax<TSource>(this IEnumerable<TSource> source, Comparison<TSource> comparison)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (default(TSource) == null)
                return UnguardedMinMaxMaybeNull(source, comparison);
            return UnguardedMinMaxNeverNull(source, comparison);
        }

        #endregion
    }
}
