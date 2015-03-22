using System;

namespace WmcSoft.Threading
{
    public class LifecycleWrappingJob<T> : JobBase where T : IJob
    {
        #region fields

        T job;
        Action<T> initialization;
        Action<T> cleanup;

        #endregion

        #region Lifecycle

        public LifecycleWrappingJob(T job, Action<T> initialization, Action<T> cleanup) {
            this.job = job;
            this.initialization = initialization;
            this.cleanup = cleanup;
        }

        #endregion

        #region Overrides

        protected override void DoExecute(IServiceProvider serviceProvider) {
            if (initialization != null)
                initialization(job);
            job.Execute(serviceProvider);
            if (cleanup != null)
                cleanup(job);
        }

        #endregion
    }

}
