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
using System.Diagnostics;

namespace WmcSoft.Collections.Generic
{
    public sealed class KeyEqualityComparer<T, K> : IEqualityComparer<T>
    {
        private readonly Func<T, K> _selector;
        private readonly IEqualityComparer<K> _comparer;

        public KeyEqualityComparer(SelectorVoucher<T, K> vouch, IEqualityComparer<K> comparer = null)
        {
            Debug.Assert(vouch.SupportsNullArgument);

            _selector = vouch;
            _comparer = comparer ?? EqualityComparer<K>.Default;
        }

        public KeyEqualityComparer(Func<T, K> selector, IEqualityComparer<K> comparer = null)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            _selector = (x) => (x != null) ? selector(x) : default(K);
            _comparer = comparer ?? EqualityComparer<K>.Default;
        }

        public bool Equals(T x, T y)
        {
            return _comparer.Equals(_selector(x), _selector(y));
        }

        public int GetHashCode(T obj)
        {
            return _comparer.GetHashCode(_selector(obj));
        }
    }
}