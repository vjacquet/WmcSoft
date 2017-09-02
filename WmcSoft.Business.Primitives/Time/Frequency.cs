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
 * Adapted from HourOfDay.java
 * ---------------------------
 * Copyright (c) 2005 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

namespace WmcSoft.Time
{
    /// <summary>
    /// Represents a frequency of events.
    /// </summary>
    public enum Frequency
    {
        /// <summary>Null frequency</summary>
        NoFrequency = -1,
        /// <summary>Only once</summary>
        Once = 0,
        /// <summary>Once a year</summary>
        Annual = 1,
        /// <summary>Twice a year</summary>
        Semiannual = 2,
        /// <summary>Every fourth month</summary>
        EveryFourthMonth = 3,
        /// <summary>Every third month</summary>
        Quarterly = 4,
        /// <summary>Every second month</summary>
        Bimonthly = 6,
        /// <summary>Once a month</summary>
        Monthly = 12,
        /// <summary>Every fourth week</summary>
        EveryFourthWeek = 13,
        /// <summary>Every second week</summary>
        Biweekly = 26,
        /// <summary>Once a week</summary>
        Weekly = 52,
        /// <summary>Once a day</summary>
        Daily = 365,
        /// <summary>Some other unknown frequency</summary>
        OtherFrequency = 999
    }
}
