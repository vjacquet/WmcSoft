﻿#region Licence

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
using WmcSoft.Time;

namespace WmcSoft.Business.Calendars
{
    public static class BusinessDayConventionExtensions
    {
        public static Date FirstDayOfMonth<TCalendar, TConvention>(this TCalendar calendar, Date date, TConvention convention)
            where TCalendar : IBusinessCalendar
            where TConvention : IBusinessDayConvention
        {
            var year = date.Year;
            var month = date.Month;
            var firstDayOfMonth = new DateTime(year, month, 1);
            return convention.Adjust(calendar, date);
        }

        public static bool IsFirstDayOfMonth<TCalendar, TConvention>(this TCalendar calendar, Date date, TConvention convention)
            where TCalendar : IBusinessCalendar
            where TConvention : IBusinessDayConvention
        {
            var prevDay = convention.Adjust(calendar, date.AddDays(-1));
            return prevDay.Month != date.Month;
        }

        public static Date LastDayOfMonth<TCalendar, TConvention>(this TCalendar calendar, Date date, TConvention convention)
            where TCalendar : IBusinessCalendar
            where TConvention : IBusinessDayConvention
        {
            var year = date.Year;
            var month = date.Month;
            var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            return convention.Adjust(calendar, date);
        }

        public static bool IsLastDayOfMonth<TCalendar, TConvention>(this TCalendar calendar, Date date, TConvention convention)
            where TCalendar : IBusinessCalendar
            where TConvention : IBusinessDayConvention
        {
            var nextDay = convention.Adjust(calendar, date.AddDays(1));
            return nextDay.Month != date.Month;
        }
    }
}
