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

        public event JobMonitoringHandler Executing;
        public event JobMonitoringHandler Executed;

        private readonly Stopwatch _stopWatch;
        private long _delayBeforeExecute;
        private long _totalTime;

        private readonly IJob _decorated;

        #endregion

        #region Lifecycle

        public MonitoredJob(IJob decorated) {
            _decorated = decorated;
            _delayBeforeExecute = -1;
            _totalTime = -1;

            _stopWatch = Stopwatch.StartNew();
        }

        #endregion

        #region  properties

        /// <summary>
        /// Gets the time to begin execution.
        /// </summary>
        /// <value>The time to begin execution.</value>
        public TimeSpan DelayBeforeExecute {
            get { return TimeSpan.FromTicks(_delayBeforeExecute); }
        }

        public TimeSpan ExecuteTime {
            get { return TimeSpan.FromTicks(_totalTime - _delayBeforeExecute); }
        }

        /// <summary>
        /// Gets the total time.
        /// </summary>
        /// <value>The total time.</value>
        public TimeSpan TotalTime {
            get { return TimeSpan.FromTicks(_totalTime); }
        }

        /// <summary>
        /// Gets the total time  in ticks.
        /// </summary>
        /// <value>The total time in ticks.</value>
        public long TotalTicks {
            get { return _totalTime; }
        }

        #endregion

        #region Overridables

        protected sealed override void DoExecute(IServiceProvider serviceProvider) {
            JobMonitoringHandler handler;

            _delayBeforeExecute = _stopWatch.ElapsedTicks;
            handler = Executing;
            if (handler != null)
                handler(_decorated);

            try {
                _decorated.Execute(serviceProvider);
            }
            finally {
                _stopWatch.Stop();
                _totalTime = _stopWatch.ElapsedTicks;

                handler = Executed;
                if (handler != null)
                    handler(_decorated);
            }
        }

        #endregion
    }

    public delegate void JobMonitoringHandler(IJob job);
}
