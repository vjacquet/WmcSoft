using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WmcSoft.Threading
{

    public sealed class QueuedJobDispatcher : JobDispatcher
    {
        #region Private fields

        Thread[] threads;
        IJobQueue jobs;

        private int activeJobs;
        ManualResetEvent onIdle = new ManualResetEvent(false);
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

            this.jobs = this.GetService<IJobQueue>() ?? new JobQueue();

            threads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++) {
                Thread thread = new Thread(Worker);
                thread.Priority = ThreadPriority.Normal;
                thread.Name = String.Format("ProactorJobDispatcher #{0}", i);
                thread.IsBackground = true;
                if (initializer != null)
                    initializer(thread);
                threads[i] = thread;
                thread.Start();
            }
        }

        #endregion

        #region Properties

        public TimeSpan ContinuationTimeSpan {
            get {
                return TimeSpan.FromTicks(continuationTicks);
            }
            set {
                Interlocked.Exchange(ref continuationTicks, value.Ticks);
            }
        }

        #endregion

        #region Overrides

        public override bool SupportsCancellation {
            get {
                return true;
            }
        }

        public override bool CancellationPending {
            get {
                return cancellationPending;
            }
        }
        bool cancellationPending;

        public override void CancelAsync() {
            if (!cancellationPending) {
                base.CancelAsync();
                cancellationPending = true;
                jobs.Clear(DisposeJob);
            }
        }

        public override void Dispatch(IJob job) {
            if (!CancellationPending) {
                Interlocked.Increment(ref activeJobs);
                jobs.Enqueue(job);
                onIdle.Reset();
            }
        }

        public override bool IsBusy {
            get { return activeJobs != 0; }
        }

        public override bool WaitWhileBusy(int millisecondsTimeout) {
            bool isBusy = IsBusy;
            if (isBusy && millisecondsTimeout != 0) {
                return onIdle.WaitOne(millisecondsTimeout, false);
            }
            return !isBusy;
        }

        protected override void Dispose(bool disposing) {
            for (int i = 0; i < threads.Length; i++) {
                if (threads[i] != null) {
                    if (!threads[i].Join(0))
                        threads[i].Abort();
                    threads[i] = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Internals

        void Worker() {
            IJob job;

            while (!CancellationPending) {
                TimeSpan timeout = TimeSpan.FromTicks(Interlocked.Read(ref continuationTicks));
                if (jobs.TryDequeue(out job, timeout)) {
                    try {
                        job.Execute(this);
                    }
                    finally {
                        DisposeJob(job);

                        if (0 == Interlocked.Decrement(ref activeJobs))
                            onIdle.Set();
                    }
                }
            }
        }

        #endregion
    }

}
