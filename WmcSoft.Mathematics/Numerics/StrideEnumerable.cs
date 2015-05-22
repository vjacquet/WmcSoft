using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
