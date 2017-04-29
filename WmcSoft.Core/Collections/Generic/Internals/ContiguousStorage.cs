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
using System.Diagnostics;

namespace WmcSoft.Collections.Generic.Internals
{
    /// <summary>
    /// Utitilies to manipulate contiguous storage.
    /// </summary>
    /// <typeparam name="T">The type of stored items.</typeparam>
    [DebuggerDisplay("Count = {_count}")]
    [DebuggerTypeProxy(typeof(ContiguousStorage<>.DebugView))]
    internal class ContiguousStorage<T>
    {
        class DebugView
        {
            private readonly ContiguousStorage<T> _storage;

            DebugView(ContiguousStorage<T> storage)
            {
                _storage = storage;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public T[] Items {
                get {
                    var array = new T[_storage._count];
                    _storage._storage.CopyTo(array, 0);
                    return array;
                }
            }
        }

        public const int MaxArrayLength = 0X7FEFFFFF; // taken from http://referencesource.microsoft.com/#mscorlib/system/array.cs,2d2b551eabe74985
        const int DefaultCapacity = 4;
        const int QuarterOfDefaultCapacity = 1;

        public static readonly T[] Empty = new T[0];

        #region Base implementation

        protected T[] _storage;
        protected int _count;

        protected ContiguousStorage()
        {
            _storage = Empty;
        }

        protected ContiguousStorage(int capacity)
        {
            _storage = (capacity > 0)
                ? new T[capacity]
                : Empty;
        }

        protected void EnsureOne()
        {
            if (_count == _storage.Length)
                Reserve(ref _storage, 1, Copy);
        }
        protected void Ensure(int count)
        {
            var requires = _storage.Length - _count - count;
            if (requires < 0)
                Reserve(ref _storage, -requires, Copy);
        }

        protected virtual void Copy(T[] source, T[] destination, int length)
        {
            Array.Copy(source, destination, length);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Resizes the array to the specified length, copying the first count items.
        /// </summary>
        /// <param name="items">The array of items.</param>
        /// <param name="length">The expected length.</param>
        /// <param name="count">The count of items to copy.</param>
        /// <remarks>Expects count < length.</remarks>
        public static void Resize(ref T[] items, int length, int count)
        {
            Debug.Assert(count < length);

            var buffer = new T[length];
            Array.Copy(items, buffer, count);
            items = buffer;
        }

        public static void Resize(ref T[] items, int length, int count, Action<T[], T[], int> copy)
        {
            Debug.Assert(count < length);

            var buffer = new T[length];
            copy(items, buffer, count);
            items = buffer;
        }

        /// <summary>
        /// Truncates the array to the specified length.
        /// </summary>
        /// <param name="items">The array of items.</param>
        /// <param name="length">The expected length.</param>
        /// <remarks>Expects items.Length > length.</remarks>
        public static void Truncate(ref T[] items, int length)
        {
            Debug.Assert(items.Length > length);

            var buffer = new T[length];
            Array.Copy(items, buffer, length);
            items = buffer;
        }

        /// <summary>
        /// Reserves space for <paramref name="n"/> more items.
        /// </summary>
        /// <param name="items">The array of items to grow.</param>
        /// <param name="n">The extra space to reserve.</param>
        /// <remarks>Expects n > 0</remarks>
        public static void Reserve(ref T[] items, int n)
        {
            Debug.Assert(n > 0);

            var length = items.Length;
            n += length;

            var capacity = length == 0 ? DefaultCapacity : length * 2;
            if ((uint)capacity > MaxArrayLength) capacity = MaxArrayLength;
            if (capacity < n) capacity = n;

            Resize(ref items, capacity, length);
        }

        public static void Reserve(ref T[] items, int n, Action<T[], T[], int> copy)
        {
            Debug.Assert(n > 0);

            var length = items.Length;
            n += length;

            var capacity = length == 0 ? DefaultCapacity : length * 2;
            if ((uint)capacity > MaxArrayLength) capacity = MaxArrayLength;
            if (capacity < n) capacity = n;

            Resize(ref items, capacity, length, copy);
        }

        /// <summary>
        /// Shrinks the array so it has enough room for at most twice as many items.
        /// </summary>
        /// <param name="items">The array of items to shrink.</param>
        /// <param name="count">The count of items.</param>
        public static void Shrink(ref T[] items, int count)
        {
            var length = items.Length;
            if (length > QuarterOfDefaultCapacity) {
                if (count <= length / 4)
                    Resize(ref items, Math.Min(length / 2, count * 2), count);
            }
        }

        public static void Fill(T[] items, int startIndex, int count, T value = default(T))
        {
            var endIndex = startIndex + count;
            while (startIndex < endIndex)
                items[startIndex++] = value;
        }

        #endregion
    }
}