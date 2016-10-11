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

namespace WmcSoft.Collections.Generic.Internals
{
    internal class BulkQueue<T> : ContiguousStorage<T>
    {
        int _head;
        int _tail;

        public BulkQueue() : base() {
        }

        public BulkQueue(int capacity) : base(capacity) {
        }

        protected override void Copy(T[] source, T[] destination, int length) {
            if (_count > 0) {
                if (_head < _tail) {
                    Array.Copy(source, _head, destination, 0, _count);
                } else {
                    Array.Copy(source, _head, destination, 0, source.Length - _head);
                    Array.Copy(source, 0, destination, source.Length - _head, _tail);
                }
            }
            _head = 0;
            _tail = (_count == destination.Length) ? 0 : _count;
        }
        public bool IsEmpty() {
            return _count == 0;
        }

        public void Enqueue(T item) {
            EnsureOne();

            _storage[_tail] = item;
            _tail = (_tail + 1) % _storage.Length;
            _count++;
        }

        public void BulkEnqueue(int count, Action<T[], int> action) {
            Ensure(count);
            action(_storage, _count);
            _count += count;
        }

        public T Peek() {
            if (IsEmpty()) throw new InvalidOperationException();

            return _storage[_head];
        }

        public T Dequeue() {
            var head = Peek();
            _storage[_head] = default(T);
            _head = (_head + 1) % _storage.Length;
            _count--;
            return head;
        }
    }
}
