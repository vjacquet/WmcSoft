#region Licence

/****************************************************************************
          Copyright 1999-2018 Vincent J. Jacquet.  All rights reserved.

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
using System.IO;

namespace WmcSoft.IO.Sources
{
    /// <summary>
    /// Decorates a <see cref="IStreamSource"/> to trace whether a stream from the underlying source was returned or not.
    /// </summary>
    public class TraceStreamSource : IStreamSource
    {
        #region Tracing strategies

        interface ITracingStrategy
        {
            void TraceInformation(string format, params object[] args);
            void TraceWarning(string format, params object[] args);
            void TraceError(string format, params object[] args);
        }

        class TraceStrategy : ITracingStrategy
        {
            public void TraceError(string format, params object[] args)
            {
                Trace.TraceError(format, args);
            }

            public void TraceInformation(string format, params object[] args)
            {
                Trace.TraceInformation(format, args);
            }

            public void TraceWarning(string format, params object[] args)
            {
                Trace.TraceWarning(format, args);
            }
        }

        class TraceSourceStrategy : ITracingStrategy
        {
            private readonly TraceSource traceSource;

            public TraceSourceStrategy(TraceSource traceSource)
            {
                this.traceSource = traceSource;
            }

            public void TraceError(string format, params object[] args)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, format, args);
            }

            public void TraceInformation(string format, params object[] args)
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, format, args);
            }

            public void TraceWarning(string format, params object[] args)
            {
                traceSource.TraceEvent(TraceEventType.Warning, 0, format, args);
            }
        }

        #endregion

        private readonly ITracingStrategy strategy;
        private readonly IStreamSource underlying;
        private readonly string preambule;

        public TraceStreamSource(IStreamSource underlying, string preambule = null)
            : this(new TraceStrategy(), underlying, preambule)
        {
        }

        public TraceStreamSource(TraceSource traceSource, IStreamSource underlying, string preambule = null)
            : this(new TraceSourceStrategy(traceSource), underlying, preambule)
        {
        }

        private TraceStreamSource(ITracingStrategy strategy, IStreamSource underlying, string preambule = null)
        {
            this.strategy = strategy;
            this.underlying = underlying;
            this.preambule = preambule == null ? "" : (preambule + ": ");
        }

        public override string ToString()
        {
            return underlying.ToString();
        }

        public Stream GetStream()
        {
            try {
                var result = underlying.GetStream();

                if (result != null) {
                    strategy.TraceInformation(preambule + "Open stream from source {0}.", underlying);
                } else {
                    strategy.TraceWarning(preambule + "Did not open stream from source {0}.", underlying);
                }
                return result;
            } catch (Exception e) {
                strategy.TraceError(preambule + "Failed to open stream from source {0}: {1}", underlying, e);
                throw;
            }
        }
    }
}
