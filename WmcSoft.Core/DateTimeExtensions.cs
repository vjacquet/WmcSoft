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
using System.Globalization;

namespace WmcSoft
{
    /// <summary>
    /// Defines the extension methods to the <see cref="DateTime"/> struct.
    /// This is a static class.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts a time to universal time.
        /// </summary>
        /// <param name="dateTime">The time.</param>
        /// <returns>The time as Utc.</returns>
        /// <remarks>When the kind is unspecified, assumes <see cref="DateTimeKind.Local"/> wheras <see cref="DateTime.ToLocalTime"/> assumes <see cref="DateTimeKind.Utc"/>.</remarks>
        public static DateTime AsLocalTime(this DateTime dateTime)
        {
            switch (dateTime.Kind) {
            case DateTimeKind.Local:
                return dateTime;
            case DateTimeKind.Utc:
                return dateTime.ToLocalTime();
            case DateTimeKind.Unspecified:
            default:
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
            }
        }

        /// <summary>
        /// Converts a time to universal time.
        /// </summary>
        /// <param name="dateTime">The time.</param>
        /// <returns>The time as Utc.</returns>
        /// <remarks>When the kind is unspecified, assumes <see cref="DateTimeKind.Utc"/> wheras <see cref="DateTime.ToUniversalTime"/> assumes <see cref="DateTimeKind.Local"/>.</remarks>
        public static DateTime AsUniversalTime(this DateTime dateTime)
        {
            switch (dateTime.Kind) {
            case DateTimeKind.Local:
                return dateTime.ToUniversalTime();
            case DateTimeKind.Utc:
                return dateTime;
            case DateTimeKind.Unspecified:
            default:
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            }
        }

        /// <summary>
        /// Gets the week of the month of the date represented by this instance.
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The week of the month</returns>
        public static int WeekOfMonth(this DateTime date)
        {
            int day = date.Day;
            int dayOfWeek = (int)FirstDayOfMonth(date).DayOfWeek;
            return Math.DivRem(6 + day + dayOfWeek, 7, out int reminder);
        }

        /// <summary>
        /// Gets the first day of the month of the date represented by this instance.
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The first day of the month</returns>
        public static DateTime FirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1, 0, 0, 0, date.Kind);
        }

        /// <summary>
        /// Gets the first day of the next month of the date represented by this instance.
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The first day of the month</returns>
        public static DateTime FirstDayOfNextMonth(this DateTime date)
        {
            return FirstDayOfMonth(date).AddMonths(1);
        }

        /// <summary>
        /// Gets the last day of the month of the date represented by this instance.
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The last day of the month</returns>
        public static DateTime LastDayOfMonth(this DateTime date)
        {
            var year = date.Year;
            var month = date.Month;
            return new DateTime(year, month, DateTime.DaysInMonth(year, month), 0, 0, 0, date.Kind);
        }

        /// <summary>
        /// Gets the first and last day of the month of the date represented by this instance.
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The first and last day of the month</returns>
        public static (DateTime firstDayOfMonth, DateTime lastDayOfMonth) GetMonth(this DateTime date)
        {
            var year = date.Year;
            var month = date.Month;
            var firstDayOfMonth = new DateTime(year, month, 1, 0, 0, 0, date.Kind);
            var lastDayOfMonth = firstDayOfMonth.AddDays(DateTime.DaysInMonth(year, month) - 1);
            return (firstDayOfMonth, lastDayOfMonth);
        }

        static DateTime UnguardedFirstDayOfWeek(DateTime date, IFormatProvider formatProvider)
        {
            var dateTimeFormatInfo = formatProvider.GetFormat<DateTimeFormatInfo>();
            var delta = dateTimeFormatInfo.FirstDayOfWeek - date.DayOfWeek;
            if (delta > 0)
                delta -= 7;
            return date.Date.AddDays(delta);
        }

        /// <summary>
        /// Gets the first day of the week of the date represented by this instance.
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="formatProvider">The format provider to retrieve <see cref="DateTimeFormatInfo.FirstDayOfWeek"/>.</param>
        /// <returns>The first day of the week</returns>
        public static DateTime FirstDayOfWeek(this DateTime date, IFormatProvider formatProvider = null)
        {
            return UnguardedFirstDayOfWeek(date, formatProvider ?? CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets the last day of the week of the date represented by this instance.
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="formatProvider">The format provider to retrieve <see cref="DateTimeFormatInfo.FirstDayOfWeek"/>.</param>
        /// <returns>The last day of the week</returns>
        public static DateTime LastDayOfWeek(this DateTime date, IFormatProvider formatProvider = null)
        {
            return UnguardedFirstDayOfWeek(date, formatProvider ?? CultureInfo.CurrentCulture)
                .AddDays(6);
        }

        /// <summary>
        /// Gets the nth day of the month of this instance.
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="n">the nth day</param>
        /// <param name="dayOfWeek">the day of the week</param>
        /// <returns>Returns the nth day of the week.</returns>
        /// <remarks>If <paramref name="n"/> is less than zero, then the count starts from then end of the month.</remarks>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="n"/> is either zero or too big to have a result in the same month.</exception>
        public static DateTime NthDayOfMonth(this DateTime date, int n, DayOfWeek dayOfWeek)
        {
            if (n > 0) {
                var (f, l) = date.GetMonth();
                var days = dayOfWeek - f.DayOfWeek;
                if (days < 0)
                    days += 7;
                days += 7 * (n - 1);
                var result = f.AddDays(days);
                if (result <= l)
                    return result;
            } else if (n < 0) {
                var (f, l) = date.GetMonth();
                var days = dayOfWeek - l.DayOfWeek;
                if (days > 0)
                    days -= 7;
                days += 7 * (n + 1);
                var result = l.AddDays(days);
                if (result >= f)
                    return result;
            }
            throw new ArgumentOutOfRangeException(nameof(n));
        }

        /// <summary>
        /// Gets the week of the year using ISO-8601 convention.
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns></returns>
        public static int Iso8601WeekOfYear(this DateTime date)
        {
            // see <https://blogs.msdn.microsoft.com/shawnste/2006/01/24/iso-8601-week-of-year-format-in-microsoft-net/>
            var cal = CultureInfo.InvariantCulture.Calendar;

            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            var day = cal.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
                date = date.AddDays(3);

            // Return the week of our adjusted day
            return cal.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        /// <summary>
        /// <para>Truncates a <see cref="DateTime"/> to a specified resolution.</para>
        /// <para>A convenient source for resolution is TimeSpan.TicksPerXXXX constants.</para>
        /// </summary>
        /// <param name="date">The DateTime object to truncate.</param>
        /// <param name="resolution">e.g. to round to nearest second, <see cref="TimeSpan.TicksPerSecond"/>.</param>
        /// <returns>The truncated date.</returns>
        /// <remarks>Found at <http://stackoverflow.com/questions/1004698/how-to-truncate-milliseconds-off-of-a-net-datetime>.</remarks>
        public static DateTime Truncate(this DateTime date, long resolution)
        {
            var ticks = date.Ticks;
            return new DateTime(ticks - (ticks % resolution), date.Kind);
        }
    }
}
