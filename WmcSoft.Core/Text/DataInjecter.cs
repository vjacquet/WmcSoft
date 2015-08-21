using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WmcSoft.Text
{
    public static class DataInjecter
    {
        static readonly Regex regex = new Regex(@"{(?<name>\w+)(\:(?<format>.*))?}", RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.Compiled);

        static string Format(IFormatProvider provider, Match m, object value) {
            string format = m.Groups["format"].Success
                ? "{0:" + m.Groups["format"].Value + "}"
                : "{0}";
            return String.Format(provider, format, value);
        }
        static string ReplaceValue(IFormatProvider provider, Match m, IDictionary<string, object> values) {
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

        public static bool CanInject(string candidate, string format) {
            var m = regex.Match(format);
            while (m.Success) {
                string name = m.Groups["name"].Value;
                if (name == candidate)
                    return true;
                m = m.NextMatch();
            }
            return false;
        }

        public static string Inject(this IDictionary<string, object> values, IFormatProvider provider, string format) {
            return regex.Replace(format, m => ReplaceValue(provider, m, values));
        }
    }
}
