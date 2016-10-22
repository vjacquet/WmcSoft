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
using System.Collections.Generic;

namespace WmcSoft.Time
{
    [Serializable]
    public struct TimeUnit : IComparable<TimeUnit>, IEquatable<TimeUnit>
    {
        public const int MillisecondsPerSecond = 1000;
        public const int MillisecondsPerMinute = 60 * MillisecondsPerSecond;
        public const int MillisecondsPerHour = 60 * MillisecondsPerMinute;
        public const int MillisecondsPerDay = 24 * MillisecondsPerHour;
        public const int MillisecondsPerWeek = 7 * MillisecondsPerDay;
        public const int MonthsPerQuarter = 3;
        public const int MonthsPerYear = 12;

        public enum Type
        {
            Millisecond = 0x01,
            Second = 0x03,
            Minute = 0x05,
            Hour = 0x09,
            Day = 0x0B,
            Week = 0x0D,
            Month = 0x10,
            Quarter = 0x30,
            Year = 0x50,
        }

        public static readonly TimeUnit Millisecond = new TimeUnit(Type.Millisecond, 1);
        public static readonly TimeUnit Second = new TimeUnit(Type.Second, MillisecondsPerSecond);
        public static readonly TimeUnit Minute = new TimeUnit(Type.Minute, MillisecondsPerMinute);
        public static readonly TimeUnit Hour = new TimeUnit(Type.Hour, MillisecondsPerHour);
        public static readonly TimeUnit Day = new TimeUnit(Type.Day, MillisecondsPerDay);
        public static readonly TimeUnit Week = new TimeUnit(Type.Week, MillisecondsPerWeek);
        public static readonly TimeUnit Month = new TimeUnit(Type.Month, 1);
        public static readonly TimeUnit Quarter = new TimeUnit(Type.Quarter, MonthsPerQuarter);
        public static readonly TimeUnit Year = new TimeUnit(Type.Year, MonthsPerYear);

        public static readonly TimeUnit[] DescendingMillisecondBased = { Week, Day, Hour, Minute, Second, Millisecond };
        public static readonly TimeUnit[] DescendingMillisecondBasedForDisplay = { Day, Hour, Minute, Second, Millisecond };

        public static readonly TimeUnit[] DescendingMonthBased = { Year, Quarter, Month };
        public static readonly TimeUnit[] DescendingMonthBasedForDisplay = { Year, Month };

        private readonly Type _type;
        private readonly int _factor;

        private TimeUnit(Type type, int factor) {
            _type = type;
            _factor = factor;
        }

        private Type BaseType {
            get { return _type & (Type.Millisecond | Type.Month); }
        }
        public TimeUnit BaseUnit {
            get { return IsConvertibleToMilliseconds() ? Millisecond : Month; }
        }

        public int Factor { get { return _factor; } }

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

        public static TimeUnit NormalizeUnit(long quantity, TimeUnit unit) {
            var baseAmount = quantity * unit._factor;
            var units = unit.DescendingUnits;
            for (var i = 0; i < units.Length; i++) {
                var aUnit = units[i];
                var remainder = baseAmount % aUnit.Factor;
                if (remainder == 0)
                    return aUnit;
            }
            throw new InvalidOperationException();
        }

        static IEnumerable<Tuple<long, TimeUnit>> Decompose(long remainder, TimeUnit[] units) {
            for (int i = 0; i < units.Length; i++) {
                var aUnit = units[i];
                var factor = aUnit.Factor;
                long portion = Math.DivRem(remainder, aUnit.Factor, out remainder);
                if (portion > 0)
                    yield return Tuple.Create(portion, aUnit);
            }
        }

        public static IEnumerable<Tuple<long, TimeUnit>> Decompose(long quantity, TimeUnit unit) {
            return Decompose(quantity * unit._factor, unit.DescendingUnits);
        }

        public static IEnumerable<Tuple<long, TimeUnit>> DecomposeForDisplay(long quantity, TimeUnit unit) {
            return Decompose(quantity * unit._factor, unit.DescendingUnitsForDisplay);
        }

        public TimeUnit NextFinerUnit() {
            TimeUnit[] descending = DescendingUnits;
            int index = Array.IndexOf(descending, this, 0, descending.Length - 1);
            if (index == -1)
                return this;
            return descending[index + 1];
        }

        public bool IsConvertibleToMilliseconds() {
            return (_type & Type.Millisecond) != 0;
        }

        public bool IsConvertibleTo(TimeUnit other) {
            return BaseType == other.BaseType;
        }

        public override string ToString() {
            return _type.ToString().ToLowerInvariant();
        }

        internal string ToString(long quantity) {
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
            if (other.BaseType.Equals(BaseType))
                return _factor.CompareTo(other._factor);
            if (BaseType.Equals(Type.Month))
                return 1; // because the other is less than or equal to week
            return -1; // because the other is greater than of equal to Month
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
            // CHECK: is it a good hash function?
            return _factor + BaseType.GetHashCode() + _type.GetHashCode();
        }

        #endregion
    }
}