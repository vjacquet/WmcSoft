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
using System.Collections;
using System.Collections.Generic;

namespace WmcSoft
{
    /// <summary>
    /// Stack disposables to Dispose them in reverse order.
    /// </summary>
    public class DisposableStack : IDisposableBin
    {
        readonly Stack<IDisposable> _stack;

        public DisposableStack()
        {
            _stack = new Stack<IDisposable>();
        }

        ~DisposableStack()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Adds a disposable to the stack.
        /// </summary>
        /// <param name="disposable">The disposable</param>
        /// <returns>Always <code>true</code></returns>
        public bool Add(IDisposable disposable)
        {
            _stack.Push(disposable);
            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            while (_stack.Count != 0) {
                _stack.Pop().Dispose();
            }
        }

        public IEnumerator<IDisposable> GetEnumerator()
        {
            return _stack.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
