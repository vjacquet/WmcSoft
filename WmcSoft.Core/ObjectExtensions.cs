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

namespace WmcSoft
{
    public static class ObjectExtensions
    {
        #region Clone

        /// <summary>
        /// Clone the instance.
        /// </summary>
        /// <typeparam name="T">The type of the instance</typeparam>
        /// <param name="instance">The instance</param>
        /// <returns>A clone of the instance.</returns>
        /// <remarks> This extensions works better for classes implementing <see cref="ICloneable"/> explicitly.</remarks>
        public static T Clone<T>(this T instance)
            where T : ICloneable {
            return (T)instance.Clone();
        }

        #endregion

        #region ConvertTo

        public static T ConvertTo<T>(this object value) {
            return (T)ConvertTo(value, typeof(T));
        }

        public static T ConvertTo<T>(this object value, IFormatProvider provider) {
            return (T)ConvertTo(value, typeof(T), provider);
        }

        public static object ConvertTo(this object value, Type type) {
            return ConvertTo(value, type, System.Globalization.CultureInfo.CurrentCulture);
        }

        public static object ConvertTo(this object value, Type type, IFormatProvider provider) {
            if (type == null) {
                throw new ArgumentNullException("type");
            }
            if (value == null) {
                if (type.AllowsNull()) {
                    return null;
                }
                return Convert.ChangeType(value, type, provider);
            }
            type = type.UnwrapNullableType();
            if (value.GetType() == type) {
                return value;
            }
            var converter = TypeDescriptor.GetConverter(type);
            if (converter.CanConvertFrom(value.GetType())) {
                return converter.ConvertFrom(value);
            }
            converter = TypeDescriptor.GetConverter(value.GetType());
            if (!converter.CanConvertTo(type)) {
                throw new InvalidOperationException();
            }
            return converter.ConvertTo(value, type);
        }

        #endregion
    }
}
