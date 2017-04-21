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
        public static TSource Min<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            Comparison<TSource> comparison = comparer.Compare;
            return Min(source, comparison);
        }

        public static TSource Min<TSource>(this IEnumerable<TSource> source, Comparison<TSource> comparison)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

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

        public static TSource Max<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            Comparison<TSource> comparison = comparer.Compare;
            return Max(source, comparison);
        }

        public static TSource Max<TSource>(this IEnumerable<TSource> source, Comparison<TSource> comparison)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

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

        static Tuple<T?, T?> ValueTypeMinMax<T>(this IEnumerable<T?> source)
            where T : struct, IComparable<T>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                var hasData = false;
                while ((hasData = enumerator.MoveNext()) && !enumerator.Current.HasValue)
                    ;
                if (!hasData)
                    return Tuple.Create(default(T?), default(T?));

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
                return new Tuple<T?, T?>(min, max);
            }
        }

        static Tuple<T, T> ValueTypeMinMax<T>(this IEnumerable<T> source)
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
                return Tuple.Create(min, max);
            }
        }

        public static Tuple<double?, double?> MinMax(this IEnumerable<double?> source)
        {
            return ValueTypeMinMax(source);
        }

        public static Tuple<double, double> MinMax(this IEnumerable<double> source)
        {
            return ValueTypeMinMax(source);
        }

        public static Tuple<long?, long?> MinMax(this IEnumerable<long?> source)
        {
            return ValueTypeMinMax(source);
        }

        public static Tuple<long, long> MinMax(this IEnumerable<long> source)
        {
            return ValueTypeMinMax(source);
        }

        public static Tuple<int?, int?> MinMax(this IEnumerable<int?> source)
        {
            return ValueTypeMinMax(source);
        }

        public static Tuple<int, int> MinMax(this IEnumerable<int> source)
        {
            return ValueTypeMinMax(source);
        }

        public static Tuple<float?, float?> MinMax(this IEnumerable<float?> source)
        {
            return ValueTypeMinMax(source);
        }

        public static Tuple<float, float> MinMax(this IEnumerable<float> source)
        {
            return ValueTypeMinMax(source);
        }

        public static Tuple<decimal?, decimal?> MinMax(this IEnumerable<decimal?> source)
        {
            return ValueTypeMinMax(source);
        }

        public static Tuple<decimal, decimal> MinMax(this IEnumerable<decimal> source)
        {
            return ValueTypeMinMax(source);
        }

        public static Tuple<TSource, TSource> MinMax<TSource>(this IEnumerable<TSource> source)
        {
            return MinMax(source, Comparer<TSource>.Default);
        }

        public static Tuple<int, int> MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            return source.Select(selector).MinMax();
        }
        public static Tuple<int?, int?> MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            return source.Select(selector).MinMax();
        }

        public static Tuple<long, long> MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            return source.Select(selector).MinMax();
        }
        public static Tuple<long?, long?> MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            return source.Select(selector).MinMax();
        }

        public static Tuple<float, float> MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            return source.Select(selector).MinMax();
        }
        public static Tuple<float?, float?> MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            return source.Select(selector).MinMax();
        }

        public static Tuple<double, double> MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            return source.Select(selector).MinMax();
        }
        public static Tuple<double?, double?> MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            return source.Select(selector).MinMax();
        }

        public static Tuple<decimal, decimal> MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            return source.Select(selector).MinMax();
        }
        public static Tuple<decimal?, decimal?> MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            return source.Select(selector).MinMax();
        }

        public static Tuple<TResult, TResult> MinMax<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return source.Select(selector).MinMax();
        }

        public static Tuple<TSource, TSource> MinMax<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            Comparison<TSource> comparison = comparer.Compare;
            return MinMax(source, comparison);
        }

        static Tuple<TSource, TSource> UnguardedMinMaxNeverNull<TSource>(IEnumerable<TSource> source, Comparison<TSource> comparison)
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
                return Tuple.Create(min, max);
            }
        }

        static Tuple<TSource, TSource> UnguardedMinMaxMaybeNull<TSource>(IEnumerable<TSource> source, Comparison<TSource> comparison)
        {
            using (var enumerator = source.GetEnumerator()) {
                var hasData = false;
                while ((hasData = enumerator.MoveNext()) && enumerator.Current == null)
                    ;
                if (!hasData)
                    return Tuple.Create(default(TSource), default(TSource));

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
                return Tuple.Create(min, max);
            }
        }

        public static Tuple<TSource, TSource> MinMax<TSource>(this IEnumerable<TSource> source, Comparison<TSource> comparison)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (default(TSource) == null)
                return UnguardedMinMaxMaybeNull(source, comparison);
            return UnguardedMinMaxNeverNull(source, comparison);
        }
    }
}