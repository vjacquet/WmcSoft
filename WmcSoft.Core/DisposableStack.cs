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
        readonly Stack<IDisposable> stack;

        public DisposableStack()
        {
            stack = new Stack<IDisposable>();
        }

        // ~DisposableStack() { } // no unmanaged resources so no need to implement the finalizer.

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Adds a disposable to the stack.
        /// </summary>
        /// <param name="disposable">The disposable</param>
        /// <returns>Returns <code>false</code> when <paramref name="disposable"/> is <c>null</c>.</returns>
        public virtual bool Add(IDisposable disposable)
        {
            if (disposable == null)
                return false;

            Push(disposable);
            return true;
        }

        protected void Push(IDisposable disposable)
        {
            stack.Push(disposable);
        }

        protected virtual void Dispose(bool disposing)
        {
            while (stack.Count != 0) {
                stack.Pop().Dispose();
            }
        }

        public IEnumerator<IDisposable> GetEnumerator()
        {
            return stack.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
