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
using System.Diagnostics;

using static WmcSoft.GeoCoordinate;

namespace WmcSoft
{
    /// <summary>
    /// Represents the latitude, a geographic coordinate that specifies the north-south position of a point on the Earth's surface.
    /// Latitude is an angle which ranges from 0° at the Equator to 90° at the north pole and -90° at the south pole.
    /// </summary>
    [DebuggerDisplay("{ToString(),nq}")]
    public partial struct Latitude : IComparable<Latitude>, IEquatable<Latitude>, IFormattable
    {
        const int Amplitude = 90;

        public static readonly Latitude MaxValue = new Latitude(Amplitude);
        public static readonly Latitude MinValue = new Latitude(-Amplitude);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly int Storage;

        private Latitude(int degrees)
        {
            Storage = Encode(degrees);
        }

        public Latitude(decimal degrees)
        {
            if (degrees < -Amplitude | degrees > Amplitude) throw new ArgumentOutOfRangeException(nameof(degrees));
            Storage = FromDecimal(degrees);
        }

        public Latitude(int degrees, int minutes = 0, int seconds = 0, int milliseconds = 0)
        {
            if (degrees < -Amplitude | degrees > Amplitude) throw new ArgumentOutOfRangeException(nameof(degrees));
            if ((degrees == -Amplitude | degrees == Amplitude) & minutes != 0 & seconds != 0 & milliseconds != 0) throw new ArgumentOutOfRangeException(nameof(degrees));
            if (minutes < 0 | minutes > 59) throw new ArgumentOutOfRangeException(nameof(minutes));
            if (seconds < 0 | seconds > 59) throw new ArgumentOutOfRangeException(nameof(seconds));
            if (milliseconds < 0 | milliseconds > 1000) throw new ArgumentOutOfRangeException(nameof(milliseconds));

            Storage = Encode(degrees, minutes, seconds, milliseconds);
        }

        public void Deconstruct(out int degrees, out int minutes, out int seconds)
        {
            Decode(Storage, out degrees, out minutes, out seconds);
        }

        public void Deconstruct(out int degrees, out int minutes, out int seconds, out int milliseconds)
        {
            Decode(Storage, out degrees, out minutes, out seconds, out milliseconds);
        }

        public int Degrees => DecodeDegrees(Storage);
        public int Minutes => DecodeMinutes(Storage);
        public int Seconds => DecodeSeconds(Storage);
        public int Milliseconds => DecodeMilliseconds(Storage);

        public override int GetHashCode()
        {
            return Storage;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Latitude))
                return false;
            return Equals((Latitude)obj);
        }

        public bool Equals(Latitude other)
        {
            return Storage.Equals(other.Storage);
        }

        public int CompareTo(Latitude other)
        {
            return Storage.CompareTo(other.Storage);
        }

        #region Operators

        public static implicit operator decimal(Latitude x)
        {
            return ToDecimal(x.Storage);
        }

        public static bool operator ==(Latitude x, Latitude y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Latitude a, Latitude b)
        {
            return !a.Equals(b);
        }

        public static bool operator <(Latitude x, Latitude y)
        {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(Latitude x, Latitude y)
        {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator >(Latitude x, Latitude y)
        {
            return x.CompareTo(y) > 0;
        }
        public static bool operator >=(Latitude x, Latitude y)
        {
            return x.CompareTo(y) >= 0;
        }

        #endregion

        #region IFormattable Membres

        public override string ToString()
        {
            return ToString(null, null);
        }

        public string ToString(IFormatProvider formatProvider)
        {
            return ToString(null, formatProvider);
        }

        public string ToString(string format, IFormatProvider formatProvider = null)
        {
            var formatter = new GeoFormatter(format, formatProvider);
            var (d, m, s) = this;
            return formatter.Format(d, m, s);
        }

        #endregion
    }
}
