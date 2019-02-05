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
using System.ComponentModel;
using System.Globalization;

namespace WmcSoft.ComponentModel
{
    /// <summary>
    /// Provides a type converter to convert an array of string objects to and from other representations.
    /// </summary>
    public class StringArrayConverter : StringConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var converted = (string)base.ConvertFrom(context, culture, value);
            if (converted == null)
                return null;

            var separator = GetListSeparator(culture);
            return converted.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null) throw new ArgumentNullException(nameof(destinationType));

            if (value == null)
                return null;

            if (destinationType == typeof(string)) {
                var separator = GetListSeparator(culture);
                switch (value) {
                case string[] array:
                    value = string.Join(separator, array);
                    break;
                case IEnumerable<string> enumerable:
                    value = string.Join(separator, enumerable);
                    break;
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        static string GetListSeparator(CultureInfo culture)
        {
            return (culture ?? CultureInfo.CurrentCulture).TextInfo.ListSeparator;
        }
    }
}
