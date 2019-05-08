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
using System.Collections.Generic;

namespace WmcSoft.Collections.Generic.Internals
{
    sealed class BulkStack<T> : ContiguousStorage<T>
    {
        public BulkStack() : base()
        {
        }

        public BulkStack(int capacity) : base(capacity)
        {
        }

        public bool IsEmpty()
        {
            return count == 0;
        }

        public void Push(T item)
        {
            EnsureOne();
            storage[count++] = item;
        }

        public void Push(IEnumerable<T> items)
        {
            var list = new List<T>(items);
            BulkPush(list.Count, list.CopyTo);
        }

        /// <summary>
        /// Pushes <paramref name="count"/> items in place.
        /// </summary>
        /// <param name="count">The count of items</param>
        /// <param name="action">The action taking as parameters the buffer to copy to and the index where to start copying.</param>
        public void BulkPush(int count, Action<T[], int> action)
        {
            Ensure(count);
            action(storage, base.count);
            base.count += count;
        }

        public T Peek()
        {
            if (IsEmpty()) throw new InvalidOperationException();

            return storage[count - 1];
        }

        public T Pop()
        {
            var top = Peek();
            --count;
            storage[count] = default(T);
            return top;
        }
    }
}