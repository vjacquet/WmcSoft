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
    /// Description résumée de SI.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class SI : SystemOfUnits
    {
        static readonly SI si;
        static readonly Unit[] Units;

        static SI() {
            si = new SI();

            Units = new Unit[] {
                meter = new Meter(),
                kilogram= new Kilogram(),
                second=new Second(),
                ampere=new Ampere(),
                kelvin=new Kelvin(),
                mole= new Mole(),
                candela= new Candela(),
                new KnownDerivedUnit("Celsius", SystemOfUnits.International, SI.Kelvin),
            };
            UnitConverter.RegisterConversion(new Conversion.CelsiusToKelvinConversion());

            Conversion.CrossSystemConversions.RegisterImperialToSIConversions();
            Conversion.CrossSystemConversions.RegisterUSCustomaryToSIConversions();
            Conversion.CrossSystemConversions.RegisterNaturalToSIConversions();
        }

        private SI()
            : base("SI", "BIPM") {
        }

        public static SI GetSystemOfUnit() {
            return si;
        }

        static public Unit GetUnit(KnownSIUnits unit) {
            return Units[(int)unit];
        }

        #region standard units

        public static Meter Meter {
            get { return meter; }
        }
        static readonly Meter meter;

        public static Kilogram Kilogram {
            get { return kilogram; }
        }
        static readonly Kilogram kilogram;

        public static Second Second {
            get { return second; }
        }
        static readonly Second second;

        public static Ampere Ampere {
            get { return ampere; }
        }
        static readonly Ampere ampere;

        public static Kelvin Kelvin {
            get { return kelvin; }
        }
        static readonly Kelvin kelvin;

        public static Mole Mole {
            get { return mole; }
        }
        static readonly Mole mole;

        public static Candela Candela {
            get { return candela; }
        }
        static Candela candela;

        public static Unit Celsius {
            get { return GetUnit(KnownSIUnits.Celsius); }
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
