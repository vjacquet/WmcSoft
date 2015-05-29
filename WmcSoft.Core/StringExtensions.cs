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
using WmcSoft.Collections.Generic;
using WmcSoft.Text;

namespace WmcSoft
{
    /// <summary>
    /// Provides a set of static methods to extend the <see cref="String"/> class.
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
        public static bool Any(this char self, params char[] candidates) {
            for (int i = 0; i < candidates.Length; i++) {
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
        public static bool BinaryAny(this char self, char[] candidates) {
            return Array.BinarySearch(candidates, self) >= 0;
        }

        public static bool ContainsAny(this string self, params char[] candidates) {
            return self.IndexOfAny(candidates) >= 0;
        }

        public static bool BinaryContainsAny(this string self, params char[] candidates) {
            foreach (var c in self) {
                if (Array.BinarySearch(candidates, c) >= 0)
                    return true;
            }
            return false;
        }

        public static bool EqualsAny(this char self, params string[] candidates) {
            var length = candidates.Length;
            for (int i = 0; i < length; i++) {
                var candidate = candidates[i];
                if (candidate != null && candidate.Length == 1 && self == candidate[0])
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Check if the string is any of the candidates chars.
        /// </summary>
        /// <remarks>This optimized version relies on the fact that candidates is sorted.</remarks>
        /// <param name="self">The char to test</param>
        /// <param name="candidates">Candidates char to test against the char</param>
        /// <returns>true if the char is any of the candidates, otherwise false.</returns>
        public static bool EqualsAny(this string self, StringComparison comparisonType, params string[] candidates) {
            if (String.IsNullOrEmpty(self))
                return false;
            for (int i = 0; i < candidates.Length; i++) {
                if (self.Equals(candidates[i], comparisonType))
                    return true;
            }
            return false;
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
        public static string Capitalize(this string self, CultureInfo culture) {
            if (String.IsNullOrEmpty(self))
                return self;
            return Char.ToUpper(self[0], culture) + self.Substring(1).ToLower(culture);
        }

        /// <summary>
        /// Capitalizes the specified string.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <returns>The capitalized string</returns>
        [Obsolete("Use ToTitleCase as it uses culture's text info.", false)]
        public static string Capitalize(this string self) {
            if (String.IsNullOrEmpty(self))
                return self;
            return Char.ToUpper(self[0]) + self.Substring(1).ToLower();
        }

        /// <summary>
        /// Capitalizes the specified array of strings.
        /// </summary>
        /// <param name="self">The array of strings.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The converted array of strings</returns>
        [Obsolete("Use ToTitleCase as it uses culture's text info.", false)]
        public static string[] Capitalize(this string[] self, CultureInfo culture) {
            List<string> list = new List<string>(self.Length);
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
        public static string[] Capitalize(this string[] self) {
            List<string> list = new List<string>(self.Length);
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
        public static string ToTitleCase(this string self, CultureInfo culture) {
            if (String.IsNullOrEmpty(self))
                return self;
            culture = culture ?? CultureInfo.CurrentCulture;
            return culture.TextInfo.ToTitleCase(self);
        }

        /// <summary>
        /// Capitalizes the specified string.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <returns>The capitalized string</returns>
        public static string ToTitleCase(this string self) {
            if (String.IsNullOrEmpty(self))
                return self;
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(self);
        }

        /// <summary>
        /// Capitalizes the specified array of strings.
        /// </summary>
        /// <param name="self">The array of strings.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The converted array of strings</returns>
        public static string[] ToTitleCase(this string[] self, CultureInfo culture) {
            culture = culture ?? CultureInfo.CurrentCulture;
            var textinfo = culture.TextInfo;
            return self.ToArray(textinfo.ToTitleCase);
        }

        /// <summary>
        /// Capitalizes the specified array of strings.
        /// </summary>
        /// <param name="self">The array of strings.</param>
        /// <returns>The converted array of strings</returns>
        public static string[] ToTitleCase(this string[] self) {
            var textinfo = CultureInfo.CurrentCulture.TextInfo;
            return self.ToArray(textinfo.ToTitleCase);
        }

        /// <summary>
        /// Converts the string in the specified array to uppercase.
        /// </summary>
        /// <param name="self">The array of strings.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The converted array of strings</returns>
        public static string[] ToUpper(this string[] self, CultureInfo culture) {
            List<string> list = new List<string>(self.Length);
            foreach (string s in self) {
                list.Add(s.ToUpper(culture));
            }
            return list.ToArray();
        }

        /// <summary>
        /// Converts the string in the specified array to uppercase.
        /// </summary>
        /// <param name="self">The array of strings.</param>
        /// <returns>The converted array of strings</returns>
        public static string[] ToUpper(this string[] self) {
            List<string> list = new List<string>(self.Length);
            foreach (string s in self) {
                list.Add(s.ToUpper());
            }
            return list.ToArray();
        }

        /// <summary>
        /// Converts the string in the specified array to uppercase.
        /// </summary>
        /// <param name="self">The array of strings.</param>
        /// <returns>The converted array of strings</returns>
        public static string[] ToUpperInvariant(this string[] self) {
            List<string> list = new List<string>(self.Length);
            foreach (string s in self) {
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
        public static string[] ToLower(this string[] self, CultureInfo culture) {
            List<string> list = new List<string>(self.Length);
            foreach (string s in self) {
                list.Add(s.ToLower(culture));
            }
            return list.ToArray();
        }

        /// <summary>
        /// Converts the string in the specified array to lowercase.
        /// </summary>
        /// <param name="self">The array of strings.</param>
        /// <returns>The converted array of strings</returns>
        public static string[] ToLower(this string[] self) {
            List<string> list = new List<string>(self.Length);
            foreach (string s in self) {
                list.Add(s.ToLower());
            }
            return list.ToArray();
        }

        /// <summary>
        /// Converts the string in the specified array to lowercase.
        /// </summary>
        /// <param name="self">The array of strings.</param>
        /// <returns>The converted array of strings</returns>
        public static string[] ToLowerInvariant(this string[] self) {
            List<string> list = new List<string>(self.Length);
            foreach (string s in self) {
                list.Add(s.ToLowerInvariant());
            }
            return list.ToArray();
        }

        #endregion

        #region FormatWith

        /// <summary>
        /// Replaces one or more format item in a specified string with the string representation of a corresponding object in a specified object.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of <paramref name="format"/> in which any format items are replaced by the string representation of the corresponding objects in <paramref name="args"/>.</returns>
        public static string FormatWith(this string format, params object[] args) {
            return String.Format(format, args);
        }

        /// <summary>
        /// Replaces one or more format item in a specified string with the string representation of a specified object.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The object to format.</param>
        /// <returns>A copy of <paramref name="format"/> in which any format items are replaced by the string representation of <paramref name="arg0"/>.</returns>
        public static string FormatWith(this string format, object arg0) {
            return String.Format(format, arg0);
        }

        /// <summary>
        /// Replaces one or more format item in a specified string with the string representation of two specified objects.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The first object to format.</param>
        /// <param name="arg1">The second object to format.</param>
        /// <returns>A copy of <paramref name="format"/> in which any format items are replaced by the string representation of <paramref name="arg0"/> and <paramref name="arg1"/>.</returns>
        public static string FormatWith(this string format, object arg0, object arg1) {
            return String.Format(format, arg0, arg1);
        }

        /// <summary>
        /// Replaces one or more format item in a specified string with the string representation of three specified objects.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The first object to format.</param>
        /// <param name="arg1">The second object to format.</param>
        /// <param name="arg2">The third object to format.</param>
        /// <returns>A copy of <paramref name="format"/> in which any format items are replaced by the string representation of <paramref name="arg0"/>, <paramref name="arg1"/> and <paramref name="arg2"/>.</returns>
        public static string FormatWith(this string format, object arg0, object arg1, object arg2) {
            return String.Format(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// Replaces one or more format item in a specified string with the string representation of a corresponding object in a specified object.
        /// A specified parameter supplies culture-specific formatting information.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of <paramref name="format"/> in which any format items are replaced by the string representation of the corresponding objects in <paramref name="args"/>.</returns>
        public static string FormatWith(this string format, IFormatProvider provider, params object[] args) {
            return String.Format(format, provider, args);
        }

        #endregion

        #region Join

        /// <summary>
        /// Joins the sequence of strings into a single string.
        /// </summary>
        /// <param name="self">The sequence of strings.</param>
        /// <returns>The joined string.</returns>
        /// <remarks>The separator is CultureInfo.CurrentCulture.TextInfo.ListSeparator.</remarks>
        public static string Join(this IEnumerable<string> self) {
            return Join(self, CultureInfo.CurrentCulture.TextInfo.ListSeparator);
        }

        /// <summary>
        /// Joins the sequence of strings into a single string.
        /// </summary>
        /// <param name="self">The sequence of strings.</param>
        /// <returns>The joined string.</returns>
        /// <remarks>The separator is currentCulture.TextInfo.ListSeparator.</remarks>
        public static string Join(this IEnumerable<string> self, CultureInfo cultureInfo) {
            return Join(self, (cultureInfo ?? CultureInfo.CurrentCulture).TextInfo.ListSeparator);
        }

        /// <summary>
        /// Joins the sequence of strings into a single string, using the specified separator.
        /// </summary>
        /// <param name="self">The sequence of strings.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>The joined string.</returns>
        public static string Join(this IEnumerable<string> self, string separator) {
            if (self == null)
                return null;

            var sb = new StringBuilder();
            using (var enumerator = self.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    sb.Append(enumerator.Current);
                    while (enumerator.MoveNext())
                        sb.Append(separator).Append(enumerator.Current);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Joins the sequence of strings into a single string, using the specified separator.
        /// </summary>
        /// <param name="self">The sequence of strings.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>The joined string.</returns>
        public static string Join(this IEnumerable<string> self, char separator) {
            if (self == null)
                return null;

            var sb = new StringBuilder();
            using (var enumerator = self.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    sb.Append(enumerator.Current);
                    while (enumerator.MoveNext())
                        sb.Append(separator).Append(enumerator.Current);
                }
            }
            return sb.ToString();
        }

        #endregion

        #region Nullify methods

        public static string NullifyWhiteSpace(this string value) {
            if (String.IsNullOrWhiteSpace(value))
                return null;
            return value;
        }

        public static string NullifyEmpty(this string value) {
            if (String.IsNullOrEmpty(value))
                return null;
            return value;
        }

        public static string Nullify(this string value, Predicate<string> predicate) {
            if (predicate == null)
                throw new ArgumentNullException("predicate");
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
        public static string PadEnds(this string self, int totalWidth, char paddingChar) {
            if (self == null)
                throw new NullReferenceException();
            if (totalWidth < 0)
                throw new ArgumentOutOfRangeException("totalWith");

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
        public static string PadEnds(this string self, int totalWidth) {
            return self.PadEnds(totalWidth, ' ');
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes the specified chars from the string.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="args">The chars to remove.</param>
        /// <returns>The string without the specified chars.</returns>
        public static string Remove(this string self, params char[] args) {
            if (self == null || self.Length == 0)
                return self;

            var sb = new StringBuilder();
            int pos = 0;
            int count = self.Length;
            while (true) {
                var found = self.IndexOfAny(args, pos, count);
                if (found < 0) {
                    sb.Append(self, pos, count);
                    break;
                } else if (found == pos) {
                    pos++;
                    count--;
                    continue;
                } else {
                    sb.Append(self, pos, found - pos);
                    count -= found - pos + 1;
                    pos = found + 1;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Removes the specified substrings from the string.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="args">The substrings to remove.</param>
        /// <returns>The string without the specified substrings.</returns>
        public static string Remove(this string self, params string[] args) {
            if (self == null || self.Length == 0)
                return self;

            StringBuilder sb = new StringBuilder(self);
            foreach (var arg in args) {
                sb.Replace(arg, "");
            }
            return sb.ToString();
        }

        #endregion

        #region StartsWith & EndsWith

        public static bool StartsWith(this string self, char c) {
            if (String.IsNullOrEmpty(self))
                return false;
            return self.StartsWith(Char.ToString(c));
        }
        public static bool StartsWith(this string self, char c, StringComparison comparison) {
            if (String.IsNullOrEmpty(self))
                return false;
            return self.StartsWith(Char.ToString(c), comparison);
        }
        public static bool StartsWith(this string self, char c, bool ignoreCase, CultureInfo culture) {
            if (String.IsNullOrEmpty(self))
                return false;
            return self.StartsWith(Char.ToString(c), ignoreCase, culture);
        }

        public static bool EndsWith(this string self, char c) {
            if (String.IsNullOrEmpty(self))
                return false;
            return self.EndsWith(Char.ToString(c));
        }
        public static bool EndsWith(this string self, char c, StringComparison comparison) {
            if (String.IsNullOrEmpty(self))
                return false;
            return self.EndsWith(Char.ToString(c), comparison);
        }
        public static bool EndsWith(this string self, char c, bool ignoreCase, CultureInfo culture) {
            if (String.IsNullOrEmpty(self))
                return false;
            return self.EndsWith(Char.ToString(c), ignoreCase, culture);
        }

        #endregion

        #region Substring

        /// <summary>
        /// Extracts the substring that precedes the <paramref name="find"/> char.
        /// </summary>
        /// <param name="self">The initial string.</param>
        /// <param name="find">The delimiter char to look for.</param>
        /// <returns>Returns the substring that precedes the <paramref name="find"/> char, or null if the delimiter is not found.</returns>
        public static string SubstringBefore(this string self, char find) {
            if (String.IsNullOrEmpty(self))
                return self;

            int index = self.IndexOf(find);
            if (index < 0)
                return null;
            return self.Substring(0, index);
        }

        /// <summary>
        /// Extracts the substring that precedes the <paramref name="find"/> string.
        /// </summary>
        /// <param name="self">The initial string.</param>
        /// <param name="find">The delimiter string to look for.</param>
        /// <returns>Returns the substring that precedes the <paramref name="find"/> string, or null if the delimiter is not found.</returns>
        public static string SubstringBefore(this string self, string find) {
            if (String.IsNullOrEmpty(self) || String.IsNullOrEmpty(find))
                return self;

            int index = self.IndexOf(find, StringComparison.Ordinal);
            if (index < 0)
                return null;
            return self.Substring(0, index);
        }

        /// <summary>
        /// Extracts the substring that follows the <paramref name="find"/> char.
        /// </summary>
        /// <param name="self">The initial string.</param>
        /// <param name="find">The delimiter char to look for.</param>
        /// <returns>Returns the substring that follows the <paramref name="find"/> char, or null if the delimiter is not found.</returns>
        public static string SubstringAfter(this string self, char find) {
            if (String.IsNullOrEmpty(self))
                return self;

            int index = self.IndexOf(find);
            if (index < 0)
                return null;
            return self.Substring(index + 1);
        }

        /// <summary>
        /// Extracts the substring that follows the <paramref name="find"/> string.
        /// </summary>
        /// <param name="self">The initial string.</param>
        /// <param name="find">The delimiter string to look for.</param>
        /// <returns>Returns the substring that follows the <paramref name="find"/> string, or null if the delimiter is not found.</returns>
        public static string SubstringAfter(this string self, string find) {
            if (String.IsNullOrEmpty(self) || String.IsNullOrEmpty(find))
                return self;

            int index = self.IndexOf(find, StringComparison.Ordinal);
            if (index < 0)
                return null;
            return self.Substring(index + find.Length);
        }

        /// <summary>
        /// Extracts the substring between the prefix and the suffix.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns>The substring between the prefix and the suffix.</returns>
        public static string SubstringBetween(this string self, string prefix, string suffix) {
            if (String.IsNullOrEmpty(self))
                return self;
            else if (String.IsNullOrEmpty(prefix))
                return SubstringBefore(self, suffix);
            else if (String.IsNullOrEmpty(prefix))
                return SubstringBefore(self, suffix);

            int start = self.IndexOf(prefix, StringComparison.Ordinal);
            if (start < 0)
                start = 0;
            int end = self.IndexOf(suffix, StringComparison.Ordinal);
            if (end < 0)
                end = self.Length;
            return self.Substring(start, end - start);
        }

        /// <summary>
        /// Extracts the <paramref name="length"/> chars at the left of the string.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="length">The length.</param>
        /// <returns>The substring.</returns>
        public static string Left(this string self, int length) {
            return self.Substring(0, length);
        }
        /// <summary>
        /// Extracts the <paramref name="length"/> chars at the right of the string.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="length">The length.</param>
        /// <returns>The substring.</returns>
        public static string Right(this string self, int length) {
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
        public static string SurroundWith(this string self, string prefix = null, string suffix = null) {
            if (self == null)
                return null;
            return prefix + self + suffix;
        }

        #endregion

        #region Tokenize

        public static IEnumerable<string> Tokenize<TTokenizer>(this string self, TTokenizer tokenizer)
            where TTokenizer : ITokenizer<string> {
            if (self == null)
                return null;
            return tokenizer.Tokenize(self);
        }

        public static IEnumerable<string> Tokenize(this string self, char separator) {
            return self.Tokenize(new CharTokenizer(separator));
        }

        public static IEnumerable<string> Tokenize(this string self, params char[] separators) {
            if (separators == null || separators.Length == 0)
                return self.Tokenize(new PredicateTokenizer(Char.IsWhiteSpace));
            if (separators.Length == 1)
                return self.Tokenize(new CharTokenizer(separators[0]));
            return self.Tokenize(new CharsTokenizer(separators));
        }

        public static IEnumerable<string> Tokenize(this string self, Predicate<char> isSeparator) {
            return self.Tokenize(new PredicateTokenizer(isSeparator));
        }

        public static IEnumerable<string> Tokenize(this string self) {
            return self.Tokenize(new PredicateTokenizer(Char.IsWhiteSpace));
        }

        #endregion

        #region Translate

        /// <summary>
        /// Translates the chars of the <paramref name="source "/> into the corresponding chars in the <paramref name="target"/>.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="source">The source chars.</param>
        /// <param name="target">The target chars.</param>
        /// <returns>The translated string.</returns>
        public static string Translate(this string self, string source, string target) {
            var normalized = target;
            if (target.Length < source.Length)
                normalized += new String('\0', source.Length - target.Length);
            var mapping = source.Zip(normalized, (s, t) => new KeyValuePair<char, char>(s, t))
                .OrderBy((t) => t.Key)
                .ToArray();

            var comparer = new AnonymousComparer<KeyValuePair<char, char>>((x, y) => x.Key.CompareTo(y.Key));
            var array = self.ToCharArray();
            int j = 0;
            for (int i = 0; i < array.Length; i++) {
                int found = Array.BinarySearch(mapping, new KeyValuePair<char, char>(array[i], '\0'), comparer);
                if (found < 0)
                    array[j++] = array[i];
                else if (mapping[found].Value != '\0')
                    array[j++] = mapping[found].Value;
            }
            return new String(array, 0, j);
        }

        #endregion

        #region Truncate

        public static string Truncate(this string self, int maxLength, string ellipsis = "…") {
            if (ellipsis == null)
                throw new ArgumentNullException("ellipsis");
            if (maxLength < ellipsis.Length)
                throw new ArgumentOutOfRangeException("maxLength");

            if (!String.IsNullOrEmpty(self) && self.Length > maxLength)
                return self.Substring(0, maxLength - ellipsis.Length) + ellipsis;
            return self;
        }

        #endregion
    }
}
