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
    /// Defines the extension methods to nullable types.
    /// This is a static class.
    /// </summary>
    public static class NullableExtensions
    {
        #region Map & Bind

        public static R? Map<T, R>(this T? value, Func<T, R> fn)
            where T : struct
            where R : struct
        {
            if (value.HasValue)
                return fn(value.GetValueOrDefault());
            return null;
        }

        public static R? Map<T, R>(this T? value, Func<T, R?> fn)
            where T : struct
            where R : struct
        {
            if (value.HasValue)
                return fn(value.GetValueOrDefault());
            return null;
        }

        //public static R Bind<T, R>(this T? value, Func<T, R> fn)
        //    where T : struct {
        //    if (value.HasValue)
        //        return fn(value.GetValueOrDefault());
        //    return default(R);
        //}

        #endregion

        #region NullIf

        /// <summary>
        /// Returns <c>null</c> if <paramref name="self"/> equals the given <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="self">The instance.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>null</c> if the instance equals the given value; otherwise, the instance.</returns>
        public static T? NullIf<T>(this T self, T value)
            where T : struct, IEquatable<T>
        {
            if (self.Equals(value))
                return null;
            return self;
        }

        /// <summary>
        /// Returns <c>null</c> if <paramref name="self"/> satisfies the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="self">The instance.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns><c>null</c> if the instance satisfies the predicate; otherwise, the instance.</returns>
        public static T? NullIf<T>(this T self, Predicate<T> predicate)
            where T : struct
        {
            if (predicate(self))
                return null;
            return self;
        }

        #endregion

        #region ToString

        public static string ToString<T>(this T? formattable, string format, IFormatProvider formatProvider = null)
            where T : struct, IFormattable
        {
            if (formattable.HasValue) {
                return formattable.GetValueOrDefault().ToString(format, formatProvider);
            }
            return string.Empty;
        }
        public static string ToString<T>(this T? formattable, IFormatProvider formatProvider)
            where T : struct, IFormattable
        {
            return formattable.ToString(null, formatProvider);
        }

        #endregion
    }
}
