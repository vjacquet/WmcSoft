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
 * Adapted from HourOfDay.java
 * ---------------------------
 * Copyright (c) 2005 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System;
using System.Diagnostics;

namespace WmcSoft.Time
{
    [DebuggerDisplay("{_storage,nq}h")]
    public struct HourOfDay : IComparable<HourOfDay>, IEquatable<HourOfDay>
    {
        private readonly short _storage;

        public HourOfDay(int hour) {
            if (hour < 0 | hour > 23) throw new ArgumentOutOfRangeException("hour");
            _storage = (short)hour;
        }

        public HourOfDay(int hour, string am_pm) {
            _storage = (short)ConvertTo24Hour(hour, am_pm);
        }

        public override string ToString() {
            return _storage + "h";
        }

        public override int GetHashCode() {
            return _storage;
        }

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != typeof(HourOfDay))
                return false;
            return Equals((HourOfDay)obj);
        }

        public bool Equals(HourOfDay other) {
            return _storage.Equals(other._storage);
        }

        public int CompareTo(HourOfDay other) {
            return _storage.CompareTo(other._storage);
        }

        public int Value { get { return _storage; } }

        public bool After(HourOfDay other) {
            return CompareTo(other) > 0;
        }

        public bool Before(HourOfDay other) {
            return CompareTo(other) < 0;
        }

        #region Operators

        public static implicit operator HourOfDay(int h) {
            return new HourOfDay(h);
        }

        public static explicit operator int(HourOfDay h) {
            return h.Value;
        }

        public static bool operator ==(HourOfDay x, HourOfDay y) {
            return x.Equals(y);
        }

        public static bool operator !=(HourOfDay a, HourOfDay b) {
            return !a.Equals(b);
        }

        public static bool operator <(HourOfDay x, HourOfDay y) {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(HourOfDay x, HourOfDay y) {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator >(HourOfDay x, HourOfDay y) {
            return x.CompareTo(y) > 0;
        }
        public static bool operator >=(HourOfDay x, HourOfDay y) {
            return x.CompareTo(y) >= 0;
        }

        #endregion

        #region Helpers

        static int ConvertTo24Hour(int hour, string am_pm) {
            if (!("AM".Equals(am_pm, StringComparison.OrdinalIgnoreCase) || "PM".Equals(am_pm, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentOutOfRangeException("am_pm");
            if (hour < 0 | hour > 12) throw new ArgumentOutOfRangeException("hour");

            int translatedAmPm = "AM".Equals(am_pm, StringComparison.OrdinalIgnoreCase) ? 0 : 12;
            translatedAmPm -= (hour == 12) ? 12 : 0;
            return hour + translatedAmPm;
        }

        #endregion
    }
}
