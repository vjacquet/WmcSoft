using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Collections.Generic
{
    [DebuggerTypeProxy(typeof(SortedCollectionDebugView<>))]
    [DebuggerDisplay("Count = {Count}")]
    public class SortedCollection<T> : ICollection<T>
    {
        #region Fields

        private readonly List<T> _storage;
        private readonly IComparer<T> _comparer;

        #endregion

        #region Lifecycle

        public SortedCollection(IComparer<T> comparer) {
            _storage = new List<T>();
            _comparer = comparer;
        }

        public SortedCollection()
            : this(Comparer<T>.Default) {
        }

        #endregion

        #region Properties

        public IComparer<T> Comparer {
            get { return _comparer; }
        }

        #endregion

        #region ICollection<T> Membres

        public void Add(T item) {
            int index = _storage.BinarySearch(item, _comparer);
            if (index >= 0) {
                do {
                    index++;
                } while (index < _storage.Count && _comparer.Compare(_storage[index], item) == 0);
                _storage.Insert(index, item);
            } else {
                _storage.Insert(~index, item);
            }
        }

        public void Clear() {
            _storage.Clear();
        }

        public bool Contains(T item) {
            return _storage.BinarySearch(item, _comparer) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex) {
            _storage.CopyTo(array, arrayIndex);
        }

        public int Count {
            get { return _storage.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public bool Remove(T item) {
            int index = _storage.BinarySearch(item, _comparer);
            if (index < 0)
                return false;
            _storage.RemoveAt(index);
            return true;
        }

        #endregion

        #region IEnumerable<T> Membres

        public IEnumerator<T> GetEnumerator() {
            return _storage.GetEnumerator();
        }

        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }

    internal class SortedCollectionDebugView<T>
    {
        private SortedCollection<T> _collection;

        public SortedCollectionDebugView(SortedCollection<T> collection) {
            if (collection == null)
                throw new ArgumentNullException("set");

            _collection = collection;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items {
            get {
                return _collection.ToArray();
            }
        }
    }
}
