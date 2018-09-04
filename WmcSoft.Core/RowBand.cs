#region Licence

/****************************************************************************
          Copyright 1999-2018 Vincent J. Jacquet.  All rights reserved.

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
using System.Collections;

namespace WmcSoft
{
    public struct RowBand<T> : IReadOnlyList<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            private readonly T[,] storage;
            private readonly int n;
            private readonly int N;
            private int index;
            private T current;

            public Enumerator(T[,] storage, int n)
            {
                if (storage == null) throw new ArgumentNullException(nameof(storage));
                if (n < 0 || n > storage.GetLength(0)) throw new ArgumentOutOfRangeException(nameof(n));

                this.storage = storage;
                this.n = n;
                N = storage.GetLength(1);
                index = 0;
                current = default;
            }

            public T Current => current;

            object IEnumerator.Current {
                get {
                    if (n == 0 || n > N)
                        throw new InvalidOperationException();
                    return current;
                }
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (index < N) {
                    current = storage[n, index++];
                    return true;
                }
                current = default;
                return false;
            }

            public void Reset()
            {
                index = -1;
                current = default;
            }
        }

        private readonly T[,] storage;
        private readonly int n;

        public RowBand(T[,] storage, int n)
        {
            if (storage == null) throw new ArgumentNullException(nameof(storage));
            if (n < 0 || n > storage.GetLength(0)) throw new ArgumentOutOfRangeException(nameof(n));

            this.storage = storage;
            this.n = n;
        }

        public T this[int index] {
            get => storage[n, index];
            //set => storage[n, index] = value;
        }

        public int Count => storage.GetLength(1);

        public Enumerator GetEnumerator()
        {
            return new Enumerator(storage, n);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
