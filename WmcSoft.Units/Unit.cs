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

namespace WmcSoft.Units
{
    /// <summary>
    /// Represents a type of metric that is part of a SystemOfUnits
    /// </summary>
    public abstract class Unit : Metric
    {
        #region properties

        public abstract SystemOfUnits SystemOfUnits { get; }

        #endregion

        #region Equal overrides

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
                return SystemOfUnits == ((Unit)obj).SystemOfUnits;
            return false;
        }

        public override int GetHashCode()
        {
            int hash = base.GetHashCode();
            var systemOfUnits = SystemOfUnits;
            if (systemOfUnits != null)
                return hash ^ systemOfUnits.GetHashCode() << 6;
            return hash;
        }

        #endregion

        #region Sugar operators

        public static implicit operator DerivedUnitTerm(Unit u)
        {
            return new DerivedUnitTerm(u, 1);
        }
        public static DerivedUnitTerm operator ^(Unit u, int power)
        {
            return new DerivedUnitTerm(u, power);
        }

        #endregion
    }
}
