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
    /// Represents a generic collection over a readonly underlying collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public class CopyOnWriteCollection<T> : ICollection<T>
    {
        private readonly ICollection<T> _source;
        private ICollection<T> _inner;

        public CopyOnWriteCollection(ICollection<T> collection) {
            _inner = _source = collection;
        }

        private ICollection<T> Readable {
            get { return _inner; }
        }

        private ICollection<T> Writtable {
            get {
                if (ReferenceEquals(_inner, _source))
                    _inner = new List<T>(_source);
                return _inner;
            }
        }

        public int Count {
            get { return Readable.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
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

        public IEnumerator<T> GetEnumerator() {
            return Readable.GetEnumerator();
        }

        public bool Remove(T item) {
            return Writtable.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Readable.GetEnumerator();
        }
    }
}
