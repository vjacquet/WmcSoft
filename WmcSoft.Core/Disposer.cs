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
using System.Threading;

namespace WmcSoft
{
    /// <summary>
    /// Executes the <see cref="Action"/> on <see cref="IDisposable.Dispose"/>.
    /// </summary>
    /// <remarks>Dispose must be called. The action is not executed in the finalizer.</remarks>
    public sealed class Disposer : IDisposable
    {
        private Action onDispose;

        /// <summary>
        /// Creates a new instance of <see cref="Disposer"/>.
        /// </summary>
        /// <param name="action">The action to execute on <see cref="IDisposable.Dispose"/>.</param>
        /// <remarks><paramref name="action"/> may be <c>null</c>.</remarks>
        public Disposer(Action action)
        {
            onDispose = action;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            var action = Interlocked.Exchange(ref onDispose, null);
            if (action != null)
                action();
        }
    }
}
