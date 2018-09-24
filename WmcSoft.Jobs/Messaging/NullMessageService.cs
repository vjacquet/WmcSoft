﻿#region Licence

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
using WmcSoft.Threading;

namespace WmcSoft.Messaging
{
    /// <summary>
    /// Represents an implementation of <see cref="IMessageService"/> that does nothing.
    /// </summary>
    public class NullMessageService : IMessageService
    {
        #region IMessageService Membres

        /// <summary>
        /// Starts a transaction.
        /// </summary>
        public void BeginTransaction()
        {
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void CommitTransaction()
        {
        }

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        public void RollbackTransaction()
        {
        }

        /// <summary>
        /// Sends a job request as a message.
        /// </summary>
        /// <param name="message">The job.</param>
        public void Send(IJob message)
        {
        }

        /// <summary>
        /// Receives the next incoming job.
        /// </summary>
        /// <param name="timeout">A <see cref="TimeSpan"/> representing the amount of time to wait for an incoming job request.</param>
        /// <returns>A job or null if the specific time elapsed.</returns>
        public IJob Receive(TimeSpan timeout)
        {
            return null;
        }

        #endregion
    }
}
