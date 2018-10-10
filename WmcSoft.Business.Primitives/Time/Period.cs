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
 * Adapted from TimeInterval.java
 * ------------------------------
 * Copyright (c) 2004 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System;

namespace WmcSoft.Time
{
    public sealed class Period : IComparable<Period>, IEquatable<Period>
    {
        public Period(int length = 0)
        {
            Length = length;
            Units = TimeUnit.Day;
        }

        public Period(int length, TimeUnit units)
        {
            Length = length;
            Units = units;
        }

        public Period(Frequency frequency)
        {
            switch (frequency) {
            case Frequency.NoFrequency:
                Length = 0;
                Units = TimeUnit.Day;
                break;
            case Frequency.Once:
                Length = 0;
                Units = TimeUnit.Year;
                break;
            case Frequency.Annual:
                Length = 1;
                Units = TimeUnit.Year;
                break;
            case Frequency.Semiannual:
            case Frequency.EveryFourthMonth:
            case Frequency.Quarterly:
            case Frequency.Bimonthly:
            case Frequency.Monthly:
                Length = 12 / (int)frequency;
                Units = TimeUnit.Month;
                break;
            case Frequency.EveryFourthWeek:
            case Frequency.Biweekly:
            case Frequency.Weekly:
                Length = 52 / (int)frequency;
                Units = TimeUnit.Week;
                break;
            case Frequency.Daily:
                Length = 1;
                Units = TimeUnit.Day;
                break;
            case Frequency.OtherFrequency:
            default:
                throw new ArgumentException();
            }
        }

        public int Length { get; }
        public TimeUnit Units { get; }
        public Frequency Frequency => Units.ToFrequency(Length);

        public double Years => throw new NotImplementedException();
        public double Months => throw new NotImplementedException();
        public double Weeks => throw new NotImplementedException();
        public double Days => throw new NotImplementedException();

        public Period Normalize()
        {
            throw new NotImplementedException();
            //if (Length == 0)
            //    return this;
            //var length = Length;
            //var units = Units;
            //switch (units._type) {
            //case TimeUnit.Type.Day:
            //    break;
            //case TimeUnit.Type.Week:
            //    break;
            //case TimeUnit.Type.Month:
            //    break;
            //case TimeUnit.Type.Quarter:
            //    length *= 3;
            //    units = TimeUnit.Month;
            //    break;
            //case TimeUnit.Type.Year:
            //    break;
            //default:
            //    break;
            //}
            //return new Period(length, units);
        }

        #region Operators

        public static explicit operator Period(Frequency frequency)
        {
            return new Period(frequency);
        }

        public static Period operator +(Period x)
        {
            return x;
        }

        public static Period operator -(Period x)
        {
            return new Period(-x.Length, x.Units);
        }

        public static Period operator +(Period x, Period y)
        {
            throw new NotImplementedException();
        }

        public static Period operator -(Period x, Period y)
        {
            throw new NotImplementedException();
        }

        public static Period operator *(Period x, int n)
        {
            return new Period(x.Length * n, x.Units);
        }

        public static Period operator *(int n, Period x)
        {
            return new Period(x.Length * n, x.Units);
        }

        public static Period operator /(Period x, int n)
        {
            if (n == 0) throw new DivideByZeroException();

            throw new NotImplementedException();
        }

        public static bool operator ==(Period x, Period y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Period x, Period y)
        {
            return !x.Equals(y);
        }

        public static bool Equals(Period x, Period y)
        {
            return x.Equals(y);
        }

        public static bool operator <(Period x, Period y)
        {
            return x.CompareTo(y) < 0;
        }

        public static bool operator >(Period x, Period y)
        {
            return x.CompareTo(y) > 0;
        }

        public static bool operator <=(Period x, Period y)
        {
            return x.CompareTo(y) <= 0;
        }

        public static bool operator >=(Period x, Period y)
        {
            return x.CompareTo(y) >= 0;
        }

        public static int Compare(Period x, Period y)
        {
            return x.CompareTo(y);
        }

        #endregion

        #region IComparable<Period> members

        public int CompareTo(Period other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return 1;
            return CompareTo((Date)obj);
        }

        #endregion

        #region IEquatable<Period> members

        public bool Equals(Period other)
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
            throw new NotImplementedException();
        }

        #endregion
    }
}
