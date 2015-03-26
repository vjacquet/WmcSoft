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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace WmcSoft.Collections.Specialized
{
    public static class NameValueCollectionExtensions
    {
        /// <summary>
        /// Gets the named value from the collection and convert it to the requested type, or returns a default value when missing.
        /// </summary>
        /// <typeparam name="T">The required type</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="name">The name</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>Returns the value converted to the requested type, or the defaultValue when the value is missing.</returns>
        public static T GetValue<T>(this NameValueCollection collection, string name, T defaultValue = default(T)) {
            var value = collection[name];
            if (value == null)
                return defaultValue;
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter.CanConvertFrom(typeof(string))) {
                return (T)converter.ConvertFromInvariantString(value);
            }
            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the named value from the collection and convert it to the requested type, or returns a default value when missing.
        /// </summary>
        /// <typeparam name="T">The required type</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="name">The name</param>
        /// <param name="converter">The type converter to use</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>Returns the value converted to the requested type, or the defaultValue when the value is missing.</returns>
        public static T GetValue<T>(this NameValueCollection collection, string name, TypeConverter converter, T defaultValue = default(T)) {
            if (converter == null)
                throw new ArgumentNullException("converter");
            var value = collection[name];
            if (value == null)
                return defaultValue;
            if (converter.CanConvertFrom(typeof(string))) {
                return (T)converter.ConvertFromInvariantString(value);
            }
            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the named values from the collection and convert them to the requested type.
        /// </summary>
        /// <typeparam name="T">The required type</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="name">The name</param>
        /// <returns>Returns the values converted to the requested type.</returns>
        public static IEnumerable<T> GetValues<T>(this NameValueCollection collection, string name) {
            var values = collection.GetValues(name);
            if (values != null) {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter.CanConvertFrom(typeof(string)))
                    return values.ConvertAll(x => (T)converter.ConvertFromInvariantString(x));
                else
                    return values.ConvertAll(x => (T)Convert.ChangeType(x, typeof(T), CultureInfo.InvariantCulture));
            }
            return Enumerable.Empty<T>();
        }

        /// <summary>
        /// Gets the named values from the collection and convert them to the requested type.
        /// </summary>
        /// <typeparam name="T">The required type</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="name">The name</param>
        /// <param name="converter">The type converter to use</param>
        /// <returns>Returns the values converted to the requested type.</returns>
        public static IEnumerable<T> GetValues<T>(this NameValueCollection collection, string name, TypeConverter converter) {
            var values = collection.GetValues(name);
            if (values != null) {
                if (!converter.CanConvertFrom(typeof(string)))
                    throw new ArgumentException("converter");

                return values.ConvertAll(x => (T)converter.ConvertFromInvariantString(x));
            }
            return Enumerable.Empty<T>();
        }
    }
}
