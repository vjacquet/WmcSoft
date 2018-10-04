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
        private readonly Date epoch;
        private readonly BitArray holidays;

        private BusinessCalendar(string name, Date epoch, BitArray holidays)
        {
            Name = name;

            this.epoch = epoch;
            this.holidays = holidays;

            // The 7 week patterns.
            // 1100 0001 1000 0011 0000 0110 0000 1100
            // 0001 1000 0011 0000 0110 0000 1100 0001
            // 1000 0011 0000 0110 0000 1100 0001 1000
            // 0011 0000 0110 0000 1100 0001 1000 0011
            // 0000 0110 0000 1100 0001 1000 0011 0000
            // 0110 0000 1100 0001 1000 0011 0000 0110 
            // 0000 1100 0001 1000 0011 0000 0110 0000 
        }

        private BusinessCalendar(string name, Date since, int count)
        {
            Name = name;

            epoch = since;
            holidays = new BitArray(count, false);
        }

        public BusinessCalendar(string name, Date since, Date until, params IDateSpecification[] specifications)
            : this(name, since, until, specifications.AsEnumerable())
        {
        }

        public BusinessCalendar(string name, Date since, Date until, IEnumerable<IDateSpecification> specifications)
            : this(name, since, 1 + since.DaysUntil(until))
        {
            var interval = Interval.Closed(since, until);
            foreach (var specification in specifications) {
                foreach (var day in specification.EnumerateOver(interval)) {
                    holidays.Set(day.DaysSince(since), true);
                }
            }
        }

        public BusinessCalendar(Date since, Date until, params IDateSpecification[] specifications)
         : this(since, until, specifications.AsEnumerable())
        {
        }

        public BusinessCalendar(Date since, Date until, IEnumerable<IDateSpecification> specifications)
          : this("Busines calendar since {since} and until {until}", since, until, specifications)
        {
        }

        public string Name { get; }

        public Date MinDate => epoch;
        public Date MaxDate => epoch.AddDays(holidays.Length - 1);

        public bool IsBusinessDay(Date date)
        {
            return !holidays[date.DaysSince(epoch)];
        }
    }
}
