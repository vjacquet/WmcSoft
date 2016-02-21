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
    /// Adapts an <see cref="IEnumerator"/> as an <see cref="IEnumerator{object}"/>.
    /// </summary>
    public sealed class EnumeratorAdapter : IEnumerator<object>
    {
        readonly IEnumerator _enumerator;

        public EnumeratorAdapter(IEnumerator enumerator) {
            _enumerator = enumerator;
        }

        public object Current
        {
            get { return _enumerator.Current; }
        }

        public void Dispose() {
            var disposable = _enumerator as IDisposable;
            if (disposable != null)
                disposable.Dispose();
        }

        public bool MoveNext() {
            return _enumerator.MoveNext();
        }

        public void Reset() {
            _enumerator.Reset();
        }
    }
}
