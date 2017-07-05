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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WmcSoft.Collections.Generic;
using WmcSoft.Text;

namespace WmcSoft
{
    /// <summary>
    /// Provides a set of static methods to extend the <see cref="String"/> class. This is a static class.
    /// </summary>
    public static class StringExtensions
    {
        #region Any

        /// <summary>
        /// Check if the char is any of the candidates chars.
        /// </summary>
        /// <param name="self">The char to test</param>
        /// <param name="candidates">Candidates char to test against the char</param>
        /// <returns>true if the char is any of the candidates, otherwise false.</returns>
        public static bool Any(this char self, params char[] candidates)
        {
            for (var i = 0; i < candidates.Length; i++) {
                if (self == candidates[i])
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Check if the char is any of the candidates chars.
        /// </summary>
        /// <remarks>This optimized version relies on the fact that candidates is sorted.</remarks>
        /// <param name="self">The char to test</param>
        /// <param name="candidates">Candidates char to test against the char</param>
        /// <returns>true if the char is any of the candidates, otherwise false.</returns>
        public static bool BinaryAny(this char self, char[] candidates)
        {
            return Array.BinarySearch(candidates, self) >= 0;
        }

        public static bool ContainsAny(this string self, params char[] candidates)
        {
            return self.IndexOfAny(candidates) >= 0;
        }

        /// <summary>
        /// Check if the string contains any of the candidates chars.
        /// </summary>
        /// <remarks>This optimized version relies on the fact that candidates is sorted.</remarks>
        /// <param name="self">The char to test</param>
        /// <param name="candidates">Candidates char to test against the char</param>
        /// <returns>true if the char is any of the candidates, otherwise false.</returns>
        public static bool BinaryContainsAny(this string self, params char[] candidates)
        {
            foreach (var c in self) {
                if (Array.BinarySearch(candidates, c) >= 0)
                    return true;
            }
            return false;
        }

        public static bool EqualsAny(this char self, params string[] candidates)
        {
            var length = candidates.Length;
            for (var i = 0; i < length; i++) {
                var candidate = candidates[i];
                if (candidate != null && candidate.Length == 1 && self == candidate[0])
                    return true;
            }
            return false;
        }

        public static bool EqualsAny(this string self, StringComparison comparisonType, params string[] candidates)
        {
            if (string.IsNullOrEmpty(self))
                return false;
            for (var i = 0; i < candidates.Length; i++) {
                if (self.Equals(candidates[i], comparisonType))
                    return true;
            }
            return false;
        }

        public static bool EqualsAny(this string self, params string[] candidates)
        {
            return self.EqualsAny(StringComparison.CurrentCulture, candidates);
        }

        #endregion

        #region AsReadOnlyList

        class ReadOnlyListAdapter : IReadOnlyList<char>
        {
            private readonly string _value;

            public ReadOnlyListAdapter(string value)
            {
                _value = value;
            }

            #region IReadOnlyList<char> Membres

            public char this[int index] {
                get { return _value[index]; }
            }

            #endregion

            #region IReadOnlyCollection<char> Membres

            public int Count {
                get { return _value.Length; }
            }

            #endregion

            #region IEnumerable<char> Membres

            public IEnumerator<char> GetEnumerator()
            {
                return _value.GetEnumerator();
            }

            #endregion

            #region IEnumerable Membres

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }

        public static IReadOnlyList<char> AsReadOnlyList(this string self)
        {
            return new ReadOnlyListAdapter(self);
        }

        #endregion

        #region AsWords

        static IEnumerable<string> UnguardedAsWords(string sentence)
        {
            var tokenizer = new WordsTokenizer();
            return tokenizer.Tokenize(sentence);
        }

        public static IEnumerable<string> AsWords(this string sentence)
        {
            if (sentence == null) throw new ArgumentNullException(nameof(sentence));

            return UnguardedAsWords(sentence);
        }

        #endregion

        #region Case operations

        /// <summary>
        /// Capitalizes the specified string.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The capitalized string</returns>
        [Obsolete("Use ToTitleCase as it uses culture's text info.", false)]
        public static string Capitalize(this string self, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(self))
                return self;
            return char.ToUpper(self[0], culture) + self.Substring(1).ToLower(culture);
        }

        /// <summary>
        /// Capitalizes the specified string.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <returns>The capitalized string</returns>
        [Obsolete("Use ToTitleCase as it uses culture's text info.", false)]
        public static string Capitalize(this string self)
        {
            if (string.IsNullOrEmpty(self))
                return self;
            return char.ToUpper(self[0]) + self.Substring(1).ToLower();
        }

        /// <summary>
        /// Capitalizes the specified array of strings.
        /// </summary>
        /// <param name="self">The array of strings.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The converted array of strings</returns>
        [Obsolete("Use ToTitleCase as it uses culture's text info.", false)]
        public static string[] Capitalize(this string[] self, CultureInfo culture)
        {
            var list = new List<string>(self.Length);
            foreach (string s in self) {
                list.Add(Capitalize(s, culture));
            }
            return list.ToArray();
        }

        /// <summary>
        /// Capitalizes the specified array of strings.
        /// </summary>
        /// <param name="self">The array of strings.</param>
        /// <returns>The converted array of strings</returns>
        [Obsolete("Use ToTitleCase as it uses culture's text info.", false)]
        public static string[] Capitalize(this string[] self)
        {
            var list = new List<string>(self.Length);
            foreach (string s in self) {
                list.Add(Capitalize(s));
            }
            return list.ToArray();
        }

        /// <summary>
        /// Capitalizes the specified string.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The capitalized string</returns>
        public static string ToTitleCase(this string self, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(self))
                return self;
            culture = culture ?? CultureInfo.CurrentCulture;
            return culture.TextInfo.ToTitleCase(self);
        }

        /// <summary>
        /// Capitalizes the specified string.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <returns>The capitalized string</returns>
        public static string ToTitleCase(this string self)
        {
            if (string.IsNullOrEmpty(self))
                return self;
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(self);
        }

        /// <summary>
        /// Capitalizes the specified array of strings.
        /// </summary>
        /// <param name="self">The array of strings.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The converted array of strings</returns>
        public static string[] ToTitleCase(this string[] self, CultureInfo culture)
        {
            culture = culture ?? CultureInfo.CurrentCulture;
            var textinfo = culture.TextInfo;
            return self.ToArray(textinfo.ToTitleCase);
        }

        /// <summary>
        /// Capitalizes the specified array of strings.
        /// </summary>
        /// <param name="self">The array of strings.</param>
        /// <returns>The converted array of strings</returns>
        public static string[] ToTitleCase(this string[] self)
        {
            var textinfo = CultureInfo.CurrentCulture.TextInfo;
            return self.ToArray(textinfo.ToTitleCase);
        }

        /// <summary>
        /// Converts the string in the specified array to uppercase.
        /// </summary>
        /// <param name="self">The array of strings.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The converted array of strings</returns>
        public static string[] ToUpper(this string[] self, CultureInfo culture)
        {
            var list = new List<string>(self.Length);
            foreach (var s in self) {
                list.Add(s.ToUpper(culture));
            }
            return list.ToArray();
        }

        /// <summary>
        /// Converts the string in the specified array to uppercase.
        /// </summary>
        /// <param name="self">The array of strings.</param>
        /// <returns>The converted array of strings</returns>
        public static string[] ToUpper(this string[] self)
        {
            var list = new List<string>(self.Length);
            foreach (var s in self) {
                list.Add(s.ToUpper());
            }
            return list.ToArray();
        }

        /// <summary>
        /// Converts the string in the specified array to uppercase.
        /// </summary>
        /// <param name="self">The array of strings.</param>
        /// <returns>The converted array of strings</returns>
        public static string[] ToUpperInvariant(this string[] self)
        {
            var list = new List<string>(self.Length);
            foreach (var s in self) {
                list.Add(s.ToUpperInvariant());
            }
            return list.ToArray();
        }

        /// <summary>
        /// Converts the string in the specified array to lowercase.
        /// </summary>
        /// <param name="self">The array of strings.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The converted array of strings</returns>
        public static string[] ToLower(this string[] self, CultureInfo culture)
        {
            var list = new List<string>(self.Length);
            foreach (var s in self) {
                list.Add(s.ToLower(culture));
            }
            return list.ToArray();
        }

        /// <summary>
        /// Converts the string in the specified array to lowercase.
        /// </summary>
        /// <param name="self">The array of strings.</param>
        /// <returns>The converted array of strings</returns>
        public static string[] ToLower(this string[] self)
        {
            var list = new List<string>(self.Length);
            foreach (var s in self) {
                list.Add(s.ToLower());
            }
            return list.ToArray();
        }

        /// <summary>
        /// Converts the string in the specified array to lowercase.
        /// </summary>
        /// <param name="self">The array of strings.</param>
        /// <returns>The converted array of strings</returns>
        public static string[] ToLowerInvariant(this string[] self)
        {
            var list = new List<string>(self.Length);
            foreach (var s in self) {
                list.Add(s.ToLowerInvariant());
            }
            return list.ToArray();
        }

        public static string ToCamelCase(this string source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source == "") return source;

            var culture = CultureInfo.InvariantCulture;
            char[] array = source.ToCharArray();
            var length = source.Length;
            array[0] = char.ToLower(array[0], culture);
            for (int i = 1; i < length; i++) {
                if ((i + 1 < length) && !char.IsUpper(array[i + 1])) {
                    break; // next is lower.
                }
                array[i] = char.ToLower(array[i], culture);
            }
            return new string(array);
        }

        public static string ToSnakeCase(this string source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return UnguardedAsWords(source).Select(w => w.ToLowerInvariant()).JoinWith("_");
        }

        public static string ToKebabCase(this string source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return UnguardedAsWords(source).Select(w => w.ToLowerInvariant()).JoinWith("-");
        }

        #endregion

        #region EnsurePrefix/EnsureSuffix/EnsureAffixes

        public static string EnsurePrefix(this string self, string prefix)
        {
            return EnsurePrefix(self, prefix, StringComparison.CurrentCulture);
        }

        public static string EnsurePrefix(this string self, string prefix, StringComparison comparison)
        {
            if (self == null || self.StartsWith(prefix, comparison))
                return self;
            return prefix + self;
        }

        public static string EnsureSuffix(this string self, string suffix)
        {
            return EnsureSuffix(self, suffix, StringComparison.CurrentCulture);
        }

        public static string EnsureSuffix(this string self, string suffix, StringComparison comparison)
        {
            if (self == null || self.EndsWith(suffix, comparison))
                return self;
            return self + suffix;
        }

        public static string EnsureAffixes(this string self, string affix)
        {
            return EnsureAffixes(self, affix, affix, StringComparison.CurrentCulture);
        }

        public static string EnsureAffixes(this string self, string affix, StringComparison comparison)
        {
            return EnsureAffixes(self, affix, affix, comparison);
        }

        public static string EnsureAffixes(this string self, string prefix, string suffix)
        {
            return EnsureAffixes(self, prefix, suffix, StringComparison.CurrentCulture);
        }

        public static string EnsureAffixes(this string self, string prefix, string suffix, StringComparison comparison)
        {
            if (self == null)
                return self;
            var starts = self.StartsWith(prefix, comparison);
            var ends = self.EndsWith(suffix, comparison);
            if (starts & ends) {
                return self;
            } else if (starts) {
                return self + suffix;
            } else if (ends) {
                return prefix + self;
            }
            return prefix + self + suffix;
        }

        #endregion

        #region FormatWith

        /// <summary>
        /// Replaces one or more format item in a specified string with the string representation of a corresponding object in a specified object.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of <paramref name="format"/> in which any format items are replaced by the string representation of the corresponding objects in <paramref name="args"/>.</returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// Replaces one or more format item in a specified string with the string representation of a specified object.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The object to format.</param>
        /// <returns>A copy of <paramref name="format"/> in which any format items are replaced by the string representation of <paramref name="arg0"/>.</returns>
        public static string FormatWith(this string format, object arg0)
        {
            return string.Format(format, arg0);
        }

        /// <summary>
        /// Replaces one or more format item in a specified string with the string representation of two specified objects.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The first object to format.</param>
        /// <param name="arg1">The second object to format.</param>
        /// <returns>A copy of <paramref name="format"/> in which any format items are replaced by the string representation of <paramref name="arg0"/> and <paramref name="arg1"/>.</returns>
        public static string FormatWith(this string format, object arg0, object arg1)
        {
            return string.Format(format, arg0, arg1);
        }

        /// <summary>
        /// Replaces one or more format item in a specified string with the string representation of three specified objects.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The first object to format.</param>
        /// <param name="arg1">The second object to format.</param>
        /// <param name="arg2">The third object to format.</param>
        /// <returns>A copy of <paramref name="format"/> in which any format items are replaced by the string representation of <paramref name="arg0"/>, <paramref name="arg1"/> and <paramref name="arg2"/>.</returns>
        public static string FormatWith(this string format, object arg0, object arg1, object arg2)
        {
            return string.Format(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// Replaces one or more format item in a specified string with the string representation of a corresponding object in a specified object.
        /// A specified parameter supplies culture-specific formatting information.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of <paramref name="format"/> in which any format items are replaced by the string representation of the corresponding objects in <paramref name="args"/>.</returns>
        public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
        {
            return string.Format(format, provider, args);
        }

        #endregion

        #region Glob

        private static bool Glob(string value, int startIndex, string pattern, int p)
        {
            while (pattern.Length != p) {
                switch (pattern[p]) {
                case '?':
                    break;
                case '*':
                    for (var i = value.Length; i >= p; i--) {
                        if (Glob(value, i, pattern, startIndex + 1)) {
                            return true;
                        }
                    }
                    return false;
                default:
                    if (value.Length == startIndex || char.ToUpper(pattern[p]) != char.ToUpper(value[startIndex])) {
                        return false;
                    }
                    break;
                }

                p++;
                startIndex++;
            }

            return value.Length == startIndex;
        }

        /// <summary>
        /// Match a string against a "glob" pattern.
        /// </summary>
        /// <param name="value">The string</param>
        /// <param name="pattern"></param>
        /// <returns>True if the value match the pattern; otherwise false.</returns>
        public static bool Glob(this string value, string pattern)
        {
            // <https://en.wikipedia.org/wiki/Glob_%28programming%29>
            return Glob(value, 0, pattern, 0);
        }

        #endregion

        #region Join

        /// <summary>
        /// Joins the sequence of strings into a single string.
        /// </summary>
        /// <param name="values">The sequence of strings.</param>
        /// <returns>The joined string.</returns>
        /// <remarks>The separator is CultureInfo.CurrentCulture.TextInfo.ListSeparator.</remarks>
        public static string JoinWithListSeparator(this IEnumerable<string> values)
        {
            return JoinWith(values, CultureInfo.CurrentCulture.TextInfo.ListSeparator);
        }

        /// <summary>
        /// Joins the sequence of strings into a single string.
        /// </summary>
        /// <param name="values">The sequence of strings.</param>
        /// <returns>The joined string.</returns>
        /// <remarks>The separator is currentCulture.TextInfo.ListSeparator.</remarks>
        public static string JoinWithListSeparator(this IEnumerable<string> values, CultureInfo cultureInfo)
        {
            return JoinWith(values, (cultureInfo ?? CultureInfo.CurrentCulture).TextInfo.ListSeparator);
        }

        /// <summary>
        /// Joins the sequence of strings into a single string, using the specified separator.
        /// </summary>
        /// <param name="values">The sequence of strings.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>The joined string.</returns>
        public static string JoinWith(this IEnumerable<string> values, string separator)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            return string.Join(separator, values);
        }

        /// <summary>
        /// Joins the sequence of strings into a single string, using the specified separator.
        /// </summary>
        /// <param name="values">The sequence of strings.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>The joined string.</returns>
        public static string JoinWith(this IEnumerable<string> values, char separator)
        {
            return JoinWith(values, separator.ToString());
        }

        #endregion

        #region Nullify methods

        /// <summary>
        /// Returns null if the string contains only whitespace.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <returns>Returns null if the string contains only whitespace; otherwise, the string.</returns>
        public static string NullifyWhiteSpace(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            return value;
        }

        /// <summary>
        /// Returns null if the string is empty.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <returns>Returns null if the string is empty; otherwise, the string.</returns>
        public static string NullifyEmpty(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            return value;
        }

        /// <summary>
        /// Returns null if the string verifies a predicate.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns null if the string verifies the predicate; otherwise, the string.</returns>
        public static string Nullify(this string value, Predicate<string> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            if (value == null || predicate(value))
                return null;
            return value;
        }

        #endregion

        #region PadEnds

        /// <summary>
        /// Returns a new string that centers the characters in this instance
        /// by padding them with a specified Unicode character, for a specified total length.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="totalWidth">The number of characters in the resulting string, 
        /// equal to the number of original characters plus any additional padding characters.</param>
        /// <param name="paddingChar">A Unicode padding character.</param>
        /// <returns>A new string that is equivalent to this instance, but center and padded on both ends with as many paddingChar characters as needed to create a length of totalWidth.
        /// However, if totalWidth is less than the length of this instance, the method returns a reference to the existing instance.
        /// If totalWidth is equal to the length of this instance, the method returns a new string that is identical to this instance.</returns>
        /// <exception cref="ArgumentOutOfRangceException">totalWidth is less than zero.</exception>
        public static string PadEnds(this string self, int totalWidth, char paddingChar)
        {
            if (self == null) throw new NullReferenceException();
            if (totalWidth < 0) throw new ArgumentOutOfRangeException("totalWith");

            var width = totalWidth - self.Length;
            if (width < 0)
                return self;
            if (width == 1)
                return self + paddingChar;
            var pad = new String(paddingChar, width / 2);
            if (width % 2 == 0) {
                return pad + self + pad;
            } else {
                return pad + self + pad + paddingChar;
            }
        }

        /// <summary>
        /// Returns a new string that centers the characters in this instance
        /// by padding them with spaces, for a specified total length.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="totalWidth">The number of characters in the resulting string, 
        /// equal to the number of original characters plus any additional padding characters.</param>
        /// <returns>A new string that is equivalent to this instance, but center and padded on both ends with as many spaces as needed to create a length of totalWidth.
        /// However, if totalWidth is less than the length of this instance, the method returns a reference to the existing instance.
        /// If totalWidth is equal to the length of this instance, the method returns a new string that is identical to this instance.</returns>
        /// <exception cref="ArgumentOutOfRangceException">totalWidth is less than zero.</exception>
        public static string PadEnds(this string self, int totalWidth)
        {
            return PadEnds(self, totalWidth, ' ');
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes the specified chars from the string.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="args">The chars to remove.</param>
        /// <returns>The string without the specified chars.</returns>
        public static string Remove(this string self, params char[] args)
        {
            if (self == null || self.Length == 0)
                return self;

            var adapter = new StringAdapter(self);
            return adapter.Remove(args);
        }

        /// <summary>
        /// Removes the specified substrings from the string.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="args">The substrings to remove.</param>
        /// <returns>The string without the specified substrings.</returns>
        public static string Remove(this string self, params string[] args)
        {
            if (self == null || self.Length == 0)
                return self;

            var sb = new StringBuilder(self);
            sb.Remove(args);
            return sb.ToString();
        }

        #endregion

        #region RemovePrefix/RemoveSuffix/RemoveAffixes

        public static string RemovePrefix(this string self, string prefix)
        {
            return RemovePrefix(self, prefix, StringComparison.CurrentCulture);
        }

        public static string RemovePrefix(this string self, string prefix, StringComparison comparison)
        {
            if (self != null && self.StartsWith(prefix, comparison))
                return self.Substring(prefix.Length);
            return self;
        }

        public static string RemoveSuffix(this string self, string suffix)
        {
            return RemoveSuffix(self, suffix, StringComparison.CurrentCulture);
        }

        public static string RemoveSuffix(this string self, string suffix, StringComparison comparison)
        {
            if (self != null && self.EndsWith(suffix, comparison))
                return self.Substring(0, self.Length - suffix.Length);
            return self;
        }

        public static string RemoveAffixes(this string self, string affix)
        {
            return RemoveAffixes(self, affix, StringComparison.CurrentCulture);
        }

        public static string RemoveAffixes(this string self, string affix, StringComparison comparison)
        {
            if (self == null || self.Length < affix.Length)
                return self;
            var starts = self.StartsWith(affix, comparison);
            var ends = self.EndsWith(affix, comparison);
            if (starts & ends) {
                if (string.Equals(self, affix, comparison))
                    return "";
                return self.Substring(affix.Length, self.Length - 2 * affix.Length);
            } else if (starts) {
                return self.Substring(affix.Length);
            } else if (ends) {
                return self.Substring(0, self.Length - affix.Length);
            }
            return self;
        }

        public static string RemoveAffixes(this string self, string prefix, string suffix)
        {
            return RemoveAffixes(self, prefix, suffix, StringComparison.CurrentCulture);
        }

        public static string RemoveAffixes(this string self, string prefix, string suffix, StringComparison comparison)
        {
            if (self == null)
                return self;
            var starts = self.StartsWith(prefix, comparison);
            var ends = self.EndsWith(suffix, comparison);
            if (starts & ends) {
                if ((prefix.Length + suffix.Length) >= self.Length)
                    return "";
                return self.Substring(prefix.Length, self.Length - prefix.Length - suffix.Length);
            } else if (starts) {
                return self.Substring(prefix.Length);
            } else if (ends) {
                return self.Substring(0, self.Length - suffix.Length);
            }
            return self;
        }

        #endregion

        #region ReplaceWord

        class WordReplacer
        {
            static readonly Regex WordMatcher = new Regex(@"(\w+)", RegexOptions.Compiled);

            readonly string _oldWord;
            readonly string _newWord;
            int _count;

            public WordReplacer(string oldWord, string newWord)
            {
                _oldWord = oldWord;
                _newWord = newWord;
            }

            string Substitute(Match match)
            {
                var value = match.ToString();
                if (value != _oldWord)
                    return value;
                _count++;
                return _newWord;
            }

            public string Invoke(string source)
            {
                _count = 0;
                var result = WordMatcher.Replace(source, Substitute);
                if (_count > 0)
                    return result;
                return source;
            }
        }

        /// <summary>
        /// Returns a new string in which all occurrences of a specified word in the current instance are replaced with another specified word.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="oldWord">The word to be replaced. </param>
        /// <param name="newWord">The word to replace all occurrences of <paramref name="oldWord"/>. </param>
        /// <returns>A string that is equivalent to the current string except that all instances of oldWord are replaced with newWord.
        /// If oldWord is not found in the current instance, the method returns the current instance unchanged. </returns>
        public static string ReplaceWord(this string self, string oldWord, string newWord)
        {
            var substituer = new WordReplacer(oldWord, newWord);
            return substituer.Invoke(self);
        }

        #endregion

        #region StartsWith & EndsWith

        public static bool StartsWith(this string self, char c)
        {
            return StartsWith(self, c, StringComparison.CurrentCulture);
        }
        public static bool StartsWith(this string self, char c, StringComparison comparison)
        {
            if (string.IsNullOrEmpty(self))
                return false;
            return self.StartsWith(Char.ToString(c), comparison);
        }
        public static bool StartsWith(this string self, char c, bool ignoreCase, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(self))
                return false;
            return self.StartsWith(Char.ToString(c), ignoreCase, culture);
        }

        public static bool StartsWithAny(this string self, params string[] prefixes)
        {
            return StartsWithAny(self, StringComparison.CurrentCulture, prefixes);
        }
        public static bool StartsWithAny(this string self, StringComparison comparison, params string[] prefixes)
        {
            if (string.IsNullOrEmpty(self))
                return false;
            if (prefixes == null || prefixes.Length == 0)
                return true;
            for (int i = 0; i < prefixes.Length; i++) {
                if (self.StartsWith(prefixes[i], comparison))
                    return true;
            }
            return false;
        }

        public static bool EndsWith(this string self, char c)
        {
            return EndsWith(self, c, StringComparison.CurrentCulture);
        }
        public static bool EndsWith(this string self, char c, StringComparison comparison)
        {
            if (string.IsNullOrEmpty(self))
                return false;
            return self.EndsWith(Char.ToString(c), comparison);
        }
        public static bool EndsWith(this string self, char c, bool ignoreCase, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(self))
                return false;
            return self.EndsWith(Char.ToString(c), ignoreCase, culture);
        }

        public static bool EndsWithAny(this string self, params string[] suffixes)
        {
            return EndsWithAny(self, StringComparison.CurrentCulture, suffixes);
        }
        public static bool EndsWithAny(this string self, StringComparison comparison, params string[] suffixes)
        {
            if (string.IsNullOrEmpty(self))
                return false;
            if (suffixes == null || suffixes.Length == 0)
                return true;
            for (int i = 0; i < suffixes.Length; i++) {
                if (self.EndsWith(suffixes[i], comparison))
                    return true;
            }
            return false;
        }

        #endregion

        #region Substring

        /// <summary>
        /// Extracts the substring that precedes the <paramref name="find"/> char.
        /// </summary>
        /// <param name="self">The initial string.</param>
        /// <param name="find">The delimiter char to look for.</param>
        /// <returns>Returns the substring that precedes the <paramref name="find"/> char, or null if the delimiter is not found.</returns>
        public static string SubstringBefore(this string self, char find)
        {
            if (string.IsNullOrEmpty(self))
                return null;

            var index = self.IndexOf(find);
            if (index < 0)
                return null;
            return self.Substring(0, index);
        }

        /// <summary>
        /// Extracts the substring that precedes the <paramref name="find"/> char.
        /// </summary>
        /// <param name="self">The initial string.</param>
        /// <param name="find">The delimiter char to look for.</param>
        /// <returns>Returns the substring that precedes the <paramref name="find"/> char, or the string if the delimiter is not found.</returns>
        public static string SubstringBeforeOrSelf(this string self, char find)
        {
            return SubstringBefore(self, find) ?? self;
        }

        /// <summary>
        /// Extracts the substring that precedes the <paramref name="find"/> string.
        /// </summary>
        /// <param name="self">The initial string.</param>
        /// <param name="find">The delimiter string to look for.</param>
        /// <returns>Returns the substring that precedes the <paramref name="find"/> string, or null if the delimiter is not found.</returns>
        public static string SubstringBefore(this string self, string find)
        {
            if (string.IsNullOrEmpty(self))
                return null;
            if (string.IsNullOrEmpty(find))
                return self;

            var index = self.IndexOf(find, StringComparison.Ordinal);
            if (index < 0)
                return null;
            return self.Substring(0, index);
        }

        /// <summary>
        /// Extracts the substring that precedes the <paramref name="find"/> string.
        /// </summary>
        /// <param name="self">The initial string.</param>
        /// <param name="find">The delimiter string to look for.</param>
        /// <returns>Returns the substring that precedes the <paramref name="find"/> string, or the string if the delimiter is not found.</returns>
        public static string SubstringBeforeOrSelf(this string self, string find)
        {
            return SubstringBefore(self, find) ?? self;
        }

        /// <summary>
        /// Extracts the substring that follows the <paramref name="find"/> char.
        /// </summary>
        /// <param name="self">The initial string.</param>
        /// <param name="find">The delimiter char to look for.</param>
        /// <returns>Returns the substring that follows the <paramref name="find"/> char, or null if the delimiter is not found.</returns>
        public static string SubstringAfter(this string self, char find)
        {
            if (string.IsNullOrEmpty(self))
                return null;

            var index = self.IndexOf(find);
            if (index < 0)
                return null;
            return self.Substring(index + 1);
        }

        /// <summary>
        /// Extracts the substring that follows the <paramref name="find"/> char.
        /// </summary>
        /// <param name="self">The initial string.</param>
        /// <param name="find">The delimiter char to look for.</param>
        /// <returns>Returns the substring that follows the <paramref name="find"/> char, or the string if the delimiter is not found.</returns>
        public static string SubstringAfterOrSelf(this string self, char find)
        {
            return SubstringAfter(self, find) ?? self;
        }

        /// <summary>
        /// Extracts the substring that follows the <paramref name="find"/> string.
        /// </summary>
        /// <param name="self">The initial string.</param>
        /// <param name="find">The delimiter string to look for.</param>
        /// <returns>Returns the substring that follows the <paramref name="find"/> string, or null if the delimiter is not found.</returns>
        public static string SubstringAfter(this string self, string find)
        {
            if (string.IsNullOrEmpty(self))
                return null;
            if (string.IsNullOrEmpty(find))
                return self;

            var index = self.IndexOf(find, StringComparison.Ordinal);
            if (index < 0)
                return null;
            return self.Substring(index + find.Length);
        }

        /// <summary>
        /// Extracts the substring that follows the <paramref name="find"/> string.
        /// </summary>
        /// <param name="self">The initial string.</param>
        /// <param name="find">The delimiter string to look for.</param>
        /// <returns>Returns the substring that follows the <paramref name="find"/> string, or the string if the delimiter is not found.</returns>
        public static string SubstringAfterOrSelf(this string self, string find)
        {
            return SubstringAfter(self, find) ?? self;
        }

        /// <summary>
        /// Extracts the substring between the prefix and the suffix.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns>The substring between the prefix and the suffix, or null if the prefix or the suffix is not found.</returns>
        public static string SubstringBetween(this string self, string prefix, string suffix)
        {
            if (string.IsNullOrEmpty(prefix))
                return SubstringBefore(self, suffix);
            else if (string.IsNullOrEmpty(suffix))
                return SubstringAfter(self, prefix);

            var start = self.IndexOf(prefix, StringComparison.Ordinal);
            if (start < 0)
                return null;
            start += prefix.Length;
            var end = self.IndexOf(suffix, start, StringComparison.Ordinal);
            if (end < 0)
                return null;
            return self.Substring(start, end - start);
        }

        /// <summary>
        /// Extracts the substring between the prefix and the suffix.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns>The substring between the prefix and the suffix, or the string if the prefix or the suffix is not found.</returns>
        public static string SubstringBetweenOrSelf(this string self, string prefix, string suffix)
        {
            return SubstringBetween(self, prefix, suffix) ?? self;
        }

        /// <summary>
        /// Extracts the <paramref name="length"/> chars at the left of the string.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="length">The length.</param>
        /// <returns>The substring.</returns>
        public static string Left(this string self, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (self == null)
                return self;

            if (length == 0)
                return string.Empty;
            if (length > self.Length)
                return self;
            return self.Substring(0, length);
        }

        /// <summary>
        /// Extracts the <paramref name="length"/> chars at the right of the string.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="length">The length.</param>
        /// <returns>The substring.</returns>
        public static string Right(this string self, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (self == null)
                return self;

            if (length == 0)
                return string.Empty;
            if (length > self.Length)
                return self;
            return self.Substring(self.Length - length, length);
        }

        #endregion

        #region SurroundWith

        /// <summary>
        /// Concatenates the prefix, the string and the suffix.
        /// </summary>
        /// <param name="self">The string</param>
        /// <param name="prefix">The prefix</param>
        /// <param name="suffix">The suffix</param>
        /// <returns>null if the string is null; otherwise, the concatenated string.</returns>
        public static string SurroundWith(this string self, string prefix = null, string suffix = null)
        {
            if (self == null)
                return null;
            return prefix + self + suffix;
        }

        #endregion

        #region Tokenize

        /// <summary>
        /// Returns the sequence of substrings of this instance using a tokenizer.
        /// </summary>
        /// <typeparam name="TTokenizer">The type of tokenizer.</typeparam>
        /// <param name="self">The strings.</param>
        /// <param name="tokenizer">The tokenizer.</param>
        /// <returns>The sequence of substrings.</returns>
        public static IEnumerable<string> Tokenize<TTokenizer>(this string self, TTokenizer tokenizer)
            where TTokenizer : ITokenizer<string, string>
        {
            if (self == null)
                return null;
            return tokenizer.Tokenize(self);
        }

        /// <summary>
        /// Returns the sequence of substrings of this instance that are delimited by a separator.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>The sequence of substrings.</returns>
        public static IEnumerable<string> Tokenize(this string self, char separator)
        {
            return Tokenize(self, new CharTokenizer(separator));
        }

        /// <summary>
        /// Returns the sequence of substrings of this instance that are delimited by a separator from a set.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="separators">The set of separators</param>
        /// <returns>The sequence of substrings.</returns>
        public static IEnumerable<string> Tokenize(this string self, params char[] separators)
        {
            if (separators == null || separators.Length == 0)
                return Tokenize(self, new PredicateTokenizer(Char.IsWhiteSpace));
            if (separators.Length == 1)
                return Tokenize(self, new CharTokenizer(separators[0]));
            return Tokenize(self, new CharsTokenizer(separators));
        }

        /// <summary>
        /// Returns the sequence of substrings of this instance that are delimited by char for which a predidate returns true.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="isSeparator">The predicate.</param>
        /// <returns>The sequence of substrings.</returns>
        public static IEnumerable<string> Tokenize(this string self, Predicate<char> isSeparator)
        {
            return Tokenize(self, new PredicateTokenizer(isSeparator));
        }

        /// <summary>
        /// Returns the sequence of substrings of this instance that are delimited by whitespaces.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <returns>The sequence of substrings.</returns>
        public static IEnumerable<string> Tokenize(this string self)
        {
            return Tokenize(self, new PredicateTokenizer(Char.IsWhiteSpace));
        }

        #endregion

        #region Translate

        /// <summary>
        /// Translates the chars of the <paramref name="source "/> into the corresponding chars in the <paramref name="target"/>.
        /// </summary>
        /// <param name="self">The array of chars.</param>
        /// <param name="source">The source chars.</param>
        /// <param name="target">The target chars.</param>
        /// <returns>The translated string.</returns>
        public static string Translate(this char[] self, string source, string target)
        {
            var normalized = target;
            if (target.Length < source.Length)
                normalized += new string('\0', source.Length - target.Length);
            var mapping = source.Zip(normalized, (s, t) => new KeyValuePair<char, char>(s, t))
                .OrderBy((t) => t.Key)
                .ToArray();

            var comparer = Comparer<KeyValuePair<char, char>>.Create((x, y) => x.Key.CompareTo(y.Key));
            int j = 0;
            for (int i = 0; i < self.Length; i++) {
                int found = Array.BinarySearch(mapping, new KeyValuePair<char, char>(self[i], '\0'), comparer);
                if (found < 0)
                    self[j++] = self[i];
                else if (mapping[found].Value != '\0')
                    self[j++] = mapping[found].Value;
            }
            return new string(self, 0, j);
        }
        /// <summary>
        /// Translates the chars of the <paramref name="source "/> into the corresponding chars in the <paramref name="target"/>.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="source">The source chars.</param>
        /// <param name="target">The target chars.</param>
        /// <returns>The translated string.</returns>
        public static string Translate(this string self, string source, string target)
        {
            return Translate(self.ToCharArray(), source, target);
        }

        #endregion

        #region Truncate

        /// <summary>
        /// Shorten the string so that it is not longer than maxLength, the ellipsis string included.
        /// </summary>
        /// <param name="self">The string</param>
        /// <param name="maxLength">The maximum length of the string</param>
        /// <param name="ellipsis">The substring added at the end of the string when it is longer than the maxLenght.</param>
        /// <returns>The shorten string when longer than the max length; otherwise, the string.</returns>
        /// <remarks>The ellipsis may be empty but not null.</remarks>
        public static string Truncate(this string self, int maxLength, string ellipsis = "\u2026")
        {
            if (ellipsis == null) throw new ArgumentNullException(nameof(ellipsis));
            if (maxLength < ellipsis.Length) throw new ArgumentOutOfRangeException(nameof(maxLength));

            if (!string.IsNullOrEmpty(self) && self.Length > maxLength)
                return self.Substring(0, maxLength - ellipsis.Length) + ellipsis;
            return self;
        }

        #endregion
    }
}