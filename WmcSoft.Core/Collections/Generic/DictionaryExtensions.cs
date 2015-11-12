using System;
using System.Collections.Generic;

namespace WmcSoft.Collections.Generic
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Gets and convert the value from dictionary or the default value if the value is missing.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="source">The source dictionary</param>
        /// <param name="key">The key</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The converted value if present, otherwise the default value.</returns>
        /// <remarks>Returns the default value if the source parameter is null.</remarks>
        public static TValue ConvertOrDefault<TKey, TValue>(this IDictionary<TKey, object> source, TKey key, TValue defaultValue = default(TValue)) {
            if (source != null) {
                object value;
                if (source.TryGetValue(key, out value))
                    return (TValue)Convert.ChangeType(value, typeof(TValue));
            }
            return defaultValue;
        }

        /// <summary>
        /// Gets the value from the dictionary or the default value if the value is missing.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="source">The source dictionary</param>
        /// <param name="key">The key</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The value if present, otherwise the default value.</returns>
        /// <remarks>Returns the default value if the source parameter is null.</remarks>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue defaultValue = default(TValue)) {
            if (source != null) {
                TValue value;
                if (source.TryGetValue(key, out value))
                    return value;
            }
            return defaultValue;
        }
    }
}
