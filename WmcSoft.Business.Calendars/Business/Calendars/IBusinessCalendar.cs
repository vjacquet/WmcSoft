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

using WmcSoft.Time;

namespace WmcSoft.Business.Calendars
{
    /// <summary>
    /// Represents a business calendar
    /// </summary>
    public interface IBusinessCalendar
    {
        /// <summary>
        /// The name fo the calendar.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The minimal date supported by the calendar, inclusive.
        /// </summary>
        Date MinDate { get; }

        /// <summary>
        /// The maximal date supported by the calendar, inclusive.
        /// </summary>
        Date MaxDate { get; }

        /// <summary>
        /// Returns wether the day is a business day or not.
        /// </summary>
        /// <param name="date">The day</param>
        /// <returns><c>true</c> if the <paramref name="date"/> is a business day; otherwise, <c>false</c>.</returns>
        bool IsBusinessDay(Date date);
    }
}
