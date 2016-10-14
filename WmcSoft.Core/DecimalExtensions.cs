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

namespace WmcSoft
{
    public static class DecimalExtensions
    {
        private static readonly decimal[] Powers;
        static DecimalExtensions() {
            const int MaxPower = 29; // last power before overflow

            Powers = new decimal[MaxPower];
            Powers[0] = 1m;

            var n = 1m;
            for (int i = 1; i < MaxPower; i++) {
                n *= 10m;
                Powers[i] = n;
            }
        }

        /// <summary>
        /// Returns the number of digits in the unscaled value.
        /// </summary>
        /// <param name="value">The decimal value.</param>
        /// <returns>Returns the <paramref name="value"/>'s precision.</returns>
        public static int Precision(this decimal value) {
            if (value < 0m)
                value = -value;
            var scale = Scale(value);
            value *= Powers[scale];
            var found = Array.BinarySearch(Powers, 1, Powers.Length - 1, value);
            if (found >= 0)
                return found + 1;
            return ~found;
        }

        /// <summary>
        /// Returns the number of digits to the right of the decimal point.
        /// </summary>
        /// <param name="value">The decimal value.</param>
        /// <returns>Returns the <paramref name="value"/>'s scale.</returns>
        public static int Scale(this decimal value) {
            // Scale mask for the flags field. This byte in the flags field contains
            // the power of 10 to divide the Decimal value by. The scale byte must
            // contain a value between 0 and 28 inclusive.
            const int ScaleMask = 0x00FF0000;

            // Number of bits scale is shifted by.
            const int ScaleShift = 16;

            var bits = decimal.GetBits(value);
            return (bits[3] & ScaleMask) >> ScaleShift;
        }

        public static decimal Round(this decimal value, RoundingMode rounding) {
            switch (rounding) {
            case RoundingMode.Ceiling:
                return decimal.Ceiling(value);
            case RoundingMode.Down:
                return decimal.Truncate(value);
            case RoundingMode.Floor:
                return decimal.Floor(value);
            case RoundingMode.HalfDown:
                return (value > 0m) ? decimal.Ceiling(value-0.5m) : -decimal.Ceiling(-value - 0.5m);
            case RoundingMode.HalfEven:
                return decimal.Round(value, MidpointRounding.ToEven);
            case RoundingMode.HalfUp:
                return decimal.Round(value, MidpointRounding.AwayFromZero);
            case RoundingMode.Unnecessary:
                var truncate = decimal.Truncate(value);
                if (value != truncate) throw new OverflowException();
                return truncate;
            case RoundingMode.Up:
                return (value > 0m) ? decimal.Ceiling(value) : decimal.Floor(value);
            }
            throw new NotSupportedException();
        }
    }

    public enum RoundingMode
    {
        /// <summary>Rounding mode to round towards positive infinity.</summary>
        Ceiling,
        /// <summary>Rounding mode to round towards zero.</summary>
        Down,
        /// <summary>Rounding mode to round towards negative infinity.</summary>
        Floor,
        /// <summary>Rounding mode to round towards "nearest neighbor" unless both neighbors are equidistant, in which case round down.</summary>
        HalfDown, // 
        /// <summary>Rounding mode to round towards the "nearest neighbor" unless both neighbors are equidistant, in which case, round towards the even neighbor.</summary>
        HalfEven,
        /// <summary>Rounding mode to round towards "nearest neighbor" unless both neighbors are equidistant, in which case round up.</summary>
        HalfUp,
        /// <summary>Rounding mode to assert that the requested operation has an exact result, hence no rounding is necessary.</summary>
        Unnecessary,
        /// <summary>Rounding mode to round away from zero.</summary>
        Up, // 
    }

}
