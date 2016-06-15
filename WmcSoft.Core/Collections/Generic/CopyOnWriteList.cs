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

using System.Collections;
using System.Collections.Generic;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Represents a generic list over a readonly underlying list.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the list.</typeparam>
    public class CopyOnWriteList<T> : IList<T>
    {
        private readonly IList<T> _source;
        private IList<T> _inner;

        public CopyOnWriteList(IList<T> list) {
            _inner = _source = list;
        }

        private IList<T> Readable {
            get { return _inner; }
        }

        private IList<T> Writtable {
            get {
                if (ReferenceEquals(_inner, _source))
                    _inner = new List<T>(_source); // copy
                return _inner;
            }
        }

        public int Count {
            get { return Readable.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public T this[int index] {
            get { return Readable[index]; }
            set { Writtable[index] = value; }
        }

        public int IndexOf(T item) {
            return Readable.IndexOf(item);
        }

        public void Insert(int index, T item) {
            Writtable.Insert(index, item);
        }

        public void RemoveAt(int index) {
            Writtable.RemoveAt(index);
        }

        public void Add(T item) {
            Writtable.Add(item);
        }

        public void Clear() {
            Writtable.Clear();
        }

        public bool Contains(T item) {
            return Readable.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            Readable.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item) {
            return Writtable.Remove(item);
        }

        public IEnumerator<T> GetEnumerator() {
            return Readable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Readable.GetEnumerator();
        }
    }
}