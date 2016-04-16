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
using System.Threading;

namespace WmcSoft.Threading
{
    public sealed class QueuedJobDispatcher : JobDispatcher, IWaitableJobDispatcher
    {
        #region Private fields

        readonly Thread[] _threads;
        readonly ManualResetEvent _onIdle = new ManualResetEvent(false);
        readonly IJobQueue _jobs;

        private int _workingJobs;
        long continuationTicks;

        #endregion

        #region Lifecycle

        public QueuedJobDispatcher(int threadCount)
            : this(null, threadCount, null) {
        }

        public QueuedJobDispatcher(IServiceProvider parentProvider, int threadCount)
            : this(parentProvider, threadCount, null) {
        }

        public QueuedJobDispatcher(int threadCount, Action<Thread> initializer)
            : this(null, threadCount, initializer) {
        }

        public QueuedJobDispatcher(IServiceProvider parentProvider, int threadCount, Action<Thread> initializer)
            : base(parentProvider) {
            continuationTicks = TimeSpan.FromMilliseconds(1000).Ticks;

            if (threadCount < 1)
                throw new ArgumentOutOfRangeException("threadCount");

            _jobs = this.GetService<IJobQueue>() ?? new JobQueue();

            _threads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++) {
                Thread thread = new Thread(Worker);
                thread.Priority = ThreadPriority.Normal;
                thread.Name = String.Format("QueuedJobDispatcher #{0}", i);
                thread.IsBackground = true;
                if (initializer != null)
                    initializer(thread);
                _threads[i] = thread;
                thread.Start();
            }
        }

        #endregion

        #region Properties

        public TimeSpan ContinuationTimeSpan {
            get { return TimeSpan.FromTicks(continuationTicks); }
            set { Interlocked.Exchange(ref continuationTicks, value.Ticks); }
        }

        #endregion

        #region Overrides

        public override bool SupportsCancellation {
            get { return true; }
        }

        public override bool CancellationPending {
            get { return cancellationPending; }
        }
        bool cancellationPending;

        public override void CancelAsync() {
            if (!cancellationPending) {
                base.CancelAsync();
                cancellationPending = true;
                _jobs.Clear(Dispose);
            }
        }

        public override void Dispatch(IJob job) {
            if (!CancellationPending) {
                Interlocked.Increment(ref _workingJobs);
                _jobs.Enqueue(job);
                _onIdle.Reset();
            }
        }

        public bool IsBusy {
            get { return _workingJobs != 0; }
        }

        public bool WaitAll(int millisecondsTimeout) {
            bool isBusy = IsBusy;
            if (isBusy && millisecondsTimeout != 0) {
                return _onIdle.WaitOne(millisecondsTimeout, false);
            }
            return !isBusy;
        }

        protected override void Dispose(bool disposing) {
            for (int i = 0; i < _threads.Length; i++) {
                if (_threads[i] != null) {
                    if (!_threads[i].Join(0))
                        _threads[i].Abort();
                    _threads[i] = null;
                }
            }
            _onIdle.Close();
            base.Dispose(disposing);
        }

        #endregion

        #region Internals

        void Worker() {
            IJob job;

            while (!CancellationPending) {
                TimeSpan timeout = TimeSpan.FromTicks(Interlocked.Read(ref continuationTicks));
                if (_jobs.TryDequeue(out job, timeout)) {
                    try {
                        job.Execute(this);
                    }
                    finally {
                        Dispose(job);

                        if (0 == Interlocked.Decrement(ref _workingJobs))
                            _onIdle.Set();
                    }
                }
            }
        }

        #endregion
    }
}
