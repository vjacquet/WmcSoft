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
using System.Linq;
using System.Collections.Generic;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Check equality on two arrays.
    /// </summary>
    /// <typeparam name="T">The type of values in the arrays.</typeparam>
    public class ArrayEqualityComparer<T> : IEqualityComparer<Array>
    {
        readonly IEqualityComparer<T> _comparer;

        public ArrayEqualityComparer(IEqualityComparer<T> comparer) {
            _comparer = comparer;
        }
        public ArrayEqualityComparer()
            : this(EqualityComparer<T>.Default) {
        }

        static bool Increment(int rank, int[] dimensions, int[] indices) {
            for (int i = rank - 1; i >= 0; i--) {
                if (++indices[i] != dimensions[i])
                    return true;
                indices[i] = 0;
            }
            return false;
        }
        #region IEqualityComparer<Array> Membres

        public bool Equals(Array x, Array y) {
            if (x == y)
                return true;
            if (x == null)
                return false;

            if (x.Rank != y.Rank)
                return false;
            if (!x.GetDimensions().SequenceEqual(y.GetDimensions()))
                return false;

            return x.AsEnumerable<T>().SequenceEqual(y.AsEnumerable<T>());
            //var rank = x.Rank;
            //var dimensions = new int[rank];
            //for (int i = 0; i < rank; i++) {
            //    dimensions[i] = x.GetLength(i);
            //    if (dimensions[i] != y.GetLength(i))
            //        return false;
            //}
            //var indices = new int[rank];
            //do {
            //    if (!_comparer.Equals((T)x.GetValue(indices), (T)y.GetValue(indices)))
            //        return false;
            //} while (Increment(rank, dimensions, indices));
            //return true;
        }

        public int GetHashCode(Array obj) {
            return obj.GetHashCode();
        }

        #endregion
    }
}
