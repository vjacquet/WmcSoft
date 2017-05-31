#region Licence

/****************************************************************************
          Copyright 1999-2017 Vincent J. Jacquet.  All rights reserved.

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
using System.Linq;
using System.Text;
using WmcSoft.Collections.Generic;

namespace WmcSoft.Collections.Specialized
{
    [DebuggerDisplay("{ToString(),nq}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public struct NGram<T> : IReadOnlyList<T>, ICollection<T>, IComparable<NGram<T>>, IEquatable<NGram<T>>
    {
        readonly IList<T> _storage;
        readonly int _startIndex;
        readonly int _count;

        public NGram(IList<T> storage, int startIndex, int count)
        {
            _storage = storage;
            _startIndex = startIndex;
            _count = count;
        }

        public NGram(IList<T> storage) : this(storage, 0, storage.Count)
        {
        }

        public NGram(params T[] values)
        {
            _storage = new List<T>(values);
            _startIndex = 0;
            _count = values.Length;
        }

        public int Count { get { return _count; } }

        public bool IsReadOnly { get { return true; } }

        public T this[int index] { get { return _storage[_startIndex + index]; } }

        private IEnumerable<T> Enumerate()
        {
            if (_storage == null)
                return Enumerable.Empty<T>();
            return _storage.Skip(_startIndex).Take(_count);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Enumerate().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            return _storage.IndexOf(item, _startIndex, _count) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _storage.CopyTo(_startIndex, array, arrayIndex, _count);
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('(');
            var endIndex = _startIndex + _count;
            sb.Append(_storage[_startIndex]);
            for (int i = _startIndex + 1; i < endIndex; i++) {
                sb.Append(", ").Append(_storage[i]);
            }
            sb.Append(')');
            return sb.ToString();
        }

        public int CompareTo(NGram<T> other)
        {
            var comparer = Comparer<T>.Default;
            return Enumerate().LexicographicalCompare(other, comparer);
        }

        public bool Equals(NGram<T> other)
        {
            if (Count != other.Count)
                return false;
            var comparer = EqualityComparer<T>.Default;
            return Enumerate().SequenceEqual(other, comparer);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(NGram<T>))
                return false;
            return base.Equals((NGram<T>)obj);
        }

        public override int GetHashCode()
        {
            if (_storage == null || _count == 0)
                return 0;
            return EqualityComparer<T>.Default.CombineHashCodes(Enumerate());
        }
    }
}