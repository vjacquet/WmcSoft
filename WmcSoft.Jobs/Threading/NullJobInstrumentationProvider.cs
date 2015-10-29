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
    /// Represents an implementation of <see cref="IJobInstrumentationProvider"/> that does nothing.
    /// </summary>
    public class NullJobDispatcherInstrumentationProvider : IJobInstrumentationProvider
    {
        #region IJobDisptacherInstrumentationProvider Membres

        /// <summary>
        /// Fires that the job dispatched.
        /// </summary>
        public void FireJobDispatched() {
        }

        /// <summary>
        /// Fires that the job executed.
        /// </summary>
        public void FireJobExecuted() {
        }

        /// <summary>
        /// Fires that the job completed.
        /// </summary>
        public void FireJobCompleted() {
        }

        /// <summary>
        /// Fires that the job failed.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void FireJobFailed(Exception exception) {
        }

        #endregion
    }
}
