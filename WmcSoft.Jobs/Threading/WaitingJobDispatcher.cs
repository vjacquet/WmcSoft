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

using System.Threading;

namespace WmcSoft.Threading
{
    public class WaitingJobDispatcher : JobDispatcherDecorator, IWaitableJobDispatcher
    {
        #region WaitingDecorator

        class WaitingDecorator : JobDispatcherDecorator, IWaitableJobDispatcher
        {
            private int workingJobs;
            private readonly ManualResetEvent onIdle = new ManualResetEvent(false);

            public WaitingDecorator(JobDispatcher jobDispatcher)
                : base(jobDispatcher)
            {

            }

            public override void Dispatch(IJob job)
            {
                var monitored = new MonitoredJob(job);
                monitored.Executed += OnExecuted;

                Interlocked.Increment(ref workingJobs);
                onIdle.Reset();

                base.Dispatch(monitored);
            }

            void OnExecuted(object sender, JobMonitoringEventArgs e)
            {
                // unregister to prevent being called twice if dispatched again.
                var monitored = (MonitoredJob)sender;
                monitored.Executed -= OnExecuted;

                if (0 == Interlocked.Decrement(ref workingJobs))
                    onIdle.Set();
            }

            #region IWaitingJobDispatcher Membres

            public bool IsBusy => workingJobs > 0;

            public bool WaitAll(int millisecondsTimeout)
            {
                bool isBusy = IsBusy;
                if (isBusy && millisecondsTimeout != 0) {
                    return onIdle.WaitOne(millisecondsTimeout, false);
                }
                return !isBusy;
            }

            #endregion
        }

        #endregion

        private IWaitableJobDispatcher jobDispatcher;

        #region Lifecycle

        static JobDispatcher Decorate(JobDispatcher jobDispatcher)
        {
            var waiting = jobDispatcher as IWaitableJobDispatcher;
            if (waiting != null)
                return jobDispatcher;
            return new WaitingDecorator(jobDispatcher);
        }

        public WaitingJobDispatcher(JobDispatcher jobDispatcher)
            : base(Decorate(jobDispatcher))
        {
            this.jobDispatcher = Inner as IWaitableJobDispatcher;
        }

        #endregion

        #region IWaitingJobDispatcher Membres

        public bool WaitAll(int millisecondsTimeout)
        {
            return jobDispatcher.WaitAll(millisecondsTimeout);
        }

        public bool IsBusy => jobDispatcher.IsBusy;

        #endregion
    }
}
