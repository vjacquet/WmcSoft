#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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
using WmcSoft.Messaging;
using WmcSoft.Properties;

namespace WmcSoft.Threading
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageQueueJob : JobBase
    {
        const int DefaultAttempts = 3;

        [ThreadStatic]
        static int attempt = 0;

        /// <summary>
        /// Gets the attempt.
        /// </summary>
        public static int Attempt {
            get { return attempt; }
        }

        /// <summary>
        /// Receives a job using <see cref="IMessageService.Receive()"/> and executes it.
        /// </summary>
        /// <param name="serviceProvider">An <see cref="System.Object"/> that implements <see cref="System.IServiceProvider"/>.</param>
        /// <remarks>The <paramref name="serviceProvider"/> must provide an instance of <see cref="IMessageService"/>.</remarks>
        protected override void DoExecute(IServiceProvider serviceProvider) {
            var dispatcher = serviceProvider.GetService<JobDispatcher>();
            var messageService = serviceProvider.GetService<IMessageService>();
            var instrumentation = serviceProvider.GetService<IJobInstrumentationProvider>();

            if (dispatcher.CancellationPending) {
                // stopping the service
                return;
            }

            try {
                TimeSpan timeout = TimeSpan.FromMilliseconds(1000);

                messageService.BeginTransaction();
                IJob job = messageService.Receive(timeout);
                if (job != null) {
                    job.Execute(serviceProvider);

                    messageService.CommitTransaction();
                    Interlocked.Exchange(ref attempt, 0);
                } else {
                    messageService.RollbackTransaction();
                }
            }
            catch (Exception exception) {
                Trace.TraceWarning(Resources.UnexceptedErrorMessage, exception.Message);
                if (Interlocked.Increment(ref attempt) >= DefaultAttempts) {
                    Trace.TraceError(Resources.TooManyConsecutiveFailedAttempts);
                    messageService.CommitTransaction(); // removes the job
                    Interlocked.Exchange(ref attempt, 0);
                } else {
                    messageService.RollbackTransaction();
                }
            }
            finally {
                dispatcher.Dispatch(new MessageQueueJob());
            }
        }
    }
}
