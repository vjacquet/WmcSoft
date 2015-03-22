using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Collections.Generic
{
    public class SortedCollectionSet<T> : ISet<T>
    {
        #region Fields

        private List<T> _storage;
        private readonly IComparer<T> _comparer;

        #endregion

        #region Lifecycle


        public SortedCollectionSet(IComparer<T> comparer) {
            _storage = new List<T>();
            _comparer = comparer;
        }

        public SortedCollectionSet()
            : this(Comparer<T>.Default) {
        }

        public SortedCollectionSet(IEnumerable<T> enumerable, IComparer<T> comparer)
            : this(comparer) {
            _storage.AddRange(enumerable);
            _storage.Sort(comparer);
        }

        public SortedCollectionSet(IEnumerable<T> enumerable)
            : this(enumerable, Comparer<T>.Default) {
        }

        #endregion

        #region Properties

        public IComparer<T> Comparer {
            get { return _comparer; }
        }

        #endregion

        #region ISet<T> Membres

        public bool Add(T item) {
            int index = _storage.BinarySearch(item, _comparer);
            if (index >= 0)
                return false;
            _storage.Insert(~index, item);
            return true;
        }

        public void ExceptWith(IEnumerable<T> other) {
            if (other == null)
                throw new ArgumentNullException("other");

            foreach (var item in other) {
                Remove(item);
            }
        }

        public void IntersectWith(IEnumerable<T> other) {
            if (other == null)
                throw new ArgumentNullException("other");

            // handle empty cases
            if (_storage.Count == 0)
                return;
            var traits = new EnumerableTraits<T>(other, Comparer);
            if (traits.HasCount && traits.Count == 0) {
                Clear();
                return;
            }
            if (traits.IsSorted) {
                if (!traits.IsSet)
                    other = SetUtilities.Unique(other, new EqualityComparerAdapter<T>(Comparer));

                var list = new List<T>(Math.Min(traits.Count, Count));
                list.AddRange(SetUtilities.Intersection(this, other));
                _storage = list;
                return;
            }

            // do not know anything about the enumerable
            var tmp = new SortedCollectionSet<T>(Comparer);
            foreach (var item in other) {
                if (Contains(item))
                    tmp.Add(item);
            }
            _storage = tmp._storage;
        }

        public bool IsProperSubsetOf(IEnumerable<T> other) {
            if (other == null)
                throw new ArgumentNullException("other");

            var tmp = new SortedCollectionSet<T>(other, Comparer);
            foreach (var item in this) {
                if (!tmp.Remove(item))
                    return false;
            }
            return tmp.Count != 0;
        }

        public bool IsProperSupersetOf(IEnumerable<T> other) {
            if (other == null)
                throw new ArgumentNullException("other");

            var tmp = new SortedCollectionSet<T>(other, Comparer);
            return tmp.IsProperSubsetOf(this);
        }

        public bool IsSubsetOf(IEnumerable<T> other) {
            if (other == null)
                throw new ArgumentNullException("other");

            var tmp = new SortedCollectionSet<T>(other, Comparer);
            foreach (var item in this) {
                if (!tmp.Remove(item))
                    return false;
            }
            return true;
        }

        public bool IsSupersetOf(IEnumerable<T> other) {
            if (other == null)
                throw new ArgumentNullException("other");

            var tmp = new SortedCollectionSet<T>(other, Comparer);
            return tmp.IsSubsetOf(this);
        }

        public bool Overlaps(IEnumerable<T> other) {
            if (other == null)
                throw new ArgumentNullException("other");

            // handle empty cases
            if (_storage.Count == 0)
                return false;
            var traits = new EnumerableTraits<T>(other);
            var collection = other as ICollection<T>;
            if (traits.HasCount && traits.Count == 0)
                return false;

            foreach (var item in other) {
                if (Contains(item))
                    return true;
            }

            return false;
        }

        public bool SetEquals(IEnumerable<T> other) {
            if (other == null)
                throw new ArgumentNullException("other");

            var tmp = new SortedCollectionSet<T>(this, Comparer);
            foreach (var item in other) {
                if (!tmp.Remove(item))
                    return false;
            }
            return tmp.Count == 0;
        }

        public void SymmetricExceptWith(IEnumerable<T> other) {
            if (other == null)
                throw new ArgumentNullException("other");

            var tmp = new SortedCollectionSet<T>(other, Comparer);
            foreach (var item in tmp) {
                if (!Remove(item))
                    Add(item);
            }
        }

        public void UnionWith(IEnumerable<T> other) {
            if (other == null)
                throw new ArgumentNullException("other");

            var traits = new EnumerableTraits<T>(other, Comparer);
            if (traits.HasCount && traits.Count == 0) {
                return;
            }
            if (traits.IsSorted) {
                if (!traits.IsSet)
                    other = SetUtilities.Unique(other, new EqualityComparerAdapter<T>(Comparer));

                var list = new List<T>(traits.Count + Count);
                list.AddRange(SetUtilities.Union(this, other));
                _storage = list;
                return;
            }

            foreach (var item in other)
                Add(item);
        }

        #endregion

        #region ICollection<T> Membres

        void ICollection<T>.Add(T item) {
            Add(item);
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
}