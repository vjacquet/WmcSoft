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
 * Adapted from TimePoint.java
 * ---------------------------
 * Copyright (c) 2005 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace WmcSoft.Time
{
    /// <summary>
    /// Represents a point in time.
    /// </summary>
    /// <remarks>Unlike <see cref="DateTime"/>, you cannot access its parts.</remarks>
    [Serializable]
    [DebuggerDisplay("{_storage,nq}")]
    public struct TimePoint : IComparable<TimePoint>, IEquatable<TimePoint>
    {
        private readonly DateTime _storage; // stores the time in UTC.

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TimePoint(DateTime dateTime, PiecewiseConstruct tag) {
            _storage = dateTime;
        }

        public TimePoint(DateTime dateTime) {
            _storage = dateTime.ToUniversalTime();
        }

        public TimePoint(DateTimeOffset dateTimeOffset) {
            _storage = dateTimeOffset.UtcDateTime;
        }

        public bool IsAfter(TimePoint other) {
            return CompareTo(other) > 0;
        }

        public bool IsBefore(TimePoint other) {
            return CompareTo(other) < 0;
        }

        public static TimePoint AtUtcMidnight(Date date) {
            return DateTime.SpecifyKind(TimeOfDay.Midnight.On(date), DateTimeKind.Utc);
        }

        public static TimePoint AtMidnight(Date date, TimeZoneInfo zone) {
            return TimeOfDay.Midnight.On(date, zone);
        }

        internal static TimePoint Now() {
            return new TimePoint(DateTime.UtcNow, PiecewiseConstruct.Tag);
        }

        #region Operators

        public static implicit operator TimePoint(DateTimeOffset x) {
            return new TimePoint(x.UtcDateTime);
        }

        public static explicit operator DateTimeOffset(TimePoint x) {
            return x._storage;
        }

        public static implicit operator TimePoint(DateTime x) {
            return new TimePoint(x);
        }

        public static explicit operator DateTime(TimePoint x) {
            return x._storage;
        }

        public static TimePoint operator +(TimePoint t, Duration d) {
            if (d.Unit.IsConvertibleToMilliseconds())
                return new TimePoint(t._storage + (TimeSpan)d);
            var months = checked((int)d.InBaseUnits());
            return new TimePoint(t._storage.AddMonths(months));
        }
        public static TimePoint Add(TimePoint t, Duration d) {
            return t + d;
        }

        public static TimePoint operator -(TimePoint t, Duration d) {
            if (d.Unit.IsConvertibleToMilliseconds())
                return new TimePoint(t._storage - (TimeSpan)d);
            var months = checked(-(int)d.InBaseUnits());
            return new TimePoint(t._storage.AddMonths(months));
        }
        public static TimePoint Subtract(TimePoint t, Duration d) {
            return t - d;
        }

        public static Duration operator -(TimePoint x, TimePoint y) {
            return y._storage - x._storage;
        }

        public static bool operator ==(TimePoint x, TimePoint y) {
            return x.Equals(y);
        }

        public static bool operator !=(TimePoint x, TimePoint y) {
            return !x.Equals(y);
        }

        public static bool Equals(TimePoint x, TimePoint y) {
            return x.Equals(y);
        }

        public static bool operator <(TimePoint x, TimePoint y) {
            return x.CompareTo(y) < 0;
        }

        public static bool operator >(TimePoint x, TimePoint y) {
            return x.CompareTo(y) > 0;
        }

        public static bool operator <=(TimePoint x, TimePoint y) {
            return x.CompareTo(y) <= 0;
        }

        public static bool operator >=(TimePoint x, TimePoint y) {
            return x.CompareTo(y) >= 0;
        }

        public static int Compare(TimePoint x, TimePoint y) {
            return x.CompareTo(y);
        }

        #endregion

        #region IComparable<TimePoint> members

        public int CompareTo(TimePoint other) {
            return _storage.CompareTo(other._storage);
        }

        #endregion

        #region IEquatable<TimePoint> members

        public bool Equals(TimePoint other) {
            return CompareTo(other) == 0;
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != GetType())
                return false;
            return Equals((TimePoint)obj);
        }

        public override int GetHashCode() {
            return _storage.GetHashCode();
        }

        #endregion
    }
}
