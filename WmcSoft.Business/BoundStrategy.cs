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

using System.Collections.Generic;

namespace WmcSoft
{
    /// <summary>
    /// Wrapper to decorated strategies not implemented as structs so the extension methods can be applied.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    public struct BoundStrategy<T> : IBoundStrategy<T>
    {
        #region Strategies

        public struct InclusiveStrategy : IBoundStrategy<T>
        {
            public bool IsWithinRange(IComparer<T> comparer, T value, T lower, T upper) {
                return (comparer.Compare(lower, value) <= 0)
                    && (comparer.Compare(value, upper) <= 0);
            }
        }

        public struct ExclusiveStrategy : IBoundStrategy<T>
        {
            public bool IsWithinRange(IComparer<T> comparer, T value, T lower, T upper) {
                return (comparer.Compare(lower, value) < 0)
                    && (comparer.Compare(value, upper) < 0);
            }
        }

        public struct LowerExclusiveStrategy : IBoundStrategy<T>
        {
            public bool IsWithinRange(IComparer<T> comparer, T value, T lower, T upper) {
                return (comparer.Compare(lower, value) < 0)
                    && (comparer.Compare(value, upper) <= 0);
            }
        }

        public sealed class UpperExclusiveStrategy : IBoundStrategy<T>
        {
            public bool IsWithinRange(IComparer<T> comparer, T value, T lower, T upper) {
                return (comparer.Compare(lower, value) <= 0)
                    && (comparer.Compare(value, upper) < 0);
            }
        }

        static public readonly InclusiveStrategy Inclusive = new InclusiveStrategy();
        static public readonly ExclusiveStrategy Exclusive = new ExclusiveStrategy();
        static public readonly LowerExclusiveStrategy LowerExclusive = new LowerExclusiveStrategy();
        static public readonly UpperExclusiveStrategy UpperExclusive = new UpperExclusiveStrategy();

        #endregion

        #region Fields

        IBoundStrategy<T> _underlying;

        #endregion

        #region Lifecycle

        public BoundStrategy(IBoundStrategy<T> strategy) {
            _underlying = strategy;
        }

        #endregion

        #region IBoundStrategy<T> Members

        public bool IsWithinRange(IComparer<T> comparer, T value, T lower, T upper) {
            return _underlying.IsWithinRange(comparer, value, lower, upper);
        }

        #endregion
    }
}
