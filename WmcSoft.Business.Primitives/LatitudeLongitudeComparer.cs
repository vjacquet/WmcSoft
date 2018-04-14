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

 ****************************************************************************/

#endregion

using System.Collections.Generic;

namespace WmcSoft
{
    /// <summary>
    /// Implements a comparer for <see cref="GeographicPosition"/> using lexycographical ordering on <see cref="Latitude"/> then <see cref="Longitude"/>.
    public struct LatitudeLongitudeComparer : IComparer<GeographicPosition>
    {
        public static LatitudeLongitudeComparer Default = default;

        /// <summary>Compares two <see cref="<see cref="GeographicPosition"/> and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
        /// <param name="x">The first <see cref="<see cref="GeographicPosition"/> to compare.</param>
        /// <param name="y">The second <see cref="<see cref="GeographicPosition"/> to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of x and y, as shown in the
        /// following table.
        /// <list type="table">
        /// <listheader>
        ///   <description>Value</description>
        ///   <description>Meaning</description>
        /// </listheader>
        /// <item>
        ///   <description>Less than zero</description>
        ///   <description> x is less than y.</description>
        /// </item>
        /// <item>
        ///   <description>Zero</description>
        ///   <description>x equals y.</description>
        /// </item>
        /// <item>
        ///   <description>Greater than zero</description>
        ///   <description>x is greater than y.</description>
        /// </item>
        /// </list>
        /// </returns>
        public int Compare(GeographicPosition x, GeographicPosition y)
        {
            var result = x.Latitude.CompareTo(y.Latitude);
            if (result == 0)
                result = x.Longitude.CompareTo(y.Longitude);
            return result;
        }
    }
}
