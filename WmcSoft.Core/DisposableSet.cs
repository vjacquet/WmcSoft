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

namespace WmcSoft
{
    /// <summary>
    /// Stack disposables to Dispose them in reverse order. 
    /// </summary>
    /// <remarks>An <see cref="IDisposable"/> is added only once.</remarks>
    public class DisposableSet : DisposableStack
    {
        readonly HashSet<IDisposable> _set;

        public DisposableSet()
        {
            _set = new HashSet<IDisposable>();
            _set.Add(null); // so null cannot be push onto the base stack.
        }

        /// <summary>
        /// Adds a disposable to the set.
        /// </summary>
        /// <param name="disposable">The disposable.</param>
        /// <returns><code>true</code> if the disposable is not already in the set.</returns>
        public override bool Add(IDisposable disposable)
        {
            if (_set.Add(disposable)) {
                Push(disposable);
                return true;
            }
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            _set.Clear();
            base.Dispose(disposing);
        }
    }
}
