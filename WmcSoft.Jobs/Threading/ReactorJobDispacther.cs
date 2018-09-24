#region Licence

/****************************************************************************
          Copyright 1999-2010 Vincent J. Jacquet.  All rights reserved.

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
using System.Threading;

namespace WmcSoft.Threading
{
    public sealed class ReactorJobDispatcher : JobDispatcher, IWaitableJobDispatcher
    {
        #region JobDispatcher Membres

        /// <summary>
        /// Dispatches the specified job.
        /// </summary>
        /// <param name="job">An <see cref="Object"/> that implements <see cref="IJob"/>.</param>
        public override void Dispatch(IJob job)
        {
            if (job == null) {
                throw new ArgumentNullException("job");
            }

            Interlocked.Increment(ref workingJobs);
            onIdle.Reset();
            ThreadPool.QueueUserWorkItem(DoJob, job);
        }

        /// <summary>
        /// Returns <c>true</c> when the <see cref="JobDispatcher"/> is busy.
        /// </summary>
        public bool IsBusy => workingJobs > 0;

        /// <summary>
        /// Blocks the current thread while the dispatcher is busy, using a 32-bit signed 
        /// integer to measure the time interval.
        /// </summary>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="System.Threading.Timeout.Infinite"/> (-1) to wait indefinitely.</param>
        /// <returns><c>true</c> if the current instance receives a signal; otherwise, <c>false</c>.</returns>
        public bool WaitAll(int millisecondsTimeout)
        {
            bool isBusy = IsBusy;
            if (isBusy && millisecondsTimeout != 0) {
                return onIdle.WaitOne(millisecondsTimeout, false);
            }
            return !isBusy;
        }

        #endregion

        #region Private

        private int workingJobs;
        private readonly ManualResetEvent onIdle = new ManualResetEvent(false);

        private void DoJob(object obj)
        {
            var job = (IJob)obj;
            try {
                job.Execute(this);
            } finally {
                if (0 == Interlocked.Decrement(ref workingJobs))
                    onIdle.Set();
                Dispose(job);
            }
        }

        #endregion
    }
}
