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
 * Adapted from CalendarDate.java
 * ------------------------------
 * Copyright (c) 2004 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System;
using System.Diagnostics;

namespace WmcSoft.Time
{
    /// <summary>
    /// Represents a whole date, having a year, month and day component.
    /// All values are in the proleptic Gregorian (ISO 8601) calendar.
    /// </summary>
    [DebuggerDisplay("{ToString(),nq}")]
    [DebuggerStepThrough]
    public partial struct Date : IComparable<Date>, IEquatable<Date>, IFormattable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly int _storage;

        public static readonly Date MinValue = DateTime.MinValue;
        public static readonly Date MaxValue = DateTime.MaxValue;

        private Date(DateTime date)
        {
            _storage = (int)(date.Ticks / TimeSpan.TicksPerDay);
        }

        private Date(int dayNumber)
        {
            if (dayNumber < MinValue._storage || dayNumber > MaxValue._storage) throw new ArgumentOutOfRangeException(nameof(dayNumber));

            _storage = dayNumber;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Date"/> structure to the specified <paramref name="year"/>, <paramref name="month"/> and <paramref name="day"/>.
        /// </summary>
        /// <param name="year">The year (1 through 9999)</param>
        /// <param name="month">The month (1 through 12)</param>
        /// <param name="day">The day (1 through the number of days in <paramref name="month"/>.</param>
        public Date(int year, int month, int day) : this(new DateTime(year, month, day))
        {
        }

        public void Deconstruct(out int year, out int month, out int day)
        {
            var date = AsDateTime;
            year = date.Year;
            month = date.Month;
            day = date.Day;
        }

        /// <summary>
        /// Gets the current date.
        /// </summary>
        public static Date Today => DateTime.Today;

        public bool IsAfter(Date other)
        {
            return CompareTo(other) > 0;
        }

        public bool IsBefore(Date other)
        {
            return CompareTo(other) < 0;
        }

        public Date NextDay()
        {
            return new Date(_storage + 1);
        }

        public Date PreviousDay()
        {
            return new Date(_storage - 1);
        }

        public Timepoint AsTimepoint(TimeZoneInfo zone)
        {
            var dateTime = TimeZoneInfo.ConvertTimeToUtc(AsDateTime, zone);
            return new Timepoint(dateTime);
        }

        public DateTime At(TimeOfDay time)
        {
            return time.On(this);
        }

        public DateTimeOffset At(TimeOfDay time, TimeZoneInfo timeZone)
        {
            return time.On(this, timeZone);
        }

        public int Year => AsDateTime.Year;
        public int Month => AsDateTime.Month;
        public int Day => AsDateTime.Day;
        public DayOfWeek DayOfWeek => (DayOfWeek)((_storage + 1) % 7);

        public Date AddDays(int value)
        {
            return new Date(_storage + value);
        }

        public Date AddMonths(int value)
        {
            return AsDateTime.AddMonths(value);
        }

        public Date AddYears(int value)
        {
            return AsDateTime.AddYears(value);
        }

        public Date Add(TimeSpan value)
        {
            return AsDateTime.Add(value);
        }

        public int DaysSince(Date date)
        {
            return _storage - date._storage;
        }

        public int DaysUntil(Date date)
        {
            return date._storage - _storage;
        }

        public DateTime ToDateTime()
        {
            return AsDateTime;
        }

        #region Operators

        DateTime AsDateTime => new DateTime(_storage * TimeSpan.TicksPerDay);

        public static implicit operator Date(DateTime date)
        {
            return new Date(date);
        }

        public static implicit operator Date(DateTimeOffset date)
        {
            return new Date(date.Date);
        }

        public static implicit operator DateTime(Date date)
        {
            return date.AsDateTime;
        }

        public static bool operator ==(Date x, Date y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Date x, Date y)
        {
            return !x.Equals(y);
        }

        public static bool Equals(Date x, Date y)
        {
            return x.Equals(y);
        }

        public static bool operator <(Date x, Date y)
        {
            return x.CompareTo(y) < 0;
        }

        public static bool operator >(Date x, Date y)
        {
            return x.CompareTo(y) > 0;
        }

        public static bool operator <=(Date x, Date y)
        {
            return x.CompareTo(y) <= 0;
        }

        public static bool operator >=(Date x, Date y)
        {
            return x.CompareTo(y) >= 0;
        }

        public static int Compare(Date x, Date y)
        {
            return x.CompareTo(y);
        }

        #endregion

        #region IComparable<Date> members

        public int CompareTo(Date other)
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

        #region IEquatable<Date> members

        public bool Equals(Date other)
        {
            return CompareTo(other) == 0;
        }

        #endregion

        #region IFormattable members

        public string ToString(string format)
        {
            return ((DateTime)this).ToString(format);
        }

        public string ToString(IFormatProvider formatProvider)
        {
            return ((DateTime)this).ToString("d", formatProvider);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ((DateTime)this).ToString(format, formatProvider);
        }

        #endregion 

        #region Overrides

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;
            return Equals((Date)obj);
        }

        public override int GetHashCode()
        {
            return _storage.GetHashCode();
        }

        public override string ToString()
        {
            return AsDateTime.ToString("d");
        }

        #endregion
    }

    public static partial class DateExtensions
    {
        public static double ToOADate(this Date date)
        {
            return ((DateTime)date).ToOADate();
        }
    }
}
