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

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WmcSoft.Time;

namespace WmcSoft.Business.Calendars
{
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
