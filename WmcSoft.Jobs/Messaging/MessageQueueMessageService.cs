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
using System.Messaging;
using WmcSoft.Threading;

namespace WmcSoft.Messaging
{
    /// <summary>
    /// Represents an implementation of <see cref="IMessageService"/> that 
    /// uses <see cref="MessageQueue"/> as a medium of messaging.
    /// </summary>
    public class MessageQueueMessageService : IMessageService
    {
        #region State

        private readonly MessageQueue messageQueue;

        #endregion

        #region Lifecycle

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueueMessageService"/> class.
        /// </summary>
        /// <param name="messageQueue">The message queue.</param>
        public MessageQueueMessageService(MessageQueue messageQueue)
        {
            this.messageQueue = messageQueue;
        }

        #endregion

        #region IMessageService Membres

        [ThreadStatic]
        private static MessageQueueTransaction transaction;

        /// <summary>
        /// Starts a transaction.
        /// </summary>
        public void BeginTransaction()
        {
            if (transaction != null) throw new InvalidOperationException();

            transaction = new MessageQueueTransaction();
            transaction.Begin();
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void CommitTransaction()
        {
            if (transaction == null) throw new InvalidOperationException();

            transaction.Commit();
            transaction = null;
        }

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            if (transaction == null) throw new InvalidOperationException();

            transaction.Abort();
            transaction = null;
        }

        /// <summary>
        /// Sends a job request as a message.
        /// </summary>
        /// <param name="message">The job.</param>
        public void Send(IJob message)
        {
            if (transaction != null) {
                messageQueue.Send(message, transaction);
            } else {
                messageQueue.Send(message, MessageQueueTransactionType.Single);
            }
        }

        /// <summary>
        /// Receives the next incoming job.
        /// </summary>
        /// <returns>A job.</returns>
        /// <exception cref="InvalidCastException">The message body does not implement <see cref="IJob"/>.</exception>
        public IJob Receive()
        {
            var message = transaction != null
                ? messageQueue.Receive(transaction)
                : messageQueue.Receive(MessageQueueTransactionType.Single);
            return (IJob)message.Body;
        }

        /// <summary>
        /// Receives the next incoming job.
        /// </summary>
        /// <param name="timeout">A <see cref="TimeSpan"/> representing the amount of time to wait for an incoming job request.</param>
        /// <returns>A job or null if the specific time elapsed.</returns>
        /// <exception cref="InvalidCastException">The message body does not implement <see cref="IJob"/>.</exception>
        public IJob Receive(TimeSpan timeout)
        {
            try {
                var message = transaction != null
                    ? messageQueue.Receive(timeout, transaction)
                    : messageQueue.Receive(timeout, MessageQueueTransactionType.Single);
                return (IJob)message.Body;
            } catch (MessageQueueException exception) {
                if (exception.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout) {
                    return null;
                }
                throw;
            }
        }

        #endregion
    }
}
