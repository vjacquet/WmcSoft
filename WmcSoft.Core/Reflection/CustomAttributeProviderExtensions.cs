using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Reflection
{
    public static class CustomAttributeProviderExtensions
    {
        /// <summary>
        /// Returns an array of custom attributes defined on this provider.
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute</typeparam>
        /// <param name="self">The custom attribute provider</param>
        /// <param name="inherit">When true, lookup the hierarchy chain for the inherited custom attribute</param>
        /// <returns>The array of the custom attributes, or an empty array if not found.</returns>
        public static T[] GetCustomAttributes<T>(this ICustomAttributeProvider self, bool inherit)
            where T : Attribute {
            object[] result = self.GetCustomAttributes(typeof(T), inherit);
            T[] array = result as T[];
            if (array != null)
                return array;
            return result.OfType<T>().ToArray();
        }

        /// <summary>
        /// Returns the first custom attributes defined on this provider, or default.
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute</typeparam>
        /// <param name="self">The custom attribute provider</param>
        /// <param name="inherit">When true, lookup the hierarchy chain for the inherited custom attribute</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(this ICustomAttributeProvider self, bool inherit)
            where T : Attribute {
            return GetCustomAttributes<T>(self, inherit).FirstOrDefault();
        }

        /// <summary>
        /// Indicate wether one or more instance of the attribute is defined on this provider.
        /// <typeparam name="T">The type of the custom attribute</typeparam>
        /// <param name="self">The custom attribute provider</param>
        /// <param name="inherit">When true, lookup the hierarchy chain for the inherited custom attribute</param>
        /// <returns>true if the custom attribute is defined</returns>
        public static bool IsDefined<T>(this ICustomAttributeProvider self, bool inherit)
            where T : Attribute {
            return self.IsDefined(typeof(T), inherit);
        }
    }
}
