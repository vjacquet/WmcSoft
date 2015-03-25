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
using System.Reflection;

namespace WmcSoft.Diagnostics
{
    /// <summary>
    /// Represents a TraceSession, starting a logical information, writing a preambule and timing the execution.
    /// </summary>
    public sealed class TraceSession : IDisposable
    {
        const string Preambule = @"Executing {0} version {1} ";
        const string Conclusion = @"Executed in {0}ms.";

        readonly Stopwatch stopwatch = new Stopwatch();

        public TraceSession(Assembly assembly) {
            Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            Trace.CorrelationManager.StartLogicalOperation();

            var name = assembly.GetName();
            Trace.TraceInformation(String.Format(Preambule, name.Name, name.Version.ToString()).PadRight(120, '-'));

            stopwatch.Start();
        }
        public TraceSession()
            : this(Assembly.GetExecutingAssembly()) {
        }

        public TraceSession(Type type) {
            Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            Trace.CorrelationManager.StartLogicalOperation();

            var name = type.Assembly.GetName();
            Trace.TraceInformation(Preambule, type.FullName, name.Version.ToString());

            stopwatch.Start();
        }

        public void Dispose() {
            Trace.TraceInformation(Conclusion, stopwatch.ElapsedMilliseconds);
            Trace.CorrelationManager.StopLogicalOperation();
        }
    }
}
