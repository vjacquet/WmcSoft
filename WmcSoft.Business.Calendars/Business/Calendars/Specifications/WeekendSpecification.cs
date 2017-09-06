﻿#region Licence

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
using WmcSoft.Time;

namespace WmcSoft.Business.Calendars.Specifications
{
    public sealed class WeekendSpecification : IDateSpecification
    {
        private readonly DayOfWeek[] _weekend;

        public WeekendSpecification(params DayOfWeek[] weekend)
        {
            _weekend = weekend;
        }

        public bool IsSatisfiedBy(Date date)
        {
            var dow = date.DayOfWeek;
            return Array.IndexOf(_weekend, dow) >= 0;
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

        private IEnumerable<Date> UnguardedEnumerateOver(Interval<Date> interval)
        {
            var start = interval.Lower.GetValueOrDefault();
            var end = UpperBoundExclusive(interval);
            while (start <= end) {
                if (IsSatisfiedBy(start))
                    yield return start;
                start = start.AddDays(1);
            }
        }

        public IEnumerable<Date> EnumerateOver(Interval<Date> interval)
        {
            if (!interval.HasLowerLimit) throw new ArgumentOutOfRangeException(nameof(interval));

            return UnguardedEnumerateOver(interval);
        }
    }
}
