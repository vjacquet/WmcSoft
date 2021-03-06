﻿#region Licence

/****************************************************************************
          Copyright 1999-2017 Vincent J. Jacquet.  All rights reserved.

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

 ****************************************************************************/

#endregion

using System.Diagnostics;
using WmcSoft.Time;

namespace WmcSoft.Business.Calendars
{
    /// <summary>
    /// Represents an empty business calendar for which every day is a business day.
    /// </summary>
    [DebuggerDisplay("[{MinDate.ToString(\"yyyy-MM-dd\"),nq} .. {MaxDate.ToString(\"yyyy-MM-dd\"),nq}]")]
    public struct EmptyCalendar : IBusinessCalendar
    {
        public string Name => "{Empty}";

        public Date MinDate => Date.MinValue;
        public Date MaxDate => Date.MaxValue;

        public bool IsBusinessDay(Date date)
        {
            return true;
        }

        #region Required overrides

        public override bool Equals(object obj)
        {
            return obj != null && obj.GetType() == typeof(EmptyCalendar);
        }

        public override int GetHashCode()
        {
            return 1515;
        }

        public static bool operator ==(EmptyCalendar left, EmptyCalendar right)
        {
            return true;
        }

        public static bool operator !=(EmptyCalendar left, EmptyCalendar right)
        {
            return false;
        }

        #endregion
    }
}
