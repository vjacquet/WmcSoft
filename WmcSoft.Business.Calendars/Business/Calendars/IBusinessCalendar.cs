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
using System.Diagnostics;
using WmcSoft.Time;

namespace WmcSoft.Business.Calendars
{
    public interface IBusinessCalendar
    {
        string Name { get; }

        Date MinDate { get; }
        Date MaxDate { get; }

        bool IsBusinessDay(Date date);
    }

    public static class BusinessCalendarExtensions
    {
        [DebuggerDisplay("[{MinDate.ToString(\"yyyy-MM-dd\"),nq} .. {MaxDate.ToString(\"yyyy-MM-dd\"),nq}]")]
        [DebuggerTypeProxy(typeof(BusinessCalendarDebugView))]
        class TruncatedBusinessCalendar : IBusinessCalendar
        {
            private readonly IBusinessCalendar _calendar;
            private readonly Date _since;
            private readonly Date _until;

            public TruncatedBusinessCalendar(IBusinessCalendar calendar, Date since, Date until)
            {
                _calendar = calendar;
                _since = since;
                _until = until;
            }

            public string Name => _calendar.Name;

            public Date MinDate => _since;
            public Date MaxDate => _until;

            public bool IsBusinessDay(Date date)
            {
                return _calendar.IsBusinessDay(date);
            }
        }

        public static IBusinessCalendar Truncate<TCalendar>(this TCalendar calendar, Date since, Date until)
            where TCalendar : IBusinessCalendar
        {
            return new TruncatedBusinessCalendar(calendar, since, until);
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
    }
}
