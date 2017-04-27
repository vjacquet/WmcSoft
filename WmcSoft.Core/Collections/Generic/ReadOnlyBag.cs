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

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Represents a read only bag of items.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the bag.</typeparam>
    public class ReadOnlyBag<T> : ICollection, ICollection<T>, IReadOnlyCollection<T>
    {
        private readonly Bag<T> _bag;

        public ReadOnlyBag(Bag<T> bag)
        {
            if (bag == null) throw new ArgumentNullException(nameof(bag));

            _bag = bag;
        }

        public int Count => _bag.Count;
        public bool IsReadOnly => true;

#pragma warning disable IDE0025 // Utiliser un corps d'expression pour les propriétés
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033", Justification = "Provided only for legacy.")]
        object ICollection.SyncRoot {
            get { return ((ICollection)_bag).SyncRoot; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033", Justification = "Provided only for legacy.")]
        bool ICollection.IsSynchronized {
            get { return ((ICollection)_bag).IsSynchronized; }
        }
#pragma warning restore IDE0025 // Utiliser un corps d'expression pour les propriétés

        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            return _bag.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _bag.CopyTo(array, arrayIndex);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)_bag).CopyTo(array, index);
        }

        public Bag<T>.Enumerator GetEnumerator()
        {
            return _bag.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
