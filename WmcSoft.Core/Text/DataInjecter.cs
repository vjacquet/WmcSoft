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
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WmcSoft.Text
{
    public static class DataInjecter
    {
        static readonly Regex regex = new Regex(@"{(?<name>\w+)(\:(?<format>.*))?}", RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.Compiled);

        static string Format(IFormatProvider provider, Match m, object value)
        {
            string format = m.Groups["format"].Success
                ? "{0:" + m.Groups["format"].Value + "}"
                : "{0}";
            return String.Format(provider, format, value);
        }
        static string ReplaceValue(IFormatProvider provider, Match m, IDictionary<string, object> values)
        {
            string name = m.Groups["name"].Value;
            object value;
            if (values.TryGetValue(name, out value) && value != null) {
                return Format(provider, m, value);
            }
            return "";
        }

        //static string ReplaceHtmlEncodedValue(Match m, IDictionary<string, object> values) {
        //    string name = m.Groups["name"].Value;
        //    object value;
        //    if (values.TryGetValue(name, out value) && value != null) {
        //        var html = value as IHtmlString;
        //        if (html != null) {
        //            return html.ToHtmlString();
        //        }

        //        string format = m.Groups["format"].Success
        //            ? "{0:" + m.Groups["format"].Value + "}"
        //            : "{0}";
        //        return HttpUtility.HtmlEncode(String.Format(format, value));
        //    }
        //    return "";
        //}

        public static bool CanInject(string candidate, string format)
        {
            var m = regex.Match(format);
            while (m.Success) {
                string name = m.Groups["name"].Value;
                if (name == candidate)
                    return true;
                m = m.NextMatch();
            }
            return false;
        }

        public static string Inject(this IDictionary<string, object> values, IFormatProvider provider, string format)
        {
            return regex.Replace(format, m => ReplaceValue(provider, m, values));
        }
    }
}
