using System;
using System.Collections.Generic;
using System.Text;

namespace WmcSoft.Threading
{
    /// <summary>
    /// Defines a mechanism for executing a job to dispatch using a <see cref="JobDispatcher"/>.
    /// </summary>
    [Serializable]
    public abstract class JobBase : IJob, IDisposable
    {
        #region IJob Membres

        /// <summary>
        /// Performs the job to be done.
        /// </summary>
        /// <param name="serviceProvider">An <see cref="System.Object"/> that implements <see cref="System.IServiceProvider"/>.</param>
        public void Execute(IServiceProvider serviceProvider) {
            DoExecute(serviceProvider);
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
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// When overridden in a derived class, releases the unmanaged resources used by 
        /// the <see cref="JobDispatcher"/>, and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources. </param>
        protected virtual void Dispose(bool disposing) {
        }

        /// <summary>
        /// Releases the resources held by the current instance.
        /// </summary>
        ~JobBase() {
            Dispose(false);
        }

        #endregion
    }
}
