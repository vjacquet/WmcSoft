using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
