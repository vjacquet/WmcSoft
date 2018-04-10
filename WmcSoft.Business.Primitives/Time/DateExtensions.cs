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
 * Adapted from CalendarDate.java
 * ------------------------------
 * Copyright (c) 2004 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System;
using System.Globalization;

namespace WmcSoft.Time
{
    public static partial class DateExtensions
    {
        /// <summary>
        /// Gets the week of the month of the date represented by this instance.
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The week of the month</returns>
        public static int WeekOfMonth(this Date date)
        {
            int day = date.Day;
            int dayOfWeek = (int)(new Date(date.Year, date.Month, 1)).DayOfWeek;
            return Math.DivRem(6 + day + dayOfWeek, 7, out int reminder);
        }

        /// <summary>
        /// Gets the first day of the month of the date represented by this instance.
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The first day of the month</returns>
        public static Date FirstDayOfMonth(this Date date)
        {
            return new Date(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Gets the first day of the next month of the date represented by this instance.
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The first day of the month</returns>
        public static Date FirstDayOfNextMonth(this Date date)
        {
            return FirstDayOfMonth(date).AddMonths(1);
        }

        /// <summary>
        /// Gets the last day of the month of the date represented by this instance.
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The last day of the month</returns>
        public static Date LastDayOfMonth(this Date date)
        {
            var year = date.Year;
            var month = date.Month;
            return new Date(year, month, DateTime.DaysInMonth(year, month));
        }

        static Date UnguardedFirstDayOfWeek(Date date, IFormatProvider formatProvider)
        {
            var dateTimeFormatInfo = (DateTimeFormatInfo)formatProvider.GetFormat(typeof(DateTimeFormatInfo));
            var firstDayOfWeek = dateTimeFormatInfo.FirstDayOfWeek;
            var dayOfWeek = date.DayOfWeek;
            var delta = firstDayOfWeek - dayOfWeek;
            if (delta > 0)
                delta -= 7;
            return date.AddDays(delta);
        }

        /// <summary>
        /// Gets the first day of the week of the date represented by this instance.
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="formatProvider">The format provider to retrieve <see cref="DateTimeFormatInfo.FirstDayOfWeek"/>.</param>
        /// <returns>The first day of the week</returns>
        public static Date FirstDayOfWeek(this Date date, IFormatProvider formatProvider = null)
        {
            return UnguardedFirstDayOfWeek(date, formatProvider ?? CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets the last day of the week of the date represented by this instance.
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="formatProvider">The format provider to retrieve <see cref="DateTimeFormatInfo.FirstDayOfWeek"/>.</param>
        /// <returns>The last day of the week</returns>
        public static Date LastDayOfWeek(this Date date, IFormatProvider formatProvider = null)
        {
            return UnguardedFirstDayOfWeek(date, formatProvider ?? CultureInfo.CurrentCulture)
                .AddDays(6);
        }

        /// <summary>
        /// Gets the week of the year using ISO-8601 convention.
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The week of the year following the ISO-8601 convention.</returns>
        public static int Iso8601WeekOfYear(this Date date)
        {
            // see <https://blogs.msdn.microsoft.com/shawnste/2006/01/24/iso-8601-week-of-year-format-in-microsoft-net/>
            var cal = CultureInfo.InvariantCulture.Calendar;
            var dateTime = (DateTime)date;

            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = cal.GetDayOfWeek(dateTime);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday) {
                dateTime = dateTime.AddDays(3);
            }

            // Return the week of our adjusted day
            return cal.GetWeekOfYear(dateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        /// <summary>
        /// Adds the given duration.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="duration">The duration.</param>
        /// <returns>The new date.</returns>
        public static Date Add(this Date date, Duration duration)
        {
            if (duration.Unit.IsConvertibleToDays())
                return date.AddDays(checked((int)duration.InBaseUnits() / TimeUnit.Day.Factor));
            if (duration.Unit.IsConvertibleToMonths())
                return date.AddMonths(checked((int)duration.InBaseUnits()));
            throw new ArgumentOutOfRangeException(nameof(duration));
        }
    }
}
