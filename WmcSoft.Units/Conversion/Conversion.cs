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

namespace WmcSoft.Units.Conversion
{
    /// <summary>
    /// Description résumée de CommonConversion.
    /// </summary>
    public class FahrenheitToCelsiusConversion : UnitConversion
    {
        public FahrenheitToCelsiusConversion()
            : base(ImperialSystemOfUnit.Fahrenheit, SI.Celsius) {
        }

        public override decimal Convert(decimal value) {
            return 5m * (value - 32m) / 9m;
        }

        public override decimal ConvertBack(decimal value) {
            return (9m * value / 5m) + 32m;
        }
    }


    public class FahrenheitToKelvinConversion : UnitConversion
    {
        public FahrenheitToKelvinConversion()
            : base(ImperialSystemOfUnit.Fahrenheit, SI.Kelvin) {
        }

        public override decimal Convert(decimal value) {
            return 5m * (value - 32m) / 9m;
        }

        public override decimal ConvertBack(decimal value) {
            return (9m * value / 5m) + 32m;
        }
    }

    public class CelsiusToKelvinConversion : UnitConversion
    {
        public CelsiusToKelvinConversion()
            : base(SI.Celsius, SI.Kelvin) {
        }

        public override decimal Convert(decimal value) {
            return value + 273.15m;
        }

        public override decimal ConvertBack(decimal value) {
            return value - 273.15m;
        }
    }
}
