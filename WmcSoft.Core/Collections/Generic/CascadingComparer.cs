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
    /// Implements a comparer that calls comparer in sequence as long as they compare equals. 
    /// </summary>
    /// <typeparam name="T">The type of the source's items to compare</typeparam>
    [Serializable]
    public sealed class CascadingComparer<T> : IComparer<T>
        , IEnumerable<IComparer<T>>
    {
        #region Fields

        readonly IComparer<T>[] _comparers;

        #endregion

        #region Lifecycle

        public CascadingComparer(params IComparer<T>[] comparers) {
            if (comparers == null) throw new ArgumentNullException("comparers");
            if (comparers.Length == 0) throw new ArgumentException();

            _comparers = comparers;
        }

        internal CascadingComparer(IEnumerable<IComparer<T>> comparers, IComparer<T> other) {
            var primary = ToArray(comparers);
            var length = primary.Length;
            _comparers = new IComparer<T>[length + 1];
            Array.Copy(primary, _comparers, length);
            _comparers[length] = other;
        }

        internal CascadingComparer(IComparer<T> other, IEnumerable<IComparer<T>> comparers) {
            var primary = ToArray(comparers);
            var length = primary.Length;
            _comparers = new IComparer<T>[length + 1];
            _comparers[0] = other;
            Array.Copy(primary, 0, _comparers, 1, length);
        }

        static IComparer<T>[] ToArray(IEnumerable<IComparer<T>> comparers) {
            var cascading = comparers as CascadingComparer<T>;
            if (cascading != null)
                return cascading._comparers;
            return comparers.ToArray();
        }

        #endregion

        #region IComparer<T> Membres

        /// <summary>Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
        /// <param name="x">The first instance to compare.</param>
        /// <param name="y">The second instance to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of x and y, as shown in the
        /// following table.
        /// <list type="table">
        /// <listheader>
        ///   <description>Value</description>
        ///   <description>Meaning</description>
        /// </listheader>
        /// <item>
        ///   <description>Less than zero</description>
        ///   <description> x is less than y.</description>
        /// </item>
        /// <item>
        ///   <description>Zero</description>
        ///   <description>x equals y.</description>
        /// </item>
        /// <item>
        ///   <description>Greater than zero</description>
        ///   <description>x is greater than y.</description>
        /// </item>
        /// </list>
        /// </returns>
        public int Compare(T x, T y) {
            int result = 0;
            for (int i = 0; i < _comparers.Length; i++) {
                result = _comparers[i].Compare(x, y);
                if (result != 0)
                    break;
            }
            return result;
        }

        #endregion

        #region IEnumerable<IComparer<T> members

        IEnumerator<IComparer<T>> IEnumerable<IComparer<T>>.GetEnumerator() {
            return ((IEnumerable<IComparer<T>>)_comparers).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable<IComparer<T>>)_comparers).GetEnumerator();
        }

        #endregion
    }
}
