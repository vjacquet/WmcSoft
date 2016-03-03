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
    /// Implements a comparer which order items relative to their index in a given sort order.
    /// </summary>
    /// <typeparam name="T">The type of the source's items to compare</typeparam>
    /// <remarks>Elements not found in the custom order are considered greater than those that are.</remarks>
    [Serializable]
    public sealed class CustomSortComparer<T> : IComparer<T>
    {
        private readonly T[] _customOrder;
        private readonly IEqualityComparer<T> _comparer;

        public CustomSortComparer(params T[] customOrder) : this(null, customOrder) {
        }

        public CustomSortComparer(IEqualityComparer<T> comparer, params T[] customOrder) {
            _comparer = comparer ?? EqualityComparer<T>.Default;
            _customOrder = customOrder;
        }

        #region IComparer<T> Membres

        private int IndexOf(T x) {
            var found = Array.FindIndex(_customOrder, _ => _comparer.Equals(_, x));
            return found & int.MaxValue; // Changes -1 to int.MaxValue so missing elements are at the end.
        }

        int IComparer<T>.Compare(T x, T y) {
            return IndexOf(x) - IndexOf(y);
        }

        #endregion
    }
}