using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Collections
{
    public sealed class ReadOnlyCollectionOfObjectAdapter : ICollection<object>, IReadOnlyCollection<object>
    {
        #region fields

        readonly ICollection _collection;

        #endregion

        #region Lifecycle

        public ReadOnlyCollectionOfObjectAdapter(ICollection collection) {
            _collection = collection;
        }

        #endregion

        #region ICollection<object> Members

        public void Add(object item) {
            throw new NotSupportedException();
        }

        public void Clear() {
            throw new NotSupportedException();
        }

        public bool Contains(object item) {
            if (item == null) {
                foreach (var e in _collection)
                    if (e == null)
                        return true;
            } else {
                foreach (var e in _collection)
                    if (item.Equals(e))
                        return true;
            }
            return false;
        }

        public void CopyTo(object[] array, int arrayIndex) {
            foreach (var item in _collection)
                array[arrayIndex++] = item;
        }

        public bool IsReadOnly {
            get { return true; }
        }

        public bool Remove(object item) {
            throw new NotSupportedException();
        }

        #endregion

        #region IReadOnlyCollection<object> Members

        public int Count {
            get { return _collection.Count; }
        }

        #endregion

        #region IEnumerable<object> Members

        public IEnumerator<object> GetEnumerator() {
            foreach (var item in _collection)
                yield return item;
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator() {
            return _collection.GetEnumerator();
        }

        #endregion
    }
}
