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
 * Adapted from TimeRate.java
 * --------------------------
 * Copyright (c) 2005 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System;
using static WmcSoft.Helpers;

namespace WmcSoft.Time
{
    public struct TimeRate : IEquatable<TimeRate>
    {
        private readonly decimal _quantity;
        private readonly Duration _unit;

        public TimeRate(decimal quantity, Duration unit) {
            _quantity = quantity;
            _unit = unit;
        }

        public override string ToString() {
            return base.ToString();
        }

        public decimal Over(Duration duration) {
            return Over(duration, RoundingMode.Unnecessary);
        }

        public decimal Over(Duration duration, RoundingMode rounding) {
            return Over(duration, Scale, rounding);
        }

        public decimal Over(Duration duration, int scale, RoundingMode rounding) {
            throw new NotImplementedException();
            //return duration.dividedBy(unit).times(quantity).decimalValue(scale, roundRule);
        }

        public int Scale {
            get {
                return _quantity.Scale();
            }
        }

        #region Operators

        public static bool operator ==(TimeRate x, TimeRate y) {
            return x.Equals(y);
        }

        public static bool operator !=(TimeRate x, TimeRate y) {
            return !x.Equals(y);
        }

        #endregion

        #region IEquatable<TimeRate> members

        public bool Equals(TimeRate other) {
            return _quantity == other._quantity && _unit == other._unit;
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != GetType())
                return false;
            return Equals((TimeRate)obj);
        }

        public override int GetHashCode() {
            return Hash(_quantity.GetHashCode(), _unit.GetHashCode());
        }

        #endregion
    }
}
