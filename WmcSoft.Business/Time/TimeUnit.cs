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
 * Adapted from TimeUnit.java, TimeUnitConversionFactors.java
 * ----------------------------------------------------------
 * Copyright (c) 2005 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System;

namespace WmcSoft.Time
{
    [Serializable]
    public struct TimeUnit : IComparable<TimeUnit>, IEquatable<TimeUnit>
    {
        const int MillisecondsPerSecond = 1000;
        const int MillisecondsPerMinute = 60 * MillisecondsPerSecond;
        const int MillisecondsPerHour = 60 * MillisecondsPerMinute;
        const int MillisecondsPerDay = 24 * MillisecondsPerHour;
        const int MillisecondsPerWeek = 7 * MillisecondsPerDay;
        const int MonthsPerQuarter = 3;
        const int MonthsPerYear = 12;

        public enum Type
        {
            Millisecond,
            Second,
            Minute,
            Hour,
            Day,
            Week,
            Month,
            Quarter,
            Year,
        }

        public static readonly TimeUnit Millisecond = new TimeUnit(Type.Millisecond, Type.Millisecond, 1);
        public static readonly TimeUnit Second = new TimeUnit(Type.Second, Type.Millisecond, MillisecondsPerSecond);
        public static readonly TimeUnit Minute = new TimeUnit(Type.Minute, Type.Millisecond, MillisecondsPerMinute);
        public static readonly TimeUnit Hour = new TimeUnit(Type.Hour, Type.Millisecond, MillisecondsPerHour);
        public static readonly TimeUnit Day = new TimeUnit(Type.Day, Type.Millisecond, MillisecondsPerDay);
        public static readonly TimeUnit Week = new TimeUnit(Type.Week, Type.Millisecond, MillisecondsPerWeek);
        public static readonly TimeUnit[] DescendingMillisecondBased = { Week, Day, Hour, Minute, Second, Millisecond };
        public static readonly TimeUnit[] DescendingMillisecondBasedForDisplay = { Day, Hour, Minute, Second, Millisecond };
        public static readonly TimeUnit Month = new TimeUnit(Type.Month, Type.Month, 1);
        public static readonly TimeUnit Quarter = new TimeUnit(Type.Quarter, Type.Month, MonthsPerQuarter);
        public static readonly TimeUnit Year = new TimeUnit(Type.Year, Type.Month, MonthsPerYear);
        public static readonly TimeUnit[] DescendingMonthBased = { Year, Quarter, Month };
        public static readonly TimeUnit[] DescendingMonthBasedForDisplay = { Year, Month };

        private readonly Type _type;
        private readonly Type _baseType;
        private readonly int _factor;

        private TimeUnit(Type type, Type baseType, int factor) {
            _type = type;
            _baseType = baseType;
            _factor = factor;
        }

        public TimeUnit BaseUnit {
            get { return _baseType == Type.Millisecond ? Millisecond : Month; }
        }

        public TimeUnit[] DescendingUnits {
            get {
                return IsConvertibleToMilliseconds() ? DescendingMillisecondBased : DescendingMonthBased;
            }
        }

        public TimeUnit[] DescendingUnitsForDisplay {
            get {
                return IsConvertibleToMilliseconds() ? DescendingMillisecondBasedForDisplay : DescendingMonthBasedForDisplay;
            }
        }

        public TimeUnit NextFinerUnit() {
            TimeUnit[] descending = DescendingUnits;
            int index = Array.IndexOf(descending, this, 0, descending.Length - 1);
            return descending[index + 1];
        }

        public bool IsConvertibleToMilliseconds() {
            return IsConvertibleTo(Millisecond);
        }

        public bool IsConvertibleTo(TimeUnit other) {
            return _baseType.Equals(other._baseType);
        }

        public override string ToString() {
            return _type.ToString().ToLowerInvariant();
        }

        public string ToString(long quantity) {
            return string.Concat(quantity, ' ', this, quantity == 1 ? "" : "s");
        }

        #region Operators

        public static bool operator ==(TimeUnit x, TimeUnit y) {
            return x.Equals(y);
        }

        public static bool operator !=(TimeUnit x, TimeUnit y) {
            return !x.Equals(y);
        }

        public static bool Equals(TimeUnit x, TimeUnit y) {
            return x.Equals(y);
        }

        public static bool operator <(TimeUnit x, TimeUnit y) {
            return x.CompareTo(y) < 0;
        }

        public static bool operator >(TimeUnit x, TimeUnit y) {
            return x.CompareTo(y) > 0;
        }

        public static bool operator <=(TimeUnit x, TimeUnit y) {
            return x.CompareTo(y) <= 0;
        }

        public static bool operator >=(TimeUnit x, TimeUnit y) {
            return x.CompareTo(y) >= 0;
        }

        public static int Compare(TimeUnit x, TimeUnit y) {
            return x.CompareTo(y);
        }

        #endregion

        #region IComparable<TimeUnit> members

        public int CompareTo(TimeUnit other) {
            if (other._baseType.Equals(_baseType))
                return _factor.CompareTo(other._factor);
            if (_baseType.Equals(Type.Month))
                return 1;
            return -1;
        }

        #endregion

        #region IEquatable<TimeUnit> members

        public bool Equals(TimeUnit other) {
            return CompareTo(other) == 0;
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != GetType())
                return false;
            return Equals((TimeUnit)obj);
        }

        public override int GetHashCode() {
            return _factor + _baseType.GetHashCode() + _type.GetHashCode();
        }

        #endregion
    }
}
