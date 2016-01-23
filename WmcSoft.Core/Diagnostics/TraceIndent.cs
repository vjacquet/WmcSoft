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
using System.Diagnostics;
using System.Threading;

namespace WmcSoft.Diagnostics
{
    /// <summary>
    /// Creates an Indent/Unindent scope.
    /// </summary>
    public sealed class TraceIndent : IDisposable
    {
        #region Private fields

        Action _onDispose;

        #endregion

        #region Lifecycle

        public TraceIndent() {
            Trace.Indent();
            _onDispose = () => Trace.Unindent();
        }

        #endregion

        #region IDisposable Membres

        ~TraceIndent() {
            try {
                _onDispose(); // here only when dispose was not called.
            }
            catch { }
        }

        public void Dispose() {
            var action = Interlocked.Exchange(ref _onDispose, null);
            Debug.Assert(action != null, "Dispose must be called once.");
            action();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
