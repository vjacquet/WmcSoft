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
        #region Min

        public static Expected<double> Min(this IEnumerable<Expected<double>> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                bool hasData = false;
                while ((hasData = enumerator.MoveNext()) && (!enumerator.Current.HasValue || Double.IsNaN(enumerator.Current.GetValueOrDefault())))
                    ;
                if (!hasData)
                    return Expected.Failed<double>(new InvalidOperationException("No elements"));

                var min = enumerator.Current.GetValueOrDefault();
                while (enumerator.MoveNext()) {
                    if (!enumerator.Current.HasValue || Double.IsNaN(enumerator.Current.GetValueOrDefault()))
                        continue;
                    if (enumerator.Current.GetValueOrDefault() < min)
                        min = enumerator.Current.GetValueOrDefault();
                }
                return Expected.Success(min);
            }
        }

        public static Expected<long> Min(this IEnumerable<Expected<long>> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                bool hasData = false;
                while ((hasData = enumerator.MoveNext()) && !enumerator.Current.HasValue)
                    ;
                if (!hasData)
                    return Expected.Failed<long>(new InvalidOperationException("No elements"));

                var min = enumerator.Current.GetValueOrDefault();
                while (enumerator.MoveNext()) {
                    if (!enumerator.Current.HasValue)
                        continue;
                    if (enumerator.Current.GetValueOrDefault() < min)
                        min = enumerator.Current.GetValueOrDefault();
                }
                return Expected.Success(min);
            }
        }

        public static Expected<int> Min(this IEnumerable<Expected<int>> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                bool hasData = false;
                while ((hasData = enumerator.MoveNext()) && !enumerator.Current.HasValue)
                    ;
                if (!hasData)
                    return Expected.Failed<int>(new InvalidOperationException("No elements"));

                var min = enumerator.Current.GetValueOrDefault();
                while (enumerator.MoveNext()) {
                    if (!enumerator.Current.HasValue)
                        continue;
                    if (enumerator.Current.GetValueOrDefault() < min)
                        min = enumerator.Current.GetValueOrDefault();
                }
                return Expected.Success(min);
            }
        }

        public static Expected<float> Min(this IEnumerable<Expected<float>> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                bool hasData = false;
                while ((hasData = enumerator.MoveNext()) && (!enumerator.Current.HasValue || Single.IsNaN(enumerator.Current.Value)))
                    ;
                if (!hasData)
                    return Expected.Failed<float>(new InvalidOperationException("No elements"));

                var min = enumerator.Current.GetValueOrDefault();
                while (enumerator.MoveNext()) {
                    if (!enumerator.Current.HasValue || Single.IsNaN(enumerator.Current.GetValueOrDefault()))
                        continue;
                    if (enumerator.Current.GetValueOrDefault() < min)
                        min = enumerator.Current.GetValueOrDefault();
                }
                return Expected.Success(min);
            }
        }

        public static Expected<decimal> Min(this IEnumerable<Expected<decimal>> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                bool hasData = false;
                while ((hasData = enumerator.MoveNext()) && !enumerator.Current.HasValue)
                    ;
                if (!hasData)
                    return Expected.Failed<decimal>(new InvalidOperationException("No elements"));

                var min = enumerator.Current.GetValueOrDefault();
                while (enumerator.MoveNext()) {
                    if (!enumerator.Current.HasValue)
                        continue;
                    if (enumerator.Current.GetValueOrDefault() < min)
                        min = enumerator.Current.GetValueOrDefault();
                }
                return Expected.Success(min);
            }
        }

        public static Expected<int> Min<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<int>> selector) {
            return source.Select(selector).Min();
        }

        public static Expected<long> Min<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<long>> selector) {
            return source.Select(selector).Min();
        }

        public static Expected<float> Min<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<float>> selector) {
            return source.Select(selector).Min();
        }

        public static Expected<double> Min<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<double>> selector) {
            return source.Select(selector).Min();
        }

        public static Expected<decimal> Min<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<decimal>> selector) {
            return source.Select(selector).Min();
        }

        public static Expected<TResult> Min<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Expected<TResult>> selector) {
            return source.Select(selector).Min();
        }

        #endregion

        #region Max

        public static Expected<double> Max(this IEnumerable<Expected<double>> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                bool hasData = false;
                while ((hasData = enumerator.MoveNext()) && (!enumerator.Current.HasValue || Double.IsNaN(enumerator.Current.GetValueOrDefault())))
                    ;
                if (!hasData)
                    return Expected.Failed<double>(new InvalidOperationException("No elements"));

                var max = enumerator.Current.GetValueOrDefault();
                while (enumerator.MoveNext()) {
                    if (!enumerator.Current.HasValue || Double.IsNaN(enumerator.Current.GetValueOrDefault()))
                        continue;
                    if (enumerator.Current.GetValueOrDefault() >= max)
                        max = enumerator.Current.GetValueOrDefault();
                }
                return Expected.Success(max);
            }
        }

        public static Expected<long> Max(this IEnumerable<Expected<long>> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                bool hasData = false;
                while ((hasData = enumerator.MoveNext()) && !enumerator.Current.HasValue)
                    ;
                if (!hasData)
                    return Expected.Failed<long>(new InvalidOperationException("No elements"));

                var max = enumerator.Current.GetValueOrDefault();
                while (enumerator.MoveNext()) {
                    if (!enumerator.Current.HasValue)
                        continue;
                    if (enumerator.Current.GetValueOrDefault() >= max)
                        max = enumerator.Current.GetValueOrDefault();
                }
                return Expected.Success(max);
            }
        }

        public static Expected<int> Max(this IEnumerable<Expected<int>> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                bool hasData = false;
                while ((hasData = enumerator.MoveNext()) && !enumerator.Current.HasValue)
                    ;
                if (!hasData)
                    return Expected.Failed<int>(new InvalidOperationException("No elements"));

                var max = enumerator.Current.GetValueOrDefault();
                while (enumerator.MoveNext()) {
                    if (!enumerator.Current.HasValue)
                        continue;
                    if (enumerator.Current.GetValueOrDefault() >= max)
                        max = enumerator.Current.GetValueOrDefault();
                }
                return Expected.Success(max);
            }
        }

        public static Expected<float> Max(this IEnumerable<Expected<float>> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                bool hasData = false;
                while ((hasData = enumerator.MoveNext()) && (!enumerator.Current.HasValue || Single.IsNaN(enumerator.Current.Value)))
                    ;
                if (!hasData)
                    return Expected.Failed<float>(new InvalidOperationException("No elements"));

                var max = enumerator.Current.GetValueOrDefault();
                while (enumerator.MoveNext()) {
                    if (!enumerator.Current.HasValue || Single.IsNaN(enumerator.Current.GetValueOrDefault()))
                        continue;
                    if (enumerator.Current.GetValueOrDefault() >= max)
                        max = enumerator.Current.GetValueOrDefault();
                }
                return Expected.Success(max);
            }
        }

        public static Expected<decimal> Max(this IEnumerable<Expected<decimal>> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                bool hasData = false;
                while ((hasData = enumerator.MoveNext()) && !enumerator.Current.HasValue)
                    ;
                if (!hasData)
                    return Expected.Failed<decimal>(new InvalidOperationException("No elements"));

                var max = enumerator.Current.GetValueOrDefault();
                while (enumerator.MoveNext()) {
                    if (!enumerator.Current.HasValue)
                        continue;
                    if (enumerator.Current.GetValueOrDefault() >= max)
                        max = enumerator.Current.GetValueOrDefault();
                }
                return Expected.Success(max);
            }
        }

        public static Expected<int> Max<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<int>> selector) {
            return source.Select(selector).Max();
        }

        public static Expected<long> Max<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<long>> selector) {
            return source.Select(selector).Max();
        }

        public static Expected<float> Max<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<float>> selector) {
            return source.Select(selector).Max();
        }

        public static Expected<double> Max<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<double>> selector) {
            return source.Select(selector).Max();
        }

        public static Expected<decimal> Max<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<decimal>> selector) {
            return source.Select(selector).Max();
        }

        public static Expected<TResult> Max<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Expected<TResult>> selector) {
            return source.Select(selector).Max();
        }

        #endregion

        #region MinMax

        public static Tuple<Expected<double>, Expected<double>> MinMax(this IEnumerable<Expected<double>> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                bool hasData = false;
                while ((hasData = enumerator.MoveNext()) && (!enumerator.Current.HasValue || Double.IsNaN(enumerator.Current.GetValueOrDefault())))
                    ;
                if (!hasData) {
                    var noelements = Expected.Failed<double>(new InvalidOperationException("No elements"));
                    return Tuple.Create(noelements, noelements);
                }

                var min = enumerator.Current.GetValueOrDefault();
                var max = enumerator.Current.GetValueOrDefault();
                while (enumerator.MoveNext()) {
                    if (!enumerator.Current.HasValue || Double.IsNaN(enumerator.Current.GetValueOrDefault()))
                        continue;
                    if (enumerator.Current.GetValueOrDefault() < min)
                        min = enumerator.Current.GetValueOrDefault();
                    else if (enumerator.Current.GetValueOrDefault() >= max)
                        max = enumerator.Current.GetValueOrDefault();
                }
                return Tuple.Create(Expected.Success(min), Expected.Success(max));
            }
        }

        public static Tuple<Expected<long>, Expected<long>> MinMax(this IEnumerable<Expected<long>> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                bool hasData = false;
                while ((hasData = enumerator.MoveNext()) && !enumerator.Current.HasValue)
                    ;
                if (!hasData) {
                    var noelements = Expected.Failed<long>(new InvalidOperationException("No elements"));
                    return Tuple.Create(noelements, noelements);
                }

                var min = enumerator.Current.GetValueOrDefault();
                var max = enumerator.Current.GetValueOrDefault();
                while (enumerator.MoveNext()) {
                    if (!enumerator.Current.HasValue)
                        continue;
                    if (enumerator.Current.GetValueOrDefault() < min)
                        min = enumerator.Current.GetValueOrDefault();
                    else if (enumerator.Current.GetValueOrDefault() >= max)
                        max = enumerator.Current.GetValueOrDefault();
                }
                return Tuple.Create(Expected.Success(min), Expected.Success(max));
            }
        }

        public static Tuple<Expected<int>, Expected<int>> MinMax(this IEnumerable<Expected<int>> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                bool hasData = false;
                while ((hasData = enumerator.MoveNext()) && !enumerator.Current.HasValue)
                    ;
                if (!hasData) {
                    var noelements = Expected.Failed<int>(new InvalidOperationException("No elements"));
                    return Tuple.Create(noelements, noelements);
                }

                var min = enumerator.Current.GetValueOrDefault();
                var max = enumerator.Current.GetValueOrDefault();
                while (enumerator.MoveNext()) {
                    if (!enumerator.Current.HasValue)
                        continue;
                    if (enumerator.Current.GetValueOrDefault() < min)
                        min = enumerator.Current.GetValueOrDefault();
                    else if (enumerator.Current.GetValueOrDefault() >= max)
                        max = enumerator.Current.GetValueOrDefault();
                }
                return Tuple.Create(Expected.Success(min), Expected.Success(max));
            }
        }

        public static Tuple<Expected<float>, Expected<float>> MinMax(this IEnumerable<Expected<float>> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                bool hasData = false;
                while ((hasData = enumerator.MoveNext()) && (!enumerator.Current.HasValue || Single.IsNaN(enumerator.Current.Value)))
                    ;
                if (!hasData) {
                    var noelements = Expected.Failed<float>(new InvalidOperationException("No elements"));
                    return Tuple.Create(noelements, noelements);
                }

                var min = enumerator.Current.GetValueOrDefault();
                var max = enumerator.Current.GetValueOrDefault();
                while (enumerator.MoveNext()) {
                    if (!enumerator.Current.HasValue || Single.IsNaN(enumerator.Current.GetValueOrDefault()))
                        continue;
                    if (enumerator.Current.GetValueOrDefault() < min)
                        min = enumerator.Current.GetValueOrDefault();
                    else if (enumerator.Current.GetValueOrDefault() >= max)
                        max = enumerator.Current.GetValueOrDefault();
                }
                return Tuple.Create(Expected.Success(min), Expected.Success(max));
            }
        }

        public static Tuple<Expected<decimal>, Expected<decimal>> MinMax(this IEnumerable<Expected<decimal>> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator()) {
                bool hasData = false;
                while ((hasData = enumerator.MoveNext()) && !enumerator.Current.HasValue)
                    ;
                if (!hasData) {
                    var noelements = Expected.Failed<decimal>(new InvalidOperationException("No elements"));
                    return Tuple.Create(noelements, noelements);
                }

                var min = enumerator.Current.GetValueOrDefault();
                var max = enumerator.Current.GetValueOrDefault();
                while (enumerator.MoveNext()) {
                    if (!enumerator.Current.HasValue)
                        continue;
                    if (enumerator.Current.GetValueOrDefault() < min)
                        min = enumerator.Current.GetValueOrDefault();
                    else if (enumerator.Current.GetValueOrDefault() >= max)
                        max = enumerator.Current.GetValueOrDefault();
                }
                return Tuple.Create(Expected.Success(min), Expected.Success(max));
            }
        }

        public static Tuple<Expected<int>, Expected<int>> MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<int>> selector) {
            return source.Select(selector).MinMax();
        }

        public static Tuple<Expected<long>, Expected<long>> MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<long>> selector) {
            return source.Select(selector).MinMax();
        }

        public static Tuple<Expected<float>, Expected<float>> MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<float>> selector) {
            return source.Select(selector).MinMax();
        }

        public static Tuple<Expected<double>, Expected<double>> MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<double>> selector) {
            return source.Select(selector).MinMax();
        }

        public static Tuple<Expected<decimal>, Expected<decimal>> MinMax<TSource>(this IEnumerable<TSource> source, Func<TSource, Expected<decimal>> selector) {
            return source.Select(selector).MinMax();
        }

        public static Tuple<Expected<TResult>, Expected<TResult>> MinMax<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Expected<TResult>> selector) {
            return source.Select(selector).MinMax();
        }

        #endregion
    }
}