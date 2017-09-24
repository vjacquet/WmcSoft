#region Licence

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

using System;
using System.Text;

namespace WmcSoft.Business.Calendars
{
    internal static class Helpers
    {
        public static string HumanizeList(params IBusinessCalendar[] calendars)
        {
            if (calendars == null) throw new ArgumentNullException(nameof(calendars));

            var sb = new StringBuilder();
            var length = calendars.Length;
            switch (length) {
            case 0:
                break;
            case 1:
                sb.Append(calendars[0].Name);
                break;
            case 2:
                sb.Append(calendars[0].Name);
                sb.Append(" & ");
                sb.Append(calendars[1].Name);
                break;
            default:
                sb.Append(calendars[0].Name);
                length--;
                for (int i = 1; i < length; i++) {
                    sb.Append(", ");
                    sb.Append(calendars[i].Name);
                }
                sb.Append(" & ");
                sb.Append(calendars[length].Name);
                break;
            }
            return sb.ToString();
        }
    }
}
