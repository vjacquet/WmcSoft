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
    /// Defines an abstraction of a job queue.
    /// </summary>
    public interface IJobQueue
    {
        /// <summary>
        /// Enqueues the specified job.
        /// </summary>
        /// <param name="job">The job.</param>
        void Enqueue(IJob job);
        /// <summary>
        /// Dequeues the next job.
        /// </summary>
        /// <returns>A job.</returns>
        IJob Dequeue();
        /// <summary>
        /// Tries to dequeue the next job.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns><c>true</c> if a job was dequed before the specified time elapsed; otherwise, <c>false</c>.</returns>
        bool TryDequeue(out IJob job, TimeSpan timeout);
        /// <summary>
        /// Clears the job queue.
        /// </summary>
        /// <param name="action">The cleanup action for each job.</param>
        void Clear(Action<IJob> action);
    }
}
