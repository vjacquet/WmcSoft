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
    public struct Longitude : IComparable<Longitude>, IEquatable<Longitude>
    {
        const int Amplitude = 180;

        public static readonly Longitude MaxValue = Unguarded(Amplitude);
        public static readonly Longitude MinValue = Unguarded(-Amplitude);

        static Longitude Unguarded(int degrees, int minutes = 0, int seconds = 0) {
            var value = new Longitude();
            value._storage = degrees * 3600 + minutes * 60 + seconds;
            return value;
        }

        private int _storage;

        public Longitude(int degrees, int minutes, int seconds) {
            if (degrees < -Amplitude | degrees > Amplitude) throw new ArgumentOutOfRangeException(nameof(degrees));
            if ((degrees == -Amplitude | degrees == Amplitude) & minutes != 0 & seconds != 0) throw new ArgumentOutOfRangeException(nameof(degrees));
            if (minutes < 0 | minutes > 59) throw new ArgumentOutOfRangeException(nameof(minutes));
            if (seconds < 0 | seconds > 59) throw new ArgumentOutOfRangeException(nameof(seconds));

            _storage = degrees * 3600 + minutes * 60 + seconds;
        }

        public int Degrees { get { return _storage / 3600; } }
        public int Minutes { get { return (_storage / 60) % 60; } }
        public int Seconds { get { return _storage % 60; } }

        public override int GetHashCode() {
            return _storage;
        }

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != typeof(Longitude))
                return false;
            return Equals((Longitude)obj);
        }

        public bool Equals(Longitude other) {
            return _storage.Equals(other._storage);
        }

        public int CompareTo(Longitude other) {
            return _storage.CompareTo(other._storage);
        }

        #region Operators

        public static implicit operator decimal(Longitude x) {
            return x._storage / 3600m;
        }

        public static bool operator ==(Longitude x, Longitude y) {
            return x.Equals(y);
        }

        public static bool operator !=(Longitude a, Longitude b) {
            return !a.Equals(b);
        }

        public static bool operator <(Longitude x, Longitude y) {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(Longitude x, Longitude y) {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator >(Longitude x, Longitude y) {
            return x.CompareTo(y) > 0;
        }
        public static bool operator >=(Longitude x, Longitude y) {
            return x.CompareTo(y) >= 0;
        }

        #endregion
    }
}