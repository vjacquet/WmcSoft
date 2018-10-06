#region Licence

/****************************************************************************
          Copyright 1999-2017 Vincent J. Jacquet.  All rights reserved.

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
using System.Collections.Generic;
using System.Diagnostics;
using WmcSoft.Time;

namespace WmcSoft.Business.Calendars
{
    public static class BusinessCalendarExtensions
    {
        #region Adapters

        [DebuggerDisplay("{Name} [{MinDate.ToString(\"yyyy-MM-dd\"),nq} .. {MaxDate.ToString(\"yyyy-MM-dd\"),nq}]")]
        [DebuggerTypeProxy(typeof(BusinessCalendarDebugView))]
        class AliasBusinessCalendar : IBusinessCalendar
        {
            private readonly IBusinessCalendar calendar;

            public AliasBusinessCalendar(IBusinessCalendar calendar, string name)
            {
                if (calendar == null) throw new NullReferenceException(); // because the class can only be instanciated by the extension method.
                if (name == null) throw new ArgumentNullException(nameof(name));

                this.calendar = calendar;
                Name = name;
            }

            public string Name { get; }

            public Date MinDate => calendar.MinDate;
            public Date MaxDate => calendar.MaxDate;

            public bool IsBusinessDay(Date date)
            {
                return calendar.IsBusinessDay(date);
            }
        }

        [DebuggerDisplay("[{MinDate.ToString(\"yyyy-MM-dd\"),nq} .. {MaxDate.ToString(\"yyyy-MM-dd\"),nq}]")]
        [DebuggerTypeProxy(typeof(BusinessCalendarDebugView))]
        class TruncatedBusinessCalendar : IBusinessCalendar
        {
            private readonly IBusinessCalendar calendar;

            public TruncatedBusinessCalendar(IBusinessCalendar calendar, Date since, Date until)
            {
                if (since < calendar.MinDate) throw new ArgumentOutOfRangeException(nameof(since));
                if (until > calendar.MaxDate) throw new ArgumentOutOfRangeException(nameof(until));

                this.calendar = calendar;
                MinDate = since;
                MaxDate = until;
            }

            public string Name => calendar.Name;

            public Date MinDate { get; }
            public Date MaxDate { get; }

            public bool IsBusinessDay(Date date)
            {
                if (date < MinDate | date > MaxDate)
                    throw new IndexOutOfRangeException();

                return calendar.IsBusinessDay(date);
            }
        }

        #endregion

        /// <summary>
        /// Creates a view on a business calendar by giving it a new name.
        /// </summary>
        /// <typeparam name="TCalendar">The type of calendar</typeparam>
        /// <param name="calendar">The business calendar to create a view on.</param>
        /// <param name="name">The new name of the business calendar.</param>
        /// <returns>A view on the business calendar.</returns>
        /// <remarks>Changes on the original calendar will be reflected in the created calendar.</remarks>
        public static IBusinessCalendar RenameAs<TCalendar>(this TCalendar calendar, string name)
            where TCalendar : IBusinessCalendar
        {
            return new AliasBusinessCalendar(calendar, name);
        }

        /// <summary>
        /// Creates a view on a business calendar starting latter and ending sooner.
        /// </summary>
        /// <typeparam name="TCalendar">The type of calendar</typeparam>
        /// <param name="calendar">The business calendar to create a view on.</param>
        /// <param name="since">The new starting date of the business calendar.</param>
        /// <param name="until">The new ending date of the business calendar.</param>
        /// <returns>A view on the business calendar.</returns>
        /// <remarks>Changes on the original calendar will be reflected in the created calendar.</remarks>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="since"/> is sooner than <paramref name="calendar"/>'s min date
        /// or <paramref name="until"/> is latter than <paramref name="calendar"/>'s max date.</exception>
        public static IBusinessCalendar Between<TCalendar>(this TCalendar calendar, Date since, Date until)
            where TCalendar : IBusinessCalendar
        {
            return new TruncatedBusinessCalendar(calendar, since, until);
        }

        /// <summary>
        /// Creates a view on a business calendar starting latter.
        /// </summary>
        /// <typeparam name="TCalendar">The type of calendar</typeparam>
        /// <param name="calendar">The business calendar to create a view on.</param>
        /// <param name="date">The new starting date of the business calendar.</param>
        /// <returns>A view on the business calendar.</returns>
        /// <remarks>Changes on the original calendar will be reflected in the created calendar.</remarks>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="date"/> is sooner than <paramref name="calendar"/>'s min date.</exception>
        public static IBusinessCalendar Since<TCalendar>(this TCalendar calendar, Date date)
            where TCalendar : IBusinessCalendar
        {
            return new TruncatedBusinessCalendar(calendar, date, calendar.MaxDate);
        }

        /// <summary>
        /// Creates a view on a business calendar ending sooner.
        /// </summary>
        /// <typeparam name="TCalendar">The type of calendar</typeparam>
        /// <param name="calendar">The business calendar to create a view on.</param>
        /// <param name="date">The new ending date of the business calendar.</param>
        /// <returns>A view on the business calendar.</returns>
        /// <remarks>Changes on the original calendar will be reflected in the created calendar.</remarks>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="date"/> is latter than <paramref name="calendar"/>'s max date.</exception>
        public static IBusinessCalendar Until<TCalendar>(this TCalendar calendar, Date date)
            where TCalendar : IBusinessCalendar
        {
            return new TruncatedBusinessCalendar(calendar, calendar.MinDate, date);
        }

        /// <summary>
        /// Returns wether the day is a holiday or not.
        /// </summary>
        /// <param name="date">The day</param>
        /// <returns><c>true</c> if the <paramref name="date"/> is a holiday; otherwise, <c>false</c>.</returns>
        public static bool IsHoliday<TCalendar>(this TCalendar calendar, Date date)
            where TCalendar : IBusinessCalendar
        {
            return !calendar.IsBusinessDay(date);
        }

        public static Date AddBusinessDays<TCalendar>(this TCalendar calendar, Date date, int days)
            where TCalendar : IBusinessCalendar
        {
            while (days > 0) {
                date = date.AddDays(1);
                if (calendar.IsBusinessDay(date))
                    days--;
            }
            while (days < 0) {
                date = date.AddDays(-1);
                if (calendar.IsBusinessDay(date))
                    days++;
            }
            return date;
        }

        public static Date NextBusinessDay<TCalendar>(this TCalendar calendar, Date date)
            where TCalendar : IBusinessCalendar
        {
            do {
                date = date.AddDays(1);
            } while (!calendar.IsBusinessDay(date));
            return date;
        }

        public static IEnumerable<Date> NextBusinessDays<TCalendar>(this TCalendar calendar, Date date, int count)
            where TCalendar : IBusinessCalendar
        {
            for (int i = 0; i < count; i++) {
                date = calendar.NextBusinessDay(date);
                yield return date;
            }
        }

        public static Date AddWeeks<TCalendar, TConvention>(this TCalendar calendar, Date date, int weeks, TConvention convention)
            where TCalendar : IBusinessCalendar
            where TConvention : IBusinessDayConvention
        {
            var target = date.AddMonths(7 * weeks);
            return convention.Adjust(calendar, target);
        }

        public static Date NextWeek<TCalendar, TConvention>(this TCalendar calendar, Date date, TConvention convention)
            where TCalendar : IBusinessCalendar
            where TConvention : IBusinessDayConvention
        {
            return AddWeeks(calendar, date, 1, convention);
        }

        public static Date AddMonths<TCalendar, TConvention>(this TCalendar calendar, Date date, int months, TConvention convention)
            where TCalendar : IBusinessCalendar
            where TConvention : IBusinessDayConvention
        {
            var target = date.AddMonths(months);
            return convention.Adjust(calendar, target);
        }

        public static Date NextMonth<TCalendar, TConvention>(this TCalendar calendar, Date date, TConvention convention)
            where TCalendar : IBusinessCalendar
            where TConvention : IBusinessDayConvention
        {
            return AddMonths(calendar, date, 1, convention);
        }

        public static Date Add<TCalendar, TConvention>(this TCalendar calendar, Date date, Duration duration, TConvention convention)
            where TCalendar : IBusinessCalendar
            where TConvention : IBusinessDayConvention
        {
            var target = date.Add(duration);
            return convention.Adjust(calendar, target);
        }
    }
}
