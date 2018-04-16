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
 * Adapted from TimeOfDay.java
 * ---------------------------
 * Copyright (c) 2005 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion


namespace WmcSoft.Time
{
    /// <summary>
    /// Provides an enumeration of AM or PM to support 12-hour clock values in the <see cref="TimeOfDay"/> type.
    /// </summary>
    /// <remarks>
    /// Though commonly used in English, these abbreviations derive from Latin: AM and PM are abbreviation respectively for "Ante Meridiem" and "Post Meridiem",
    /// meaning "before mid-day" and "after mid-day".
    /// </remarks>
    public enum Meridiem
    {
        /// <summary>
        /// Ante Meridiem, meaning "before mid-day".
        /// </summary>
        AM,
        /// <summary>
        /// Post Meridiem, meaning "after mid-day".
        /// </summary>
        PM
    }
}
