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

using static WmcSoft.Time.Algorithms;

namespace WmcSoft.Business.Calendars
{
    /// <summary>
    /// Decorates an existing calendar Business calendar with no predefined set of business days.
    /// </summary>
    [DebuggerDisplay("[{MinDate.ToString(\"yyyy-MM-dd\"),nq} .. {MaxDate.ToString(\"yyyy-MM-dd\"),nq}]")]
    [DebuggerTypeProxy(typeof(BusinessCalendarDebugView))]
    public class BespokeCalendar<TCalendar> : IBusinessCalendar
        where TCalendar : IBusinessCalendar
    {
        private readonly TCalendar _calendar;
        private readonly DayOfWeek[] _weekends;
        private readonly HashSet<Date> _addedHolidays = new HashSet<Date>();
        private readonly HashSet<Date> _removedHolidays = new HashSet<Date>();

        public BespokeCalendar(TCalendar calendar, Date since, Date until, params DayOfWeek[] weekends)
            : this($"Bespoke of {calendar.Name}", calendar, since, until, weekends)
        {
        }

        public BespokeCalendar(TCalendar calendar, params DayOfWeek[] weekends)
            : this($"Bespoke of {calendar.Name}", calendar, weekends)
        {
        }

        public BespokeCalendar(string name, TCalendar calendar, Date since, Date until, params DayOfWeek[] weekends)
        {
            var min = Max(calendar.MinDate, since);
            var max = Min(calendar.MaxDate, until);
            if (min > max) throw new ArgumentOutOfRangeException(nameof(calendar));

            Name = name;
            MinDate = Max(calendar.MinDate, since);
            MaxDate = Min(calendar.MaxDate, until);
            _calendar = calendar;
            _weekends = weekends ?? new DayOfWeek[0];
        }

        public BespokeCalendar(string name, TCalendar calendar, params DayOfWeek[] weekends)
        {
            Name = name;
            MinDate = calendar.MinDate;
            MaxDate = calendar.MaxDate;
            _calendar = calendar;
            _weekends = weekends ?? new DayOfWeek[0];
        }

        public string Name { get; }

        public Date MinDate { get; }
        public Date MaxDate { get; }

        public void Add(Date holiday)
        {
            _removedHolidays.Remove(holiday);
            if (_calendar.IsBusinessDay(holiday))
                _addedHolidays.Add(holiday);
        }

        public void Remove(Date holiday)
        {
            _addedHolidays.Remove(holiday);
            if (!_calendar.IsBusinessDay(holiday))
                _removedHolidays.Add(holiday);
        }

        public bool IsBusinessDay(Date date)
        {
            return !IsHoliday(date) && !IsWeekend(date.DayOfWeek);
        }

        bool IsHoliday(Date date)
        {
            if (_addedHolidays.Contains(date))
                return true;
            if (_removedHolidays.Contains(date))
                return false;
            return !_calendar.IsBusinessDay(date);
        }

        bool IsWeekend(DayOfWeek day)
        {
            return Array.IndexOf(_weekends, day) >= 0;
        }
    }

    public static class BespokeCalendar
    {
        public static BespokeCalendar<TCalendar> From<TCalendar>(string name, TCalendar calendar, Date since, Date until, params DayOfWeek[] weekends)
            where TCalendar : IBusinessCalendar
        {
            return new BespokeCalendar<TCalendar>(name, calendar, since, until, weekends);
        }

        public static void Add<TCalendar>(this BespokeCalendar<TCalendar> calendar, params IDateSpecification[] specifications)
            where TCalendar : IBusinessCalendar
        {
            var interval = Interval.Closed(calendar.MinDate, calendar.MaxDate);
            foreach (var specification in specifications) {
                foreach (var day in specification.EnumerateOver(interval)) {
                    calendar.Add(day);
                }
            }
        }

        public static void Remove<TCalendar>(this BespokeCalendar<TCalendar> calendar, params IDateSpecification[] specifications)
            where TCalendar : IBusinessCalendar
        {
            var interval = Interval.Closed(calendar.MinDate, calendar.MaxDate);
            foreach (var specification in specifications) {
                foreach (var day in specification.EnumerateOver(interval)) {
                    calendar.Remove(day);
                }
            }
        }
    }
}
