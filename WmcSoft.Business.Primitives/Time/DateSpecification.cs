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

namespace WmcSoft.Time
{
    public static class DateSpecification
    {
        /// <summary>
        /// Specifies a fixed day in a month, like January the 1rst.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="day">The day of the month</param>
        /// <returns>The <paramref name="day"/> of <paramref name="month"/>.</returns>
        public static AnnualDateSpecification Fixed(int month, int day)
        {
            return new FixedDateSpecification(month, day);
        }

        /// <summary>
        /// Specifies the <paramref name="n"/>th <paramref name="dayOfWeek"/> of <paramref name="month"/>.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="dayOfWeek">The day of the week in the month.</param>
        /// <param name="n">The number of occurences, zero-based. A negative value indicates the number of occurences from the end of the month.</param>
        /// <returns>The <paramref name="n"/>th <paramref name="dayOfWeek"/> of <paramref name="month"/>.</returns>
        public static AnnualDateSpecification NthOccurenceOfWeekdayInMonth(int month, DayOfWeek dayOfWeek, int n)
        {
            return new FloatingDateSpecification(month, dayOfWeek, n);
        }
    }
}
