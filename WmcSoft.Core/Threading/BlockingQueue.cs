/*
 * Les coulisses du CLR: 9 structures et algorithmes de données parallèles réutilisables
 * 
 * http://msdn.microsoft.com/msdnmag/issues/07/05/CLRInsideOut/default.aspx?loc=fr
 * 
 */

// 2009-02-26 VJA
//  - renamed fields using this library conventions;
//  - added constructors to allow parametrize maxBufferSize;
// 2015-03-24 VJA
//  - added readonly where applicable.
//  - moved Cell as internal class.

using System.Collections.Generic;
using System.Threading;

namespace WmcSoft.Threading
{
    public class BlockingQueue<T>
    {
        class Cell
        {
            internal T _obj;
            internal Cell(T obj) { _obj = obj; }
        }

        private Queue<Cell> _queue = new Queue<Cell>();

        public void Enqueue(T item) {
            Cell c = new Cell(item);

            lock (_queue) {
                _queue.Enqueue(c);
                Monitor.Pulse(_queue);
                Monitor.Wait(_queue);
            }
        }

        public T Dequeue() {
            Cell c;
            lock (_queue) {
                while (_queue.Count == 0)
                    Monitor.Wait(_queue);
                c = _queue.Dequeue();
                Monitor.Pulse(_queue);
            }
            return c._obj;
        }

        //public bool TryDequeue(out T item, TimeSpan timeout) {
        //    Cell<T> c;
        //    lock (_queue) {
        //        if (_queue.Count == 0) {
        //            if (!Monitor.Wait(_queue, timeout) || _queue.Count == 0) {
        //                item = default(T);
        //                return false;
        //            }
        //        }
        //        c = _queue.Dequeue();
        //        Monitor.Pulse(_queue);
        //        item = c._obj;
        //    }
        //    return true;
        //}
    }
}
