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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WmcSoft.Collections.Generic
{
    public static class IndexExtensions
    {
        class LookupAdapter<TKey, TValue> : ILookup<TKey, TValue>
        {
            private readonly IReadOnlyIndex<TKey, TValue> _index;

            public LookupAdapter(IReadOnlyIndex<TKey, TValue> index)
            {
                _index = index;
            }

            public IEnumerable<TValue> this[TKey key] => _index[key];

            public int Count => _index.Count;

            public bool Contains(TKey key) => _index.ContainsKey(key);

            public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator()
            {
                foreach (var key in _index.Keys)
                    yield return new Grouping<TKey, TValue>(key, _index[key]);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public static ILookup<TKey, TValue> AsLookup<TKey, TValue>(this IReadOnlyIndex<TKey, TValue> index)
        {
            return new LookupAdapter<TKey, TValue>(index);
        }
    }
}