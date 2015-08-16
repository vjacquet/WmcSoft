using System.ComponentModel;
using System.Globalization;

namespace WmcSoft.ComponentModel
{
    public static class TypeConverterExtensions
    {
        public static bool CanConvertFrom<T>(this TypeConverter converter) {
            return converter.CanConvertFrom(typeof(T));
        }
        public static bool CanConvertFrom<T>(this TypeConverter converter, ITypeDescriptorContext context) {
            return converter.CanConvertFrom(context, typeof(T));
        }

        public static bool CanConvertTo<T>(this TypeConverter converter) {
            return converter.CanConvertTo(typeof(T));
        }
        public static bool CanConvertTo<T>(this TypeConverter converter, ITypeDescriptorContext context) {
            return converter.CanConvertTo(context, typeof(T));
        }

        public static T ConvertTo<T>(this TypeConverter converter, object value) {
            return (T)converter.ConvertTo(value, typeof(T));
        }
        public static T ConvertTo<T>(this TypeConverter converter, ITypeDescriptorContext context, CultureInfo culture, object value) {
            return (T)converter.ConvertTo(context, culture, value, typeof(T));
        }
    }
}
