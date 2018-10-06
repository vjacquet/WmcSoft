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

namespace WmcSoft
{
    /// <summary>
    /// Defines the extension methods to the <see cref="IConvertible"/> interface.
    /// This is a static class.
    /// </summary>
    public static class ConvertibleExtensions
    {
        static T UnguardedChangeType<T>(object value)
        {
            var underlyingType = Nullable.GetUnderlyingType(typeof(T));
            if (value == DBNull.Value) {
                if (underlyingType != null || !typeof(T).IsValueType)
                    return default;
                throw new InvalidCastException();
            }
            return (T)Convert.ChangeType(value, underlyingType ?? typeof(T));
        }

        /// <summary>
        /// Returns an object of the specified type and whose value is equivalent to the specified object.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="value">A convertible object.</param>
        /// <returns>An object whose type is <typeparamref name="T"/> and whose value is equivalent to value.-or-A <c>null</c> reference , if <paramref name="value"/> is null.</returns>
        /// <exception cref="InvalidCastException">This conversion is not supported. -or-value is <c>null</c> and <typeparamref name="T"/> is a value type.</exception>
        /// <exception cref="FormatException"><paramref name="value"/> is not in a format recognized by <typeparamref name="T"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> represents a number that is out of the range of <typeparamref name="T"/>.</exception>
        public static T ConvertTo<T>(this IConvertible value)
        {
            return UnguardedChangeType<T>(value);
        }

        /// <summary>
        /// Returns an object of the specified type and whose value is equivalent to the specified object.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="value">A convertible object.</param>
        /// <returns>An object whose type is <typeparamref name="T"/> and whose value is equivalent to value.-or-A <c>null</c> reference , if <paramref name="value"/> is null.</returns>
        /// <exception cref="InvalidCastException">This conversion is not supported.</exception>
        /// <exception cref="FormatException"><paramref name="value"/> is not in a format recognized by <typeparamref name="T"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> represents a number that is out of the range of <typeparamref name="T"/>.</exception>
        public static T? ConvertTo<T>(this T? value)
            where T : struct, IConvertible
        {
            return UnguardedChangeType<T?>(value);
        }
    }
}
