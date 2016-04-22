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
using System.Linq;
using System.Reflection;

namespace WmcSoft.Reflection
{
    /// <summary>
    /// Defines extension methods to the <see cref="ICustomAttributeProvider"/> interface.
    /// This is a static class.
    /// </summary>
    public static class CustomAttributeProviderExtensions
    {
        /// <summary>
        /// Returns an array of custom attributes defined on this provider.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the custom attribute</typeparam>
        /// <param name="self">The custom attribute provider</param>
        /// <param name="inherit">When true, lookup the hierarchy chain for the inherited custom attribute</param>
        /// <returns>The array of the custom attributes, or an empty array if not found.</returns>
        public static TAttribute[] GetCustomAttributes<TAttribute>(this ICustomAttributeProvider self, bool inherit)
            where TAttribute : Attribute {
            return (TAttribute[])self.GetCustomAttributes(typeof(TAttribute), inherit);
        }

        /// <summary>
        /// Returns the first custom attributes defined on this provider, or default.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the custom attribute</typeparam>
        /// <param name="self">The custom attribute provider</param>
        /// <param name="inherit">When true, lookup the hierarchy chain for the inherited custom attribute</param>
        /// <returns></returns>
        public static TAttribute GetCustomAttribute<TAttribute>(this ICustomAttributeProvider self, bool inherit)
            where TAttribute : Attribute {
            return GetCustomAttributes<TAttribute>(self, inherit).FirstOrDefault();
        }

        /// <summary>
        /// Indicate wether one or more instance of the attribute is defined on this provider.
        /// <typeparam name="TAttribute">The type of the custom attribute</typeparam>
        /// <param name="self">The custom attribute provider</param>
        /// <param name="inherit">When true, lookup the hierarchy chain for the inherited custom attribute</param>
        /// <returns>true if the custom attribute is defined</returns>
        public static bool IsDefined<TAttribute>(this ICustomAttributeProvider self, bool inherit)
            where TAttribute : Attribute {
            return self.IsDefined(typeof(TAttribute), inherit);
        }
    }
}
