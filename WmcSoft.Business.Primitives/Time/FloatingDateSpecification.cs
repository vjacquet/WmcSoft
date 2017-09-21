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
 * Adapted from FloatingDateSpecification.java
 * -------------------------------------------
 * Copyright (c) 2005 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System;

namespace WmcSoft.Time
{
    sealed class FloatingDateSpecification : AnnualDateSpecification
    {
        private readonly int _month;
        private readonly DayOfWeek _dayOfWeek;
        private readonly int _occurrence;
        private bool _fromStartOfMonth;

        public FloatingDateSpecification(int month, DayOfWeek dayOfWeek, int occurrence)
        {
            _month = month;
            _dayOfWeek = dayOfWeek;
            if (occurrence > 0) {
                _fromStartOfMonth = true;
                _occurrence = occurrence - 1; // zero-based
            } else if (occurrence < 0) {
                _fromStartOfMonth = false;
                _occurrence = -occurrence - 1; // zero-based
            } else {
                throw new ArgumentException(nameof(occurrence));
            }
        }

        public override Date OfYear(int year)
        {
            var firstOfMonth = new Date(year, _month, 1);
            int dayOfWeekOffset = (int)_dayOfWeek - (int)firstOfMonth.DayOfWeek;
            int dateOfFirstOccurrenceOfDayOfWeek = (dayOfWeekOffset + 7) % 7 + 1;
            if (_fromStartOfMonth) {
                int day = _occurrence * 7 + dateOfFirstOccurrenceOfDayOfWeek;
                return new Date(year, _month, day);
            } else {
                int daysInMonth = DateTime.DaysInMonth(year, _month);
                int maxOccurences = (daysInMonth - dateOfFirstOccurrenceOfDayOfWeek) / 7;
                int day = (maxOccurences - _occurrence) * 7 + dateOfFirstOccurrenceOfDayOfWeek;
                return new Date(year, _month, day);
            }
        }
    }
}
