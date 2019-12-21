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
 * Adapted from AnnualDateSpecification.java
 * -----------------------------------------
 * Copyright (c) 2005 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System.Collections.Generic;
using System.Linq;

namespace WmcSoft.Time
{
    /// <summary>
    /// Base class for specification on date that occurs once in a year.
    /// </summary>
    public abstract class AnnualDateSpecification : IDateSpecification
    {
        /// <summary>
        /// Returns the <see cref="Date"/> of the <paramref name="year"/> satisfying the specification.
        /// </summary>
        /// <param name="year">Year for which the <see cref="Date"/> should be computed.</param>
        /// <returns>The <see cref="Date"/> of the <paramref name="year"/> satisfying the specification.</returns>
        public abstract Date OfYear(int year);

        protected virtual IEnumerable<Date> UnguardedEnumerateOver(Date since, Date until)
        {
            var year = since.Year;

            var current = OfYear(year);
            if (current < since)
                current = OfYear(++year);

            while (current <= until) {
                yield return current;
                current = OfYear(++year);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Date> EnumerateBetween(Date since, Date until)
        {
            if (since > until)
                return Enumerable.Empty<Date>();
            return UnguardedEnumerateOver(since, until);
        }

        /// <inheritdoc/>
        public virtual bool IsSatisfiedBy(Date date)
        {
            return date == OfYear(date.Year);
        }
    }
}
