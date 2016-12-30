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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WmcSoft
{
    /// <summary>
    /// Defines the extension methods to the <see cref="Type"/> class. This is a static class. 
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns an enumeration of the attributes of a certain type from the specified type.
        /// </summary>
        /// <typeparam name="A">The type of the attributes</typeparam>
        /// <param name="type">The type to enumerate the attributes from.</param>
        /// <returns>The enumeration of the attributes of a certain type.</returns>
        public static IEnumerable<A> GetAttributes<A>(this Type type) where A : Attribute {
            return TypeDescriptor.GetAttributes(type).OfType<A>();
        }

        /// <summary>
        /// Returns the display name of the type.
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="inherit">true to search this member's inheritance chain to find the attributes; otherwise, false.</param>
        /// <returns>The display name of the type.</returns>
        public static string GetDisplayName(this Type type, bool inherit) {
            var attributes = (DisplayNameAttribute[])type.GetCustomAttributes(typeof(DisplayNameAttribute), inherit);
            if (attributes != null && attributes.Length > 0)
                return attributes[0].DisplayName;
            return type.Name;
        }

        /// <summary>
        /// Returns the description of the type.
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="inherit">true to search this member's inheritance chain to find the attributes; otherwise, false.</param>
        /// <returns>The description of the type.</returns>
        public static string GetDescription(this Type type, bool inherit) {
            var attributes = (DescriptionAttribute[])type.GetCustomAttributes(typeof(DescriptionAttribute), inherit);
            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            return "";
        }

        /// <summary>
        /// Returns the underlying type of the type if it is a nullable type, otherwise the type itself.
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The underlying type if the type is a nullable type, otherwise the type itself.</returns>
        public static Type UnwrapNullableType(this Type type) {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        /// <summary>
        /// Returns true if the type is nullable.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the type is nullable, otherwise <c>false</c>.</returns>
        public static bool IsNullable(this Type type) {
            return type.IsGenericType
                && !type.IsGenericTypeDefinition
                && ReferenceEquals(type.GetGenericTypeDefinition(), typeof(Nullable<>));
        }

        /// <summary>
        /// Returns true if the is nullable or is a reference type.
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns><c>true</c> if the type is nullable or is a reference type, otherwise <c>false</c>.</returns>
        public static bool AllowsNull(this Type type) {
            return IsNullable(type) || !type.IsValueType;
        }

        /// <summary>
        /// Returns the <see cref="Type"/> referred to by the array or <see cref="IEnumerable{T}"/> type.
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The <see cref="Type"/> referred to by the array or <see cref="IEnumerable{T}"/> type, or <c>null</c> if <paramref name="type"/> is not enumerable.</returns>
        public static Type GetGenericElementType(this Type type) {
            if (type.IsArray)
                return type.GetElementType();
            return type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>)))
                .Select(i=> i.GetGenericArguments()[0])
                .FirstOrDefault();
        }
    }
}
