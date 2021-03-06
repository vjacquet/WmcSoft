﻿#region Licence

/****************************************************************************
          Copyright 1999-2017 Vincent J. Jacquet.  All rights reserved.

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

using System.Collections.Generic;

namespace WmcSoft.Collections.Generic
{
    public class KeyValuePairComparer<TKey, TValue> : IComparer<KeyValuePair<TKey, TValue>>
    {
        readonly IComparer<TKey> KeyComparer;
        readonly IComparer<TValue> ValueComparer;

        public KeyValuePairComparer(IComparer<TKey> keyComparer = null, IComparer<TValue> valueComparer = null)
        {
            KeyComparer = keyComparer ?? Comparer<TKey>.Default;
            ValueComparer = valueComparer ?? Comparer<TValue>.Default;
        }

        public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
        {
            var r = KeyComparer.Compare(x.Key, y.Key);
            if (r != 0)
                return r;
            return ValueComparer.Compare(x.Value, y.Value);
        }
    }
}
