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
using System.Collections.Specialized;
using System.ComponentModel;

namespace WmcSoft.Collections.Specialized
{
    public static class NameValueCollectionExtensions
    {
        #region Conversion adapters

        internal class EmptyArray<TElement>
        {
            public static readonly TElement[] Instance = new TElement[0];
        }

        // TODO: Shouldn't we use Converter<in TInput, out TOutput> instead ?
        public interface IConverter<in TInput, out TOutput>
        {
            TOutput Convert(TInput input);
        }

        struct TypeConverterAdapter<TOutput> : IConverter<string, TOutput>
        {
            private readonly TypeConverter _converter;

            public TypeConverterAdapter(TypeConverter converter) {
                if (converter == null) throw new ArgumentNullException("converter");
                if (!converter.CanConvertFrom(typeof(string))) throw new ArgumentException("converter");
                _converter = converter;
            }

            public TOutput Convert(string input) {
                return (TOutput)_converter.ConvertFromInvariantString(input);
            }
        }

        struct ConverterAdapter<TOutput> : IConverter<string, TOutput>
        {
            private readonly Converter<string, TOutput> _converter;

            public ConverterAdapter(Converter<string, TOutput> converter) {
                if (converter == null) throw new ArgumentNullException("converter");
                _converter = converter;
            }

            public TOutput Convert(string input) {
                return _converter(input);
            }
        }

        #endregion

        #region GetValue(s)

        // TODO: Deal with conversions failure: If cannot convert (TryConvert ?), should we use default value?

        static T GetValue<TConverter, T>(NameValueCollection collection, string name, TConverter converter, Func<T> defaultValue)
            where TConverter : IConverter<string, T> {
            var value = collection[name];
            return (value == null) ? defaultValue() : converter.Convert(value);
        }

        static T GetValue<TConverter, T>(NameValueCollection collection, string name, TConverter converter, T defaultValue)
            where TConverter : IConverter<string, T> {
            var value = collection[name];
            return (value == null) ? defaultValue : converter.Convert(value);
        }

        static T[] GetValues<TConverter, T>(NameValueCollection collection, string name, TConverter converter, T[] defaultValue)
            where TConverter : IConverter<string, T> {
            var values = collection.GetValues(name);
            return (values == null) ? defaultValue : values.ConvertAll(converter.Convert);
        }

        /// <summary>
        /// Gets the named value from the collection and convert it to the requested type, or returns a default value when missing.
        /// </summary>
        /// <typeparam name="T">The required type</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="name">The name</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>Returns the value converted to the requested type, or the defaultValue when the value is missing.</returns>
        public static T GetValue<T>(this NameValueCollection collection, string name, T defaultValue = default(T)) {
            return GetValue(collection, name, new TypeConverterAdapter<T>(TypeDescriptor.GetConverter(typeof(T))), defaultValue);
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
            return GetValue(collection, name, new TypeConverterAdapter<T>(converter), defaultValue);
        }

        /// <summary>
        /// Gets the named value from the collection and convert it to the requested type, or returns a default value when missing.
        /// </summary>
        /// <typeparam name="T">The required type</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="name">The name</param>
        /// <param name="defaultValue">The default value generator</param>
        /// <returns>Returns the value converted to the requested type, or the defaultValue when the value is missing.</returns>
        public static T GetValue<T>(this NameValueCollection collection, string name, Func<T> defaultValue) {
            return GetValue(collection, name, new TypeConverterAdapter<T>(TypeDescriptor.GetConverter(typeof(T))), defaultValue);
        }

        /// <summary>
        /// Gets the named value from the collection and convert it to the requested type, or returns a default value when missing.
        /// </summary>
        /// <typeparam name="T">The required type</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="name">The name</param>
        /// <param name="converter">The type converter to use</param>
        /// <param name="defaultValue">The default value generator</param>
        /// <returns>Returns the value converted to the requested type, or the defaultValue when the value is missing.</returns>
        public static T GetValue<T>(this NameValueCollection collection, string name, TypeConverter converter, Func<T> defaultValue) {
            return GetValue(collection, name, new TypeConverterAdapter<T>(converter), defaultValue);
        }

        /// <summary>
        /// Gets the named values from the collection and convert them to the requested type.
        /// </summary>
        /// <typeparam name="T">The required type</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="name">The name</param>
        /// <returns>Returns the values converted to the requested type.</returns>
        public static T[] GetValues<T>(this NameValueCollection collection, string name) {
            return GetValues(collection, name, new TypeConverterAdapter<T>(TypeDescriptor.GetConverter(typeof(T))), EmptyArray<T>.Instance);
        }

        /// <summary>
        /// Gets the named values from the collection and convert them to the requested type.
        /// </summary>
        /// <typeparam name="T">The required type</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="name">The name</param>
        /// <param name="converter">The type converter to use</param>
        /// <returns>Returns the values converted to the requested type.</returns>
        public static T[] GetValues<T>(this NameValueCollection collection, string name, TypeConverter converter) {
            return GetValues(collection, name, new TypeConverterAdapter<T>(converter), EmptyArray<T>.Instance);
        }

        /// <summary>
        /// Gets the named values from the collection and convert them to the requested type.
        /// </summary>
        /// <typeparam name="T">The required type</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="name">The name</param>
        /// <param name="converter">The converter to use</param>
        /// <returns>Returns the values converted to the requested type.</returns>
        public static T[] GetValues<T>(this NameValueCollection collection, string name, Converter<string, T> converter) {
            return GetValues(collection, name, new ConverterAdapter<T>(converter), EmptyArray<T>.Instance);
        }

        #endregion

        #region PopValue(s)

        /// <summary>
        /// Gets the named value and then removes it from the <see cref="NameValueCollection"/>.
        /// </summary>
        /// <typeparam name="T">The required type</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="name">The name of the value</param>
        /// <returns>The value or default(T) if missing.</returns>
        public static T PopValue<T>(this NameValueCollection collection, string name) {
            return PopValue(collection, name, TypeDescriptor.GetConverter(typeof(T)), default(T));
        }

        /// <summary>
        /// Gets the named value and then removes it from the <see cref="NameValueCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="name">The name of the value</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The value or <paramref name="defaultValue"/> if missing.</returns>
        public static T PopValue<T>(this NameValueCollection collection, string name, T defaultValue) {
            return PopValue(collection, name, TypeDescriptor.GetConverter(typeof(T)), defaultValue);
        }

        /// <summary>
        /// Gets the named value and then removes it from the <see cref="NameValueCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="name">The name of the value</param>
        /// <param name="converter">The type converter to use</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The value or <paramref name="defaultValue"/> if missing.</returns>
        public static T PopValue<T>(this NameValueCollection collection, string name, TypeConverter converter, T defaultValue) {
            var result = GetValue(collection, name, converter, defaultValue);
            collection.Remove(name);
            return result;
        }

        /// <summary>
        /// Gets the named value and then removes it from the <see cref="NameValueCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="name">The name of the value</param>
        /// <param name="defaultValue">The default value generator</param>
        /// <returns>The value or <paramref name="defaultValue"/> if missing.</returns>
        public static T PopValue<T>(this NameValueCollection collection, string name, Func<T> defaultValue) {
            return PopValue(collection, name, TypeDescriptor.GetConverter(typeof(T)), defaultValue);
        }

        /// <summary>
        /// Gets the named value and then removes it from the <see cref="NameValueCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="name">The name of the value</param>
        /// <param name="converter">The type converter to use</param>
        /// <param name="defaultValue">The default value generator</param>
        /// <returns>The value or <paramref name="defaultValue"/> if missing.</returns>
        public static T PopValue<T>(this NameValueCollection collection, string name, TypeConverter converter, Func<T> defaultValue) {
            var result = GetValue(collection, name, converter, defaultValue);
            collection.Remove(name);
            return result;
        }

        /// <summary>
        /// Gets the named value and then removes it from the <see cref="NameValueCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="name">The name of the value</param>
        /// <returns>An enumerable of the values.</returns>
        /// <remarks>The value is removed from the <see cref="NameValueCollection"/> even if the values are not enumerated.</remarks>
        public static T[] PopValues<T>(this NameValueCollection collection, string name) {
            return PopValues<T>(collection, name, TypeDescriptor.GetConverter(typeof(T)));
        }

        /// <summary>
        /// Gets the named value and then removes it from the <see cref="NameValueCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="name">The name of the value</param>
        /// <param name="converter">The type converter to use</param>
        /// <returns>An enumerable of the values.</returns>
        /// <remarks>The value is removed from the <see cref="NameValueCollection"/> even if the values are not enumerated.</remarks>
        public static T[] PopValues<T>(this NameValueCollection collection, string name, TypeConverter converter) {
            var result = GetValues<T>(collection, name, converter);
            collection.Remove(name);
            return result;
        }

        #endregion
    }
}
