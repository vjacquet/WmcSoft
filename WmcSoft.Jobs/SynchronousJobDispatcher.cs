using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace WmcSoft.Threading
{
    public sealed class SynchronousJobDispatcher : JobDispatcher
    {
        #region Private fields

        int _refCount;

        #endregion

        #region Overrides

        /// <summary>
        /// Dispatches the specified job.
        /// </summary>
        /// <param name="job">An <see cref="System.Object"/> that implements <see cref="IJob"/>.</param>
        public override void Dispatch(IJob job) {
            if (job == null) {
                throw new ArgumentNullException("job");
            }

            Interlocked.Increment(ref _refCount);
            job.Execute(this);
            Interlocked.Decrement(ref _refCount);
        }

        /// <summary>
        /// Blocks the current thread while the dispatcher is busy, using a 32-bit signed 
        /// integer to measure the time interval.
        /// </summary>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="System.Threading.Timeout.Infinite"/> (-1) to wait indefinitely.</param>
        /// <returns><c>true</c> if the current instance receives a signal; otherwise, <c>false</c>.</returns>
        public override bool WaitWhileBusy(int millisecondsTimeout) {
            return true;
        }

        /// <summary>
        /// Returns <c>true</c> when the <see cref="JobDispatcher"/> is busy.
        /// </summary>
        public override bool IsBusy {
            get { return _refCount > 0; }
        }

        #endregion
    }
}
