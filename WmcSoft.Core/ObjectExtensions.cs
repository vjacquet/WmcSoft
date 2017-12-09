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
using System.ComponentModel;
using System.Globalization;

namespace WmcSoft
{
    public static class ObjectExtensions
    {
        #region ConvertTo

        public static T ConvertTo<T>(this object value)
        {
            return (T)ConvertTo(value, typeof(T));
        }

        public static T ConvertTo<T>(this object value, CultureInfo culture)
        {
            return (T)ConvertTo(value, typeof(T), culture);
        }

        public static object ConvertTo(this object value, Type destinationType)
        {
            return ConvertTo(value, destinationType, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts the given value object to the specified type, using the arguments.
        /// </summary>
        /// <param name="value">The <see cref="object"/> to convert.</param>
        /// <param name="destinationType">The <see cref="Type"/> to convert the value parameter to.</param>
        /// <param name="culture">A <see cref="CultureInfo"/>. If <c>null</c> is passed, the current culture is assumed.</param>
        /// <returns>An <see cref="object"/> that represents the converted value.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="destinationType"/> is <c>null</c>.</exception>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        /// <remarks>This method eagerly tries to convert the value.</remarks>
        public static object ConvertTo(this object value, Type destinationType, CultureInfo culture)
        {
            if (destinationType == null) throw new ArgumentNullException(nameof(destinationType));

            if (value == null) {
                if (destinationType.AllowsNull())
                    return null;
                return Convert.ChangeType(value, destinationType, culture);
            }

            destinationType = destinationType.UnwrapNullableType();
            var sourceType = value.GetType();
            if (destinationType.IsAssignableFrom(sourceType))
                return value;

            if (value is IConvertible convertible)
                return convertible.ToType(destinationType, culture);

            var converter = TypeDescriptor.GetConverter(destinationType);
            if (converter.CanConvertFrom(sourceType))
                return converter.ConvertFrom(null, culture, value);

            converter = TypeDescriptor.GetConverter(sourceType);
            return converter.ConvertTo(null, culture, value, destinationType);
        }

        #endregion
    }
}
