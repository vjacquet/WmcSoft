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
using System.Text.RegularExpressions;

namespace WmcSoft.Text.RegularExpressions
{
    public static class RegexExtensions
    {
        public static string GetGroupValue(this Match match, string groupName) {
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
