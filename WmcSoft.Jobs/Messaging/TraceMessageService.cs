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
using WmcSoft.Threading;

namespace WmcSoft.Messaging
{
    /// <summary>
    /// Represents an implementation of <see cref="IMessageService"/> that only traces the sending of a job, instead of actually sending it.
    /// </summary>
    public class TraceMessageService : IMessageService
    {
        #region Fields

        readonly IMessageService _messageService;
        readonly TraceSource _traceSource;
        readonly int _id;

        #endregion

        #region Lifecycle

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceMessageService"/> class.
        /// </summary>
        public TraceMessageService(IMessageService messageService, TraceSource source, int id = 0)
        {
            _messageService = messageService;
            _traceSource = source;
            _id = id;
        }

        #endregion

        #region Helpers

        void Eval(string message, Action<IMessageService> action)
        {
            try {
                var stopwatch = Stopwatch.StartNew();
                _traceSource.TraceInformation(">> " + message);
                action(_messageService);
                _traceSource.TraceInformation("<< " + message + " - " + stopwatch);
            } catch (Exception e) {
                _traceSource.TraceEvent(TraceEventType.Error, _id, "<< " + message + ": " + e.Message + "\r\n" + e.StackTrace);
                throw;
            }
        }

        T Eval<T>(string message, Func<IMessageService, T> action)
        {
            try {
                var stopwatch = Stopwatch.StartNew();
                _traceSource.TraceInformation(">> " + message);
                var result = action(_messageService);
                _traceSource.TraceInformation("<< " + message + " - " + stopwatch);
                return result;
            } catch (Exception e) {
                _traceSource.TraceEvent(TraceEventType.Error, _id, "<< " + message + ": " + e.Message + "\r\n" + e.StackTrace);
                throw;
            }
        }

        #endregion

        #region IMessageService Membres

        /// <summary>
        /// Starts a transaction.
        /// </summary>
        public void BeginTransaction()
        {
            Eval("BeginTransaction", ms => ms.BeginTransaction());
        }
        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void CommitTransaction()
        {
            Eval("CommitTransaction", ms => ms.CommitTransaction());
        }
        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            Eval("RollbackTransaction", ms => ms.RollbackTransaction());
        }

        /// <summary>
        /// Sends a job request as a message.
        /// </summary>
        /// <param name="message">The job.</param>
        public void Send(IJob message)
        {
            Eval("Send", ms => ms.Send(message));
        }

        /// <summary>
        /// Receives the next incoming job.
        /// </summary>
        /// <param name="timeout">A <see cref="TimeSpan"/> representing the amount of time to wait for an incoming job request.</param>
        /// <returns>
        /// A job or null if the specific time elapsed.
        /// </returns>
        public IJob Receive(TimeSpan timeout)
        {
            return Eval("Receive", ms => ms.Receive(timeout));
        }

        #endregion
    }
}