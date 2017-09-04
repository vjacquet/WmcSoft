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
 * Adapted from FixedDateSpecification.java
 * ----------------------------------------
 * Copyright (c) 2005 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

namespace WmcSoft.Time
{
    sealed class FixedDateSpecification : AnnualDateSpecification
    {
        private readonly int _month;
        private readonly int _day;

        public FixedDateSpecification(int month, int day)
        {
            _month = month;
            _day = day;
        }

        public override Date OfYear(int year)
        {
            return new Date(year, _month, _day);
        }

        public override bool IsSatisfiedBy(Date date)
        {
            return date.Day == _day && date.Month == _month;
        }
    }
}
