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

using System.Collections.Generic;
using WmcSoft.Time;

namespace WmcSoft.Business.Calendars.Specifications
{
    public sealed class GregorianEasterSpecification : AnnualDateSpecification
    {
        public override Date OfYear(int year)
        {
            // United States Naval Observatory Easter <http://www.cpearson.com/excel/easter.aspx>

            var c = year / 100;
            var N = year - 19 * (year / 19);
            var K = (c - 17) / 25;
            var i = c - c / 4 - (c - K) / 3 + 19 * N + 15;
            i = i - 30 * (i / 30);
            i = i - (i / 28) * (1 - (i / 28) * (29 / (i + 1)) * ((21 - N) / 11));
            var j = year + year / 4 + i + 2 - c + c / 4;
            j = j - 7 * (j / 7);
            var l = i - j;
            var m = 3 + (l + 40) / 44;
            var d = l + 28 - 31 * (m / 4);
            return new Date(year, (int)m, (int)d);
        }
    }
}
