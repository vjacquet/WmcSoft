﻿#region Licence

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
using System.Collections.Generic;

namespace WmcSoft.Collections.Generic.Internals
{
    sealed class BulkQueue<T> : ContiguousStorage<T>
    {
        int _head;
        int _tail;

        public BulkQueue() : base()
        {
        }

        public BulkQueue(int capacity) : base(capacity)
        {
        }

        protected override void Copy(T[] source, T[] destination, int length)
        {
            if (count > 0) {
                if (_head < _tail) {
                    Array.Copy(source, _head, destination, 0, count);
                } else {
                    Array.Copy(source, _head, destination, 0, source.Length - _head);
                    Array.Copy(source, 0, destination, source.Length - _head, _tail);
                }
            }
            _head = 0;
            _tail = (count == destination.Length) ? 0 : count;
        }
        public bool IsEmpty()
        {
            return count == 0;
        }

        public void Enqueue(T item)
        {
            EnsureOne();

            storage[_tail] = item;
            _tail = (_tail + 1) % storage.Length;
            count++;
        }

        public void Enqueue(IEnumerable<T> items)
        {
            var list = new List<T>(items);
            BulkEnqueue(list.Count, list.CopyTo);
        }

        /// <summary>
        /// Enqueues <paramref name="count"/> items in place.
        /// </summary>
        /// <param name="count">The count of items</param>
        /// <param name="action">The action taking as parameters the buffer to copy to and the index where to start copying.</param>
        public void BulkEnqueue(int count, Action<T[], int> action)
        {
            Ensure(count);
            action(storage, base.count);
            base.count += count;
        }

        public T Peek()
        {
            if (IsEmpty()) throw new InvalidOperationException();

            return storage[_head];
        }

        public T Dequeue()
        {
            var head = Peek();
            storage[_head] = default;
            _head = (_head + 1) % storage.Length;
            count--;
            return head;
        }
    }
}
