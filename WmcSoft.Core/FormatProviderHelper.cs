using System;
using System.Globalization;

namespace WmcSoft
{
    public static class FormatProviderHelper
    {
        public static string HandleOtherFormats(string format, object arg) {
            var formattable = arg as IFormattable;
            if (formattable != null)
                return formattable.ToString(format, CultureInfo.CurrentCulture);
            if (arg != null)
                return arg.ToString();
            return String.Empty;
        }
    }
}
