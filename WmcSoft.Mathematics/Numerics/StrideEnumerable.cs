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

namespace WmcSoft.Numerics
{
    /// <summary>
    /// Stride enumerable on an array
    /// </summary>
    /// <typeparam name="T">The type of the items of the array</typeparam>
    /// <remarks>This class is private because it has some undefined behavior in release mode.</remarks>
    sealed class StrideEnumerable<T> : IEnumerable<T>
    {
        readonly T[] _data;
        readonly int _start;
        readonly int _count;
        readonly int _stride;

        public StrideEnumerable(T[] data, int start, int count, int stride = 1) {
#if DEBUG
            if (data == null)
                throw new ArgumentNullException("data");
            if (start >= data.Length)
                throw new ArgumentOutOfRangeException("start");
            if (stride < 1)
                throw new ArgumentOutOfRangeException("stride");
#endif
            _data = data;
            _start = start;
            _count = count;
            _stride = stride;
        }

        #region IEnumerable<T> Membres

        public IEnumerator<T> GetEnumerator() {
            return new StrideEnumerator<T>(_data, _start, _count, _stride);
        }

        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
