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
        /// <param name="dictionary">The source dictionary</param>
        /// <param name="key">The key</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The converted value if present, otherwise the default value.</returns>
        /// <remarks>Returns the default value if the source parameter is null.</remarks>
        public static TValue ConvertOrDefault<TKey, TValue>(this IDictionary<TKey, object> dictionary, TKey key, TValue defaultValue = default)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            if (dictionary.TryGetValue(key, out object value))
                return (TValue)Convert.ChangeType(value, typeof(TValue));
            return defaultValue;
        }

        #endregion

        #region Get

        /// <summary>
        /// Gets the value from the dictionary or the default value if the value is missing.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="dictionary">The source dictionary</param>
        /// <param name="key">The key</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The value if present, otherwise the default value.</returns>
        /// <remarks>Returns the default value if the source parameter is null.</remarks>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            if (dictionary.TryGetValue(key, out TValue value))
                return value;
            return defaultValue;
        }

        #endregion

        #region Pop

        public static TValue Pop<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            var value = dictionary[key];
            dictionary.Remove(key);
            return value;
        }

        public static TValue PopOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
        {
            if (dictionary != null) {
                if (dictionary.TryGetValue(key, out TValue value)) {
                    dictionary.Remove(key);
                    return value;
                }
            }
            return defaultValue;
        }

        #endregion

        #region Project

        /// <summary>
        /// Projects the dictionary values in an array at the specified <paramref name="keys"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source dictionary</param>
        /// <param name="keys">The keys of the expected values.</param>
        /// <returns>An array of <typeparamref name="TValue"/>.</returns>
        public static TValue[] Project<TKey, TValue>(this IDictionary<TKey, TValue> source, params TKey[] keys)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var length = keys.Length;
            var result = new TValue[length];
            for (int i = 0; i < length; i++) {
                var key = keys[i];
                if (source.TryGetValue(key, out TValue value))
                    result[i] = value;
            }
            return result;
        }

        /// <summary>
        /// Projects the dictionary values in an array at the specified <paramref name="coordinates"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TCoord">The type of the coordinates.</typeparam>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="source">The source dictionary</param>
        /// <param name="map">The function to transform the dictionary's key to coordinate.</param>
        /// <param name="coordinates">The coordinates of the expected values.</param>
        /// <returns>An array of <typeparamref name="TValue"/>.</returns>
        /// <remarks>When multiple keys map to the same coordinate, the projected value is the value of the last enumerated key.</remarks>
        public static TValue[] Project<TKey, TCoord, TValue>(this IDictionary<TKey, TValue> source, Func<TKey, TCoord> map, params TCoord[] coordinates)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (map == null) throw new ArgumentNullException(nameof(map));

            var mapper = new Dictionary<TCoord, int>(coordinates.Length);
            mapper.AddRange(coordinates.Select((x, i) => new KeyValuePair<TCoord, int>(x, i)));

            var result = new TValue[coordinates.Length];
            foreach (var kv in source) {
                var key = map(kv.Key);
                if (mapper.TryGetValue(key, out int index))
                    result[index] = kv.Value;
            }
            return result;
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
            where TDictionary : IDictionary<TKey, TValue>
        {
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
            where TDictionary : IDictionary<TKey, TValue>
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            if (other != null) {
                foreach (var entry in other) {
                    if (dictionary.TryGetValue(entry.Key, out TValue existing) && comparer.Equals(existing, entry.Value)) {
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
            where TDictionary : IDictionary<TKey, TValue>
        {
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
            where TDictionary : IDictionary<TKey, TValue>
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            if (other != null) {
                foreach (var entry in dictionary.ToArray()) {
                    if (other.TryGetValue(entry.Key, out TValue existing)) {
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
            where TDictionary : IDictionary<TKey, TValue>
        {
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
            where TDictionary : IDictionary<TKey, TValue>
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            if (other != null) {
                foreach (var entry in other) {
                    if (dictionary.TryGetValue(entry.Key, out TValue existing)) {
                        dictionary[entry.Key] = merger(existing, entry.Value);
                    } else {
                        dictionary.Add(entry.Key, entry.Value);
                    }
                }
            }
            return dictionary;
        }

        #endregion

        #region DifferencesKeys

        /// <summary>
        /// Returns the list of keys for which values from one dictionary are different in the other.
        /// </summary>
        /// <typeparam name="TKey">The type of keys.</typeparam>
        /// <typeparam name="TValue">The type of values.</typeparam>
        /// <param name="first">The first dictionary.</param>
        /// <param name="second">The second dictionary.</param>
        /// <returns>The list of keys for which values from one dictionary are different in the other.</returns>
        public static List<TKey> KeySymmetricDifferences<TKey, TValue>(this Dictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));

            if (second == null)
                return first.Keys.ToList();

            var comparer = EqualityComparer<TValue>.Default;
            var set = new Dictionary<TKey, TValue>(first, first.Comparer);
            foreach (var kv in second) {
                if (!set.TryGetValue(kv.Key, out TValue value)) {
                    set.Add(kv.Key, kv.Value);
                } else if (comparer.Equals(kv.Value, value)) {
                    set.Remove(kv.Key);
                }
            }
            return set.Keys.ToList();
        }

        #endregion

        #region RenameKey

        /// <summary>
        /// Renames a key.
        /// </summary>
        /// <typeparam name="TKey">The type of key.</typeparam>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="oldKey">The old key.</param>
        /// <param name="newKey">The new key.</param>
        /// <returns><c>true</c> if the <paramref name="oldKey"/> existed and was renamed; otherwise, <c>false</c>.</returns>
        public static bool RenameKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey oldKey, TKey newKey)
        {
            if (oldKey == null) throw new ArgumentNullException(nameof(oldKey));

            try {
                if (dictionary.TryGetValue(oldKey, out TValue value)) {
                    dictionary.Add(newKey, value);
                    dictionary.Remove(oldKey);
                    return true;
                }
                return false;
            } catch (ArgumentNullException e) {
                throw new ArgumentNullException(nameof(newKey), e.Message);
            } catch (ArgumentException e) {
                throw new ArgumentException(nameof(newKey), e.Message);
            }
        }

        #endregion
    }
}
