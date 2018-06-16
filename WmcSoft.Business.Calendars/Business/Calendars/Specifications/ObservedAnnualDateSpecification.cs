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
    /// <summary>
    /// Decorates a holidays that is moved on a working day if it falls on a weekend.
    /// </summary>
    public sealed class ObservedAnnualDateSpecification : AnnualDateSpecification
    {
        private readonly AnnualDateSpecification _specification;
        private readonly DayOfWeek _before;
        private readonly DayOfWeek _after;

        public ObservedAnnualDateSpecification(AnnualDateSpecification specification, DayOfWeek before, DayOfWeek after)
        {
            _specification = specification;
            _before = before;
            _after = after;
        }

        protected override IEnumerable<Date> UnguardedEnumerateOver(Date since, Date until)
        {
            return _specification.EnumerateBetween(since, until).Select(Adjust)
                .SkipWhile(d => d < since)
                .TakeWhile(d => d <= until);
        }

        private Date Adjust(Date date)
        {
            if (date.DayOfWeek == _before)
                return date.AddDays(-1);
            else if (date.DayOfWeek == _after)
                return date.AddDays(+1);
            else
                return date;
        }

        public override Date OfYear(int year)
        {
            var date = _specification.OfYear(year);
            return Adjust(date);
        }
    }
}
