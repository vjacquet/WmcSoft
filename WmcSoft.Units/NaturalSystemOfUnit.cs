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
    /// Description résumée de NaturalSystemOfUnit.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class NaturalSystemOfUnit : SystemOfUnits
    {
        static NaturalSystemOfUnit _systemOfUnits;

        static NaturalSystemOfUnit() {
            _systemOfUnits = new NaturalSystemOfUnit();
        }

        private NaturalSystemOfUnit()
            : base("Natural", null) {
        }

        public static NaturalSystemOfUnit GetSystemOfUnit() {
            return _systemOfUnits;
        }

        static Unit[] _units;
        static public Unit GetUnit(KnownNaturalUnits unit) {
            if (_units == null) {
                InitializeKnownUnits();
            }
            return _units[(int)unit];
        }

        internal static bool IsInitialized {
            get {
                return (_units != null);
            }
        }

        static void InitializeKnownUnits() {
            lock (_systemOfUnits) {
                if (_units == null) {
                    _units = new Unit[5];
                    _units[(int)KnownNaturalUnits.PlanckTime] = new KnownDerivedUnit("PlanckTime", SystemOfUnits.Natural, SI.Second);
                    _units[(int)KnownNaturalUnits.PlanckLength] = new KnownDerivedUnit("PlanckLength", SystemOfUnits.Natural, SI.Meter);
                    _units[(int)KnownNaturalUnits.PlanckMass] = new KnownDerivedUnit("PlanckMass", SystemOfUnits.Natural, SI.Kilogram);
                    _units[(int)KnownNaturalUnits.PlanckCharge] = new KnownDerivedUnit("PlanckCharge", SystemOfUnits.Natural, SI.Ampere, SI.Second);
                    _units[(int)KnownNaturalUnits.PlanckTemperature] = new KnownDerivedUnit("PlanckTemperature", SystemOfUnits.Natural, SI.Kelvin);

                    Conversion.CrossSystemConversions.RegisterImperialToSIConversions();
                    Conversion.CrossSystemConversions.RegisterUSCustomaryToSIConversions();
                    Conversion.CrossSystemConversions.RegisterNaturalToSIConversions();
                }
            }
        }

        #region units
        public static Unit PlanckTime {
            get { return GetUnit(KnownNaturalUnits.PlanckTime); }
        }

        public static Unit PlanckLength {
            get { return GetUnit(KnownNaturalUnits.PlanckLength); }
        }

        public static Unit PlanckMass {
            get { return GetUnit(KnownNaturalUnits.PlanckMass); }
        }

        public static Unit PlanckCharge {
            get { return GetUnit(KnownNaturalUnits.PlanckCharge); }
        }

        public static Unit PlanckTemperature {
            get { return GetUnit(KnownNaturalUnits.PlanckTemperature); }
        }

        #endregion
    }

    public enum KnownNaturalUnits
    {
        PlanckTime,
        PlanckLength,
        PlanckMass,
        PlanckCharge,
        PlanckTemperature,
    }
}
