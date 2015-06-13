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
    //[System.Diagnostics.DebuggerStepThrough]
    public sealed class ImperialSystemOfUnit : SystemOfUnits
    {
        static readonly ImperialSystemOfUnit _systemOfUnits;
        static readonly Unit[] Units;

        static ImperialSystemOfUnit() {
            _systemOfUnits = new ImperialSystemOfUnit();

            var inch = new Inch();
            var foot = new ScaledUnit("foot", "ft", 12m, inch);
            var yard = new ScaledUnit("yard", "yd", 3m, foot);
            var chain = new ScaledUnit("chain", "ch", 22m, yard);
            var furlong = new ScaledUnit("furlong", "fur", 10m, chain);
            var mile = new ScaledUnit("mile", "mi", 8m, furlong);
            var league = new ScaledUnit("league", "lea", 3m, mile);

            Units = new Unit[] {
                       new KnownDerivedUnit("Fahrenheit", SystemOfUnits.Imperial, SI.Kelvin),
                       inch,
                       foot,
                       yard,
                       chain,
                       furlong,
                       mile,
                       league,
                    };

            Conversion.CrossSystemConversions.RegisterImperialToSIConversions();
            Conversion.CrossSystemConversions.RegisterUSCustomaryToSIConversions();
            Conversion.CrossSystemConversions.RegisterNaturalToSIConversions();
        }

        private ImperialSystemOfUnit()
            : base("Imperial", null) {
        }

        public static ImperialSystemOfUnit GetSystemOfUnit() {
            return _systemOfUnits;
        }

        static public Unit GetUnit(KnownImperialUnits unit) {
            return Units[(int)unit];
        }

        #region units

        public static Unit Fahrenheit {
            get { return GetUnit(KnownImperialUnits.Fahrenheit); }
        }

        public static Unit Inch {
            get { return GetUnit(KnownImperialUnits.Inch); }
        }

        public static Unit Foot {
            get { return GetUnit(KnownImperialUnits.Foot); }
        }

        public static Unit Yard {
            get { return GetUnit(KnownImperialUnits.Yard); }
        }

        public static Unit Chain {
            get { return GetUnit(KnownImperialUnits.Chain); }
        }

        public static Unit Furlong {
            get { return GetUnit(KnownImperialUnits.Furlong); }
        }

        public static Unit Mile {
            get { return GetUnit(KnownImperialUnits.Mile); }

        }

        public static Unit League {
            get { return GetUnit(KnownImperialUnits.League); }

        }

        #endregion
    }

    public enum KnownImperialUnits
    {
        Fahrenheit,
        Inch,
        Foot,
        Yard,
        Chain,
        Furlong,
        Mile,
        League,
    }


    public sealed class Inch : Unit
    {
        public Inch() {
        }

        public override string Name {
            get { return "Inch"; }
        }

        public override string Symbol {
            get { return "in"; }
        }

        public override string Definition {
            get { return "1/12 of a foot"; }
        }

        public override SystemOfUnits SystemOfUnits {
            get { return SystemOfUnits.Imperial; }
        }
    }
}
