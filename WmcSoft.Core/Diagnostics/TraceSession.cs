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
using System.Threading;

namespace WmcSoft.Diagnostics
{
    /// <summary>
    /// Represents a TraceSession, starting a logical information, writing a preambule and timing the execution.
    /// </summary>
    public sealed class TraceSession : IDisposable
    {
        #region Constants

        const string Preambule = @"Executing {0} version {1}.";
        const string Conclusion = @"Executed in {0}ms.";

        #endregion

        #region Private fields

        private readonly Stopwatch stopwatch = new Stopwatch();
        private Action disposer;
        private Action<string> tracer;

        #endregion

        #region LifeCycle

        private void Initialize(string name, Version version, TraceSource traceSource)
        {
            Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            Trace.CorrelationManager.StartLogicalOperation();

            disposer = () => {
                tracer(string.Format(Conclusion, stopwatch.ElapsedMilliseconds));
                Trace.CorrelationManager.StopLogicalOperation();
            };

            if (traceSource == null)
                tracer = (s) => Trace.WriteLine(s);
            else
                tracer = (s) => traceSource.TraceInformation(s);

            tracer(String.Format(Preambule, name, version));

            stopwatch.Start();
        }

        public TraceSession(TraceSource traceSource, Type type)
        {
            var name = type.Assembly.GetName();
            Trace.TraceInformation(Preambule, type.FullName, name.Version.ToString());
            Initialize(type.FullName, name.Version, traceSource);
        }

        public TraceSession(TraceSource traceSource, Assembly assembly)
        {
            var name = assembly.GetName();
            Initialize(name.Name, name.Version, traceSource);
        }

        public TraceSession(TraceSource traceSource)
            : this(traceSource, Assembly.GetExecutingAssembly())
        {
        }

        public TraceSession(Assembly assembly)
            : this(null, assembly)
        {
        }

        public TraceSession()
            : this(null, Assembly.GetExecutingAssembly())
        {
        }

        public TraceSession(Type type)
            : this(null, type)
        {
        }

        #endregion

        #region IDisposable Membres

        public void Dispose()
        {
            var action = Interlocked.Exchange(ref disposer, null);
            Debug.Assert(action != null, "Dispose must be called once.");
            action();
            tracer = null;
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
