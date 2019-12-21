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

using System.Collections.Generic;
using System.Linq;
using WmcSoft.Time;

namespace WmcSoft.Business.Calendars.Specifications
{
    /// <summary>
    /// Represents a predicate to determine if a date is a business day on a given calendar.
    /// </summary>
    /// <typeparam name="TCalendar">The type of business calendar.</typeparam>
    public class BusinessDaySpecification<TCalendar> : IDateSpecification
        where TCalendar : IBusinessCalendar
    {
        private readonly TCalendar _calendar;

        public BusinessDaySpecification(TCalendar calendar)
        {
            _calendar = calendar;
        }

        public IEnumerable<Date> EnumerateBetween(Date since, Date until)
        {
            if (since > until)
                yield break;

            var date = since;
            while (!IsSatisfiedBy(date))
                date = _calendar.NextBusinessDay(date);
            while (date <= until) {
                yield return date;
                date = _calendar.NextBusinessDay(date);
            }
        }

        public bool IsSatisfiedBy(Date candidate)
        {
            return _calendar.IsBusinessDay(candidate);
        }
    }
}
