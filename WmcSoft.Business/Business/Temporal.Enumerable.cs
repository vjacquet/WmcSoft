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
        class AsOfEnumerable<T> : ITemporalEnumerable<T>
            where T : ITemporal
        {
            private readonly IEnumerable<T> _filtered;

            public AsOfEnumerable(DateTime asOf, IEnumerable<T> source)
            {
                AsOf = asOf;
                _filtered = source.Where(x => x.IsValidOn(asOf));
            }

            public DateTime AsOf { get; }

            public IEnumerator<T> GetEnumerator()
            {
                return _filtered.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        class BetweenEnumerable<T> : ITemporalIntervalEnumerable<T>
            where T : ITemporal
        {
            private readonly IEnumerable<T> _filtered;

            public BetweenEnumerable(DateTime since, DateTime until, IEnumerable<T> source)
            {
                Since = since;
                Until = until;
                _filtered = source.Where(x => x.IsValidOn(since, until));
            }

            public DateTime Since { get; }
            public DateTime Until { get; }

            public IEnumerator<T> GetEnumerator()
            {
                return _filtered.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public static ITemporalEnumerable<T> AsOf<T>(this IEnumerable<T> source, DateTime date)
             where T : ITemporal
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new AsOfEnumerable<T>(date, source);
        }

        public static ITemporalIntervalEnumerable<T> Between<T>(this IEnumerable<T> source, DateTime since, DateTime until)
             where T : ITemporal
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new BetweenEnumerable<T>(since, until, source);
        }
    }

    public interface ITemporalEnumerable<out T> : IEnumerable<T>
        where T : ITemporal
    {
        DateTime AsOf { get; }
    }

    public interface ITemporalIntervalEnumerable<out T> : IEnumerable<T>
    {
        DateTime Since { get; }
        DateTime Until { get; }
    }
}
