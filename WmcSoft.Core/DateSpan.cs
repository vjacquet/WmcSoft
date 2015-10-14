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

 ****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft
{
    public struct DateSpan : IComparable, IComparable<DateSpan>, IEquatable<DateSpan>
    {
        #region Fields

        const int MonthsInYear = 12;
        const int DaysInWeek = 7;

        readonly int _days;
        readonly int _months;

        private DateSpan(int days, int month, PiecewiseConstruct tag) {
            _days = days;
            _months = month;
        }

        public DateSpan(int years, int months, int days) {
            _days = days;
            _months = MonthsInYear * years + months;
        }

        public DateSpan(int weeks, int days) {
            _days = DaysInWeek * weeks + days;
            _months = 0;
        }

        #endregion

        #region Properties

        public int Days { get { return _days % DaysInWeek; } }
        public int Weeks { get { return _days / DaysInWeek; } }
        public int Months { get { return _months % MonthsInYear; } }
        public int Years { get { return _months / MonthsInYear; } }

        #endregion

        #region Operators

        public static DateSpan operator +(DateSpan x) {
            return x;
        }

        public static DateSpan operator -(DateSpan x) {
            return new DateSpan(-x.Days, -x.Months, PiecewiseConstruct.Tag);
        }
        public DateSpan Negate() {
            return -this;
        }

        public static DateSpan operator +(DateSpan x, DateSpan y) {
            return new DateSpan(x.Days + y.Days, x.Months + y.Months, PiecewiseConstruct.Tag);
        }
        public DateSpan Add(DateSpan x) {
            return this + x;
        }

        public static DateSpan operator -(DateSpan x, DateSpan y) {
            return new DateSpan(x.Days - y.Days, x.Months - y.Months, PiecewiseConstruct.Tag);
        }
        public DateSpan Substract(DateSpan x) {
            return this - x;
        }

        public static bool operator ==(DateSpan x, DateSpan y) {
            return x.Equals(y);
        }

        public static bool operator !=(DateSpan x, DateSpan y) {
            return !x.Equals(y);
        }

        public static bool Equals(DateSpan x, DateSpan y) {
            return x.Equals(y);
        }

        public static bool operator <(DateSpan x, DateSpan y) {
            return x.CompareTo(y) < 0;
        }

        public static bool operator >(DateSpan x, DateSpan y) {
            return x.CompareTo(y) > 0;
        }

        public static bool operator <=(DateSpan x, DateSpan y) {
            return x.CompareTo(y) <= 0;
        }

        public static bool operator >=(DateSpan x, DateSpan y) {
            return x.CompareTo(y) >= 0;
        }

        public static int Compare(DateSpan x, DateSpan y) {
            return x.CompareTo(y);
        }

        #endregion

        #region IComparable<DateSpan> members

        public int CompareTo(DateSpan other) {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj) {
            if (obj == null || obj.GetType() != GetType())
                return 1;
            return CompareTo((DateSpan)obj);
        }

        #endregion

        #region IEquatable<DateSpan> members

        public bool Equals(DateSpan other) {
            return _days == other._days && _months == other._months;
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != GetType())
                return false;
            return Equals((DateSpan)obj);
        }

        public override int GetHashCode() {
            return _months * 397 ^ _days;
        }

        #endregion
    }
}
