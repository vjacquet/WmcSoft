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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace WmcSoft.Diagnostics
{
    /// <summary>
    /// Creates an Indent/Unindent scope.
    /// </summary>
    public sealed class TraceIndent : IDisposable
    {
        #region Private fields

        int disposed;

        #endregion

        #region Lifecycle

        public TraceIndent()
        {
            Trace.Indent();
        }

        #endregion

        #region IDisposable Membres

#if TRACE
        [SuppressMessage("Microsoft.Performance", "CA1821", Justification = "The #if block invalidate this warning.")]
        ~TraceIndent()
        {
            Trace.TraceError("TraceIndent.Dispose should have been called.");
        }
#endif

        public void Dispose()
        {
            if (Interlocked.Increment(ref disposed) == 1) {
                Trace.Unindent();
                GC.SuppressFinalize(this);
            }

            Debug.Assert(disposed == 1, "Dispose must be called once.");
        }

        #endregion
    }
}
