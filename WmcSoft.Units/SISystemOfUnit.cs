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
    /// Description r�sum�e de SI.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class SI : SystemOfUnits
    {
        static SI() {
            si = new SI();
        }
        static SI si;

        private SI()
            : base("SI", "BIPM") {
        }

        public static SI GetSystemOfUnit() {
            return si;
        }

        static Unit[] units;
        static public Unit GetUnit(KnownSIUnits unit) {
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
            lock (si) {
                if (units == null) {
                    units = new Unit[8];
                    units[(int)KnownSIUnits.Meter] = meter;
                    units[(int)KnownSIUnits.Kilogram] = kilogram;
                    units[(int)KnownSIUnits.Second] = second;
                    units[(int)KnownSIUnits.Ampere] = ampere;
                    units[(int)KnownSIUnits.Kelvin] = kelvin;
                    units[(int)KnownSIUnits.Mole] = mole;
                    units[(int)KnownSIUnits.Candela] = candela;
                    units[(int)KnownSIUnits.Celsius] = new KnownDerivedUnit("Celsius", SystemOfUnits.SI, SI.Kelvin);

                    UnitConverter.RegisterConversion(new Conversion.CelsiusToKelvinConversion());

                    Conversion.CrossSystemConversions.RegisterImperialToSIConversions();
                    Conversion.CrossSystemConversions.RegisterUSCustomaryToSIConversions();
                    Conversion.CrossSystemConversions.RegisterNaturalToSIConversions();
                }
            }
        }

        #region standard units
        public static Meter Meter {
            get {
                return meter;
            }
        }
        static Meter meter = new Meter();

        public static Kilogram Kilogram {
            get {
                return kilogram;
            }
        }
        static Kilogram kilogram = new Kilogram();

        public static Second Second {
            get {
                return second;
            }
        }
        static Second second = new Second();

        public static Ampere Ampere {
            get {
                return ampere;
            }
        }
        static Ampere ampere = new Ampere();

        public static Kelvin Kelvin {
            get {
                return kelvin;
            }
        }
        static Kelvin kelvin = new Kelvin();

        public static Mole Mole {
            get {
                return mole;
            }
        }
        static Mole mole = new Mole();

        public static Candela Candela {
            get {
                return candela;
            }
        }
        static Candela candela = new Candela();

        public static Unit Celsius {
            get {
                return GetUnit(KnownSIUnits.Celsius);
            }
        }
        #endregion
    }

    public enum KnownSIUnits
    {
        Meter,
        Kilogram,
        Second,
        Ampere,
        Kelvin,
        Mole,
        Candela,
        Celsius,
    }
}