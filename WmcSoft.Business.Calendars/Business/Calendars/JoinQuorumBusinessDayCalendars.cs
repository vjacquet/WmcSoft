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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WmcSoft.Time;

using static WmcSoft.Business.Calendars.Helpers;

namespace WmcSoft.Business.Calendars
{
    /// <summary>
    /// Join calendars so that a date is a business day if it is a business day in a quorum number of calendars.
    /// </summary>
    [DebuggerDisplay("[{MinDate.ToString(\"yyyy-MM-dd\"),nq} .. {MaxDate.ToString(\"yyyy-MM-dd\"),nq}]")]
    [DebuggerTypeProxy(typeof(BusinessCalendarDebugView))]
    public class JoinQuorumBusinessDayCalendars : IBusinessCalendar, IEnumerable<IBusinessCalendar>
    {
        private readonly List<IBusinessCalendar> _calendars;
        private readonly int _quorum;

        public JoinQuorumBusinessDayCalendars(int quorum, params IBusinessCalendar[] calendars)
            : this($"Business day in at least {quorum} of {HumanizeList(calendars)} calendars", quorum, calendars)
        {
        }

        public JoinQuorumBusinessDayCalendars(string name, int quorum, params IBusinessCalendar[] calendars)
        {
            Name = name;
            _calendars = new List<IBusinessCalendar>(calendars);
            _quorum = 2;
        }

        public string Name { get; }

        public Date MinDate => _calendars.Min(c => c.MinDate);
        public Date MaxDate => _calendars.Max(c => c.MaxDate);

        public bool IsBusinessDay(Date date)
        {
            return _calendars.Count(c => c.IsBusinessDay(date)) >= _quorum;
        }

        public void Add(IBusinessCalendar calendar)
        {
            _calendars.Add(calendar);
        }

        public IEnumerator<IBusinessCalendar> GetEnumerator()
        {
            return _calendars.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
