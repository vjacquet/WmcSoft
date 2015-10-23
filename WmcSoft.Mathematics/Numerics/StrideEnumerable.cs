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
using System.Diagnostics;

namespace WmcSoft.Numerics
{
    /// <summary>
    /// Stride enumerable on an array
    /// </summary>
    /// <typeparam name="T">The type of the items of the array</typeparam>
    /// <remarks>This class is private because it has some undefined behavior in release mode.</remarks>
    sealed class StrideEnumerable<T> : IEnumerable<T>, IReadOnlyCollection<T>, ICollection
    {
        public static readonly StrideEnumerable<T> Empty = new StrideEnumerable<T>(null, 0, 0);

        readonly T[] _data;
        readonly int _start;
        readonly int _count;
        readonly int _stride;

        public StrideEnumerable(T[] data) : this(data, 0, data.Length, 1) {
        }

        public StrideEnumerable(T[] data, int start, int count, int stride = 1) {
            Debug.Assert(stride > 0);
            Debug.Assert(start >= 0);
            Debug.Assert(count >= 0);

            _data = data;
            _start = start;
            _count = count;
            _stride = stride;
        }

        #region IReadOnlyCollection<T> Membres

        public int Count { get { return _count; } }

        #endregion

        #region IEnumerable<T> Membres

        public IEnumerator<T> GetEnumerator() {
            return new StrideEnumerator<T>(_data, _start, _count, _stride);
        }

        #endregion

        #region ICollection Membres

        public void CopyTo(Array array, int index) {
            if (_stride == 1) {
                Array.Copy(_data, 0, array, index, _count);
            } else {
                int end = _start + _count * _stride;
                for (int i = _start; i < end; i += _stride) {
                    array.SetValue(_data[i], index++);
                }
            }
        }

        public object SyncRoot {
            get { return _data; }
        }

        public bool IsSynchronized {
            get { return true; }
        }

        #endregion

        #region IEnumerable Membres

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
