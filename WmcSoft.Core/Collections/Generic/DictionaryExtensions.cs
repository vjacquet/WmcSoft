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
using System.Linq;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Provides a set of static methods to extend dictionary related classes or interfaces.
    /// </summary>
    public static class DictionaryExtensions
    {
        #region Convert

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

        #endregion

        #region Get

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

        #endregion

        #region Pop

        public static TValue Pop<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key) {
            var value = source[key];
            source.Remove(key);
            return value;
        }

        public static TValue PopOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue defaultValue = default(TValue)) {
            if (source != null) {
                TValue value;
                if (source.TryGetValue(key, out value)) {
                    source.Remove(key);
                    return value;
                }
            }
            return defaultValue;
        }

        #endregion

        #region Set methods

        /// <summary>
        /// Removes from a dictionary values with keys existing in another one.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <typeparam name="TDictionary">The type of the dictionary source dictionary, for chaining.</typeparam>
        /// <param name="dictionary">The source dictionary.</param>
        /// <param name="other">The other dictionary.</param>
        /// <returns>The <paramref name="dictionary"/>.</returns>
        public static TDictionary ExceptWith<TKey, TValue, TDictionary>(this TDictionary dictionary, IEnumerable<KeyValuePair<TKey, TValue>> other)
            where TDictionary : IDictionary<TKey, TValue> {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            if (other != null) {
                foreach (var entry in other) {
                    dictionary.Remove(entry.Key);
                }
            }
            return dictionary;
        }

        /// <summary>
        /// Removes from a dictionary values with keys existing in another one when the values compares equal.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <typeparam name="TDictionary">The type of the dictionary source dictionary, for chaining.</typeparam>
        /// <param name="dictionary">The source dictionary.</param>
        /// <param name="other">The other dictionary.</param>
        /// <param name="comparer">A function to combine the values when the key exists in both dictionary.</param>
        /// <returns>The <paramref name="dictionary"/>.</returns>
        public static TDictionary ExceptWith<TKey, TValue, TDictionary>(this TDictionary dictionary, IEnumerable<KeyValuePair<TKey, TValue>> other, IEqualityComparer<TValue> comparer)
            where TDictionary : IDictionary<TKey, TValue> {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            if (other != null) {
                foreach (var entry in other) {
                    TValue existing;
                    if (dictionary.TryGetValue(entry.Key, out existing) && comparer.Equals(existing, entry.Value)) {
                        dictionary.Remove(entry.Key);
                    }
                }
            }
            return dictionary;
        }

        /// <summary>
        /// Modifies the <paramref name="dictionary"/> to contain all elements that are present in both dictionaries.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <typeparam name="TDictionary">The type of the dictionary source dictionary, for chaining.</typeparam>
        /// <param name="dictionary">The source dictionary.</param>
        /// <param name="other">The other dictionary.</param>
        /// <returns>The <paramref name="dictionary"/>.</returns>
        public static TDictionary IntersectWith<TKey, TValue, TDictionary>(this TDictionary dictionary, IDictionary<TKey, TValue> other)
            where TDictionary : IDictionary<TKey, TValue> {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            if (other != null) {
                foreach (var key in dictionary.Keys.ToArray()) {
                    if (!other.ContainsKey(key))
                        dictionary.Remove(key);
                }
            }
            return dictionary;
        }

        /// <summary>
        /// Modifies the <paramref name="dictionary"/> to contain all elements that are present in both dictionaries by merging the values.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <typeparam name="TDictionary">The type of the dictionary source dictionary, for chaining.</typeparam>
        /// <param name="dictionary">The source dictionary.</param>
        /// <param name="other">The other dictionary.</param>
        /// <returns>The <paramref name="dictionary"/>.</returns>
        public static TDictionary IntersectWith<TKey, TValue, TDictionary>(this TDictionary dictionary, IDictionary<TKey, TValue> other, Func<TValue, TValue, TValue> merger)
            where TDictionary : IDictionary<TKey, TValue> {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            if (other != null) {
                foreach (var entry in dictionary.ToArray()) {
                    TValue existing;
                    if (other.TryGetValue(entry.Key, out existing)) {
                        dictionary[entry.Key] = merger(entry.Value, existing);
                    } else {
                        dictionary.Remove(entry.Key);
                    }
                }
            }
            return dictionary;
        }

        /// <summary>
        /// Modifies the <paramref name="dictionary"/> to contain all elements that are present in itself, the specified dictionary, or both.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <typeparam name="TDictionary">The type of the dictionary source dictionary, for chaining.</typeparam>
        /// <param name="dictionary">The source dictionary.</param>
        /// <param name="other">The other dictionary.</param>
        /// <returns>The <paramref name="dictionary"/>.</returns>
        /// <remarks>When the key exists in both dictionary, the value is overwritten.</remarks>
        public static TDictionary UnionWith<TKey, TValue, TDictionary>(this TDictionary dictionary, IEnumerable<KeyValuePair<TKey, TValue>> other)
            where TDictionary : IDictionary<TKey, TValue> {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            if (other != null) {
                foreach (var entry in other) {
                    dictionary[entry.Key] = entry.Value;
                }
            }
            return dictionary;
        }

        /// <summary>
        /// Modifies the <paramref name="dictionary"/> to contain all elements that are present in itself, the specified dictionary, 
        /// or both by merging the existing values.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <typeparam name="TDictionary">The type of the dictionary source dictionary, for chaining.</typeparam>
        /// <param name="dictionary">The source dictionary.</param>
        /// <param name="other">The other dictionary.</param>
        /// <param name="merger">A function to combine the values when the key exists in both dictionary.</param>
        /// <returns>The <paramref name="dictionary"/>.</returns>
        public static TDictionary UnionWith<TKey, TValue, TDictionary>(this TDictionary dictionary, IEnumerable<KeyValuePair<TKey, TValue>> other, Func<TValue, TValue, TValue> merger)
            where TDictionary : IDictionary<TKey, TValue> {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            if (other != null) {
                foreach (var entry in other) {
                    TValue existing;
                    if (dictionary.TryGetValue(entry.Key, out existing)) {
                        dictionary[entry.Key] = merger(existing, entry.Value);
                    } else {
                        dictionary.Add(entry.Key, entry.Value);
                    }
                }
            }
            return dictionary;
        }

        #endregion
    }
}