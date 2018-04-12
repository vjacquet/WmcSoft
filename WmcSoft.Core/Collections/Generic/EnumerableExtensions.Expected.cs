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
using WmcSoft.Collections.Generic.Accumulators;

namespace WmcSoft.Collections.Generic
{
    public static partial class EnumerableExtensions
    {
        #region Min

        static Expected<T> ValueTypeMin<T>(this IEnumerable<Expected<T>> source)
            where T : struct, IComparable<T>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                var hasData = false;
                while ((hasData = enumerator.MoveNext()) && !enumerator.Current.HasValue)
                    ;
                if (!hasData)
                    return Expected.Failed<T>(new InvalidOperationException("No elements"));

                var min = enumerator.Current.GetValueOrDefault();
                while (enumerator.MoveNext()) {
                    if (!enumerator.Current.HasValue)
                        continue;
                    if (enumerator.Current.GetValueOrDefault().CompareTo(min) < 0)
                        min = enumerator.Current.GetValueOrDefault();
                }
                return Expected.Success(min);
            }
        }

        public static Expected<double> Min(this IEnumerable<Expected<double>> source)
        {
            return ValueTypeMin(source);
        }

        public static Expected<long> Min(this IEnumerable<Expected<long>> source)
        {
            return ValueTypeMin(source);
        }

        public static Expected<int> Min(this IEnumerable<Expected<int>> source)
        {
            return ValueTypeMin(source);
        }

        public static Expected<float> Min(this IEnumerable<Expected<float>> source)
        {
            return ValueTypeMin(source);
        }

        public static Expected<decimal> Min(this IEnumerable<Expected<decimal>> source)
        {
            return ValueTypeMin(source);
        }

        public static Expected<int> Min<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<int>> selector)
        {
            return source.Select(selector).Min();
        }

        public static Expected<long> Min<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<long>> selector)
        {
            return source.Select(selector).Min();
        }

        public static Expected<float> Min<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<float>> selector)
        {
            return source.Select(selector).Min();
        }

        public static Expected<double> Min<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<double>> selector)
        {
            return source.Select(selector).Min();
        }

        public static Expected<decimal> Min<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<decimal>> selector)
        {
            return source.Select(selector).Min();
        }

        public static Expected<TResult> Min<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Expected<TResult>> selector)
        {
            return source.Select(selector).Min();
        }

        #endregion

        #region Max

        static Expected<T> ValueTypeMax<T>(this IEnumerable<Expected<T>> source)
            where T : struct, IComparable<T>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                var hasData = false;
                while ((hasData = enumerator.MoveNext()) && !enumerator.Current.HasValue)
                    ;
                if (!hasData)
                    return Expected.Failed<T>(new InvalidOperationException("No elements"));

                var max = enumerator.Current.GetValueOrDefault();
                while (enumerator.MoveNext()) {
                    if (!enumerator.Current.HasValue)
                        continue;
                    if (max.CompareTo(enumerator.Current.GetValueOrDefault()) <= 0) // ensures stability
                        max = enumerator.Current.GetValueOrDefault();
                }
                return Expected.Success(max);
            }
        }

        public static Expected<double> Max(this IEnumerable<Expected<double>> source)
        {
            return ValueTypeMax(source);
        }

        public static Expected<long> Max(this IEnumerable<Expected<long>> source)
        {
            return ValueTypeMax(source);
        }

        public static Expected<int> Max(this IEnumerable<Expected<int>> source)
        {
            return ValueTypeMax(source);
        }

        public static Expected<float> Max(this IEnumerable<Expected<float>> source)
        {
            return ValueTypeMax(source);
        }

        public static Expected<decimal> Max(this IEnumerable<Expected<decimal>> source)
        {
            return ValueTypeMax(source);
        }

        public static Expected<int> Max<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<int>> selector)
        {
            return source.Select(selector).Max();
        }

        public static Expected<long> Max<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<long>> selector)
        {
            return source.Select(selector).Max();
        }

        public static Expected<float> Max<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<float>> selector)
        {
            return source.Select(selector).Max();
        }

        public static Expected<double> Max<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<double>> selector)
        {
            return source.Select(selector).Max();
        }

        public static Expected<decimal> Max<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<decimal>> selector)
        {
            return source.Select(selector).Max();
        }

        public static Expected<TResult> Max<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Expected<TResult>> selector)
        {
            return source.Select(selector).Max();
        }

        #endregion

        #region MinMax

        static (Expected<T> min, Expected<T> max) ValueTypeMinMax<T>(this IEnumerable<Expected<T>> source)
            where T : struct, IComparable<T>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                var hasData = false;
                while ((hasData = enumerator.MoveNext()) && !enumerator.Current.HasValue)
                    ;
                if (!hasData) {
                    var noelements = Expected.Failed<T>(new InvalidOperationException("No elements"));
                    return (noelements, noelements);
                }
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
                return (Expected.Success(min), Expected.Success(max));
            }
        }

        public static (Expected<double> min, Expected<double> max) MinMax(this IEnumerable<Expected<double>> source)
        {
            return ValueTypeMinMax(source);
        }

        public static (Expected<long> min, Expected<long> max) MinMax(this IEnumerable<Expected<long>> source)
        {
            return ValueTypeMinMax(source);
        }

        public static (Expected<int> min, Expected<int> max) MinMax(this IEnumerable<Expected<int>> source)
        {
            return ValueTypeMinMax(source);
        }

        public static (Expected<float> min, Expected<float> max) MinMax(this IEnumerable<Expected<float>> source)
        {
            return ValueTypeMinMax(source);
        }

        public static (Expected<decimal> min, Expected<decimal> max) MinMax(this IEnumerable<Expected<decimal>> source)
        {
            return ValueTypeMinMax(source);
        }

        public static (Expected<int> min, Expected<int> max) MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<int>> selector)
        {
            return source.Select(selector).MinMax();
        }

        public static (Expected<long> min, Expected<long> max) MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<long>> selector)
        {
            return source.Select(selector).MinMax();
        }

        public static (Expected<float> min, Expected<float> max) MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<float>> selector)
        {
            return source.Select(selector).MinMax();
        }

        public static (Expected<double> min, Expected<double> max) MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<double>> selector)
        {
            return source.Select(selector).MinMax();
        }

        public static (Expected<decimal> min, Expected<decimal> max) MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<decimal>> selector)
        {
            return source.Select(selector).MinMax();
        }

        public static (Expected<TResult> min, Expected<TResult> max) MinMax<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Expected<TResult>> selector)
        {
            return source.Select(selector).MinMax();
        }

        #endregion

        #region Sum

        static Expected<T> ValueTypeAccumulate<T, TAccumulator>(this IEnumerable<Expected<T>> source, TAccumulator accumulator)
           where TAccumulator : struct, IAccumulator<T, T>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                var hasData = false;
                while ((hasData = enumerator.MoveNext()) && !enumerator.Current.HasValue)
                    ;
                if (!hasData)
                    return Expected.Failed<T>(new InvalidOperationException("No elements"));

                var result = enumerator.Current.GetValueOrDefault();
                while (enumerator.MoveNext()) {
                    if (enumerator.Current.HasValue) {
                        result = accumulator.Accumulate(result, enumerator.Current.GetValueOrDefault());
                    }
                }
                return Expected.Success(result);
            }
        }

        public static Expected<double> Sum(this IEnumerable<Expected<double>> source)
        {
            return ValueTypeAccumulate(source, new DoubleAdder());
        }

        public static Expected<long> Sum(this IEnumerable<Expected<long>> source)
        {
            return ValueTypeAccumulate(source, new Int64Adder());
        }

        public static Expected<int> Sum(this IEnumerable<Expected<int>> source)
        {
            return ValueTypeAccumulate(source, new Int32Adder());
        }

        public static Expected<float> Sum(this IEnumerable<Expected<float>> source)
        {
            return ValueTypeAccumulate(source, new SingleAdder());
        }

        public static Expected<decimal> Sum(this IEnumerable<Expected<decimal>> source)
        {
            return ValueTypeAccumulate(source, new DecimalAdder());
        }

        public static Expected<int> Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<int>> selector)
        {
            return source.Select(selector).Sum();
        }

        public static Expected<long> Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<long>> selector)
        {
            return source.Select(selector).Sum();
        }

        public static Expected<float> Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<float>> selector)
        {
            return source.Select(selector).Sum();
        }

        public static Expected<double> Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<double>> selector)
        {
            return source.Select(selector).Sum();
        }

        public static Expected<decimal> Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<decimal>> selector)
        {
            return source.Select(selector).Sum();
        }

        #endregion
    }
}
