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
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace WmcSoft.Units
{
    /// <summary>
    /// Represents part of a derived unit comprising a single unit and its power.
    /// </summary>
    [DebuggerDisplay("{Unit} ^ {_power}")]
    public class DerivedUnitTerm
    {
        #region Fields

        // "-  0123456789", as exponents
        const string sub = "\u207B  \u2070\u00B9\u00B2\u00B3\u2074\u2075\u2076\u2077\u2078\u2079";

        readonly Unit _unit;
        readonly int _power;
        readonly string _symbol;

        #endregion

        #region Lifecycle

        public DerivedUnitTerm(Unit unit, int power) {
            if (unit == null)
                throw new ArgumentNullException("unit");
            _unit = unit;
            _power = power;
            var symbol = _unit.Symbol ?? "?";
            if (_power != 1) {
                var sb = new StringBuilder(_unit.Symbol);
                foreach (char c in _power.ToString(CultureInfo.InvariantCulture)) {
                    sb.Append(sub[c - '-']);
                }
                _symbol = sb.ToString();
            } else {
                _symbol = _unit.Symbol;
            }
        }

        #endregion

        #region Properties

        public int Power {
            get { return _power; }
        }

        public Unit Unit {
            get { return _unit; }
        }

        public string Symbol {
            get { return _symbol; }
        }

        #endregion

        public override bool Equals(object obj) {
            if ((obj == null) || (GetType() != obj.GetType()))
                return false;
            DerivedUnitTerm that = (DerivedUnitTerm)obj;
            return (_power == that._power) && _unit.Equals(that._unit);
        }

        public override int GetHashCode() {
            return (_unit.GetHashCode() * 397) ^ _power;
        }
    }


}
