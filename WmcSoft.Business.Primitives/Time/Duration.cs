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
 * Adapted from Duration.java
 * --------------------------
 * Copyright (c) 2005 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System;
using System.Diagnostics;
using System.Linq;

namespace WmcSoft.Time
{
    /// <summary>
    /// Represents a duration.
    /// </summary>
    [Serializable]
    public struct Duration : IComparable<Duration>, IEquatable<Duration>
    {
        public static readonly Duration None;

        // TODO: Shouldn't the quantity be always in ms or months? Otherwise equal values won't overflow at the same time.
        // TODO: Implement Parse & ToString using ISO 8601 <https://en.wikipedia.org/wiki/ISO_8601#Durations>
        // TODO: Should Ratio be public?
        // NOTE: addedTo and substractFrom are implemented as operators in Timepoint.

        private readonly long _quantity;
        private readonly TimeUnit _unit;

        public Duration(long quantity, TimeUnit unit)
        {
            if (quantity < 0L) throw new ArgumentOutOfRangeException(nameof(quantity));

            _quantity = quantity;
            _unit = unit;
        }

        public Duration(int days, int hours, int minutes, int seconds, long milliseconds)
        {
            if (milliseconds < 0) throw new ArgumentOutOfRangeException(nameof(milliseconds));
            if (seconds < 0) throw new ArgumentOutOfRangeException(nameof(seconds));
            if (minutes < 0) throw new ArgumentOutOfRangeException(nameof(minutes));
            if (hours < 0) throw new ArgumentOutOfRangeException(nameof(hours));
            if (days < 0) throw new ArgumentOutOfRangeException(nameof(days));

            _quantity = milliseconds;
            _unit = TimeUnit.Millisecond;

            if (seconds > 0) {
                if (_quantity == 0) {
                    _unit = TimeUnit.Second;
                    _quantity = seconds;
                } else {
                    _quantity += seconds * TimeUnit.MillisecondsPerSecond / _unit.Factor;
                }
            }
            if (minutes > 0) {
                if (_quantity == 0) {
                    _unit = TimeUnit.Minute;
                    _quantity = minutes;
                } else {
                    _quantity += minutes * TimeUnit.MillisecondsPerMinute / _unit.Factor;
                }
            }
            if (hours > 0) {
                if (_quantity == 0) {
                    _unit = TimeUnit.Hour;
                    _quantity = hours;
                } else {
                    _quantity += hours * TimeUnit.MillisecondsPerHour / _unit.Factor;
                }
            }
            if (days > 0) {
                if (_quantity == 0) {
                    _unit = TimeUnit.Day;
                    _quantity = days;
                } else {
                    _quantity += days * TimeUnit.MillisecondsPerDay / _unit.Factor;
                }
            }
        }

        public TimeUnit Unit => _unit;
        public TimeUnit BaseUnit => _unit.BaseUnit;

        public static Duration Milliseconds(long howMany)
        {
            return new Duration(howMany, TimeUnit.Millisecond);
        }

        public static Duration Seconds(int howMany)
        {
            return new Duration(howMany, TimeUnit.Second);
        }

        public static Duration Minutes(int howMany)
        {
            return new Duration(howMany, TimeUnit.Minute);
        }

        public static Duration Hours(int howMany)
        {
            return new Duration(howMany, TimeUnit.Hour);
        }

        public static Duration Days(int howMany)
        {
            return new Duration(howMany, TimeUnit.Day);
        }

        public static Duration Weeks(int howMany)
        {
            return new Duration(howMany, TimeUnit.Week);
        }

        public static Duration Months(int howMany)
        {
            return new Duration(howMany, TimeUnit.Month);
        }

        public static Duration Quarters(int howMany)
        {
            return new Duration(howMany, TimeUnit.Quarter);
        }

        public static Duration Years(int howMany)
        {
            return new Duration(howMany, TimeUnit.Year);
        }

        public static Duration Of(int howMany, TimeUnit unit)
        {
            return new Duration(howMany, unit);
        }

        public long InBaseUnits()
        {
            return _quantity * _unit.Factor;
        }

        public static Duration Between(Timepoint x, Timepoint y)
        {
            return y - x;
        }

        public static Duration Between(Date x, Date y)
        {
            return BetweenDates(x, y);
        }

        static Duration BetweenDates(DateTime x, DateTime y)
        {
            if (x.Day == y.Day) {
                var years = y.Year - x.Year;
                var months = y.Month - x.Month;
                return Months(years * 12 + months);
            } else {
                var timeSpan = y - x;
                return Days((int)timeSpan.TotalDays);
            }
        }

        #region Operators

        public static implicit operator Duration(TimeSpan x)
        {
            return new Duration(x.Days, x.Hours, x.Minutes, x.Seconds, x.Milliseconds);
        }

        public static implicit operator Duration(Interval<Date> interval)
        {
            return BetweenDates(interval.Lower.Value, interval.Upper.Value);
        }

        public static explicit operator TimeSpan(Duration x)
        {
            if (x.BaseUnit != TimeUnit.Millisecond) throw new InvalidCastException();

            var ticks = x.InBaseUnits() * TimeSpan.TicksPerMillisecond;
            return new TimeSpan(ticks);
        }

        public static Duration operator +(Duration x, Duration y)
        {
            if (!x._unit.HasSameBaseTypes(y._unit)) throw new ArgumentException();

            var quantity = x.InBaseUnits() + y.InBaseUnits();
            return new Duration(quantity, x._unit.BaseUnit);
        }
        public static Duration Add(Duration x, Duration y)
        {
            return x + y;
        }

        public static Duration operator -(Duration x, Duration y)
        {
            if (!x._unit.HasSameBaseTypes(y._unit)) throw new ArgumentException();

            var quantity = x.InBaseUnits() - y.InBaseUnits();
            if (quantity < 0) throw new OverflowException();

            return new Duration(quantity, x._unit.BaseUnit);

            //var comparison = x._unit.CompareTo(y._unit);
            //if (comparison < 0) {
            //    var quantity = x._quantity + (y.InBaseUnits() / x._unit.Factor);
            //    return new Duration(quantity, x._unit);
            //} else if (comparison > 0) {
            //    var quantity = (x.InBaseUnits() / y._unit.Factor) + y._quantity;
            //    return new Duration(quantity, y._unit);
            //}
            //return new Duration(x._quantity + y._quantity, y._unit);
        }
        public static Duration Subtract(Duration x, Duration y)
        {
            return x - y;
        }

        public static bool operator ==(Duration x, Duration y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Duration x, Duration y)
        {
            return !x.Equals(y);
        }

        public static bool Equals(Duration x, Duration y)
        {
            return x.Equals(y);
        }

        public static bool operator <(Duration x, Duration y)
        {
            return x.CompareTo(y) < 0;
        }

        public static bool operator >(Duration x, Duration y)
        {
            return x.CompareTo(y) > 0;
        }

        public static bool operator <=(Duration x, Duration y)
        {
            return x.CompareTo(y) <= 0;
        }

        public static bool operator >=(Duration x, Duration y)
        {
            return x.CompareTo(y) >= 0;
        }

        public static int Compare(Duration x, Duration y)
        {
            return x.CompareTo(y);
        }

        internal Ratio DividedBy(Duration divisor)
        {
            Debug.Assert(_unit.HasSameBaseTypes(divisor._unit));

            return new Ratio(InBaseUnits(), divisor.InBaseUnits());
        }

        //public static Ratio operator /(Duration x, Duration y) {
        //    return x.DividedBy(y);
        //}

        #endregion

        #region IComparable<Duration> members

        /// <summary>
        /// Compares two durations and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="other">The other duration.</param>
        /// <returns>A signed integer value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <list type="table">
        /// <listheader>
        ///   <description>Value</description>
        ///   <description>Meaning</description>
        /// </listheader>
        /// <item>
        ///   <description>Less than zero</description>
        ///   <description> This instance precedes <paramref name="other"/> in the sort order.</description>
        /// </item>
        /// <item>
        ///   <description>Zero</description>
        ///   <description>This instance occurs in the same position in the sort order as <paramref name="other"/>.</description>
        /// </item>
        /// <item>
        ///   <description>Greater than zero</description>
        ///   <description>This instance follows <paramref name="other"/> in the sort order.</description>
        /// </item>
        /// </list>
        /// </returns>
        /// <remarks><see cref="Duration"/>s in milliseconds are always less than <see cref="Duration"/>s in months.</remarks>
        public int CompareTo(Duration other)
        {
            if (_unit.BaseUnit == other.BaseUnit) {
                return InBaseUnits().CompareTo(other.InBaseUnits());
            }
            return (_unit.BaseUnit == TimeUnit.Month) ? 1 : -1;
        }

        #endregion

        #region IEquatable<Duration> members

        public bool Equals(Duration other)
        {
            return CompareTo(other) == 0;
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;
            return Equals((Duration)obj);
        }

        public override int GetHashCode()
        {
            return _quantity.GetHashCode();
        }

        public override string ToString()
        {
            return string.Join(", ", TimeUnit.DecomposeForDisplay(_quantity, _unit).Select(p => p.Item2.ToString(p.Item1)));
        }

        #endregion
    }
}
