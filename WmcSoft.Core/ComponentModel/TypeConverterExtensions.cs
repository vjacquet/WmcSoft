#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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

using System.ComponentModel;
using System.Globalization;

namespace WmcSoft.ComponentModel
{
    public static class TypeConverterExtensions
    {
        public static bool CanConvertFrom<T>(this TypeConverter converter)
        {
            return converter.CanConvertFrom(typeof(T));
        }
        public static bool CanConvertFrom<T>(this TypeConverter converter, ITypeDescriptorContext context)
        {
            return converter.CanConvertFrom(context, typeof(T));
        }

        public static bool CanConvertTo<T>(this TypeConverter converter)
        {
            return converter.CanConvertTo(typeof(T));
        }
        public static bool CanConvertTo<T>(this TypeConverter converter, ITypeDescriptorContext context)
        {
            return converter.CanConvertTo(context, typeof(T));
        }

        public static T ConvertTo<T>(this TypeConverter converter, object value)
        {
            return (T)converter.ConvertTo(value, typeof(T));
        }
        public static T ConvertTo<T>(this TypeConverter converter, ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return (T)converter.ConvertTo(context, culture, value, typeof(T));
        }
    }
}
