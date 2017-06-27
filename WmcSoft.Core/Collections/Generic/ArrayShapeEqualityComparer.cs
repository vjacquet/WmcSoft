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
    /// Check equality on the shape of two arrays.
    /// </summary>
    public struct ArrayShapeEqualityComparer : IEqualityComparer<Array>
    {
        public static readonly ArrayShapeEqualityComparer Default;

        public bool Equals(Array x, Array y)
        {
            if (x == y)
                return true;
            if (x == null)
                return false;

            if (x.Rank != y.Rank || x.Length != y.Length)
                return false;
            var length = x.Rank;
            for (int i = 0; i < length; i++) {
                if (x.GetUpperBound(i) != y.GetUpperBound(i) || x.GetUpperBound(i) != y.GetUpperBound(i))
                    return false;
            }
            return true;
        }

        static int GetDimensionHashCode(Array obj, int i)
        {
            return EqualityComparer.CombineHashCodes(obj.GetLowerBound(i), obj.GetUpperBound(i));
        }

        public int GetHashCode(Array obj)
        {
            if (obj == null)
                return 0;
            return EqualityComparer.CombineHashCodes(obj.Rank, obj.Length, GetDimensionHashCode(obj, 0), GetDimensionHashCode(obj, obj.Rank - 1));
        }
    }
}
