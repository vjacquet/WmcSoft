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

 ****************************************************************************
 * Adapted from TimeInterval.java
 * ------------------------------
 * Copyright (c) 2004 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System;
using System.Collections.Generic;

namespace WmcSoft.Time
{
    /// <summary>
    /// Defines the extension methods to the <see cref="Interval<Date>"/> class.
    /// This is a static class. 
    /// </summary>
    public static class DateInterval
    {
        /// <summary>
        /// Creates an interval including both <paramref name="start"/> and <paramref name="end"/>.
        /// </summary>
        /// <param name="start">The start date.</param>
        /// <param name="end">The end date.</param>
        /// <returns>The interval between <paramref name="start"/> and <paramref name="end"/>, included.</returns>
        public static Interval<Date> Inclusive(Date start, Date end)
        {
            return Interval.Closed(start, end);
        }

        /// <summary>
        /// Creates an interval for the given <paramref name="month"/> of the <paramref name="year"/>. All days of the <paramref name="month"/> are included.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month</param>
        /// <returns>The interval containing all days of the month.</returns>
        public static Interval<Date> Month(int year, int month)
        {
            var start = new Date(year, month, 1);
            var end = new Date(year, month, DateTime.DaysInMonth(year, month));
            return Inclusive(start, end);
        }

        /// <summary>
        /// Creates an interval for the given <paramref name="year"/>. All days of the <paramref name="year"/> are included.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>The interval containing all days of the year.</returns>
        public static Interval<Date> Year(int year)
        {
            var start = new Date(year, 1, 1);
            var end = new Date(year, 12, 31);
            return Inclusive(start, end);
        }

        /// <summary>
        /// Enumerates all days in the interval in chronological order.
        /// </summary>
        /// <param name="interval">The date interval.</param>
        /// <returns>A lazy enumeration of all days in the interval, in chronological order.</returns>
        public static IEnumerable<Date> Days(this Interval<Date> interval)
        {
            var start = interval.Lower.Value;
            var end = interval.Upper.Value;
            if (interval.IsLowerIncluded)
                yield return start;
            while (start < end) {
                start = start.NextDay();
                yield return start;
            }
            if (interval.IsUpperIncluded)
                yield return end;
        }

        /// <summary>
        /// Enumerates all days in the interval in reverse chronological order.
        /// </summary>
        /// <param name="interval">The date interval.</param>
        /// <returns>A lazy enumeration of all days in the interval, in reverse chronological order.</returns>
        public static IEnumerable<Date> DaysBackwards(this Interval<Date> interval)
        {
            var start = interval.Lower.Value;
            var end = interval.Upper.Value;
            if (interval.IsUpperIncluded)
                yield return end;
            while (start < end) {
                end = end.PreviousDay();
                yield return end;
            }
            if (interval.IsLowerIncluded)
                yield return start;
        }
    }
}
