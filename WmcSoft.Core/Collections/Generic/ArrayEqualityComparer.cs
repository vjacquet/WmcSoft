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

        public ArrayEqualityComparer(IEqualityComparer<T> comparer)
        {
            _comparer = comparer;
        }
        public ArrayEqualityComparer()
            : this(EqualityComparer<T>.Default)
        {
        }

        #region IEqualityComparer<Array> Membres

        public bool Equals(Array x, Array y)
        {
            if (!ArrayShapeEqualityComparer.Default.Equals(x, y))
                return false;

            return x.AsEnumerable<T>().SequenceEqual(y.AsEnumerable<T>(), _comparer);
        }

        public int GetHashCode(Array obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }
}
