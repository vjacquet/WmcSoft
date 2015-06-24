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
using System.Resources;

namespace WmcSoft.Units
{
    /// <summary>
    /// Description résumée de SIBaseUnit.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class SIBaseUnit : Unit
    {
        readonly string _symbol;

        internal SIBaseUnit() {
            _symbol = RM.GetSymbol(GetType().Name);
        }

        public override bool Equals(object obj) {
            if (obj == null)
                return false;
            return GetType() == obj.GetType();
        }

        public override int GetHashCode() {
            return GetType().GetHashCode();
        }

        public override SystemOfUnits SystemOfUnits {
            get { return SystemOfUnits.SI; }
        }

        public override string Name {
            get { return RM.GetName(GetType().Name); }
        }

        public override string Symbol {
            get { return _symbol; }
        }

        public override string Definition {
            get { return RM.GetDefinition(GetType().Name); }
        }

        public virtual Unit WithPrefix(SIPrefix prefix) {
            if (prefix == SIPrefix.None)
                return this;
            return new SIPrefixedScaledUnit(prefix, this);
        }
    }

    #region SIBaseUnit types

    public sealed class Meter : SIBaseUnit
    {
        public Meter() {
        }
    }

    public sealed class Kilogram : SIBaseUnit
    {
        public Kilogram() {
        }

        public override Unit WithPrefix(SIPrefix prefix) {
            if (prefix == SIPrefix.Kilo)
                return this;
            return new SIPrefixedScaledUnit(prefix, this);
        }
    }

    public sealed class Second : SIBaseUnit
    {
        public Second() {
        }
    }

    public sealed class Ampere : SIBaseUnit
    {
        public Ampere() {
        }
    }
    public sealed class Kelvin : SIBaseUnit
    {
        public Kelvin() {
        }
    }
    public sealed class Mole : SIBaseUnit
    {
        public Mole() {
        }
    }
    public sealed class Candela : SIBaseUnit
    {
        public Candela() {
        }
    }

    #endregion
}
