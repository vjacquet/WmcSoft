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

namespace WmcSoft.Threading
{
    /// <summary>
    /// Base class when creating job dispatchers decorating another one.
    /// </summary>
    public class JobDispatcherDecorator : JobDispatcher
    {
        #region Private fields

        JobDispatcher jobDispatcher;
        protected JobDispatcher Inner => jobDispatcher;

        #endregion

        #region Lifecycle

        /// <summary>
        /// Initializes a new instance of the <see cref="JobDispatcherDecorator"/> class.
        /// </summary>
        /// <param name="jobDispatcher">The job dispatcher.</param>
        protected JobDispatcherDecorator(JobDispatcher jobDispatcher)
        {
            this.jobDispatcher = jobDispatcher;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Cancels the async operation.
        /// </summary>
        public override void CancelAsync()
        {
            jobDispatcher.CancelAsync();
        }

        /// <summary>
        /// Gets a value indicating whether a cancellation is pending.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if a cancellation is pending; otherwise, <c>false</c>.
        /// </value>
        public override bool CancellationPending => jobDispatcher.CancellationPending;

        /// <summary>
        /// Gets a value indicating whether the dispatcher supports cancellation.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the dispatcher supports cancellation; otherwise, <c>false</c>.
        /// </value>
        public override bool SupportsCancellation => jobDispatcher.SupportsCancellation;

        /// <summary>
        /// Dispatches the specified job.
        /// </summary>
        /// <param name="job">An <see cref="System.Object"/> that implements <see cref="IJob"/>.</param>
        public override void Dispatch(IJob job)
        {
            jobDispatcher.Dispatch(job);
        }

        /// <summary>
        /// When overridden in a derived class, releases the unmanaged resources used by
        /// the <see cref="JobDispatcher"/>, and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (jobDispatcher != null) {
                jobDispatcher.Dispose();
                jobDispatcher = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>
        /// A service object of type <paramref name="serviceType"/>.
        /// -or-
        /// <c>null</c> if there is no service object of type <paramref name="serviceType"/>.
        /// </returns>
        public override object GetService(Type serviceType)
        {
            if (serviceType == typeof(JobDispatcher))
                return this;
            object instance = base.GetService(serviceType);
            if (instance == null)
                instance = jobDispatcher.GetService(serviceType);
            return instance;
        }

        #endregion
    }
}
