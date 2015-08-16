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
    /// Enumerator on an array
    /// </summary>
    /// <typeparam name="T">The type of the items of the array</typeparam>
    /// <remarks>This class is private because it has some undefined behavior in release mode.</remarks>
    sealed class StrideEnumerator<T> : IEnumerator<T>
    {
        readonly T[] _data;
        readonly int _start;
        readonly int _stride;
        readonly int _end;
        int _pos;

        public StrideEnumerator(T[] data) {
            _data = data;
            _start = 0;
            _stride = 1;
            _end = data.Length - _stride;
            Reset();
        }

        public StrideEnumerator(T[] data, int start, int count, int stride = 1) {
            int end = _start + count * _stride;
#if DEBUG
            if (data == null)
                throw new ArgumentNullException("data");
            if (start >= data.Length)
                throw new ArgumentOutOfRangeException("start");
            if (end >= data.Length)
                throw new ArgumentOutOfRangeException("count");
            if (stride < 1)
                throw new ArgumentOutOfRangeException("stride");
#endif
            _data = data;
            _start = start;
            _stride = stride;
            _end = end - _stride;
            Reset();
        }

        #region IEnumerator<T> Membres

        public T Current {
            get {
#if DEBUG
                if (_pos < _start || _end < _pos)
                    throw new InvalidOperationException();
#endif
                return _data[_pos];
            }
        }

        #endregion

        #region IDisposable Membres

        public void Dispose() {
        }

        #endregion

        #region IEnumerator Membres

        object System.Collections.IEnumerator.Current {
            get { return Current; }
        }

        public bool MoveNext() {
            if (_pos < _end) {
                _pos += _stride;
                return true;
            }
            return false;
        }

        public void Reset() {
            _pos = _start - _stride;
        }

        #endregion
    }
}
