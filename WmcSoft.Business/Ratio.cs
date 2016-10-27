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
 * Adapted from Ratio.java
 * -----------------------
 * Copyright (c) 2004 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System;

namespace WmcSoft
{
    /// <summary>
    /// Ratio represents the unitless division of two quantities of the same type.
    /// </summary>
    /// <remarks>
    /// The key to its usefulness is that it defers the calculation of a decimal
    /// value for the ratio.An object which has responsibility for the two values in
    /// the ratio and understands their quantities can create the ratio, which can
    /// then be used by any client in a unitless form, so that the client is not
    /// required to understand the units of the quantity.At the same time, this
    /// gives control of the precision and rounding rules to the client, when the
    /// time comes to compute a decimal value for the ratio. The client typically has
    /// the responsibilities that enable an appropriate choice of these parameters.
    /// </remarks>
    [Serializable]
    internal struct Ratio : IEquatable<Ratio>
    {
        #region Constants

        public static Ratio Zero = new Ratio(0m);

        #endregion

        #region Fields

        private readonly decimal _numerator;
        private readonly decimal _denominator;

        #endregion

        #region Lifecycle

        internal Ratio(decimal numerator) {
            _numerator = numerator;
            _denominator = 1m;
        }

        public Ratio(decimal numerator, decimal denominator) {
            if (denominator == 0m) {
                throw new DivideByZeroException();
            } else if (denominator > 0) {
                _numerator = numerator;
                _denominator = denominator;
            } else {
                _numerator = -numerator;
                _denominator = -denominator;
            }
        }

        #endregion

        public decimal GetDecimalValue(int scale, RoundingMode roundingRule) {
            var value = _numerator / _denominator;
            return value.Round(scale, roundingRule);
        }

        public Ratio MultipliedBy(decimal multiplicand) {
            return new Ratio(_numerator * multiplicand, _denominator);
        }

        #region IEquatable<Ratio> Membres

        public bool Equals(Ratio other) {
            return _numerator == other._numerator && _denominator == other._denominator;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return Equals((Ratio)obj);
        }

        public override int GetHashCode() {
            return _numerator.GetHashCode() ^ _denominator.GetHashCode();
        }

        #endregion

        #region Overrides

        public override string ToString() {
            if (_denominator == 1m || _numerator == 0m)
                return _numerator.ToString();
            return _numerator.ToString() + '/' + _denominator.ToString();
        }

        #endregion
    }
}
