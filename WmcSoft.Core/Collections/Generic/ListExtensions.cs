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
        static void Guard<T>(IReadOnlyList<T> list, int startIndex, int count)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (startIndex < 0 || startIndex > list.Count) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0 || startIndex > (list.Count - count)) throw new ArgumentOutOfRangeException(nameof(count));
        }

        static void Guard<T>(IList<T> list, int startIndex, int count)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (startIndex < 0 || startIndex > list.Count) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0 || startIndex > (list.Count - count)) throw new ArgumentOutOfRangeException(nameof(count));
        }

        #region Equals

        static bool UnguardedStartsWith<T>(IReadOnlyList<T> list, IReadOnlyList<T> value, int startIndex, IEqualityComparer<T> comparer)
        {
            var count = value.Count;
            for (int i = 0; i < count; i++, startIndex++) {
                if (!comparer.Equals(list[startIndex], value[i]))
                    return false;
            }
            return true;
        }

        #endregion

        #region IndexOf

        static int UnguardedIndexOf<T>(IReadOnlyList<T> list, T value, int startIndex, int endIndex, IEqualityComparer<T> comparer)
        {
            for (int i = startIndex; i < endIndex; i++) {
                if (comparer.Equals(list[i], value))
                    return i;
            }
            return -1;
        }

        public static int IndexOf<T>(this IList<T> list, T value, int startIndex, int count)
        {
            Guard(list, startIndex, count);

            var comparer = EqualityComparer<T>.Default;
            return UnguardedIndexOf(list.AsReadOnly(), value, startIndex, startIndex + count, comparer);
        }

        static int UnguardedIndexOf<T>(IReadOnlyList<T> list, IReadOnlyList<T> value, int startIndex, int endIndex, IEqualityComparer<T> comparer)
        {
            endIndex -= value.Count - 1;
            if (endIndex < 0)
                return -1;

            while ((startIndex = UnguardedIndexOf(list, value[0], startIndex, endIndex, comparer)) >= 0) {
                if (UnguardedStartsWith(list, value, startIndex, comparer))
                    return startIndex;
                startIndex++;
            }
            return -1;
        }

        public static int IndexOf<T>(this IList<T> list, IReadOnlyList<T> value, int startIndex, int count)
        {
            Guard(list, startIndex, count);
            if (value == null) throw new ArgumentNullException(nameof(value));

            switch (value.Count) {
            case 0:
                return -1;
            case 1:
                return UnguardedIndexOf(list.AsReadOnly(), value[0], startIndex, startIndex + count, EqualityComparer<T>.Default);
            default:
                return UnguardedIndexOf(list.AsReadOnly(), value, startIndex, startIndex + count, EqualityComparer<T>.Default);
            }
        }

        public static int IndexOf<T>(this IList<T> list, IReadOnlyList<T> value)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (value == null) throw new ArgumentNullException(nameof(value));

            switch (value.Count) {
            case 0:
                return -1;
            case 1:
                return UnguardedIndexOf(list.AsReadOnly(), value[0], 0, list.Count, EqualityComparer<T>.Default);
            default:
                return UnguardedIndexOf(list.AsReadOnly(), value, 0, list.Count, EqualityComparer<T>.Default);
            }
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes the first occurrence that matches the conditions defined by the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns><c>true</c> if item is successfully removed; otherwise, <c>false</c>.</returns>
        public static bool Remove<T>(this List<T> list, Predicate<T> match)
        {
            var index = list.FindIndex(match);
            if (index >= 0) {
                list.RemoveAt(index);
                return true;
            }
            return false;
        }

        #endregion

        #region Repeat

        class RepeatedList<T> : IReadOnlyList<T>
        {
            readonly IReadOnlyList<T> _list;
            readonly int _count;

            public RepeatedList(IReadOnlyList<T> list, int times)
            {
                _list = list;
                _count = _list.Count * times;
            }

            public T this[int index] {
                get {
                    if (index < 0 || index >= _count) throw new IndexOutOfRangeException();
                    return _list[index % _list.Count];
                }
            }

            public int Count {
                get { return _count; }
            }

            public IEnumerator<T> GetEnumerator()
            {
                var times = _count / _list.Count;
                while (times != 0) {
                    foreach (var item in _list)
                        yield return item;
                    times--;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public static IReadOnlyList<T> Repeat<T>(this IReadOnlyList<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            switch (count) {
            case 0:
                return new T[0];
            case 1:
                return source;
            default:
                return new RepeatedList<T>(source, count);
            }
        }

        #endregion

        #region Rotation

        static int UnguardedFindRotationPoint<T>(this IList<T> list, IReadOnlyList<T> rotation, int startIndex, int endIndex)
        {
            var doubled = rotation.Repeat(2);
            return UnguardedIndexOf(doubled, list.Sublist(startIndex, endIndex - startIndex).AsReadOnly(), 0, doubled.Count, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Finds the amount by which to rotate <paramref name="list"/> so it equals <paramref name="rotation"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="rotation">The target rotation.</param>
        /// <returns>The amount by wich to rotate the list.</returns>
        public static int FindRotationPoint<T>(this IList<T> list, IReadOnlyList<T> rotation)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (rotation == null) throw new ArgumentNullException("rotation");

            if (rotation.Count != list.Count)
                return -1;
            return UnguardedFindRotationPoint(list, rotation, 0, list.Count);
        }

        /// <summary>
        /// Finds the amount by which to rotate part of <paramref name="list"/> so it equals <paramref name="rotation"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="rotation">The target rotation.</param>
        /// <param name="startIndex">The start index in the list.</param>
        /// <param name="count">The end index in the list.</param>
        /// <returns>The amount by wich to rotate the list.</returns>
        public static int FindRotationPoint<T>(this IList<T> list, IReadOnlyList<T> rotation, int startIndex, int count)
        {
            if (rotation == null) throw new ArgumentNullException("rotation");
            Guard(list, startIndex, count);

            if (rotation.Count != count)
                return -1;
            return UnguardedFindRotationPoint(list, rotation, startIndex, startIndex + count);
        }

        public static bool IsRotation<T>(this IList<T> list, IReadOnlyList<T> rotation)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (rotation == null) throw new ArgumentNullException("rotation");

            if (rotation.Count != list.Count)
                return false;
            return UnguardedFindRotationPoint(list, rotation, 0, list.Count) >= 0;
        }

        public static bool IsRotation<T>(this IList<T> list, IReadOnlyList<T> rotation, int startIndex, int count)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (rotation == null) throw new ArgumentNullException("rotation");

            if (rotation.Count != count)
                return false;
            return UnguardedFindRotationPoint(list, rotation, startIndex, startIndex + count) >= 0;
        }

        #endregion

        #region Sublist

        class SublistAdapter<T> : IList<T>
        {
            private readonly IList<T> _base;
            private int _startIndex;
            private int _endIndex;

            public SublistAdapter(IList<T> list, int startIndex, int count)
            {
                _base = list;
                _startIndex = startIndex;
                _endIndex = _startIndex + count;
            }

            static int Clamp(int x, int min, int max)
            {
                if (x < min)
                    return min;
                if (x > max)
                    return max;
                return x;
            }

            public int Count {
                get { return Clamp(_endIndex - _startIndex, 0, _base.Count - _startIndex); }
            }

            public bool IsReadOnly {
                get { return _base.IsReadOnly; }
            }

            public T this[int index] {
                get { return _base[_startIndex + index]; }
                set { _base[_startIndex + index] = value; }
            }

            public int IndexOf(T item)
            {
                var comparer = EqualityComparer<T>.Default;
                for (var i = _startIndex; i < _endIndex; i++) {
                    if (comparer.Equals(_base[i], item))
                        return i - _startIndex;
                }
                return -1;
            }

            public void Insert(int index, T item)
            {
                if (index < 0 || index > Count)
                    throw new ArgumentOutOfRangeException();
                _base.Insert(_startIndex + index, item);
                _endIndex++;
            }

            private void DoRemoveAt(int index)
            {
                _base.RemoveAt(_startIndex + index);
                _endIndex--;
            }

            public void RemoveAt(int index)
            {
                if (index < 0 || index > Count)
                    throw new ArgumentOutOfRangeException();
                DoRemoveAt(index);
            }

            public void Add(T item)
            {
                _base.Insert(_endIndex, item);
                _endIndex++;
            }

            public void Clear()
            {
                while (_endIndex > _startIndex) {
                    _base.RemoveAt(--_endIndex);
                }
            }

            public bool Contains(T item)
            {
                return IndexOf(item) >= 0;
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                var count = Count;
                if (count > 0)
                    _base.CopyTo(_startIndex, array, arrayIndex, count);
            }

            public bool Remove(T item)
            {
                var index = IndexOf(item);
                if (index < 0)
                    return false;
                DoRemoveAt(index);
                return true;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return _base.Skip(_startIndex).Take(Count).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public static IList<T> Sublist<T>(this IList<T> list, int startIndex, int count)
        {
            Guard(list, startIndex, count);

            return new SublistAdapter<T>(list, startIndex, count);
        }

        class ReadOnlySublistAdapter<T> : IReadOnlyList<T>
        {
            private readonly IReadOnlyList<T> _base;
            private readonly int _startIndex;
            private readonly int _endIndex;

            public ReadOnlySublistAdapter(IReadOnlyList<T> list, int startIndex, int count)
            {
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

            public IEnumerator<T> GetEnumerator()
            {
                return _base.Skip(_startIndex).Take(Count).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public static IReadOnlyList<T> ReadOnlySublist<T>(this IReadOnlyList<T> list, int startIndex, int count)
        {
            Guard(list, startIndex, count);

            return new ReadOnlySublistAdapter<T>(list, startIndex, count);
        }

        #endregion

        #region SortBackwards

        /// <summary>
        /// Sorts all the elements in the list in backwards order.
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="list">The source list</param>
        public static void SortBackwards<T>(this List<T> list)
        {
            SortBackwards(list, Comparer<T>.Default);
        }

        /// <summary>
        /// Sorts all the elements in the list in backwards order, using the given comparison.
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="list">The source list</param>
        /// <param name="comparison">The comparison function.</param>
        public static void SortBackwards<T>(this List<T> list, Comparison<T> comparison)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));

            list.Sort((x, y) => comparison(y, x));
        }

        /// <summary>
        /// Sorts all the elements in the list in backwards order, using the given comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="list">The source list</param>
        /// <param name="comparer">The comparer object.</param>
        public static void SortBackwards<T>(this List<T> list, IComparer<T> comparer)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));

            list.Sort(new ReverseComparer<T>(comparer ?? Comparer<T>.Default));
        }

        /// <summary>
        /// Sorts in backwards order the range of elements from the list, defined by the start index and the count of elements, using the given comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="list">The source list</param>
        /// <param name="startIndex">The start index</param>
        /// <param name="count">The count of element to sort</param>
        /// <param name="comparer">The comparer object.</param>
        public static void SortBackwards<T>(this List<T> list, int startIndex, int count, IComparer<T> comparer)
        {
            Guard((IReadOnlyList<T>)list, startIndex, count);

            list.Sort(startIndex, count, new ReverseComparer<T>(comparer));
        }

        #endregion
    }
}