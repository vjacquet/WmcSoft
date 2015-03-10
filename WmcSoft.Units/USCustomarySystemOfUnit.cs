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
    /// Description résumée de USCustomarySystemOfUnit.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class USCustomarySystemOfUnit : SystemOfUnits
    {
        static USCustomarySystemOfUnit systemOfUnits;

        static USCustomarySystemOfUnit() {
            systemOfUnits = new USCustomarySystemOfUnit();
        }

        private USCustomarySystemOfUnit()
            : base("US Customary", null) {
        }

        public static USCustomarySystemOfUnit GetSystemOfUnit() {
            return systemOfUnits;
        }

        static Unit[] units;
        static public Unit GetUnit(KnownUSCustomaryUnits unit) {
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

        static void InitializeKnownUnits() {
            lock (systemOfUnits) {
                if (units == null) {
                    units = new Unit[1];
                    units[(int)KnownUSCustomaryUnits.Fahrenheit] = new KnownDerivedUnit("Fahrenheit", SystemOfUnits.USCustomary, SI.Kelvin);

                    Conversion.CrossSystemConversions.RegisterImperialToSIConversions();
                    Conversion.CrossSystemConversions.RegisterUSCustomaryToSIConversions();
                    Conversion.CrossSystemConversions.RegisterNaturalToSIConversions();
                }
            }
        }

        #region units
        #endregion
    }

    public enum KnownUSCustomaryUnits
    {
        Fahrenheit,
    }
}
