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
 * Adapted from TimeUnit.java, TimeUnitConversionFactors.java
 * ----------------------------------------------------------
 * Copyright (c) 2005 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System;

namespace WmcSoft.Time
{
    public partial struct TimeUnit
    {
        public static Period operator *(int n, TimeUnit units)
        {
            return new Period(n, units);
        }

        public static Period operator *(TimeUnit units, int n)
        {
            return new Period(n, units);
        }

        public Frequency ToFrequency(int length)
        {
            length = Math.Abs(length);
            if (length == 0)
                return _type == Type.Year ? Frequency.Once : Frequency.NoFrequency;

            switch (_type) {
            case Type.Year:
                return length == 1 ? Frequency.Annual : Frequency.OtherFrequency;
            case Type.Semester:
                length *= 6;
                goto case Type.Month;
            case Type.Quarter:
                length *= 3;
                goto case Type.Month;
            case Type.Month:
                return (12 % length == 0 && length <= 12)
                    ? (Frequency)(12 / length)
                    : Frequency.OtherFrequency;
            case Type.Week:
                switch (length) {
                case 1:
                    return Frequency.Weekly;
                case 2:
                    return Frequency.Biweekly;
                case 4:
                    return Frequency.EveryFourthWeek;
                default:
                    return Frequency.OtherFrequency;
                }
            case Type.Day:
                return length == 1 ? Frequency.Daily : Frequency.OtherFrequency;
            default:
                throw new InvalidOperationException();
            }
        }
    }
}
