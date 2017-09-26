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
using WmcSoft.Time;

namespace WmcSoft.Business.Calendars
{
    public static class BusinessDayConventions
    {
        #region Implementations

        public struct UnadjustedConvention : IBusinessDayConvention
        {
            public Date Adjust<TCalendar>(TCalendar calendar, Date date)
                where TCalendar : IBusinessCalendar
            {
                return date;
            }
        }

        public struct FollowingConvention : IBusinessDayConvention
        {
            public Date Adjust<TCalendar>(TCalendar calendar, Date date)
                where TCalendar : IBusinessCalendar
            {
                while (!calendar.IsBusinessDay(date))
                    date = date.AddDays(1);
                return date;
            }
        }

        public struct ModifiedFollowingConvention : IBusinessDayConvention
        {
            public Date Adjust<TCalendar>(TCalendar calendar, Date date)
                where TCalendar : IBusinessCalendar
            {
                var adjusted = Following.Adjust(calendar, date);
                if (date.Month == adjusted.Month)
                    return adjusted;
                return Preceding.Adjust(calendar, date);
            }
        }

        public struct PrecedingConvention : IBusinessDayConvention
        {
            public Date Adjust<TCalendar>(TCalendar calendar, Date date)
                where TCalendar : IBusinessCalendar
            {
                while (!calendar.IsBusinessDay(date))
                    date = date.AddDays(-1);
                return date;
            }
        }

        public struct ModifiedPrecedingConvention : IBusinessDayConvention
        {
            public Date Adjust<TCalendar>(TCalendar calendar, Date date)
                where TCalendar : IBusinessCalendar
            {
                var adjusted = Preceding.Adjust(calendar, date);
                if (date.Month == adjusted.Month)
                    return adjusted;
                return Following.Adjust(calendar, date);
            }
        }

        public struct HalfMonthModifiedFollowingConvention : IBusinessDayConvention
        {
            public Date Adjust<TCalendar>(TCalendar calendar, Date date)
                where TCalendar : IBusinessCalendar
            {
                var adjusted = ModifiedFollowing.Adjust(calendar, date);
                if (date.Day <= 15 && adjusted.Day > 15)
                    return Preceding.Adjust(calendar, date);
                return adjusted;
            }
        }

        public struct NearestConvention : IBusinessDayConvention
        {
            public Date Adjust<TCalendar>(TCalendar calendar, Date date)
                where TCalendar : IBusinessCalendar
            {
                if (calendar.IsBusinessDay(date))
                    return date;
                var before = date.AddDays(-1);
                var after = date.AddDays(+1);
                while (true) {
                    if (calendar.IsBusinessDay(after))
                        return after;
                    if (calendar.IsBusinessDay(before))
                        return before;
                    before = before.AddDays(-1);
                    after = after.AddDays(+1);
                }
            }
        }

        #endregion

        /// <summary>
        /// Do not adjust.
        /// </summary>
        public static UnadjustedConvention Unadjusted { get; }

        /// <summary>
        /// Choose the first business day after the given holiday.
        /// </summary>
        public static FollowingConvention Following { get; }

        /// <summary>
        /// Choose the first business day after the given holiday
        /// unless it belongs to a different month, in which case
        /// choose the first business day before the holiday.
        /// </summary>
        public static ModifiedFollowingConvention ModifiedFollowing { get; }

        /// <summary>
        /// Choose the first business day before the given holiday.
        /// </summary>
        public static PrecedingConvention Preceding { get; }

        /// <summary>
        /// Choose the first business day before the given holiday unless it belongs
        /// to a different month, in which case choose the first business day after
        /// the holiday.
        /// </summary>
        public static ModifiedPrecedingConvention ModifiedPreceding { get; }

        /// <summary>
        /// Choose the first business day after the given holiday unless that day
        /// crosses the mid-month(15th) or the end of month, in which case choose
        /// the first business day before the holiday.
        /// </summary>
        public static HalfMonthModifiedFollowingConvention HalfMonthModifiedFollowing { get; }

        /// <summary>
        /// Choose the nearest business day  to the given holiday.If both the
        /// preceding and following business days are equally far away, default
        /// to following business day.
        /// </summary>
        public static NearestConvention Nearest { get; }
    }
}
