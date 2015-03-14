using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Diagnostics
{
    public static class TraceSourceExtensions
    {
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
    }
}
