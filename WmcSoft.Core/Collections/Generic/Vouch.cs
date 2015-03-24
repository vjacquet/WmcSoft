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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Collections.Generic
{
    public static class Vouch
    {
        public static Voucher<T> IsSorted<T>(IEnumerable<T> enumerable, IComparer<T> comparer) {
            return new Voucher<T>(false, enumerable, comparer);
        }
        public static Voucher<T> IsSorted<T>(IEnumerable<T> enumerable) {
            return IsSorted(enumerable, Comparer<T>.Default);
        }

        public static Voucher<T> IsSortedSet<T>(IEnumerable<T> enumerable, IComparer<T> comparer) {
            return new Voucher<T>(true, enumerable, comparer);
        }
        public static Voucher<T> IsSortedSet<T>(IEnumerable<T> enumerable) {
            return IsSortedSet(enumerable, Comparer<T>.Default);
        }

        public static Voucher<T> IsSet<T>(IEnumerable<T> enumerable) {
            return new Voucher<T>(true, enumerable, null);
        }
    }

    public sealed class Voucher<T>
    {
        #region Fields

        readonly IEnumerable<T> _enumerable;
        readonly IComparer<T> _comparer;
        readonly bool _isSet;

        #endregion

        internal Voucher(bool isSet, IEnumerable<T> enumerable, IComparer<T> comparer) {
            _isSet = isSet;
            _enumerable = enumerable;
            _comparer = comparer;
        }

        public IComparer<T> Comparer {
            get { return _comparer; }
        }

        public IEnumerator<T> GetEnumerator() {
            return _enumerable.GetEnumerator();
        }
    }
}
