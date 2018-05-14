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

        /// <summary>
        /// Writes a debugging trace message to the trace listeners in the <see cref="TraceSource.Listeners"/>
        /// collection using the specified event identifier, and message.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="message">The trace message to write.</param>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceVerbose(this TraceSource traceSource, int id, string message)
        {
            traceSource.TraceEvent(TraceEventType.Verbose, id, message);
        }

        /// <summary>
        /// Writes a debugging trace event to the trace listeners in the System.Diagnostics.TraceSource.Listeners
        /// collection using the specified event type, event identifier, and argument array
        /// and format.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="format">A composite format string that contains text intermixed with zero or more format items, which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is invalid.-or- The number that indicates an argument to format is less than zero,
        /// or greater than or equal to the number of specified objects to format.</exception>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceVerbose(this TraceSource traceSource, int id, string format, params object[] args)
        {
            traceSource.TraceEvent(TraceEventType.Verbose, id, format, args);
        }

        /// <summary>
        /// Writes a informational trace message to the trace listeners in the <see cref="TraceSource.Listeners"/>
        /// collection using the specified event identifier.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="message">The trace message to write.</param>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceInformation(this TraceSource traceSource, int id)
        {
            traceSource.TraceEvent(TraceEventType.Information, id);
        }

        /// <summary>
        /// Writes a informational trace message to the trace listeners in the <see cref="TraceSource.Listeners"/>
        /// collection using the specified event identifier, and message.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceInformation(this TraceSource traceSource, int id, string message)
        {
            traceSource.TraceEvent(TraceEventType.Information, id, message);
        }

        /// <summary>
        /// Writes a informational trace event to the trace listeners in the System.Diagnostics.TraceSource.Listeners
        /// collection using the specified event type, event identifier, and argument array
        /// and format.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="format">A composite format string that contains text intermixed with zero or more format items, which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is invalid.-or- The number that indicates an argument to format is less than zero,
        /// or greater than or equal to the number of specified objects to format.</exception>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceInformation(this TraceSource traceSource, int id, string format, params object[] args)
        {
            traceSource.TraceEvent(TraceEventType.Information, id, format, args);
        }

        /// <summary>
        /// Writes a noncritical problem message to the trace listeners in the <see cref="TraceSource.Listeners"/>
        /// collection using the specified event identifier.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="message">The trace message to write.</param>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceWarning(this TraceSource traceSource, int id)
        {
            traceSource.TraceEvent(TraceEventType.Warning, id);
        }

        /// <summary>
        /// Writes a noncritical problem message to the trace listeners in the <see cref="TraceSource.Listeners"/>
        /// collection using the specified event identifier, and message.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceWarning(this TraceSource traceSource, int id, string message)
        {
            traceSource.TraceEvent(TraceEventType.Warning, id, message);
        }

        /// <summary>
        /// Writes a noncritical problem to the trace listeners in the System.Diagnostics.TraceSource.Listeners
        /// collection using the specified event identifier, format, and argument array
        /// and format.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="format">A composite format string that contains text intermixed with zero or more format items, which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is invalid.-or- The number that indicates an argument to format is less than zero,
        /// or greater than or equal to the number of specified objects to format.</exception>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceWarning(this TraceSource traceSource, int id, string format, params object[] args)
        {
            traceSource.TraceEvent(TraceEventType.Warning, id, format, args);
        }

        /// <summary>
        /// Writes a noncritical problem message to the trace listeners in the <see cref="TraceSource.Listeners"/>
        /// collection using the specified event identifier, and message.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="message">The trace message to write.</param>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceError(this TraceSource traceSource, int id)
        {
            traceSource.TraceEvent(TraceEventType.Error, id);
        }

        /// <summary>
        /// Writes a fatal error message to the trace listeners in the <see cref="TraceSource.Listeners"/>
        /// collection using the specified event identifier.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceError(this TraceSource traceSource, int id, string message)
        {
            traceSource.TraceEvent(TraceEventType.Error, id, message);
        }

        /// <summary>
        /// Writes a fatal error to the trace listeners in the System.Diagnostics.TraceSource.Listeners
        /// collection using the specified event identifier, format, and argument array
        /// and format.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="format">A composite format string that contains text intermixed with zero or more format items, which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is invalid.-or- The number that indicates an argument to format is less than zero,
        /// or greater than or equal to the number of specified objects to format.</exception>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceError(this TraceSource traceSource, int id, string format, params object[] args)
        {
            traceSource.TraceEvent(TraceEventType.Error, id, format, args);
        }

        /// <summary>
        /// Writes a fatal error to the trace listeners in the <see cref="TraceSource.Listeners"/>
        /// collection using the specified event identifier.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="message">The trace message to write.</param>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceCritical(this TraceSource traceSource, int id)
        {
            traceSource.TraceEvent(TraceEventType.Critical, id);
        }

        /// <summary>
        /// Writes a noncritical trace message to the trace listeners in the <see cref="TraceSource.Listeners"/>
        /// collection using the specified event identifier, and message.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceCritical(this TraceSource traceSource, int id, string message)
        {
            traceSource.TraceEvent(TraceEventType.Critical, id, message);
        }

        /// <summary>
        /// Writes a recoverable error to the trace listeners in the System.Diagnostics.TraceSource.Listeners
        /// collection using the specified event identifier, format, and argument array
        /// and format.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="format">A composite format string that contains text intermixed with zero or more format items, which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is invalid.-or- The number that indicates an argument to format is less than zero,
        /// or greater than or equal to the number of specified objects to format.</exception>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceCritical(this TraceSource traceSource, int id, string format, params object[] args)
        {
            traceSource.TraceEvent(TraceEventType.Critical, id, format, args);
        }

        #endregion

        #region Trace### w/o id

        /// <summary>
        /// Writes a debugging trace message to the trace listeners in the <see cref="TraceSource.Listeners"/>
        /// collection using the specified message.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="message">The trace message to write.</param>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceVerbose(this TraceSource traceSource, string message)
        {
            traceSource.TraceEvent(TraceEventType.Verbose, 0, message);
        }

        /// <summary>
        /// Writes a debugging trace event to the trace listeners in the System.Diagnostics.TraceSource.Listeners
        /// collection using the specified event type, and argument array
        /// and format.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="format">A composite format string that contains text intermixed with zero or more format items, which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is invalid.-or- The number that indicates an argument to format is less than zero,
        /// or greater than or equal to the number of specified objects to format.</exception>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceVerbose(this TraceSource traceSource, string format, params object[] args)
        {
            traceSource.TraceEvent(TraceEventType.Verbose, 0, format, args);
        }

        /// <summary>
        /// Writes a informational trace message to the trace listeners in the <see cref="TraceSource.Listeners"/>
        /// collection using the specified event identifier, and message.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="message">The trace message to write.</param>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceInformation(this TraceSource traceSource)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0);
        }

        /// <summary>
        /// Writes a informational trace message to the trace listeners in the <see cref="TraceSource.Listeners"/>
        /// collection using the specified message.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceInformation(this TraceSource traceSource, string message)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, message);
        }

        /// <summary>
        /// Writes a informational trace event to the trace listeners in the System.Diagnostics.TraceSource.Listeners
        /// collection using the specified event type, and argument array
        /// and format.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="format">A composite format string that contains text intermixed with zero or more format items, which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is invalid.-or- The number that indicates an argument to format is less than zero,
        /// or greater than or equal to the number of specified objects to format.</exception>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceInformation(this TraceSource traceSource, string format, params object[] args)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, format, args);
        }

        /// <summary>
        /// Writes a noncritical problem message to the trace listeners in the <see cref="TraceSource.Listeners"/>
        /// collection.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="message">The trace message to write.</param>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceWarning(this TraceSource traceSource)
        {
            traceSource.TraceEvent(TraceEventType.Warning, 0);
        }

        /// <summary>
        /// Writes a noncritical problem to the trace listeners in the System.Diagnostics.TraceSource.Listeners
        /// collection using the specified event type, and argument array
        /// and format.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="format">A composite format string that contains text intermixed with zero or more format items, which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is invalid.-or- The number that indicates an argument to format is less than zero,
        /// or greater than or equal to the number of specified objects to format.</exception>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceWarning(this TraceSource traceSource, string message)
        {
            traceSource.TraceEvent(TraceEventType.Warning, 0, message);
        }

        /// <summary>
        /// Writes a noncritical problem to the trace listeners in the System.Diagnostics.TraceSource.Listeners
        /// collection using the specified format and argument array
        /// and format.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="format">A composite format string that contains text intermixed with zero or more format items, which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is invalid.-or- The number that indicates an argument to format is less than zero,
        /// or greater than or equal to the number of specified objects to format.</exception>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceWarning(this TraceSource traceSource, string format, params object[] args)
        {
            traceSource.TraceEvent(TraceEventType.Warning, 0, format, args);
        }

        /// <summary>
        /// Writes a noncritical problem message to the trace listeners in the <see cref="TraceSource.Listeners"/>
        /// collection.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="message">The trace message to write.</param>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceError(this TraceSource traceSource)
        {
            traceSource.TraceEvent(TraceEventType.Error, 0);
        }

        /// <summary>
        /// Writes a noncritical problem message to the trace listeners in the <see cref="TraceSource.Listeners"/>
        /// collection using the specified event identifier, and message.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="message">The trace message to write.</param>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceError(this TraceSource traceSource, string message)
        {
            traceSource.TraceEvent(TraceEventType.Error, 0, message);
        }

        /// <summary>
        /// Writes a fatal error to the trace listeners in the System.Diagnostics.TraceSource.Listeners
        /// collection using the specified format and argument array
        /// and format.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="format">A composite format string that contains text intermixed with zero or more format items, which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is invalid.-or- The number that indicates an argument to format is less than zero,
        /// or greater than or equal to the number of specified objects to format.</exception>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceError(this TraceSource traceSource, string format, params object[] args)
        {
            traceSource.TraceEvent(TraceEventType.Error, 0, format, args);
        }

        /// <summary>
        /// Writes a fatal error to the trace listeners in the <see cref="TraceSource.Listeners"/>
        /// collection.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="message">The trace message to write.</param>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceCritical(this TraceSource traceSource)
        {
            traceSource.TraceEvent(TraceEventType.Critical, 0);
        }

        /// <summary>
        /// Writes a noncritical trace message to the trace listeners in the <see cref="TraceSource.Listeners"/>
        /// collection using the specified message.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceCritical(this TraceSource traceSource, string message)
        {
            traceSource.TraceEvent(TraceEventType.Critical, 0, message);
        }

        /// <summary>
        /// Writes a recoverable error to the trace listeners in the System.Diagnostics.TraceSource.Listeners
        /// collection using the specified format and argument array
        /// and format.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="format">A composite format string that contains text intermixed with zero or more format items, which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is invalid.-or- The number that indicates an argument to format is less than zero,
        /// or greater than or equal to the number of specified objects to format.</exception>
        /// <exception cref="ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
        public static void TraceCritical(this TraceSource traceSource, string format, params object[] args)
        {
            traceSource.TraceEvent(TraceEventType.Critical, 0, format, args);
        }

        #endregion

        #region TraceXxxBlock methods

        public static IDisposable TraceBlock(this TraceSource traceSource, TraceEventType eventType, int id, string message, Func<Stopwatch, string> exit)
        {
            var stopwatch = Stopwatch.StartNew();
            traceSource.TraceEvent(eventType, id, message);
            return new Disposer(() => traceSource.TraceEvent(eventType, id, exit(stopwatch)));
        }
        public static IDisposable TraceBlock(this TraceSource traceSource, TraceEventType eventType, int id, string message)
        {
            return traceSource.TraceBlock(eventType, id, message, (sw) => string.Format("{0} - {1}ms", message, sw.ElapsedMilliseconds));
        }

        #endregion;
    }
}
