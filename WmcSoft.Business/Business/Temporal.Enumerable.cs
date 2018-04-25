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
using System.Linq;

namespace WmcSoft.Business
{
    /// <summary>
    /// Defines the extension methods to the <see cref="ITemporal"/> interface.
    /// This is a static class.
    /// </summary>
    public static partial class Temporal
    {
        static Func<IBox<T>, IBox<U>> Transform<T, U>(Func<T, U> func)
        {
            return x => new BoxOnTransformedTemporal<U>(func(x.Value), x.Source);
        }

        static Func<IBox<T>, U, IBox<V>> Transform<T, U, V>(Func<T,U, V> func)
        {
            return (x,y) => new BoxOnTransformedTemporal<V>(func(x.Value, y), x.Source);
        }

        static Func<IBox<T>, bool> Filter<T>(Func<T, bool> predicate)
        {
            return x => predicate(x.Value);
        }

        internal static IEnumerable<IBox<T>> Box<T>(IEnumerable<T> source)
            where T : ITemporal
        {
            return source.Select(x => new BoxOnTemporal<T>(x));
        }

        internal static IEnumerable<T> Unbox<T>(IEnumerable<IBox<T>> source)
        {
            return source.Select(x => x.Value);
        }

        #region AsOf

        public class AsOfEnumerable<T> : ITemporalEnumerable<T>
        {
            private readonly IEnumerable<IBox<T>> _valid;

            internal AsOfEnumerable(DateTime asOf, IEnumerable<IBox<T>> valid)
            {
                AsOf = asOf;
                _valid = valid;
            }

            public DateTime AsOf { get; }

            public IEnumerator<T> GetEnumerator()
            {
                return Unbox(_valid).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #region Linq

            public AsOfEnumerable<T> Where(Func<T, bool> predicate)
            {
                return new AsOfEnumerable<T>(AsOf, _valid.Where(Filter(predicate)));
            }

            public AsOfEnumerable<U> Select<U>(Func<T, U> selector)
            {
                return new AsOfEnumerable<U>(AsOf, _valid.Select(Transform(selector)));
            }

            public AsOfEnumerable<V> Join<U, K, V>(IEnumerable<U> inner, Func<T, K> outerKeySelector, Func<U, K> innerKeySelector, Func<T, U, V> resultSelector)
            {
                if (typeof(ITemporal).IsAssignableFrom(typeof(U))) {
                    return new AsOfEnumerable<V>(AsOf, _valid.Join(inner.Where(x => ((ITemporal)x).IsValidOn(AsOf)), x => outerKeySelector(x.Value), innerKeySelector, Transform(resultSelector)));
                }else {
                    return new AsOfEnumerable<V>(AsOf, _valid.Join(inner, x => outerKeySelector(x.Value), innerKeySelector, Transform(resultSelector)));
                }
            }

            #endregion
        }

        public static AsOfEnumerable<T> AsOf<T>(this IEnumerable<T> source, DateTime date)
             where T : ITemporal
        {
            var result = Enumerable.Where(source, x => x.IsValidOn(date));
            return new AsOfEnumerable<T>(date, Box(result));
        }

        #endregion

        #region Between

        class BetweenEnumerable<T> : ITemporalIntervalEnumerable<T>
          where T : ITemporal
        {
            private readonly IEnumerable<T> _valid;

            public BetweenEnumerable(DateTime since, DateTime until, IEnumerable<T> valid)
            {
                Since = since;
                Until = until;
                _valid = valid;
            }

            public DateTime Since { get; }
            public DateTime Until { get; }

            public IEnumerator<T> GetEnumerator()
            {
                return _valid.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public static ITemporalIntervalEnumerable<T> Between<T>(this IEnumerable<T> source, DateTime since, DateTime until)
             where T : ITemporal
        {
            var result = Enumerable.Where(source, x => x.IsValidOn(since, until));
            return new BetweenEnumerable<T>(since, until, result);
        }

        #endregion
    }

    internal interface IBox<out T>
    {
        T Value { get; }
        ITemporal Source { get; }
    }

    internal class BoxOnTemporal<T> : IBox<T>
        where T : ITemporal
    {
        public BoxOnTemporal(T value)
        {
            Value = value;
        }
        public T Value { get; }
        public ITemporal Source => Value;
    }

    internal class BoxOnTransformedTemporal<T> : IBox<T>
    {
        public BoxOnTransformedTemporal(T value, ITemporal source)
        {
            Value = value;
            Source = source;
        }
        public T Value { get; }
        public ITemporal Source { get; }
    }

    public interface ITemporalEnumerable<out T> : IEnumerable<T>
    {
        DateTime AsOf { get; }
    }

    public interface ITemporalIntervalEnumerable<out T> : IEnumerable<T>
    {
        DateTime Since { get; }
        DateTime Until { get; }
    }
}
