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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WmcSoft.Time;

namespace WmcSoft.Business.Calendars
{
    [DebuggerDisplay("[{MinDate.ToString(\"yyyy-MM-dd\"),nq} .. {MaxDate.ToString(\"yyyy-MM-dd\"),nq}]")]
    [DebuggerTypeProxy(typeof(BusinessCalendarDebugView))]
    public sealed class BusinessCalendar : IBusinessCalendar
    {
        readonly Date _epoch;
        readonly BitArray _holidays;

        private BusinessCalendar(Date epoch, BitArray holidays)
        {
            _epoch = epoch;
            _holidays = holidays;

            // The 7 week patterns.
            // 1100 0001 1000 0011 0000 0110 0000 1100
            // 0001 1000 0011 0000 0110 0000 1100 0001
            // 1000 0011 0000 0110 0000 1100 0001 1000
            // 0011 0000 0110 0000 1100 0001 1000 0011
            // 0000 0110 0000 1100 0001 1000 0011 0000
            // 0110 0000 1100 0001 1000 0011 0000 0110 
            // 0000 1100 0001 1000 0011 0000 0110 0000 
        }

        private BusinessCalendar(Date since, int count)
        {
            _epoch = since;

            _holidays = new BitArray(count, false);
        }

        [Obsolete("Use the specifications instead.", false)]
        public BusinessCalendar(Date since, Date until, params Predicate<Date>[] holidays)
            : this(since, 1 + since.DaysUntil(until))
        {
            var length = _holidays.Length;
            for (int i = 0; i < length; i++) {
                if (holidays.Any(p => p(since)))
                    _holidays.Set(i, true);
                since = since.AddDays(1);
            }
        }

        public BusinessCalendar(Date since, Date until, params IDateSpecification[] specifications)
            : this(since, 1 + since.DaysUntil(until))
        {
            var interval = Interval.Closed(since, until);
            foreach (var specification in specifications) {
                foreach (var day in specification.EnumerateOver(interval)) {
                    _holidays.Set(day.DaysSince(since), true);
                }
            }
        }

        public Date MinDate => _epoch;
        public Date MaxDate => _epoch.AddDays(_holidays.Length - 1);

        public bool IsBusinessDay(Date date)
        {
            return !_holidays[date.DaysSince(_epoch)];
        }



        [Obsolete("Use the specifications instead.", false)]
        public static bool Saturdays(Date date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday;
        }

        [Obsolete("Use the specifications instead.", false)]
        public static bool Sundays(Date date)
        {
            return date.DayOfWeek == DayOfWeek.Sunday;
        }

        [Obsolete("Use the specifications instead.", false)]
        public static bool WeekEnds(Date date)
        {
            var dow = date.DayOfWeek;
            return dow == DayOfWeek.Saturday || dow == DayOfWeek.Sunday;
        }

        [Obsolete("Use the specifications instead.", false)]
        public static bool Christmas(Date date)
        {
            return date.Month == 12 && date.Day == 25;
        }

        [Obsolete("Use the specifications instead.", false)]
        public static bool NewYear(Date date)
        {
            return date.Month == 1 && date.Day == 1;
        }
    }

    class BusinessCalendarDebugView
    {
        [DebuggerDisplay("{Date.ToString(\"ddd yyyy-MM-dd\"),nq}", Name = "{Index,nq}")]
        public class Holiday
        {
            public int Index { get; set; }
            public Date Date { get; set; }
        }

        [DebuggerDisplay("#{Holidays.Length,nq}", Name = "{Name,nq}")]
        public class Year
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int Name { get; set; }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public Holiday[] Holidays { get; set; }
        }

        public BusinessCalendarDebugView(IBusinessCalendar calendar)
        {
            var result = new List<Holiday>();
            var since = calendar.MinDate;
            var until = calendar.MaxDate;
            var index = 0;

            while (since <= until) {
                if (!calendar.IsBusinessDay(since))
                    result.Add(new Holiday { Index = index, Date = since });
                since = since.AddDays(1);
                index++;
            }
            var query = from h in result
                        group h by h.Date.Year into g
                        select new Year { Name = g.Key, Holidays = g.ToArray() };
            Years = query.ToArray();
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public Year[] Years { get; }
    }
}
