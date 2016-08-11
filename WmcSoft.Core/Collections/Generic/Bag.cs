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

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Represents a bag of items.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the bag.</typeparam>
    public class Bag<T> : ICollection<T>
    {
        #region RandomAdapter class

        class RandomAdapter : Random
        {
            public override int Next(int minValue, int maxValue) {
                return maxValue;
            }

            public override int Next(int maxValue) {
                return maxValue;
            }

            public override int Next() {
                throw new NotSupportedException();
            }

            public override void NextBytes(byte[] buffer) {
                throw new NotSupportedException();
            }

            public override double NextDouble() {
                throw new NotSupportedException();
            }
        }

        static readonly Random Default = new RandomAdapter();

        #endregion

        private readonly List<T> _storage;
        private readonly Random _random;

        public Bag() : this(Default) {
        }

        public Bag(int capacity) : this(capacity, Default) {
        }

        public Bag(IEnumerable<T> collection) : this(collection, Default) {
        }

        public Bag(Random random) {
            _storage = new List<T>();
            _random = random;
        }

        public Bag(int capacity, Random random) {
            _storage = new List<T>(capacity);
            _random = random;
        }

        public Bag(IEnumerable<T> collection, Random random) {
            _storage = new List<T>(collection);
            _random = random;
        }

        public int Count {
            get { return _storage.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public void Add(T item) {
            _storage.Add(item);
        }

        public void Clear() {
            _storage.Clear();
        }

        public bool Contains(T item) {
            return _storage.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            _storage.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator() {
            return _storage.GetEnumerator();
        }

        public bool Remove(T item) {
            var index = _storage.IndexOf(item);
            if (index < 0)
                return false;
            var last = Count - 1;
            _storage[index] = _storage[last];
            _storage.RemoveAt(last);
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _storage.GetEnumerator();
        }

        public T Pick() {
            var last = Count - 1;
            var index = _random.Next(last);
            var item = _storage[index];
            _storage[index] = _storage[last];
            _storage.RemoveAt(last);
            return item;
        }
    }
}