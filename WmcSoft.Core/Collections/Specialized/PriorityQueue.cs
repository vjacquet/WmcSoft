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
using System.Runtime.InteropServices;
using WmcSoft.Collections.Generic.Internals;

namespace WmcSoft.Collections.Specialized
{
    /// <summary>
    /// Represents a priority queue. The First element is the biggest.
    /// </summary>
    /// <typeparam name="T">The type of the element in the priority queue.</typeparam>
    [Serializable]
    [ComVisible(true)]
    public class PriorityQueue<T> : IReadOnlyCollection<T>, ICloneable<PriorityQueue<T>>
    {
        const int MaxCapacity = 0X7FEFFFFE;
        const int DefaultCapacity = 16;

        #region Class PriorityQueueEnumerator

        [Serializable]
        public struct Enumerator : IEnumerator<T>
        {
            readonly PriorityQueue<T> _priorityQueue;
            readonly int _version;
            int _index;

            public Enumerator(PriorityQueue<T> heap) {
                _priorityQueue = heap;
                _version = heap._version;
                _index = 0;
            }

            #region IEnumerator<T> Members

            public T Current {
                get {
                    if (_index < 1) {
                        throw new InvalidOperationException();
                    }
                    return _priorityQueue._items[_index];
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose() {
            }

            #endregion

            #region Membres de IEnumerator

            public void Reset() {
                if (_version != _priorityQueue._version) {
                    throw new InvalidOperationException();
                }
                _index = 0;
            }

            object IEnumerator.Current {
                get { return Current; }
            }

            public bool MoveNext() {
                if (_version != _priorityQueue._version) {
                    throw new InvalidOperationException();
                }
                if (_index < _priorityQueue._count) {
                    ++_index;
                    return true;
                }
                return false;
            }

            #endregion
        }

        #endregion

        #region Class SyncPriorityQueue

        class SyncPriorityQueue : PriorityQueue<T>
        {
            PriorityQueue<T> _heap;

            public SyncPriorityQueue(PriorityQueue<T> heap) {
                _heap = heap;
            }

            public override int Capacity {
                get {
                    lock (_heap.SyncRoot) {
                        return _heap.Capacity;
                    }
                }
                set {
                    lock (_heap.SyncRoot) {
                        _heap.Capacity = value;
                    }
                }
            }

            public override void Clear() {
                lock (_heap.SyncRoot) {
                    _heap.Clear();
                }
            }

            public override PriorityQueue<T> Clone() {
                lock (_heap.SyncRoot) {
                    return new SyncPriorityQueue((PriorityQueue<T>)_heap.Clone());
                }
            }

            public override bool Contains(T value) {
                lock (_heap.SyncRoot) {
                    return _heap.Contains(value);
                }
            }

            public override void CopyTo(Array array, int index) {
                lock (_heap.SyncRoot) {
                    _heap.CopyTo(array, index);
                }
            }

            public override int Count {
                get {
                    lock (_heap.SyncRoot) {
                        return _heap.Count;
                    }
                }
            }

            public override T Dequeue() {
                lock (_heap.SyncRoot) {
                    return _heap.Dequeue();
                }
            }

            public override void Enqueue(T value) {
                lock (_heap.SyncRoot) {
                    _heap.Enqueue(value);
                }
            }

            public override Enumerator GetEnumerator() {
                lock (_heap.SyncRoot) {
                    return _heap.GetEnumerator();
                }
            }

            public override bool IsSynchronized {
                get { return true; }
            }

            public override T Peek() {
                lock (_heap.SyncRoot) {
                    return _heap.Peek();
                }
            }

            public override object SyncRoot {
                get { return _heap.SyncRoot; }
            }

        }
        #endregion

        readonly IComparer<T> _comparer;
        T[] _items;
        int _count;
        int _version;

        #region Lifetime

        /// <summary>
        /// Initializes a new instance of the Heap class that is empty, 
        /// has the default initial capacity and is sorted according to 
        /// the specified IComparer interface.
        /// </summary>
        public PriorityQueue()
            : this(Comparer<T>.Default, 0) {
        }

        /// <summary>
        /// Initializes a new instance of the SortedList class that is empty, 
        /// has the default initial capacity and is sorted according to the 
        /// IComparable interface implemented by each key added to the SortedList.
        /// </summary>
        /// <param name="comparer"></param>
        public PriorityQueue(IComparer<T> comparer)
            : this(comparer, 0) {
        }

        /// <summary>
        /// Initializes a new instance of the Heap class that contains elements 
        /// copied from the specified collection, has the same initial capacity 
        /// as the number of elements copied and is sorted according to the 
        /// IComparable interface implemented by each key.
        /// </summary>
        /// <param name="collection"></param>
        public PriorityQueue(ICollection<T> collection)
            : this(collection, Comparer<T>.Default) {
        }

        /// <summary>
        /// Initializes a new instance of the Heap class that is empty, has 
        /// the specified initial capacity and is sorted according to the 
        /// IComparable interface implemented by each key added to the Heap.
        /// </summary>
        /// <param name="initialCapacity"></param>
        public PriorityQueue(int initialCapacity)
            : this(Comparer<T>.Default, initialCapacity) {
        }

        /// <summary>
        /// Initializes a new instance of the Heap class that is empty, has the 
        /// specified initial capacity and is sorted according to the specified 
        /// IComparer interface.
        /// </summary>
        /// <param name="comparer"></param>
        /// <param name="initialCapacity"></param>
        public PriorityQueue(IComparer<T> comparer, int initialCapacity) {
            if (initialCapacity < 0) throw new ArgumentOutOfRangeException("initialCapacity");

            _comparer = comparer ?? Comparer<T>.Default;
            _items = initialCapacity == 0 ? ContiguousStorage<T>.Empty : new T[initialCapacity + 1];
        }

        /// <summary>
        /// Initializes a new instance of the Heap class that contains elements 
        /// copied from the specified collection, has the same initial capacity 
        /// as the number of elements copied and is sorted according to the 
        /// specified IComparer interface.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="comparer"></param>
        public PriorityQueue(ICollection<T> collection, IComparer<T> comparer) {
            if (collection == null) throw new ArgumentNullException("collection");

            _comparer = comparer ?? Comparer<T>.Default;
            if (collection.Count == 0) {
                _items = ContiguousStorage<T>.Empty;
            } else {
                _items = new T[collection.Count + 1];
                foreach (T item in collection) {
                    if (item == null) throw new ArgumentNullException("value");

                    DoEnqueue(item);
                }
            }
        }

        #endregion

        #region Membres de ICollection

        public virtual bool IsSynchronized {
            get { return false; }
        }

        public virtual int Count {
            get { return _count; }
        }

        public virtual void CopyTo(Array array, int index) {
            Array.Copy(_items, 1, array, index, _count);
        }

        public virtual object SyncRoot {
            get { return this; }
        }

        #endregion

        #region Membres de IEnumerable

        public virtual Enumerator GetEnumerator() {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

        #region ICloneable<T> Members

        public virtual PriorityQueue<T> Clone() {
            var clone = new PriorityQueue<T>(_count);
            clone._count = _count;
            Array.Copy(_items, 1, clone._items, 1, _count);
            return clone;
        }

        #endregion

        #region Membres de ICloneable

        object ICloneable.Clone() {
            return Clone();
        }

        #endregion

        bool Prioritized(T x, T y) {
            return _comparer.Compare(x, y) < 0;
        }

        void ReorganizeDownUp(int index) {
            var value = _items[index];
            while (index > 1 && Prioritized(_items[index / 2], value)) {
                _items[index] = _items[index / 2];
                index /= 2;
            }
            _items[index] = value;
        }

        void ReorganizeUpDown(int index) {
            var value = _items[index];
            var half = _count / 2;
            while (index <= half) {
                var i = index * 2;
                if (i < _count && Prioritized(_items[i], _items[i + 1]))
                    ++i;
                if (Prioritized(_items[i], value))
                    break;
                _items[index] = _items[i];
                index = i;
            }
            _items[index] = value;
        }

        private void DoEnqueue(T value) {
            var index = ++_count;
            _items[index] = value;
            ReorganizeDownUp(index);
        }

        public virtual void Enqueue(T value) {
            if (value == null) throw new ArgumentNullException("value");

            ++_version;
            EnsureCapacity(_count + 1);
            DoEnqueue(value);
        }

        public virtual T Dequeue() {
            if (_count < 1) {
                throw new InvalidOperationException();
            }

            ++_version;

            T value = _items[1];
            _items[1] = _items[_count--];
            ReorganizeUpDown(1);
            return value;
        }

        public virtual T Peek() {
            if (_count < 1)
                throw new InvalidOperationException();

            return _items[1];
        }

        public virtual bool Contains(T value) {
            if (value == null)
                throw new ArgumentNullException("value");

            return Array.IndexOf(_items, value, 1) >= 0;
        }

        public virtual void Clear() {
            ContiguousStorage<T>.Fill(_items, 1, _count);
            _count = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int Capacity {
            get {
                return Math.Max(0, _items.Length - 1);
            }
            set {
                var capacity = Math.Max(0, _items.Length - 1);
                if (value != capacity) {
                    if (value < _count) {
                        throw new ArgumentOutOfRangeException("value");
                    }
                    if (value == 0) {
                        _items = ContiguousStorage<T>.Empty;
                    } else {
                        GrowCapacity(value);
                    }
                }
            }
        }

        private void GrowCapacity(int value) {
            var array = new T[value];
            if (_count > 0) {
                Array.Copy(_items, 0, array, 0, _count);
            }
            _items = array;
        }

        private void EnsureCapacity(int min) {
            var length = _items.Length - 1;
            if (length < min) {
                int capacity = length == -1 ? DefaultCapacity : length * 2;
                // Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
                // Note that this check works even when _items.Length overflowed thanks to the (uint) cast
                if ((uint)capacity > MaxCapacity) capacity = MaxCapacity;
                if (capacity < min) capacity = min;
                GrowCapacity(capacity);
            }
        }

        public static PriorityQueue<T> Synchronized(PriorityQueue<T> heap) {
            if (heap == null) {
                throw new ArgumentNullException("heap");
            }
            return new SyncPriorityQueue(heap);
        }

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
