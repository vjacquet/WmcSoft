using System;
using System.Collections;
using System.Collections.Generic;

namespace WmcSoft.Collections.Generic
{
    class DisposeMonitorEnumerable<T> : IEnumerable<T>, IEnumerator<T>
    {
        readonly IEnumerable<T> _enumerable;
        int _disposed;
        IEnumerator<T> _enumerator;

        public DisposeMonitorEnumerable(IEnumerable<T> enumerable)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            _enumerable = enumerable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            _enumerator = _enumerable.GetEnumerator();
            return this;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public T Current { get { return _enumerator.Current; } }
        object IEnumerator.Current { get { return Current; } }

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            _enumerator.Reset();
        }

        public void Dispose()
        {
            _disposed++;
        }

        public int Tally { get { return _disposed; } }
    }
}
