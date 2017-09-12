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

 ****************************************************************************
 * Adapted from DateSpecification.java
 * -----------------------------------
 * Copyright (c) 2005 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using WmcSoft.Time;

namespace WmcSoft.Business.Calendars.Specifications
{
    public sealed class WeekendSpecification : IDateSpecification
    {
        const int DaysOfWeek = 7;
        private readonly DayOfWeek[] _weekend;
        private readonly int[] _nextWeekendDay;

        public WeekendSpecification(params DayOfWeek[] weekend)
        {
            _weekend = weekend;

            // optimize EnumerateOver by precomputing the number of days until the next week end day.
            var twoWeeks = 0;
            foreach (DayOfWeek day in weekend)
                twoWeeks |= (0b0000001_0000001 << (int)day);

            _nextWeekendDay = new int[DaysOfWeek];
            _nextWeekendDay[0] = DaysUntilNextWeekendDay(ref twoWeeks);
            for (int i = 1; i < DaysOfWeek; i++) {
                _nextWeekendDay[i] = _nextWeekendDay[i - 1] - 1;
                if (_nextWeekendDay[i] == 0) {
                    _nextWeekendDay[i] = DaysUntilNextWeekendDay(ref twoWeeks);
                }
            }
        }

        static int DaysUntilNextWeekendDay(ref int twoWeeks)
        {
            var count = 0;
            do {
                twoWeeks >>= 1;
                count++;
            }
            while ((twoWeeks & 1) != 1);
            return count;
        }

        bool IsWeekend(DayOfWeek dow)
        {
            return Array.IndexOf(_weekend, dow) >= 0;
        }

        public bool IsSatisfiedBy(Date date)
        {
            return IsWeekend(date.DayOfWeek);
        }

        private Date UpperBoundExclusive(Interval<Date> interval)
        {
            if (!interval.HasUpperLimit) {
                return Date.MaxValue;
            } else if (interval.IsUpperIncluded) {
                return interval.GetUpperOrDefault();
            } else {
                return interval.GetUpperOrDefault().AddDays(-1);
            }
        }

        private IEnumerable<Date> UnguardedEnumerateOver(Date start, Date end)
        {
            if (IsSatisfiedBy(start))
                yield return start;
            start = start.AddDays(_nextWeekendDay[(int)start.DayOfWeek]);
            while (start <= end) {
                yield return start;
                start = start.AddDays(_nextWeekendDay[(int)start.DayOfWeek]);
            }
        }

        public IEnumerable<Date> EnumerateOver(Interval<Date> interval)
        {
            if (!interval.HasLowerLimit) throw new ArgumentOutOfRangeException(nameof(interval));

            var start = interval.Lower.GetValueOrDefault();
            var end = UpperBoundExclusive(interval);
            return UnguardedEnumerateOver(start, end);
        }
    }
}
