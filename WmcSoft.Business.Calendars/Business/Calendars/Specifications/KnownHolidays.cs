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
using WmcSoft.Time;

namespace WmcSoft.Business.Calendars.Specifications
{
    public static class KnownHolidays
    {
        public static readonly AnnualDateSpecification NewYearDay = DateSpecification.Fixed(1, 1);

        public static readonly AnnualDateSpecification LabourDay = DateSpecification.Fixed(5, 1);

        public static readonly AnnualDateSpecification ChristmasEve = DateSpecification.Fixed(12, 24);

        public static readonly AnnualDateSpecification Christmas = DateSpecification.Fixed(12, 25);

        public static readonly AnnualDateSpecification BoxingDay = DateSpecification.Fixed(12, 26);

        public static readonly AnnualDateSpecification NewYearEve = DateSpecification.Fixed(12, 31);

        public static readonly AnnualDateSpecification MemorialDay = DateSpecification.NthOccurenceOfWeekdayInMonth(5, DayOfWeek.Monday, -1);

        public static readonly GregorianEasterSpecification GregorianEaster = new GregorianEasterSpecification();

        public static readonly ShiftedAnnualDateSpecification GregorianEasterFriday = new ShiftedAnnualDateSpecification(GregorianEaster, -2);

        public static readonly ShiftedAnnualDateSpecification GregorianEasterMonday = new ShiftedAnnualDateSpecification(GregorianEaster, +1);

        public static AdjustedAnnualDateSpecification Adjust(this AnnualDateSpecification specification, DayOfWeek dayOfWeek, int shift)
        {
            return new AdjustedAnnualDateSpecification(specification, dayOfWeek, shift);
        }
    }
}
