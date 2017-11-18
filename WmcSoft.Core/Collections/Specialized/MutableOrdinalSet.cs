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
using WmcSoft.Collections.Generic;

namespace WmcSoft.Collections.Specialized
{
    public class MutableOrdinalSet<T> : ISet<T>
    {
        class Settings
        {
            public Settings(IOrdinal<T> ordinal, T lowerBound, int extent)
            {
                Ordinal = ordinal;
                LowerBound = lowerBound;
                Extent = extent + 1;
            }

            public readonly IOrdinal<T> Ordinal;
            public readonly T LowerBound;
            public readonly int Extent;

            public T UpperBound { get { return Ordinal.Advance(LowerBound, Extent - 1); } }

            public int IndexOf(T item)
            {
                var n = Ordinal.Distance(LowerBound, item);
                if (n >= 0 && n < Extent)
                    return n;
                return -1;
            }
        }

        private readonly Settings _settings;
        private BitArray _storage;

        #region Constructors

        /// <summary>
        /// Creates a new OrdinalSet instance with a specified lower and upper bound.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <param name="lowerBound">The lower bound for the set.</param>
        /// <param name="upperBound">The upper bound for the set.</param>
        public MutableOrdinalSet(IOrdinal<T> ordinal, T lowerBound, T upperBound)
        {
            // make sure lowerbound is less than or equal to upperbound
            var extent = ordinal.Distance(lowerBound, upperBound);
            if (extent <= 0)
                throw new ArgumentException("The set's lower bound cannot be greater than its upper bound.");
            _settings = new Settings(ordinal, lowerBound, extent);

            // Create the BitArray
            _storage = new BitArray(extent + 1);
        }

        /// <summary>
        /// Creates a new OrdinalSet instance whose initial values are assigned from a collection.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <param name="lowerBound">The lower bound for the set.</param>
        /// <param name="upperBound">The upper bound for the set.</param>
        /// <param name="collection">The collection whose elements are copied to the new set.</param>
        public MutableOrdinalSet(IOrdinal<T> ordinal, T lowerBound, T upperBound, IEnumerable<T> collection)
            : this(ordinal, lowerBound, upperBound)
        {
            // Populuate the BitArray with the passed-in initialData array.
            foreach (var item in collection) {
                var n = _settings.IndexOf(item);
                if (n >= 0)
                    _storage.Set(n, true);
                else
                    throw OutOfRange(item);
            }
        }

        private MutableOrdinalSet(MutableOrdinalSet<T> template, IEnumerable<T> collection)
        {
            _settings = template._settings;
            _storage = new BitArray(_settings.Extent + 1);

            // Populuate the BitArray with the passed-in initialData array but skip out of bounds
            foreach (var item in collection) {
                var n = _settings.IndexOf(item);
                if (n >= 0)
                    _storage.Set(n, true);
            }
        }

        private MutableOrdinalSet(MutableOrdinalSet<T> template, IEnumerable<T> collection, out bool hasOutOfBound)
        {
            _settings = template._settings;
            _storage = new BitArray(_settings.Extent + 1);

            hasOutOfBound = false;
            // Populuate the BitArray with the passed-in initialData array but skip out of bounds
            foreach (var item in collection) {
                var n = _settings.IndexOf(item);
                if (n >= 0)
                    _storage.Set(n, true);
                else
                    hasOutOfBound = true;
            }
        }

        ArgumentException OutOfRange(T value)
        {
            var lowerBound = _settings.LowerBound;
            var upperBound = _settings.UpperBound;
            return new ArgumentOutOfRangeException($"Attempting to add an element with value {value} that is outside of the set's universe.  Value must be between {lowerBound} and {upperBound}.");
        }

        #endregion

        #region Properties

        public IOrdinal<T> Ordinal {
            get { return _settings.Ordinal; }
        }

        #endregion

        #region ISet<T> Membres

        /// <summary>
        /// Adds an element to the set and returns a value that indicates if it was successfully added.
        /// </summary>
        /// <param name="item">The element to add to the set.</param>
        /// <returns>true if item is added to the set; otherwise, false. </returns>
        public bool Add(T item)
        {
            var n = _settings.IndexOf(item);
            if (n >= 0) {
                if (_storage.Get(n))
                    return false;
                _storage.Set(n, true);
                return true;
            }
            throw OutOfRange(item);
        }

        /// <summary>
        /// Removes all elements that are in a specified collection from the current Set.
        /// </summary>
        /// <param name="other">The collection of items to remove from the Set.</param>
        public void ExceptWith(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            foreach (var item in other) {
                Remove(item);
            }
        }


        /// <summary>
        /// Modifies the current Set so that it contains only elements that are also in a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current Set.</param>
        public void IntersectWith(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            // handle empty cases
            if (_storage.Count == 0 || _storage.Cardinality() == 0)
                return;

            var tmp = new BitArray(_storage.Count);
            foreach (var item in other) {
                var n = _settings.IndexOf(item);
                if (n >= 0) {
                    tmp.Set(n, true);
                }
            }
            _storage = tmp;
        }

        /// <summary>
        /// Determines whether a Set is a proper subset of the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current Set.</param>
        /// <returns>true if the Set is a proper subset of other; otherwise, false.</returns>
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            bool hasOutOfBound;
            var tmp = new MutableOrdinalSet<T>(this, other, out hasOutOfBound);
            foreach (var item in this) {
                if (!tmp.Remove(item))
                    return false;
            }
            return tmp.Count != 0 || hasOutOfBound;
        }

        /// <summary>
        /// Determines whether a Set is a proper superset of the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current Set. </param>
        /// <returns>true if the Set is a proper superset of other; otherwise, false.</returns>
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            bool hasOutOfBound;
            var tmp = new MutableOrdinalSet<T>(this, other, out hasOutOfBound);
            return !hasOutOfBound && tmp.IsProperSubsetOf(this);
        }

        /// <summary>
        /// Determines whether a Set is a subset of the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current Set.</param>
        /// <returns>true if the current Set is a subset of other; otherwise, false.</returns>
        public bool IsSubsetOf(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            bool hasOutOfBound;
            var tmp = new MutableOrdinalSet<T>(this, other, out hasOutOfBound);
            foreach (var item in this) {
                if (!tmp.Remove(item))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determines whether a Set is a superset of the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current Set. </param>
        /// <returns>true if the Set is a superset of other; otherwise, false.</returns>
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            var tmp = new MutableOrdinalSet<T>(this, other);
            return tmp.IsSubsetOf(this);
        }

        /// <summary>
        /// Determines whether the current Set and a specified collection share common elements.
        /// </summary>
        /// <param name="other">The collection to compare to the current Set.</param>
        /// <returns>true if the Set and other share at least one common element; otherwise, false.</returns>
        public bool Overlaps(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            // handle empty cases
            if (_storage.Count == 0)
                return false;
            var traits = new EnumerableTraits<T>(other);
            if (traits.HasCount && traits.Count == 0)
                return false;

            foreach (var item in other) {
                if (Contains(item))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the current Set and the specified collection contain the same elements.
        /// </summary>
        /// <param name="other">The collection to compare to the current Set.</param>
        /// <returns>true if the current Set is equal to other; otherwise, false.</returns>
        public bool SetEquals(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            bool hasOutOfBound;
            var tmp = new MutableOrdinalSet<T>(this, other, out hasOutOfBound);
            if (hasOutOfBound)
                return false;
            foreach (var item in other) {
                if (!tmp.Remove(item))
                    return false;
            }
            return tmp.Count == 0;
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            bool hasOutOfBound;
            var tmp = new MutableOrdinalSet<T>(this, other, out hasOutOfBound);
            foreach (var item in tmp) {
                if (!Remove(item))
                    Add(item);
            }
        }

        /// <summary>
        /// Modifies the current Set so that it contains all elements that are present 
        /// in either the current object or the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current Set.</param>
        public void UnionWith(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            foreach (var item in other)
                Add(item);
        }

        #endregion

        #region ICollection<T> Membres

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        /// <summary>
        /// Removes all elements from the set.
        /// </summary>
        public virtual void Clear()
        {
            _storage.SetAll(false);
        }

        /// <summary>
        /// Determines whether the set contains a specific element.
        /// </summary>
        /// <param name="item">The element to locate in the set.</param>
        /// <returns>true if the set contains item; otherwise, false.</returns>
        public virtual bool Contains(T item)
        {
            var n = _settings.IndexOf(item);
            return n >= 0 && _storage.Get(n);
        }

        /// <summary>
        /// Copies the complete Set to a compatible one-dimensional array, starting at the specified array index.
        /// </summary>
        /// <param name="array">A one-dimensional array that is the destination of the elements copied from the set. The array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            var ordinal = _settings.Ordinal;
            var current = _settings.LowerBound;
            var pos = 0;
            if (_storage.Get(0))
                array[arrayIndex++] = current;
            for (int i = 1; i < _storage.Length; i++) {
                if (_storage.Get(i)) {
                    current = ordinal.Advance(current, i - pos);
                    array[arrayIndex++] = current;
                    pos = i;
                }
            }
        }

        public int Count {
            get { return _storage.Cardinality(); }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public bool Remove(T item)
        {
            var n = _settings.IndexOf(item);
            if (n >= 0 && _storage.Get(n)) {
                _storage.Set(n, false);
                return true;
            }
            return false;
        }

        #endregion

        #region IEnumerable<T> Membres

        public IEnumerator<T> GetEnumerator()
        {
            var ordinal = _settings.Ordinal;
            var current = _settings.LowerBound;
            var pos = 0;
            if (_storage.Get(0))
                yield return current;
            for (int i = 1; i < _storage.Length; i++) {
                if (_storage.Get(i)) {
                    current = ordinal.Advance(current, i - pos);
                    yield return current;
                    pos = i;
                }
            }
        }

        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
