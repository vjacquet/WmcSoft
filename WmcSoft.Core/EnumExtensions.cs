using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft
{
    public static class EnumExtensions
    {
        public static bool IsValid(this Enum enumValue, int value, int minValue, int maxValue) {
            return value >= minValue && value <= maxValue;
        }

        public static bool IsValid<T>(this T enumValue, int minValue, int maxValue) where T : struct, IConvertible {
            var value = ((IConvertible)enumValue).ToInt32(CultureInfo.InvariantCulture);
            return value >= minValue && value <= maxValue;
        }
    }
}
