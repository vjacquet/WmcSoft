﻿#region Licence

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
 * Adapted from TimeOfDay.java
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
    /// <summary>
    /// Represents a time of day, as would be read from a clock, within the range 00:00 to 23:59
    /// </summary>
    [DebuggerDisplay("{ToString(),nq}")]
    [DebuggerStepThrough]
    public partial struct TimeOfDay : IEquatable<TimeOfDay>, IComparable<TimeOfDay>, IComparable, IFormattable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly TimeSpan _storage;

        public static readonly TimeOfDay MinValue = new TimeOfDay(0);
        public static readonly TimeOfDay MaxValue = new TimeOfDay(23, 59);

        public static readonly TimeOfDay Midnight = new TimeOfDay(0);
        public static readonly TimeOfDay Noon = new TimeOfDay(12);

        /// <summary>
        /// Initializes a new instance of a <see cref="TimeOfDay"/> structure to the specified
        /// hour and minute.
        /// </summary>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="hour"/> is less than 0 or greater than 23.
        /// <para>-or-</para>
        /// <paramref name="minute"/> is less than 0 or greater than 59.
        /// </exception>
        public TimeOfDay(int hour, int minute)
        {
            if (hour < 0 | hour > 23) throw new ArgumentOutOfRangeException(nameof(hour));
            if (minute < 0 | minute > 59) throw new ArgumentOutOfRangeException(nameof(minute));

            _storage = new TimeSpan(hour, minute, 0);
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="TimeOfDay"/> structure to the specified hour.
        /// </summary>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="hour"/> is less than 0 or greater than 23.
        /// </exception>
        public TimeOfDay(int hour)
        {
            if (hour < 0 | hour > 23) throw new ArgumentOutOfRangeException(nameof(hour));

            _storage = new TimeSpan(hour, 0, 0);
        }

        public void Deconstruct(out int hour, out int minute)
        {
            hour = _storage.Hours;
            minute = _storage.Minutes;
        }

        public HourOfDay Hour => new HourOfDay(_storage.Hours);

        public MinuteOfHour Minutes => new MinuteOfHour(_storage.Minutes);

        public bool IsAfter(TimeOfDay other)
        {
            return CompareTo(other) > 0;
        }

        public bool IsBefore(TimeOfDay other)
        {
            return CompareTo(other) < 0;
        }

        public DateTime On(Date date)
        {
            DateTime dateTime = date;
            return dateTime.Add(_storage);
        }

        public DateTimeOffset On(Date date, TimeZoneInfo timeZone)
        {
            var dateTimeOffset = new DateTimeOffset(On(date));
            return TimeZoneInfo.ConvertTime(dateTimeOffset, timeZone);
        }

        public TimeSpan ToTimeSpan()
        {
            return _storage;
        }

        #region Operators

        public static bool operator ==(TimeOfDay x, TimeOfDay y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(TimeOfDay x, TimeOfDay y)
        {
            return !x.Equals(y);
        }

        public static bool operator <(TimeOfDay x, TimeOfDay y)
        {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(TimeOfDay x, TimeOfDay y)
        {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator >(TimeOfDay x, TimeOfDay y)
        {
            return x.CompareTo(y) > 0;
        }
        public static bool operator >=(TimeOfDay x, TimeOfDay y)
        {
            return x.CompareTo(y) >= 0;
        }

        public static implicit operator TimeSpan(TimeOfDay x)
        {
            return x._storage;
        }

        #endregion

        #region IComparable<TimeOfDay> members

        public int CompareTo(TimeOfDay other)
        {
            return _storage.CompareTo(other._storage);
        }

        public int CompareTo(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return 1;
            return CompareTo((Date)obj);
        }

        #endregion

        #region IEquatable<TimeOfDay> members

        public bool Equals(TimeOfDay other)
        {
            return _storage.Equals(other._storage);
        }

        #endregion

        #region IFormattable members

        public string ToString(string format)
        {
            var date = On(Date.MinValue);
            return date.ToString(format);
        }

        public string ToString(IFormatProvider formatProvider)
        {
            var date = On(Date.MinValue);
            return date.ToString("d", formatProvider);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            var date = On(Date.MinValue);
            return date.ToString(format, formatProvider);
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return _storage.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(TimeOfDay))
                return false;
            return Equals((TimeOfDay)obj);
        }

        public override string ToString()
        {
            return _storage.Hours.ToString("00") + ':' + _storage.Minutes.ToString("00");
        }

        #endregion
    }
}
