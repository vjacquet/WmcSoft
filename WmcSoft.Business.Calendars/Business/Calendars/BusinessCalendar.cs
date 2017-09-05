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
using System.Linq;
using WmcSoft.Time;

namespace WmcSoft.Business.Calendars
{
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

        public BusinessCalendar(Date since, TimeSpan duration, params Predicate<DateTime>[] holidays)
        {
            _epoch = since;

            var length = (int)duration.TotalDays;
            _holidays = new BitArray(length, false);

            for (int i = 0; i < length; i++) {
                if (holidays.Any(p => p(since)))
                    _holidays.Set(i, true);
                since = since.AddDays(1);
            }
        }

        public Date MinDate => _epoch;
        public Date MaxDate => _epoch.AddDays(_holidays.Length);

        public bool IsBusinessDay(Date date)
        {
            return !_holidays[date.DaysSince(_epoch)];
        }



        public static bool Saturdays(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday;
        }

        public static bool Sundays(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Sunday;
        }

        public static bool WeekEnds(DateTime date)
        {
            var dow = date.DayOfWeek;
            return dow == DayOfWeek.Saturday || dow == DayOfWeek.Sunday;
        }

        public static bool Christmas(DateTime date)
        {
            return date.Month == 12 && date.Day == 25;
        }

        public static bool NewYear(DateTime date)
        {
            return date.Month == 1 && date.Day == 1;
        }
    }
}
