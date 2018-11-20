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
using System.Threading;

namespace WmcSoft.Diagnostics
{
    /// <summary>
    /// Traces the time ellapsed in a block when leaving it.
    /// </summary>
    /// <example>
    /// using(new TimingTrace("Description of the operation")) {
    ///     // ...
    /// }
    /// </exexample>
    public sealed class TimingTrace : IDisposable
    {
        #region Private fields

        private readonly Stopwatch stopwatch = Stopwatch.StartNew();
        private Action onDispose;

        #endregion

        #region LifeCycle

        public TimingTrace(string message)
        {
            onDispose = () => Trace.WriteLine(Format(message));
        }

        public TimingTrace(string category, string message)
        {
            onDispose = () => Trace.WriteLine(category, Format(message));
        }

        public TimingTrace(TraceSource traceSource, string message)
        {
            onDispose = () => traceSource.TraceInformation(Format(message));
        }

        public TimingTrace(TraceSource traceSource, Func<string> message)
        {
            onDispose = () => traceSource.TraceInformation(Format(message()));
        }

        public TimingTrace(TraceSource traceSource, TraceEventType eventType, int id, string message)
        {
            onDispose = () => traceSource.TraceEvent(eventType, id, Format(message));
        }

        public TimingTrace(TraceSource traceSource, TraceEventType eventType, int id, Func<string> message)
        {
            onDispose = () => traceSource.TraceEvent(eventType, id, Format(message()));
        }

        #endregion

        #region Helpers

        string Format(string message)
        {
            return string.Format("{0}ms\t{1}", stopwatch.ElapsedMilliseconds, message);
        }

        #endregion

        #region IDisposable Membres

        public void Dispose()
        {
            var action = Interlocked.Exchange(ref onDispose, null);
            Debug.Assert(action != null, "Dispose must be called once.");
            action();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
