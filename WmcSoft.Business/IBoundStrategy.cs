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

namespace WmcSoft
{
    /// <summary>
    /// Defines a strategy to deal with bounds on ranges of values.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    public interface IBoundStrategy<T>
    {
        bool IsWithinRange(IComparer<T> comparer, T value, T lower, T upper);
    }

    public static class BoundStrategyExtensions
    {
        public static bool IsWithinRange<TStrategy, T>(this TStrategy strategy, T value, T lower, T upper)
            where TStrategy : IBoundStrategy<T> {
            return strategy.IsWithinRange(Comparer<T>.Default, value, lower, upper);
        }

        public static bool IsWithinRange<TStrategy, T>(this TStrategy strategy, T value, Range<T> range)
            where T : IComparable<T>
            where TStrategy : IBoundStrategy<T> {
            return strategy.IsWithinRange(Comparer<T>.Default, value, range.Lower, range.Upper);
        }

        public static bool IsWithinAnyRanges<TStrategy, T>(this TStrategy strategy, T value, params Range<T>[] ranges)
            where T : IComparable<T>
            where TStrategy : IBoundStrategy<T> {
            return Array.FindIndex(ranges, r => strategy.IsWithinRange(Comparer<T>.Default, value, r.Lower, r.Upper)) >= 0;
        }

        public static bool IsWithinRange<TStrategy, T>(this TStrategy strategy, Range<T> range, params T[] values)
            where T : IComparable<T>
            where TStrategy : IBoundStrategy<T> {
            return Array.FindIndex(values, v => strategy.IsWithinRange(Comparer<T>.Default, v, range.Lower, range.Upper)) >= 0;
        }

        public static bool AreAllWithinRange<TStrategy, T>(this TStrategy strategy, Range<T> range, params T[] values)
            where T : IComparable<T>
            where TStrategy : IBoundStrategy<T> {
            return Array.TrueForAll(values, v => strategy.IsWithinRange(Comparer<T>.Default, v, range.Lower, range.Upper));
        }
    }
}
