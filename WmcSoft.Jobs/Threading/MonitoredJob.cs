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

namespace WmcSoft.Threading
{
    public class MonitoredJob : JobBase
    {
        #region Private fields

        private readonly Stopwatch stopWatch;
        private long delayBeforeExecute;
        private long totalTime;

        private readonly IJob _decorated;

        #endregion

        #region Lifecycle

        public MonitoredJob(IJob decorated)
        {
            _decorated = decorated;
            delayBeforeExecute = -1;
            totalTime = -1;

            stopWatch = Stopwatch.StartNew();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the time to begin execution.
        /// </summary>
        /// <value>The time to begin execution.</value>
        public TimeSpan DelayBeforeExecute => TimeSpan.FromTicks(delayBeforeExecute);

        public TimeSpan ExecuteTime => TimeSpan.FromTicks(totalTime - delayBeforeExecute);

        /// <summary>
        /// Gets the total time.
        /// </summary>
        /// <value>The total time.</value>
        public TimeSpan TotalTime => TimeSpan.FromTicks(totalTime);

        /// <summary>
        /// Gets the total time  in ticks.
        /// </summary>
        /// <value>The total time in ticks.</value>
        public long TotalTicks => totalTime;

        #endregion

        #region Events

        public event JobMonitoringEventHandler Executing;
        public event JobMonitoringEventHandler Executed;

        #endregion

        #region Overridables

        protected sealed override void DoExecute(IServiceProvider serviceProvider)
        {
            delayBeforeExecute = stopWatch.ElapsedTicks;
            var e = new JobMonitoringEventArgs(_decorated);
            var handler = Executing;
            if (handler != null)
                handler(this, e);

            try {
                _decorated.Execute(serviceProvider);
            } finally {
                stopWatch.Stop();
                totalTime = stopWatch.ElapsedTicks;

                handler = Executed;
                if (handler != null)
                    handler(this, e);
            }
        }

        #endregion
    }

    //public delegate void JobMonitoringHandler(IJob job);
    public delegate void JobMonitoringEventHandler(object sender, JobMonitoringEventArgs e);

    public class JobMonitoringEventArgs : EventArgs
    {
        public JobMonitoringEventArgs(IJob job)
        {
            Job = job;
        }

        public IJob Job { get; }
    }
}
