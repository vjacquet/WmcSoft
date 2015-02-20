using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WmcSoft.Text.RegularExpressions
{
    public static class RegexExtensions
    {
        public static string GetGroupValue(this Match match, string groupName, IFormatProvider provider = null) {
            var value = match.Groups[groupName];
            if (value.Success)
                return value.Value;
            return null;
        }

        public static T? GetGroupValue<T>(this Match match, string groupName, IFormatProvider provider = null) where T : struct {
            var value = match.Groups[groupName];
            if (value.Success)
                return (T)Convert.ChangeType(value.Value, typeof(T), provider);
            return null;
        }

        public static bool TryMatch(this Regex regex, string input, out Match match) {
            match = regex.Match(input);
            return match.Success;
        }
    }
}
