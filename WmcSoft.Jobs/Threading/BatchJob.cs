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
    [Serializable]
    public abstract class BatchJob : IJob, IDisposable
    {
        #region BatchJobDispatcher

        class BatchJobDispatcher : JobDispatcherDecorator
        {
            #region WrapJob class

            class WrapJob : IJob
            {
                BatchJobDispatcher batch;
                IJob job;

                internal WrapJob(BatchJobDispatcher batch, IJob job)
                {
                    this.batch = batch;
                    this.job = job;
                }

                #region IJob Membres

                void IJob.Execute(IServiceProvider serviceProvider)
                {
                    job.Execute(batch);
                    batch.JobExecuted(this);
                }

                #endregion
            }

            #endregion

            int _jobCount;
            readonly BatchJob _batch;

            public BatchJobDispatcher(BatchJob batch, JobDispatcher dispatcher)
                : base(dispatcher)
            {
                _batch = batch;
                _jobCount = 0;
            }

            public override void Dispatch(IJob job)
            {
                Interlocked.Increment(ref _jobCount);
                Inner.Dispatch(new WrapJob(this, job));
            }

            protected void JobExecuted(IJob job)
            {
                Interlocked.Decrement(ref _jobCount);
                if (_jobCount == 0)
                    _batch.OnBatchComplete(EventArgs.Empty);
            }

        }

        #endregion

        #region IJob Membres

        /// <summary>
        /// Performs the job to be done.
        /// </summary>
        /// <param name="serviceProvider">An <see cref="System.Object"/> that implements <see cref="System.IServiceProvider"/>.</param>
        public void Execute(IServiceProvider serviceProvider)
        {
            var parent = serviceProvider.GetService<JobDispatcher>();
            var dispatcher = new BatchJobDispatcher(this, parent);
            DoExecute(dispatcher);
        }

        #endregion

        #region Overridables

        /// <summary>
        /// Override this method to perform the job to be done.
        /// </summary>
        /// <param name="serviceProvider">An <see cref="System.Object"/> that implements <see cref="System.IServiceProvider"/>.</param>
        protected abstract void DoExecute(IServiceProvider serviceProvider);

        #endregion

        #region IDisposable Membres

        /// <summary>
        /// Releases all resources used by the <see cref="JobDispatcher"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// When overridden in a derived class, releases the unmanaged resources used by 
        /// the <see cref="JobDispatcher"/>, and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources. </param>
        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Releases the resources held by the current instance.
        /// </summary>
        ~BatchJob()
        {
            Dispose(false);
        }

        #endregion

        #region Events

        public event EventHandler BatchComplete;

        protected virtual void OnBatchComplete(EventArgs e)
        {
            EventHandler handler = BatchComplete;
            if (handler != null) {
                handler(this, e);
            }
        }

        #endregion
    }
}
