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
using System.Globalization;

namespace WmcSoft
{
    /// <summary>
    /// Defines the extension methods to the <see cref="IFormatProvider"/> interface.
    /// This is a static class.
    /// </summary>
    public static class FormatProviderExtensions
    {
        /// <summary>
        /// Returns an object that provides formatting services for the specified type.
        /// </summary>
        /// <typeparam name="T">The type of format object to return.</typeparam>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>An instance of the object specified by <typeparamref name="T"/>, if the <see cref="IFormatProvider"/> implementation can supply that type of object; otherwise, <c>null</c>.</returns>
        /// <remarks>This method is eager to find a type: it also checks <see cref="TextInfo"/>, <see cref="RegionInfo"/> and <see cref="Calendar"/>.</remarks>
        public static T GetFormat<T>(this IFormatProvider formatProvider)
        {
            // TODO: Remove fallback to CurrentCulture?
            formatProvider = formatProvider ?? CultureInfo.CurrentCulture;
            var result = (T)formatProvider.GetFormat(typeof(T));
            if (result != null) {
                return result;
            } else if (typeof(TextInfo).IsAssignableFrom(typeof(T))) {
                var cultureProvider = formatProvider as CultureInfo ?? CultureInfo.CurrentCulture;
                return (T)(object)cultureProvider.TextInfo;
            } else if (typeof(RegionInfo).IsAssignableFrom(typeof(T))) {
                var cultureProvider = formatProvider as CultureInfo ?? CultureInfo.CurrentCulture;
                return (T)(object)new RegionInfo(cultureProvider.LCID);
            } else if (typeof(Calendar).IsAssignableFrom(typeof(T))) {
                var cultureProvider = formatProvider as CultureInfo ?? CultureInfo.CurrentCulture;
                return (T)(object)cultureProvider.Calendar;
            }
            return default(T);
        }
    }
}
