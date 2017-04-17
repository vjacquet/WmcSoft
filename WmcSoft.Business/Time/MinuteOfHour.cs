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
 * Adapted from MinuteOfHour.java
 * ------------------------------
 * Copyright (c) 2005 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System;
using System.Diagnostics;

namespace WmcSoft.Time
{
    /// <summary>
    /// Represents the minutes of an hour.
    /// </summary>
    /// <remarks>The value is in the range [0, 59].</remarks>
    [DebuggerDisplay("{_storage,nq}mn")]
    [Serializable]
    public struct MinuteOfHour : IComparable<MinuteOfHour>, IEquatable<MinuteOfHour>
    {
        private readonly short _storage;

        public MinuteOfHour(int minute) {
            if (minute < 0 | minute > 59) throw new ArgumentOutOfRangeException(nameof(minute));
            _storage = unchecked((short)minute);
        }

        public int Value { get { return _storage; } }

        public override string ToString() {
            return _storage + "mn";
        }

        public override int GetHashCode() {
            return _storage;
        }

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != typeof(MinuteOfHour))
                return false;
            return Equals((MinuteOfHour)obj);
        }

        public bool Equals(MinuteOfHour other) {
            return _storage.Equals(other._storage);
        }

        public int CompareTo(MinuteOfHour other) {
            return _storage.CompareTo(other._storage);
        }

        public bool IsAfter(MinuteOfHour other) {
            return CompareTo(other) > 0;
        }

        public bool IsBefore(MinuteOfHour other) {
            return CompareTo(other) < 0;
        }

        #region Operators

        public static implicit operator MinuteOfHour(int h) {
            return new MinuteOfHour(h);
        }

        public static explicit operator int(MinuteOfHour h) {
            return h.Value;
        }

        public static bool operator ==(MinuteOfHour x, MinuteOfHour y) {
            return x.Equals(y);
        }

        public static bool operator !=(MinuteOfHour x, MinuteOfHour y) {
            return !x.Equals(y);
        }

        public static bool operator <(MinuteOfHour x, MinuteOfHour y) {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(MinuteOfHour x, MinuteOfHour y) {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator >(MinuteOfHour x, MinuteOfHour y) {
            return x.CompareTo(y) > 0;
        }
        public static bool operator >=(MinuteOfHour x, MinuteOfHour y) {
            return x.CompareTo(y) >= 0;
        }

        #endregion
    }
}
