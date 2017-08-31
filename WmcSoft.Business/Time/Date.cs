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
    [DebuggerDisplay("{_storage,nq}")]
    [Serializable]
    public struct Date : IComparable<Date>, IEquatable<Date>
    {
        readonly DateTime _storage;

        private Date(DateTime date)
        {
            _storage = DateTime.SpecifyKind(date.Date, DateTimeKind.Unspecified);
        }

        public Date(int year, int month, int day)
        {
            _storage = new DateTime(year, month, day);
        }

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
            return new Date(_storage.AddDays(1));
        }

        public Date PreviousDay()
        {
            return new Date(_storage.AddDays(-1));
        }

        public Timepoint AsTimePoint(TimeZoneInfo zone)
        {
            var dateTime = TimeZoneInfo.ConvertTimeToUtc(_storage, zone);
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

        #region Operators

        public static explicit operator Date(DateTime date)
        {
            return new Date(date);
        }

        public static explicit operator Date(DateTimeOffset date)
        {
            return new Date(date.Date);
        }

        public static implicit operator DateTime(Date date)
        {
            return date._storage;
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

        #endregion
    }
}