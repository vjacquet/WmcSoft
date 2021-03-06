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
using System.ComponentModel.Design;
using System.Threading;
using WmcSoft.Properties;

namespace WmcSoft.Threading
{
    public abstract class JobDispatcher : IServiceProvider, IDisposable
    {
        #region NullJobDispatcher Class

        sealed class NullJobDispatcher : JobDispatcher
        {
            public override void Dispatch(IJob job)
            {
            }
        }

        #endregion

        #region Internal job Classes

        struct ThreadStartJob : IJob
        {
            ThreadStart start;

            internal ThreadStartJob(ThreadStart start)
            {
                if (start == null) throw new ArgumentNullException(nameof(start));

                this.start = start;
            }

            #region IJob Membres

            void IJob.Execute(IServiceProvider serviceProvider)
            {
                start();
            }

            #endregion
        }

        struct ActionJob<T> : IJob
        {
            Action<T> action;

            internal ActionJob(Action<T> action)
            {
                if (action == null) throw new ArgumentNullException(nameof(action));

                this.action = action;
            }

            #region IJob Membres

            void IJob.Execute(IServiceProvider serviceProvider)
            {
                var service = serviceProvider.GetService(typeof(T));
                var t = service == null ? default : (T)service;
                action(t);
            }

            #endregion
        }

        #endregion

        #region Lifecycle

        protected JobDispatcher()
        {
            serviceContainer = new ServiceContainer();
        }

        protected JobDispatcher(IServiceProvider parentProvider)
        {
            serviceContainer = new ServiceContainer(parentProvider);
        }

        #endregion

        #region Static Properties

        public static readonly JobDispatcher Null = new NullJobDispatcher();

        #endregion

        #region Fields

        readonly IServiceProvider serviceContainer;

        #endregion

        #region Abstracts & overridables

        /// <summary>
        /// Dispatches the specified job.
        /// </summary>
        /// <param name="job">An <see cref="System.Object"/> that implements <see cref="IJob"/>.</param>
        public abstract void Dispatch(IJob job);

        /// <summary>
        /// Dispatches the specified job.
        /// </summary>
        /// <param name="job"></param>
        public void Dispatch(ThreadStart job)
        {
            Dispatch(new ThreadStartJob(job));
        }

        /// <summary>
        /// Dispatches the specified job.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="job"></param>
        public void Dispatch<T>(Action<T> job)
        {
            Dispatch(new ActionJob<T>(job));
        }

        /// <summary>
        /// Disposes the specified job if it implements the interface
        /// </summary>
        /// <param name="job">An <see cref="System.Object"/> that implements <see cref="IJob"/>.</param>
        protected virtual void Dispose(IJob job)
        {
            IDisposable disposable = job as IDisposable;
            if (disposable != null) {
                disposable.Dispose();
            }
        }

        ///// <summary>
        ///// Blocks the current thread while the dispatcher is busy.
        ///// </summary>
        ///// <returns><c>true</c> if the current instance receives a signal; otherwise <see cref="WaitWhileBusy()"/> never returns.</returns>
        //public bool WaitWhileBusy() {
        //    return WaitWhileBusy(-1);
        //}

        ///// <summary>
        ///// Blocks the current thread while the dispatcher is busy, using a 32-bit signed 
        ///// integer to measure the time interval.
        ///// </summary>
        ///// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="System.Threading.Timeout.Infinite"/> (-1) to wait indefinitely.</param>
        ///// <returns><c>true</c> if the current instance receives a signal; otherwise, <c>false</c>.</returns>
        //public abstract bool WaitWhileBusy(int millisecondsTimeout);

        ///// <summary>
        ///// Returns <c>true</c> when the <see cref="JobDispatcher"/> is busy.
        ///// </summary>
        //public abstract bool IsBusy { get; }

        /// <summary>
        /// Trait property to indicate if the JobDispatcher supports cancellation.
        /// </summary>
        public virtual bool SupportsCancellation => false;

        /// <summary>
        /// Returns <c>true</c> if the job dispatcher has been cancelled.
        /// </summary>
        public virtual bool CancellationPending => false;

        /// <summary>
        /// Requests to cancel the job dispatcher.
        /// </summary>
        public virtual void CancelAsync()
        {
            throw new NotSupportedException(Resources.DispatcherDoesntSuppportCancellation);
        }

        #endregion

        #region IServiceProvider Membres

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public virtual object GetService(Type serviceType)
        {
            if (serviceType == typeof(IServiceProvider) || serviceType == typeof(JobDispatcher))
                return this;
            return serviceContainer.GetService(serviceType);
        }

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
        ~JobDispatcher()
        {
            Dispose(false);
        }

        #endregion
    }
}
