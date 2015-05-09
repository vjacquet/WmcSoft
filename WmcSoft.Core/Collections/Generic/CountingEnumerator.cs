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

using System.Collections.Generic;

namespace WmcSoft.Collections.Generic
{
   public sealed class CountingEnumerator<T> : IEnumerator<T>
    {
        readonly IEnumerator<T> _base;
        int _count;

        #region Lifecycle

        public CountingEnumerator(IEnumerable<T> enumerable)
            : this(enumerable.GetEnumerator()) {
        }
        public CountingEnumerator(IEnumerator<T> enumerator) {
            _base = enumerator;
        }

        #endregion

        #region Properties

        public int Count {
            get { return _count; }
        }

        #endregion

        #region IEnumerator<T> Membres

        public T Current {
            get { return _base.Current; }
        }

        #endregion

        #region IDisposable Membres

        public void Dispose() {
            _base.Dispose();
        }

        #endregion

        #region IEnumerator Membres

        object System.Collections.IEnumerator.Current {
            get { return Current; }
        }

        public bool MoveNext() {
            if (_base.MoveNext()) {
                _count++;
                return true;
            }
            return false;
        }

        public void Reset() {
            _base.Reset();
            _count = 0;
        }

        #endregion
    }
}
