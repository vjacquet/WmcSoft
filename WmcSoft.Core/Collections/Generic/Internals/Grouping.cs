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
using System.Diagnostics;
using System.Linq;

namespace WmcSoft.Collections.Generic
{
    [DebuggerDisplay("Key={Key,nq}, Count={_kv.Value.Count,nq}")]
    internal class Grouping<TKey, TValue> : IGrouping<TKey, TValue>
    {
        private readonly TKey _key;
        private readonly IReadOnlyCollection<TValue> _values;

        public Grouping(TKey key, IReadOnlyCollection<TValue> values)
        {
            _key = key;
            _values = values;
        }

        public TKey Key { get { return _key; } }

        public IEnumerator<TValue> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
