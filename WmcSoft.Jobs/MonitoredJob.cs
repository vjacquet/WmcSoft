using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace WmcSoft.Threading
{
    public class MonitoredJob : JobBase
    {

        #region Private fields

        public event JobMonitoringHandler Executing;
        public event JobMonitoringHandler Executed;

        private Stopwatch stopWatch;
        private long delayBeforeExecute;
        private long totalTime;

        IJob decorated;

        #endregion

        #region Lifecycle

        public MonitoredJob(IJob decorated) {
            this.decorated = decorated;
            delayBeforeExecute = -1;
            totalTime = -1;

            stopWatch = Stopwatch.StartNew();
        }

        #endregion

        #region  properties

        /// <summary>
        /// Gets the time to begin execution.
        /// </summary>
        /// <value>The time to begin execution.</value>
        public TimeSpan DelayBeforeExecute {
            get { return TimeSpan.FromTicks(delayBeforeExecute); }
        }

        public TimeSpan ExecuteTime {
            get { return TimeSpan.FromTicks(totalTime - delayBeforeExecute); }
        }

        /// <summary>
        /// Gets the total time.
        /// </summary>
        /// <value>The total time.</value>
        public TimeSpan TotalTime {
            get { return TimeSpan.FromTicks(totalTime); }
        }

        /// <summary>
        /// Gets the total time  in ticks.
        /// </summary>
        /// <value>The total time in ticks.</value>
        public long TotalTimeInTicks {
            get { return totalTime; }
        }

        #endregion

        #region Overridables

        protected sealed override void DoExecute(IServiceProvider serviceProvider) {
            JobMonitoringHandler handler;

            delayBeforeExecute = stopWatch.ElapsedTicks;
            handler = Executing;
            if (handler != null)
                handler(decorated);

            try {
                decorated.Execute(serviceProvider);
            }
            finally {
                stopWatch.Stop();
                totalTime = stopWatch.ElapsedTicks;

                handler = Executed;
                if (handler != null)
                    handler(decorated);
            }
        }

        #endregion
    }

    public delegate void JobMonitoringHandler(IJob job);
}
