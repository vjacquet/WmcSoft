﻿#region Licence

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

namespace WmcSoft.Globalization
{
    public static class Dates
    {
        /// <summary>
        /// Gets the current date.
        /// </summary>
        public static DateTime Today => DateTime.Today;

        /// <summary>
        /// Gets the day after the current date.
        /// </summary>
        public static DateTime Tomorrow => DayAfter(DateTime.Today);

        /// <summary>
        /// Gets the day after the current date.
        /// </summary>
        public static DateTime Yesterday => DayBefore(DateTime.Today);

        /// <summary>
        /// Returns the day before the <paramref name="date"/>^.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The day before.</returns>
        public static DateTime DayBefore(DateTime date)
        {
            return date.AddDays(-1);
        }

        /// <summary>
        /// Returns the day after the <paramref name="date"/>^.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The day before.</returns>
        public static DateTime DayAfter(DateTime date)
        {
            return date.AddDays(+1);
        }
    }
}