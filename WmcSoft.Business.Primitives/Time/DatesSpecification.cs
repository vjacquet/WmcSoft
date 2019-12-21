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

using System.Collections.Generic;
using System.Linq;

using static System.Math;
using static System.Array;

namespace WmcSoft.Time
{
    /// <summary>
    /// Represents a specification satisfied by a list of dates.
    /// </summary>
    public sealed class DatesSpecification : IDateSpecification
    {
        private readonly Date[] _dates;

        /// <summary>
        /// Construct the specification with the <paramref name="dates"/> in chronological order.
        /// </summary>
        /// <param name="dates">The dates satistying this specification, in chronological order.</param>
        public DatesSpecification(IEnumerable<Date> dates)
        {
            _dates = dates.ToArray();
        }

        /// <inheritdoc/>
        public IEnumerable<Date> EnumerateBetween(Date since, Date until)
        {
            var first = BinarySearch(_dates, since);
            if (first < 0)
                first = ~first;

            var last = BinarySearch(_dates, first, _dates.Length - first, until);
            if (last < 0)
                last = Min(~last - 1, _dates.Length - 1);

            while (first <= last) {
                yield return _dates[first++];
            }
        }

        /// <inheritdoc/>
        public bool IsSatisfiedBy(Date candidate)
        {
            return BinarySearch(_dates, candidate) >= 0;
        }
    }
}
