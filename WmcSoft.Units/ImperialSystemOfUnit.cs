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
    /// Description résumée de ImperialSystemOfUnit.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public sealed class ImperialSystemOfUnit : SystemOfUnits
    {
        static ImperialSystemOfUnit _systemOfUnits;

        static ImperialSystemOfUnit() {
            _systemOfUnits = new ImperialSystemOfUnit();
        }

        private ImperialSystemOfUnit()
            : base("Imperial", null) {
        }

        public static ImperialSystemOfUnit GetSystemOfUnit() {
            return _systemOfUnits;
        }

        static Unit[] units;
        static public Unit GetUnit(KnownImperialUnits unit) {
            if (units == null) {
                InitializeKnownUnits();
            }
            return units[(int)unit];
        }

        internal static bool IsInitialized {
            get {
                return (units != null);
            }
        }

        static private void InitializeKnownUnits() {
            lock (_systemOfUnits) {
                if (units == null) {
                    units = new Unit[1];
                    units[(int)KnownImperialUnits.Fahrenheit] = new KnownDerivedUnit("Fahrenheit", SystemOfUnits.Imperial, SI.Kelvin);

                    Conversion.CrossSystemConversions.RegisterImperialToSIConversions();
                    Conversion.CrossSystemConversions.RegisterUSCustomaryToSIConversions();
                    Conversion.CrossSystemConversions.RegisterNaturalToSIConversions();
                }
            }
        }

        #region units
        public static Unit Fahrenheit {
            get {
                return GetUnit(KnownImperialUnits.Fahrenheit);
            }
        }
        #endregion

    }

    public enum KnownImperialUnits
    {
        Fahrenheit,
    }
}
