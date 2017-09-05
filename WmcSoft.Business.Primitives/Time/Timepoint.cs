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
    /// Represents a point in time, an instant on the global timeline.
    /// </summary>
    /// <remarks>Unlike <see cref="DateTime"/>, you cannot access its parts.</remarks>
    [Serializable]
    [DebuggerDisplay("{new DateTime(_storage,DateTimeKind.Utc), nq}")]
    public struct Timepoint : IComparable<Timepoint>, IEquatable<Timepoint>
    {
        // The name `instant` was considered but put aside
        // as it can also mean a very short period of time.

        private readonly long _storage; // stores the ticks in UTC.

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Timepoint(long ticks)
        {
            _storage = ticks;
        }

        public Timepoint(DateTime dateTime)
        {
            _storage = dateTime.ToUniversalTime().Ticks;
        }

        public Timepoint(DateTimeOffset dateTimeOffset)
        {
            _storage = dateTimeOffset.UtcDateTime.Ticks;
        }

        public bool IsAfter(Timepoint other)
        {
            return CompareTo(other) > 0;
        }

        public bool IsBefore(Timepoint other)
        {
            return CompareTo(other) < 0;
        }

        public static Timepoint AtUtcMidnight(Date date)
        {
            return DateTime.SpecifyKind(TimeOfDay.Midnight.On(date), DateTimeKind.Utc);
        }

        public static Timepoint AtMidnight(Date date, TimeZoneInfo zone)
        {
            return TimeOfDay.Midnight.On(date, zone);
        }

        internal static Timepoint Now()
        {
            return new Timepoint(DateTime.UtcNow.Ticks);
        }

        #region Operators

        public static implicit operator Timepoint(DateTimeOffset x)
        {
            return new Timepoint(x.UtcDateTime);
        }

        public static explicit operator DateTimeOffset(Timepoint x)
        {
            return new DateTimeOffset((DateTime)x);
        }

        public static implicit operator Timepoint(DateTime x)
        {
            return new Timepoint(x);
        }

        public static explicit operator DateTime(Timepoint x)
        {
            return new DateTime(x._storage, DateTimeKind.Utc);
        }

        public static Timepoint operator +(Timepoint t, Duration d)
        {
            var dateTime = (DateTime)t;
            if (d.Unit.IsConvertibleToMilliseconds())
                return new Timepoint(dateTime + (TimeSpan)d);
            var months = checked((int)d.InBaseUnits());
            return new Timepoint(dateTime.AddMonths(months));
        }
        public static Timepoint Add(Timepoint t, Duration d)
        {
            return t + d;
        }

        public static Timepoint operator -(Timepoint t, Duration d)
        {
            var dateTime = (DateTime)t;
            if (d.Unit.IsConvertibleToMilliseconds())
                return new Timepoint(dateTime - (TimeSpan)d);
            var months = checked(-(int)d.InBaseUnits());
            return new Timepoint(dateTime.AddMonths(months));
        }
        public static Timepoint Subtract(Timepoint t, Duration d)
        {
            return t - d;
        }

        public static Duration operator -(Timepoint x, Timepoint y)
        {
            return (DateTime)y - (DateTime)x;
        }

        public static bool operator ==(Timepoint x, Timepoint y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Timepoint x, Timepoint y)
        {
            return !x.Equals(y);
        }

        public static bool Equals(Timepoint x, Timepoint y)
        {
            return x.Equals(y);
        }

        public static bool operator <(Timepoint x, Timepoint y)
        {
            return x.CompareTo(y) < 0;
        }

        public static bool operator >(Timepoint x, Timepoint y)
        {
            return x.CompareTo(y) > 0;
        }

        public static bool operator <=(Timepoint x, Timepoint y)
        {
            return x.CompareTo(y) <= 0;
        }

        public static bool operator >=(Timepoint x, Timepoint y)
        {
            return x.CompareTo(y) >= 0;
        }

        public static int Compare(Timepoint x, Timepoint y)
        {
            return x.CompareTo(y);
        }

        #endregion

        #region IComparable<TimePoint> members

        public int CompareTo(Timepoint other)
        {
            return _storage.CompareTo(other._storage);
        }

        #endregion

        #region IEquatable<TimePoint> members

        public bool Equals(Timepoint other)
        {
            return CompareTo(other) == 0;
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;
            return Equals((Timepoint)obj);
        }

        public override int GetHashCode()
        {
            return _storage.GetHashCode();
        }

        #endregion
    }
}
