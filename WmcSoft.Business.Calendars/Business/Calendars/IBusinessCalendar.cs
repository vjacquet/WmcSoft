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
        [DebuggerDisplay("{Name} [{MinDate.ToString(\"yyyy-MM-dd\"),nq} .. {MaxDate.ToString(\"yyyy-MM-dd\"),nq}]")]
        [DebuggerTypeProxy(typeof(BusinessCalendarDebugView))]
        class AliasBusinessCalendar : IBusinessCalendar
        {
            private readonly IBusinessCalendar _calendar;
            private readonly string _name;

            public AliasBusinessCalendar(IBusinessCalendar calendar, string name)
            {
                if (calendar == null) throw new NullReferenceException(); // because the class can only be instanciated by the extension method.
                if (name == null) throw new ArgumentNullException(nameof(name));

                _calendar = calendar;
                _name = name;
            }

            public string Name => _name;

            public Date MinDate => _calendar.MinDate;
            public Date MaxDate => _calendar.MaxDate;

            public bool IsBusinessDay(Date date)
            {
                return _calendar.IsBusinessDay(date);
            }
        }

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

        [DebuggerDisplay("[{MinDate.ToString(\"yyyy-MM-dd\"),nq} .. {MaxDate.ToString(\"yyyy-MM-dd\"),nq}]")]
        [DebuggerTypeProxy(typeof(BusinessCalendarDebugView))]
        class TruncatedBusinessCalendar : IBusinessCalendar
        {
            private readonly IBusinessCalendar _calendar;
            private readonly Date _since;
            private readonly Date _until;

            public TruncatedBusinessCalendar(IBusinessCalendar calendar, Date since, Date until)
            {
                if (since < calendar.MinDate) throw new ArgumentOutOfRangeException(nameof(since));
                if (until > calendar.MaxDate) throw new ArgumentOutOfRangeException(nameof(until));

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
