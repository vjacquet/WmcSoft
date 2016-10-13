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

using System;

namespace WmcSoft
{
    [Serializable]
    public struct GeographicPosition : IComparable<GeographicPosition>, IEquatable<GeographicPosition>
    {
        public GeographicPosition(Latitude latitude, Longitude longitude) {
            Latitude = latitude;
            Longitude = longitude;
        }
        public Latitude Latitude { get; }
        public Longitude Longitude { get; }

        public override int GetHashCode() {
            var h1 = Latitude.GetHashCode();
            var h2 = Longitude.GetHashCode();
            return (((h1 << 5) + h1) ^ h2);
        }

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != typeof(GeographicPosition))
                return false;
            return Equals((GeographicPosition)obj);
        }

        public bool Equals(GeographicPosition other) {
            return Latitude.Equals(other.Latitude)
                && Longitude.Equals(other.Longitude);
        }

        public int CompareTo(GeographicPosition other) {
            var tri = Latitude.CompareTo(other.Latitude);
            if(tri == 0)
                tri= Longitude.CompareTo(other.Longitude);
            return tri;
        }

        #region Operators

        public static bool operator ==(GeographicPosition x, GeographicPosition y) {
            return x.Equals(y);
        }

        public static bool operator !=(GeographicPosition a, GeographicPosition b) {
            return !a.Equals(b);
        }

        public static bool operator <(GeographicPosition x, GeographicPosition y) {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(GeographicPosition x, GeographicPosition y) {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator >(GeographicPosition x, GeographicPosition y) {
            return x.CompareTo(y) > 0;
        }
        public static bool operator >=(GeographicPosition x, GeographicPosition y) {
            return x.CompareTo(y) >= 0;
        }

        #endregion
    }
}
