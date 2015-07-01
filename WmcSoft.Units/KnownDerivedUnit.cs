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
    /// Description résumée de KnowUnit.
    /// </summary>
    internal sealed class KnownDerivedUnit : DerivedUnit
    {
        public KnownDerivedUnit(string name, SystemOfUnits systemOfUnits, params DerivedUnitTerm[] terms)
            : base(name, systemOfUnits, terms) {
        }

        public override string Name {
            get {
                if (_localizedName == null) {
                    _localizedName = RM.GetName(name);
                }
                return _localizedName;
            }
        }
        string _localizedName;

        public override string Definition {
            get {
                if (_localizedDefinition == null) {
                    _localizedDefinition = RM.GetDefinition(name);
                }
                return _localizedDefinition;
            }
        }
        string _localizedDefinition;

        public override string Symbol {
            get {
                if (_localizedSymbol == null) {
                    _localizedSymbol = RM.GetSymbol(name);
                }
                return _localizedSymbol;
            }
        }
        string _localizedSymbol;

    }
}
