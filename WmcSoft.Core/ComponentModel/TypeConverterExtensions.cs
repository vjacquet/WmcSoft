using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.ComponentModel
{
   public static class TypeConverterExtensions
    {
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
