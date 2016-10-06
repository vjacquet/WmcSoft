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

namespace WmcSoft.Data
{
    public static class DataConvert
    {
        /// <summary>
        /// Returns an object of the specified type and whose value is equivalent to the specified object.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="value">An object that implements the <see cref="IConvertible"/> interface.</param>
        /// <returns>An object whose type is <typeparamref name="T"/> and whose value is equivalent to value or a null reference, if value is null or DBNull and <typeparamref name="T"/> is not a value type. </returns>
        public static T ChangeType<T>(object value) {
            var underlyingType = Nullable.GetUnderlyingType(typeof(T));
            if (value == DBNull.Value) {
                if (underlyingType != null || !typeof(T).IsValueType)
                    return default(T);
                throw new InvalidCastException();
            }
            return (T)Convert.ChangeType(value, underlyingType ?? typeof(T));
        }

        /// <summary>
        /// Returns an object of the specified type and whose value is equivalent to the specified object.
        /// </summary>
        /// <typeparam name="T">The type of object to return. </typeparam>
        /// <param name="value">An object that implements the <see cref="IConvertible"/> interface.</param>
        /// <param name="defaultValue">The default value if the value is null or DBNull</param>
        /// <returns>An object whose type is <typeparamref name="T"/> and whose value is equivalent to value or default, if value is null or DBNull and <typeparamref name="T"/> is not a value type. </returns>
        public static T ChangeTypeOrDefault<T>(object value, T defaultValue = default(T)) {
            if (value == DBNull.Value)
                return defaultValue;

            var underlyingType = Nullable.GetUnderlyingType(typeof(T));
            return (T)Convert.ChangeType(value, underlyingType ?? typeof(T));
        }
    }
}
