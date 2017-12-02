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

using static System.Math;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Defines the extension methods to the <see cref="List{T}"/> class.
    /// This is a static class. 
    /// </summary>
    public static class ListExtensions
    {
        static void Guard<T>(IReadOnlyList<T> list, int startIndex, int count)
        {
            if (startIndex < 0 || startIndex > list.Count) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0 || startIndex > (list.Count - count)) throw new ArgumentOutOfRangeException(nameof(count));
        }

        static void Guard<T>(IList<T> list, int startIndex, int count)
        {
            if (startIndex < 0 || startIndex > list.Count) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0 || startIndex > (list.Count - count)) throw new ArgumentOutOfRangeException(nameof(count));
        }

        static int NormalizeZeroBasedIndex(int start, int length)
        {
            return start < 0 ? Max(length + start, 0) : Min(start, length);
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

        #region RemoveFirst / RemoveLast

        /// <summary>
        /// Removes the first occurrence that matches the conditions defined by the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns><c>true</c> if item is successfully removed; otherwise, <c>false</c>.</returns>
        public static bool RemoveFirst<T>(this List<T> list, Predicate<T> match)
        {
            var index = list.FindIndex(match);
            if (index >= 0) {
                list.RemoveAt(index);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the last occurrence that matches the conditions defined by the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns><c>true</c> if item is successfully removed; otherwise, <c>false</c>.</returns>
        public static bool RemoveLast<T>(this List<T> list, Predicate<T> match)
        {
            var index = list.FindLastIndex(match);
            if (index >= 0) {
                list.RemoveAt(index);
                return true;
            }
            return false;
        }

        #endregion

        #region RemoveDuplicates

        /// <summary>
        /// Removes all the duplicates from the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values,
        ///   or <c>null</c> to use the default <see cref="EqualityComparer{T}"/> implementation.</param>
        /// <returns>The number of elements removed from the <see cref="List{T}"/>.</returns>
        public static int RemoveDuplicates<T>(this List<T> list, IEqualityComparer<T> comparer = null)
        {
            var unique = new HashSet<T>(comparer);
            return list.RemoveAll(x => !unique.Add(x));
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

        /// <summary>
        /// Adapts a readonly list to create one with the same elements repeated <paramref name="count"/> times.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="count">The repetition count</param>
        /// <returns>The adapated readonly list.</returns>
        public static IReadOnlyList<T> Repeat<T>(this IReadOnlyList<T> list, int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            switch (count) {
                case 0:
                    return new T[0];
                case 1:
                    return list;
                default:
                    return new RepeatedList<T>(list, count);
            }
        }

        #endregion

        #region Rotation

        static int UnguardedFindRotationPoint<T>(this IList<T> list, IReadOnlyList<T> rotation, int startIndex, int endIndex, IEqualityComparer<T> comparer)
        {
            var doubled = rotation.Repeat(2);
            return UnguardedIndexOf(doubled, list.Sublist(startIndex, endIndex - startIndex).AsReadOnly(), 0, doubled.Count, comparer);
        }

        /// <summary>
        /// Finds the amount by which to rotate list so it equals <paramref name="rotation"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="rotation">The target rotation.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values,
        ///   or <c>null</c> to use the default <see cref="EqualityComparer{T}"/> implementation.</param>
        /// <returns>The amount by wich to rotate the list.</returns>
        public static int FindRotationPoint<T>(this IList<T> list, IReadOnlyList<T> rotation, IEqualityComparer<T> comparer = null)
        {
            if (rotation == null) throw new ArgumentNullException(nameof(rotation));

            if (rotation.Count != list.Count)
                return -1;
            return UnguardedFindRotationPoint(list, rotation, 0, list.Count, comparer ?? EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Finds the amount by which to rotate part of list so it equals <paramref name="rotation"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="rotation">The target rotation.</param>
        /// <param name="startIndex">The start index in the list.</param>
        /// <param name="count">The end index in the list.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values,
        ///   or <c>null</c> to use the default <see cref="EqualityComparer{T}"/> implementation.</param>
        /// <returns>The amount by wich to rotate the list.</returns>
        public static int FindRotationPoint<T>(this IList<T> list, IReadOnlyList<T> rotation, int startIndex, int count, IEqualityComparer<T> comparer = null)
        {
            if (rotation == null) throw new ArgumentNullException(nameof(rotation));
            Guard(list, startIndex, count);

            if (rotation.Count != count)
                return -1;
            return UnguardedFindRotationPoint(list, rotation, startIndex, startIndex + count, comparer ?? EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="rotation"/> is a rotation of the list.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="rotation">The target rotation.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values,
        ///   or <c>null</c> to use the default <see cref="EqualityComparer{T}"/> implementation.</param>
        /// <returns><c>true</c> if <paramref name="rotation"/> is a rotation of the list; otherwise, <c>false</c>.</returns>
        public static bool IsRotation<T>(this IList<T> list, IReadOnlyList<T> rotation, IEqualityComparer<T> comparer = null)
        {
            if (rotation == null) throw new ArgumentNullException(nameof(rotation));

            if (rotation.Count != list.Count)
                return false;
            return UnguardedFindRotationPoint(list, rotation, 0, list.Count, comparer ?? EqualityComparer<T>.Default) >= 0;
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="rotation"/> is a rotation of the sublist of <paramref name="count"/> elements, started at <paramref name="startIndex"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="rotation">The target rotation.</param>
        /// <param name="startIndex">The start index in the list.</param>
        /// <param name="count">The end index in the list.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values,
        ///   or <c>null</c> to use the default <see cref="EqualityComparer{T}"/> implementation.</param>
        /// <returns><c>true</c> if <paramref name="rotation"/> is a rotation of the list; otherwise, <c>false</c>.</returns>
        public static bool IsRotation<T>(this IList<T> list, IReadOnlyList<T> rotation, int startIndex, int count, IEqualityComparer<T> comparer = null)
        {
            if (rotation == null) throw new ArgumentNullException(nameof(rotation));
            Guard(list, startIndex, count);

            if (rotation.Count != count)
                return false;
            return UnguardedFindRotationPoint(list, rotation, startIndex, startIndex + count, comparer ?? EqualityComparer<T>.Default) >= 0;
        }

        #endregion

        #region Slice

        /// <summary>
        /// Returns a shallow copy of a portion of an array into a new array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="start">Zero-based index at which to start copying elements.</param>
        /// <param name="end">Zero-based index at which to end copying elements. The element at <paramref name="end"/> is not copied.</param>
        /// <returns>The shallow copy of the specified portion of the array.</returns>
        public static T[] Slice<T>(this IList<T> list, int start, int end)
        {
            var length = list.Count;
            var k = NormalizeZeroBasedIndex(start, length);
            var final = NormalizeZeroBasedIndex(end, length);
            var count = Max(final - k, 0);
            var result = new T[count];
            list.CopyTo(k, result, 0, count);
            return result;
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
            list.Sort((x, y) => comparison(y, x));
        }

        /// <summary>
        /// Sorts all the elements in the list in backwards order, using the given comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="list">The source list</param>
        /// <param name="comparer">The comparer object.</param>
        public static void SortBackwards<T>(this List<T> list, IComparer<T> comparer = null)
        {
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

        #region Splice

        /// <summary>
        /// Changes the content of the <paramref name="list"/> by removing existing elements and/or adding new elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="start">Zero-based index at which to start changing the list.</param>
        /// <returns>An array containing the removed elements. If no elements are removed, returns an empty array.</returns>
        public static T[] Splice<T>(this IList<T> list, int start)
        {
            start = NormalizeZeroBasedIndex(start, list.Count);
            var count = list.Count - start;
            if (count < 0)
                return new T[0];
            var removed = new T[count];
            for (var i = list.Count - 1; i >= start; i--) {
                removed[--count] = list[i];
                list.RemoveAt(i);
            }
            return removed;
        }

        /// <summary>
        /// Changes the content of the <paramref name="list"/> by removing existing elements and/or adding new elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="start">Zero-Index at which to start changing the list.</param>
        /// <param name="deleteCount">The number of elements to remove.</param>
        /// <param name="items">The items to insert at the <paramref name="start"/> position.</param>
        /// <returns>An array containing the removed elements. If no elements are removed, returns an empty array.</returns>
        public static T[] Splice<T>(this IList<T> list, int start, int deleteCount, params T[] items)
        {
            start = NormalizeZeroBasedIndex(start, list.Count);
            deleteCount = Min(list.Count - start, deleteCount);
            var removed = new T[deleteCount];
            list.CopyTo(start, removed, 0, deleteCount);
            if (deleteCount < items.Length) {
                int i = 0;
                while (i != deleteCount) {
                    list[start++] = items[i++];
                }
                while (i != items.Length) {
                    list.Insert(start++, items[i++]);
                }
            } else {
                for (int i = 0; i < items.Length; i++) {
                    list[start++] = items[i];
                }
                deleteCount -= items.Length;
                while (deleteCount-- != 0)
                    list.RemoveAt(start);
            }
            return removed;
        }

        #endregion

        #region Unique

        public static int UnguardedUnique<T>(List<T> list, int startIndex, int count, Func<T, T, bool> relation)
        {
            if (count == 0)
                return 0;

            var first = startIndex;
            var last = startIndex + count;
            var result = first;
            while (++first != last) {
                if (!relation(list[result], list[first]) && ++result != first)
                    list[result] = list[first];
            }
            last -= result + 1;
            list.RemoveRange(result, last);
            return last;
        }

        /// <summary>
        /// Removes all but the first element from every consecutive group of equal elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values,
        ///   or <c>null</c> to use the default <see cref="EqualityComparer{T}"/> implementation.</param>
        /// <returns>The number of elements removed from the <see cref="List{T}"/>.</returns>
        public static int Unique<T>(this List<T> list, IEqualityComparer<T> comparer = null)
        {
            return Unique(list, (comparer ?? EqualityComparer<T>.Default).Equals);
        }

        /// <summary>
        /// Removes all but the first element from every consecutive group of equivalent elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="relation">The equivalence relation.</param>
        /// <returns>The number of elements removed from the <see cref="List{T}"/>.</returns>
        public static int Unique<T>(this List<T> list, Func<T, T, bool> relation)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));

            return UnguardedUnique(list, 0, list.Count, relation);
        }

        /// <summary>
        /// Removes all but the first element from every consecutive group of equal elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="startIndex">The start index in the list.</param>
        /// <param name="count">The end index in the list.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values,
        ///   or <c>null</c> to use the default <see cref="EqualityComparer{T}"/> implementation.</param>
        /// <returns>The number of elements removed from the <see cref="List{T}"/>.</returns>
        public static int Unique<T>(this List<T> list, int startIndex, int count, IEqualityComparer<T> comparer = null)
        {
            return Unique(list, startIndex, count, (comparer ?? EqualityComparer<T>.Default).Equals);
        }

        /// <summary>
        /// Removes all but the first element from every consecutive group of equivalent elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="startIndex">The start index in the list.</param>
        /// <param name="count">The end index in the list.</param>
        /// <param name="relation">The equivalence relation.</param>
        /// <returns>The number of elements removed from the <see cref="List{T}"/>.</returns>
        public static int Unique<T>(this List<T> list, int startIndex, int count, Func<T, T, bool> relation)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            Guard((IReadOnlyList<T>)list, startIndex, count);

            return UnguardedUnique(list, startIndex, count, relation);
        }

        #endregion
    }
}
