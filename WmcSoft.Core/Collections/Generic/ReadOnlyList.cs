﻿#region Licence

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
    sealed class ReadOnlyList<T> : IReadOnlyList<T>
    {
        #region fields

        readonly IList<T> _list;

        #endregion

        #region Lifecycle

        public ReadOnlyList(IList<T> list) {
            _list = list;
        }

        #endregion

        #region IReadOnlyList<T> Membres

        public T this[int index] {
            get { return _list[index]; }
        }

        #endregion

        #region IReadOnlyCollection<T> Membres

        public int Count {
            get { return _list.Count; }
        }

        #endregion

        #region IEnumerable<T> Membres

        public IEnumerator<T> GetEnumerator() {
            return _list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
