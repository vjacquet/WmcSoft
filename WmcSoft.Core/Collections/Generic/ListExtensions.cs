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
using System.Linq;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Defines the extension methods to the <see cref="List{T}"/> class. This is a static class. 
    /// </summary>
    public static class ListExtensions
    {
        #region IndexOf

        static int UnguardedIndexOf<T>(IList<T> list, T value, int startIndex, int endIndex, IEqualityComparer<T> comparer) {
            for (int i = startIndex; i < endIndex; i++) {
                if (comparer.Equals(list[i], value))
                    return i;
            }
            return -1;
        }

        public static int IndexOf<T>(this IList<T> list, T value, int startIndex, int count) {
            if (list == null) throw new ArgumentNullException("list");
            if (startIndex < 0 || startIndex > list.Count) throw new ArgumentOutOfRangeException("startIndex");
            if (count < 0 || startIndex > (list.Count - count)) throw new ArgumentOutOfRangeException("count");

            var comparer = EqualityComparer<T>.Default;
            return UnguardedIndexOf(list, value, startIndex, startIndex + count, comparer);
        }

        #endregion

        #region Sublist

        class SublistAdapter<T> : IList<T>
        {
            private readonly IList<T> _base;
            private int _startIndex;
            private int _endIndex;

            public SublistAdapter(IList<T> list, int startIndex, int count) {
                _base = list;
                _startIndex = startIndex;
                _endIndex = _startIndex + count;
            }

            static int Clamp(int x, int min, int max) {
                if (x < min)
                    return min;
                if (x > max)
                    return max;
                return x;
            }

            public int Count {
                get {
                    return Clamp(_endIndex - _startIndex, 0, _base.Count - _startIndex);
                }
            }

            public bool IsReadOnly {
                get { return _base.IsReadOnly; }
            }

            public T this[int index] {
                get { return _base[_startIndex + index]; }
                set { _base[_startIndex + index] = value; }
            }

            public int IndexOf(T item) {
                var comparer = EqualityComparer<T>.Default;
                for (int i = _startIndex; i < _endIndex; i++) {
                    if (comparer.Equals(_base[i], item))
                        return i - _startIndex;
                }
                return -1;
            }

            public void Insert(int index, T item) {
                if (index < 0 || index > Count)
                    throw new ArgumentOutOfRangeException();
                _base.Insert(_startIndex + index, item);
                _endIndex++;
            }

            private void DoRemoveAt(int index) {
                _base.RemoveAt(_startIndex + index);
                _endIndex--;
            }

            public void RemoveAt(int index) {
                if (index < 0 || index > Count)
                    throw new ArgumentOutOfRangeException();
                DoRemoveAt(index);
            }

            public void Add(T item) {
                _base.Insert(_endIndex, item);
                _endIndex++;
            }

            public void Clear() {
                while (_endIndex > _startIndex) {
                    _base.RemoveAt(--_endIndex);
                }
            }

            public bool Contains(T item) {
                return IndexOf(item) >= 0;
            }

            public void CopyTo(T[] array, int arrayIndex) {
                var count = Count;
                if (count > 0)
                    _base.CopyTo(_startIndex, array, arrayIndex, count);
            }

            public bool Remove(T item) {
                var index = IndexOf(item);
                if (index < 0)
                    return false;
                DoRemoveAt(index);
                return true;
            }

            public IEnumerator<T> GetEnumerator() {
                return _base.Skip(_startIndex).Take(Count).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }

        public static IList<T> Sublist<T>(this IList<T> list, int startIndex, int count) {
            if (list == null) throw new ArgumentNullException("list");
            if (startIndex < 0 || startIndex > list.Count) throw new ArgumentOutOfRangeException("startIndex");
            if (count < 0 || startIndex > (list.Count - count)) throw new ArgumentOutOfRangeException("count");

            return new SublistAdapter<T>(list, startIndex, count);
        }

        class ReadOnlySublistAdapter<T> : IReadOnlyList<T>
        {
            private readonly IReadOnlyList<T> _base;
            private readonly int _startIndex;
            private readonly int _endIndex;

            public ReadOnlySublistAdapter(IReadOnlyList<T> list, int startIndex, int count) {
                _base = list;
                _startIndex = startIndex;
                _endIndex = _startIndex + count;
            }

            public int Count {
                get { return _endIndex - _startIndex; }
            }

            public T this[int index] {
                get {
                    if (index < _startIndex || index >= _endIndex)
                        throw new IndexOutOfRangeException();
                    return _base[_startIndex + index];
                }
            }

            public IEnumerator<T> GetEnumerator() {
                return _base.Skip(_startIndex).Take(Count).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }

        public static IReadOnlyList<T> ReadOnlySublist<T>(this IReadOnlyList<T> list, int startIndex, int count) {
            if (list == null) throw new ArgumentNullException("list");
            if (startIndex < 0 || startIndex > list.Count) throw new ArgumentOutOfRangeException("startIndex");
            if (count < 0 || startIndex > (list.Count - count)) throw new ArgumentOutOfRangeException("count");

            return new ReadOnlySublistAdapter<T>(list, startIndex, count);
        }

        #endregion

        #region SortBackwards

        /// <summary>
        /// Sorts all the elements in the list in backwards order.
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="source">The source list</param>
        public static void SortBackwards<T>(this List<T> source) {
            SortBackwards(source, Comparer<T>.Default);
        }

        /// <summary>
        /// Sorts all the elements in the list in backwards order, using the given comparison.
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="source">The source list</param>
        /// <param name="comparison">The comparison function.</param>
        public static void SortBackwards<T>(this List<T> source, Comparison<T> comparison) {
            source.Sort((x, y) => comparison(y, x));
        }

        /// <summary>
        /// Sorts all the elements in the list in backwards order, using the given comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="source">The source list</param>
        /// <param name="comparer">The comparer object.</param>
        public static void SortBackwards<T>(this List<T> source, IComparer<T> comparer) {
            source.Sort(new ReverseComparer<T>(comparer));
        }

        /// <summary>
        /// Sorts in backwards order the range of elements from the list, defined by the start index and the count of elements, using the given comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="source">The source list</param>
        /// <param name="index">The start index</param>
        /// <param name="count">The count of element to sort</param>
        /// <param name="comparer">The comparer object.</param>
        public static void SortBackwards<T>(this List<T> source, int index, int count, IComparer<T> comparer) {
            source.Sort(index, count, new ReverseComparer<T>(comparer));
        }

        #endregion
    }
}
