/*
 * Les coulisses du CLR: 9 structures et algorithmes de données parallèles réutilisables
 * 
 * http://msdn.microsoft.com/msdnmag/issues/07/05/CLRInsideOut/default.aspx?loc=fr
 * 
 */

// 2009-02-26 VJA
//  - renamed fields using this library conventions.
//  - added constructors to allow parametrize maxBufferSize.
//  - added readonly where applicable.
//  - added sealed.

using System;
using System.Collections.Generic;
using System.Threading;

namespace WmcSoft.Threading
{
    public sealed class BoundedBuffer<T>
    {
        private readonly int _maxBufferSize;
        private Queue<T> _queue;
        private int _consumersWaiting;
        private int _producersWaiting;

        public BoundedBuffer()
            : this(128) {
        }

        public BoundedBuffer(int maxBufferSize) {
            _maxBufferSize = maxBufferSize;
            _queue = new Queue<T>(maxBufferSize);
        }

        public void Enqueue(T item) {
            lock (_queue) {
                while (_queue.Count == (_maxBufferSize - 1)) {
                    _producersWaiting++;
                    Monitor.Wait(_queue);
                    _producersWaiting--;
                }
                _queue.Enqueue(item);
                if (_consumersWaiting > 0)
                    Monitor.PulseAll(_queue);
            }
        }

        public T Dequeue() {
            T item;
            lock (_queue) {
                while (_queue.Count == 0) {
                    _consumersWaiting++;
                    Monitor.Wait(_queue);
                    _consumersWaiting--;
                }
                item = _queue.Dequeue();
                if (_producersWaiting > 0)
                    Monitor.PulseAll(_queue);
            }
            return item;
        }

        //public bool TryDequeue(out T item, TimeSpan timeout) {
        //    lock (_queue) {
        //        if (_queue.Count == 0) {
        //            _consumersWaiting++;
        //            if (!Monitor.Wait(_queue, timeout) || _queue.Count == 0) {
        //                item = default(T);
        //                return false;
        //            }
        //            _consumersWaiting--;
        //        }
        //        item = _queue.Dequeue();
        //        if (_producersWaiting > 0)
        //            Monitor.PulseAll(_queue);
        //    }
        //    return true;
        //}
    }
}
