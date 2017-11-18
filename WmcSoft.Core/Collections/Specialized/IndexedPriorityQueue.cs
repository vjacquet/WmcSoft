#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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
using System.Runtime.InteropServices;
using WmcSoft.Collections.Generic.Internals;

namespace WmcSoft.Collections.Specialized
{
    /// <summary>
    /// Represents an indexed priority queue. The First element is the biggest.
    /// </summary>
    /// <typeparam name="T">The type of the element in the priority queue.</typeparam>
    /// <remarks>Based on Robert Sedgewick & Kevin Wayne, Algorithms, Fourth edition, Page 320.</remarks> 
    [Serializable]
    [ComVisible(true)]
    [DebuggerDisplay("Count={Count,nq}")]
    [DebuggerTypeProxy(typeof(IndexedPriorityQueue<>.DebugView))]
    public class IndexedPriorityQueue<T> : IReadOnlyCollection<T>, ICloneable
    {
        internal class DebugView
        {
            private readonly IndexedPriorityQueue<T> _priorityQueue;

            public DebugView(IndexedPriorityQueue<T> priorityQueue)
            {
                if (priorityQueue == null) throw new ArgumentNullException(nameof(priorityQueue));
                _priorityQueue = priorityQueue;
            }

            public T[] Items { get { return _priorityQueue._items; } }

            public Array PQ {
                get {
                    var length = _priorityQueue.Count;
                    var array = Array.CreateInstance(typeof(int), new[] { length }, new[] { 1 });
                    Array.Copy(_priorityQueue._pq, 1, array, 1, length);
                    return array;
                }
            }

            public int[] QP { get { return _priorityQueue._qp; } }
        }

        #region Class Enumerator

        [Serializable]
        public struct Enumerator : IEnumerator<T>
        {
            readonly IndexedPriorityQueue<T> _priorityQueue;
            readonly int _version;
            IndexedPriorityQueue<T> _clone;
            T _current;

            public Enumerator(IndexedPriorityQueue<T> heap)
            {
                _priorityQueue = heap;
                _clone = new IndexedPriorityQueue<T>(_priorityQueue);
                _version = heap._version;
                _current = default(T);
            }

            #region IEnumerator<T> Members

            public T Current { get { return _current; } }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
            }

            #endregion

            #region Membres de IEnumerator

            public void Reset()
            {
                if (_version != _priorityQueue._version) throw new InvalidOperationException();

                _clone = new IndexedPriorityQueue<T>(_priorityQueue);
                _current = default(T);
            }

            object IEnumerator.Current {
                get { return Current; }
            }

            public bool MoveNext()
            {
                if (_version != _priorityQueue._version) throw new InvalidOperationException();

                if (_clone.Count > 0) {
                    _current = _clone.TopPriority;
                    _clone.Dequeue();
                    return true;
                }
                return false;
            }

            #endregion
        }

        #endregion

        readonly IComparer<T> _comparer;
        T[] _items;
        int[] _pq;
        int[] _qp;
        int _count;
        int _version;

        #region Lifetime

        protected IndexedPriorityQueue()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Heap class that contains elements 
        /// copied from the specified collection, has the same initial capacity 
        /// as the number of elements copied and is sorted according to the 
        /// IComparable interface implemented by each key.
        /// </summary>
        /// <param name="collection"></param>
        public IndexedPriorityQueue(ICollection<T> collection)
            : this(collection, Comparer<T>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Heap class that is empty, has 
        /// the specified initial capacity and is sorted according to the 
        /// IComparable interface implemented by each key added to the Heap.
        /// </summary>
        /// <param name="capacity"></param>
        public IndexedPriorityQueue(int capacity)
            : this(Comparer<T>.Default, capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Heap class that is empty, has the 
        /// specified initial capacity and is sorted according to the specified 
        /// IComparer interface.
        /// </summary>
        /// <param name="comparer"></param>
        /// <param name="capacity"></param>
        public IndexedPriorityQueue(IComparer<T> comparer, int capacity)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));

            _comparer = comparer ?? Comparer<T>.Default;
            _items = new T[capacity];
            _pq = new int[capacity + 1];
            _qp = new int[capacity];
        }

        static int ExtractCount(ICollection<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (collection.Count == 0) throw new ArgumentException(nameof(collection));

            return collection.Count;
        }
        /// <summary>
        /// Initializes a new instance of the Heap class that contains elements 
        /// copied from the specified collection, has the same initial capacity 
        /// as the number of elements copied and is sorted according to the 
        /// specified IComparer interface.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="comparer"></param>
        public IndexedPriorityQueue(ICollection<T> collection, IComparer<T> comparer)
            : this(comparer, ExtractCount(collection))
        {

            _items = new T[collection.Count + 1];
            int i = 0;
            foreach (T item in collection) {
                if (item == null) throw new ArgumentNullException("value");

                UnguardedEnqueue(i++, item);
            }
        }

        private IndexedPriorityQueue(IndexedPriorityQueue<T> pq)
        {
            _comparer = pq._comparer;
            _count = pq._count;
            _items = (T[])pq._items.Clone();
            _pq = (int[])pq._pq.Clone();
            _qp = (int[])pq._qp.Clone();
        }

        #endregion

        public bool Contains(int i)
        {
            return _qp[i] != 0;
        }

        public int Count {
            get { return _count; }
        }

        bool Prioritized(T x, T y)
        {
            return _comparer.Compare(x, y) < 0;
        }
        bool PrioritizedIndex(int i, int j)
        {
            return Prioritized(_items[_pq[i]], _items[_pq[j]]);
        }

        void Swap(int i, int j)
        {
            var tmp = _pq[i];
            _pq[i] = _pq[j];
            _pq[j] = tmp;
            _qp[_pq[i]] = i;
            _qp[_pq[j]] = j;
        }

        /// <summary>
        /// Reorganizes down up.
        /// </summary>
        /// <param name="k">The index</param>
        void Swim(int k)
        {
            while (k > 1 && PrioritizedIndex(k / 2, k)) {
                Swap(k, k / 2);
            }
        }

        /// <summary>
        /// Reorganizes up down
        /// </summary>
        /// <param name="k"></param>
        void Sink(int k)
        {
            var n = _count;
            while (2 * k <= n) {
                var j = k * 2;
                if (j < n && PrioritizedIndex(j, j + 1))
                    ++j;
                if (!PrioritizedIndex(k, j))
                    break;
                Swap(k, j);
                k = j;
            }
        }

        void UnguardedEnqueue(int i, T value)
        {
            _count++;
            _qp[i] = _count;
            _pq[_count] = i;
            _items[i] = value;
            Swim(_count);
        }

        public void Enqueue(int i, T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (Contains(i)) throw new ArgumentException(nameof(i));

            UnguardedEnqueue(i, value);
            ++_version;
        }

        /// <summary>
        /// Changes the value associated with the index <paramref name="i"/>.
        /// </summary>
        /// <param name="i">The index.</param>
        /// <param name="value">The new value.</param>
        /// <return>The old value.</return>
        public T ChangeValue(int i, T value)
        {
            if (!Contains(i)) throw new ArgumentOutOfRangeException(nameof(i));

            var result = _items[i];
            ++_version;
            _items[i] = value;
            Swim(_qp[i]);
            Sink(_qp[i]);
            return result;
        }

        public T IncreaseValue(int i, T value)
        {
            if (!Contains(i)) throw new ArgumentOutOfRangeException(nameof(i));

            var result = _items[i];
            if (!Prioritized(result, value))
                throw new ArgumentException(nameof(value));
            ++_version;
            _items[i] = value;
            Swim(_qp[i]);
            return result;
        }

        public T DecreaseValue(int i, T value)
        {
            if (!Contains(i)) throw new ArgumentOutOfRangeException(nameof(i));

            var result = _items[i];
            if (Prioritized(result, value) || !Prioritized(value, result))
                throw new ArgumentException(nameof(value));
            ++_version;
            _items[i] = value;
            Sink(_qp[i]);
            return result;
        }

        /// <summary>
        /// Removes the value associated with the index <paramref name="i"/>.
        /// </summary>
        /// <param name="i">The index.</param>
        /// <returns><c>true</c> if the value was removed; otherwise <c>false</c>.</returns>
        public bool Remove(int i)
        {
            if (Contains(i)) {
                int index = _qp[i];
                Swap(index, _count--);
                Swim(index);
                Sink(index);
                _items[i] = default(T);
                _qp[i] = 0;
                return true;
            }
            return false;
        }

        int UnguardedDequeue()
        {
            var index = _pq[1];
            ++_version;
            Swap(1, _count--);
            Sink(1);
            _qp[index] = 0;
            _items[index] = default(T);
            _pq[_count + 1] = -1;
            return index;
        }

        public int Dequeue()
        {
            if (_count < 1) throw new InvalidOperationException();

            return UnguardedDequeue();
        }

        public int Dequeue(out T value)
        {
            if (_count < 1) throw new InvalidOperationException();

            value = _items[_pq[1]];
            return UnguardedDequeue();
        }

        public int Peek()
        {
            if (_count < 1) throw new InvalidOperationException();

            return _pq[1];
        }

        public T TopPriority {
            get {
                if (_count < 1) throw new InvalidOperationException();

                return _items[_pq[1]];
            }
        }

        public T this[int i] {
            get { return _items[i]; }
        }

        public bool Contains(T value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return Array.IndexOf(_items, value, 0) >= 0;
        }

        public void Clear()
        {
            ContiguousStorage<T>.Fill(_items, 1, _count);
            _count = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Capacity {
            get { return _items.Length - 1; }
        }

        #region IEnumerable Members

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region ICloneable Members

        protected virtual object Clone(Cloning.Shallow strategy)
        {
            var clone = (IndexedPriorityQueue<T>)MemberwiseClone();
            if (_items != null) {
                clone._items = _items.Clone<T[]>();
                clone._pq = _pq.Clone<int[]>();
                clone._qp = _qp.Clone<int[]>();
            }
            return clone;
        }

        object ICloneable.Clone()
        {
            return Clone(Cloning.SuggestShallow);
        }

        #endregion
    }
}
