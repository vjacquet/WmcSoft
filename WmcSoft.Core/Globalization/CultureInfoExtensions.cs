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

using System.Collections.Generic;
using System.Globalization;

namespace WmcSoft.Globalization
{
    public static class CultureInfoExtensions
    {
        #region ToList 

        public static string ToList(this CultureInfo culture, params string[] values)
        {
            return ToList(culture.TextInfo, values);
        }
        public static string ToList(this CultureInfo culture, IEnumerable<string> values)
        {
            return ToList(culture.TextInfo, values);
        }
        public static string ToList(this CultureInfo culture, params object[] values)
        {
            return ToList(culture.TextInfo, values);
        }
        public static string ToList<T>(this CultureInfo culture, IEnumerable<T> values)
        {
            return ToList(culture.TextInfo, values);
        }

        public static string ToList(this TextInfo textInfo, params string[] values)
        {
            var separator = textInfo.ListSeparator;
            return string.Join(separator, values);
        }
        public static string ToList(this TextInfo textInfo, IEnumerable<string> values)
        {
            var separator = textInfo.ListSeparator;
            return string.Join(separator, values);
        }
        public static string ToList(this TextInfo textInfo, params object[] values)
        {
            var separator = textInfo.ListSeparator;
            return string.Join(separator, values);
        }
        public static string ToList<T>(this TextInfo textInfo, IEnumerable<T> values)
        {
            var separator = textInfo.ListSeparator;
            return string.Join(separator, values);
        }

        #endregion
    }
}
