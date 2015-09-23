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
    public sealed class BoundStrategy<T> : IBoundStrategy<T>
    {
        #region Strategies

        public struct InclusiveStrategy<T> : IBoundStrategy<T>
        {
            public bool IsWithinRange(IComparer<T> comparer, T value, T lower, T upper) {
                return (comparer.Compare(lower, value) <= 0)
                    && (comparer.Compare(value, upper) <= 0);
            }
        }

        public struct ExclusiveStrategy<T> : IBoundStrategy<T>
        {
            public bool IsWithinRange(IComparer<T> comparer, T value, T lower, T upper) {
                return (comparer.Compare(lower, value) < 0)
                    && (comparer.Compare(value, upper) < 0);
            }
        }

        public struct LowerExclusiveStrategy<T> : IBoundStrategy<T>
        {
            public bool IsWithinRange(IComparer<T> comparer, T value, T lower, T upper) {
                return (comparer.Compare(lower, value) < 0)
                    && (comparer.Compare(value, upper) <= 0);
            }
        }

        public sealed class UpperExclusiveStrategy<T> : IBoundStrategy<T>
        {
            public bool IsWithinRange(IComparer<T> comparer, T value, T lower, T upper) {
                return (comparer.Compare(lower, value) <= 0)
                    && (comparer.Compare(value, upper) < 0);
            }
        }

        static public readonly InclusiveStrategy<T> Inclusive = new InclusiveStrategy<T>();
        static public readonly ExclusiveStrategy<T> Exclusive = new ExclusiveStrategy<T>();
        static public readonly LowerExclusiveStrategy<T> LowerExclusive = new LowerExclusiveStrategy<T>();
        static public readonly UpperExclusiveStrategy<T> UpperExclusive = new UpperExclusiveStrategy<T>();

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
