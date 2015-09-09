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
using System.Threading;

namespace WmcSoft
{
    /// <summary>
    /// Executes the Action on Dispose.
    /// </summary>
    public sealed class Disposer : IDisposable
    {
        #region EmptyDisposable

        class EmptyDisposable : IDisposable
        {
            #region IDisposable Membres

            public void Dispose() {
            }

            #endregion
        }
        public static IDisposable Empty = new EmptyDisposable();

        #endregion

        #region Fields

        Action _action;

        #endregion

        #region Lifecycle

        public Disposer(Action action) {
            _action = action;
        }

        #endregion

        #region IDisposable Membres

        public void Dispose() {
            var action = Interlocked.Exchange(ref _action, null);
            if (action != null)
                action();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
