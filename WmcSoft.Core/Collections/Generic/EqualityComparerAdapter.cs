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
using System.Collections.Generic;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Adapts an <see cref="IComparable{T}"/> as an <see cref="IEqualityComparer{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.This type parameter is contravariant. That is,
    /// you can use either the type you specified or any type that is less derived.</typeparam>
    public sealed class EqualityComparerAdapter<T> : IEqualityComparer<T>
    {
        #region Fields

        private readonly IComparer<T> _comparer;
        private readonly Func<T, int> _hasher;

        #endregion

        #region Lifecycle

        public EqualityComparerAdapter(IComparer<T> comparer, Func<T, int> hasher = null) {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            _comparer = comparer;
            _hasher = hasher ?? Hash;
        }

        public EqualityComparerAdapter(IComparer<T> comparer, params T[] knownValues) {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            var array = (T[])knownValues.Clone();
            Array.Sort(array, comparer);
            _hasher = x => Hash(x, array);
        }

        #endregion

        #region IEqualityComparer<T> Members

        public bool Equals(T x, T y) {
            return _comparer.Compare(x, y) == 0;
        }

        public int GetHashCode(T obj) {
            return _hasher(obj);
        }

        #endregion

        #region Helpers

        int Hash(T obj) {
            // We know nothing about the comparer, therefore we can deal with only two types of instances:
            // those that compares equal to the default, and those that do not.
            if (_comparer.Compare(obj, default(T)) == 0)
                return 0;
            return -1;
        }

        int Hash(T obj, T[] knownValues) {
            return Array.BinarySearch(knownValues, obj, _comparer);
        }

        #endregion
    }
}