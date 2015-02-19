using System;

namespace WmcSoft
{
    public static class FormatProviderExtensions
    {
        public static T GetFormat<T>(this IFormatProvider formatProvider) {
            return (T)formatProvider.GetFormat(typeof(T));
        }
    }
}
