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

namespace WmcSoft.Diagnostics
{
    public static class TraceSourceExtensions
    {
        #region Trace### with id

        public static void TraceVerbose(this TraceSource traceSource, int id, string message) {
            traceSource.TraceEvent(TraceEventType.Verbose, id, message);
        }
        public static void TraceVerbose(this TraceSource traceSource, int id, string format, params object[] args) {
            traceSource.TraceEvent(TraceEventType.Verbose, id, format, args);
        }

        public static void TraceWarning(this TraceSource traceSource, int id) {
            traceSource.TraceEvent(TraceEventType.Warning, id);
        }
        public static void TraceWarning(this TraceSource traceSource, int id, string message) {
            traceSource.TraceEvent(TraceEventType.Warning, id, message);
        }
        public static void TraceWarning(this TraceSource traceSource, int id, string format, params object[] args) {
            traceSource.TraceEvent(TraceEventType.Warning, id, format, args);
        }

        public static void TraceError(this TraceSource traceSource, int id) {
            traceSource.TraceEvent(TraceEventType.Error, id);
        }
        public static void TraceError(this TraceSource traceSource, int id, string message) {
            traceSource.TraceEvent(TraceEventType.Error, id, message);
        }
        public static void TraceError(this TraceSource traceSource, int id, string format, params object[] args) {
            traceSource.TraceEvent(TraceEventType.Error, id, format, args);
        }

        public static void TraceCritical(this TraceSource traceSource, int id) {
            traceSource.TraceEvent(TraceEventType.Critical, id);
        }
        public static void TraceCritical(this TraceSource traceSource, int id, string message) {
            traceSource.TraceEvent(TraceEventType.Critical, id, message);
        }
        public static void TraceCritical(this TraceSource traceSource, int id, string format, params object[] args) {
            traceSource.TraceEvent(TraceEventType.Critical, id, format, args);
        }

        #endregion

        #region Trace### w/o id

        public static void TraceVerbose(this TraceSource traceSource, string message) {
            traceSource.TraceEvent(TraceEventType.Verbose, 0, message);
        }
        public static void TraceVerbose(this TraceSource traceSource, string format, params object[] args) {
            traceSource.TraceEvent(TraceEventType.Verbose, 0, format, args);
        }

        public static void TraceWarning(this TraceSource traceSource) {
            traceSource.TraceEvent(TraceEventType.Warning, 0);
        }
        public static void TraceWarning(this TraceSource traceSource, string message) {
            traceSource.TraceEvent(TraceEventType.Warning, 0, message);
        }
        public static void TraceWarning(this TraceSource traceSource, string format, params object[] args) {
            traceSource.TraceEvent(TraceEventType.Warning, 0, format, args);
        }

        public static void TraceError(this TraceSource traceSource) {
            traceSource.TraceEvent(TraceEventType.Error, 0);
        }
        public static void TraceError(this TraceSource traceSource, string message) {
            traceSource.TraceEvent(TraceEventType.Error, 0, message);
        }
        public static void TraceError(this TraceSource traceSource, string format, params object[] args) {
            traceSource.TraceEvent(TraceEventType.Error, 0, format, args);
        }

        public static void TraceCritical(this TraceSource traceSource) {
            traceSource.TraceEvent(TraceEventType.Critical, 0);
        }
        public static void TraceCritical(this TraceSource traceSource, string message) {
            traceSource.TraceEvent(TraceEventType.Critical, 0, message);
        }
        public static void TraceCritical(this TraceSource traceSource, string format, params object[] args) {
            traceSource.TraceEvent(TraceEventType.Critical, 0, format, args);
        }

        #endregion

        #region TraceXxxBlock methods

        public static IDisposable TraceBlock(this TraceSource traceSource, TraceEventType eventType, int id, string message, Func<Stopwatch, string> exit) {
            var stopwatch = Stopwatch.StartNew();
            traceSource.TraceEvent(eventType, id, message);
            return new Disposer(() => traceSource.TraceEvent(eventType, id, exit(stopwatch)));
        }
        public static IDisposable TraceBlock(this TraceSource traceSource, TraceEventType eventType, int id, string message) {
            return traceSource.TraceBlock(eventType, id, message, (sw) => String.Format("{0} - {1}ms", message, sw.ElapsedMilliseconds));
        }

        #endregion;
    }
}
