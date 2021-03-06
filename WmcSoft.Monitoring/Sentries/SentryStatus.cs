﻿#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft.Monitoring.Sentries
{
    /// <summary>
    /// Specifies what status <see cref="ISentry"/> may report.
    /// </summary>
    public enum SentryStatus
    {
        /// <summary>
        /// The sentry does not have anything to report.
        /// </summary>
        None,
        /// <summary>
        /// The sentry reports no problem.
        /// </summary>
        Success,
        /// <summary>
        /// The sentry reports a warning.
        /// </summary>
        Warning,
        /// <summary>
        /// The sentry reports an error.
        /// </summary>
        Error,
    }
}
