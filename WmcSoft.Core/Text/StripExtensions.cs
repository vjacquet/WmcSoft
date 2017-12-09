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
using System.Text;
using WmcSoft.Text;

namespace WmcSoft
{
    /// <summary>
    /// Provides a set of static methods to extend the <see cref="Strip"/> class.
    /// This is a static class.
    /// </summary>
    public static class StripExtensions
    {
        #region Any

        public static bool ContainsAny(this Strip self, params char[] candidates)
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
        public static bool BinaryContainsAny(this Strip self, params char[] candidates)
        {
            foreach (var c in self) {
                if (Array.BinarySearch(candidates, c) >= 0)
                    return true;
            }
            return false;
        }

        public static bool EqualsAny(this Strip self, StringComparison comparisonType, params string[] candidates)
        {
            if (string.IsNullOrEmpty(self))
                return false;
            for (var i = 0; i < candidates.Length; i++) {
                if (self.Equals(candidates[i], comparisonType))
                    return true;
            }
            return false;
        }

        public static bool EqualsAny(this Strip self, params string[] candidates)
        {
            return self.EqualsAny(StringComparison.CurrentCulture, candidates);
        }

        #endregion

        #region Join

        /// <summary>
        /// Joins the sequence of strings into a single string.
        /// </summary>
        /// <param name="values">The sequence of strings.</param>
        /// <returns>The joined string.</returns>
        /// <remarks>The separator is CultureInfo.CurrentCulture.TextInfo.ListSeparator.</remarks>
        public static string JoinWithListSeparator(this IEnumerable<Strip> values)
        {
            return JoinWith(values, CultureInfo.CurrentCulture.TextInfo.ListSeparator);
        }

        /// <summary>
        /// Joins the sequence of strings into a single string.
        /// </summary>
        /// <param name="values">The sequence of strings.</param>
        /// <returns>The joined string.</returns>
        /// <remarks>The separator is currentCulture.TextInfo.ListSeparator.</remarks>
        public static string JoinWithListSeparator(this IEnumerable<Strip> values, CultureInfo cultureInfo)
        {
            return JoinWith(values, (cultureInfo ?? CultureInfo.CurrentCulture).TextInfo.ListSeparator);
        }

        /// <summary>
        /// Joins the sequence of strings into a single string, using the specified separator.
        /// </summary>
        /// <param name="values">The sequence of strings.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>The joined string.</returns>
        public static string JoinWith(this IEnumerable<Strip> values, string separator)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            return UnguardedJoin(separator, values);
        }

        /// <summary>
        /// Joins the sequence of strings into a single string, using the specified separator.
        /// </summary>
        /// <param name="values">The sequence of strings.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>The joined string.</returns>
        public static string JoinWith(this IEnumerable<Strip> values, char separator)
        {
            return JoinWith(values, separator.ToString());
        }

        static string UnguardedJoin(string separator, IEnumerable<Strip> values)
        {
            var sb = new StringBuilder();
            using (var enumerator = values.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    enumerator.Current.AppendTo(sb);
                    while (enumerator.MoveNext())
                        sb.Append(separator);
                    enumerator.Current.AppendTo(sb);
                }
            }
            return sb.ToString();
        }

        #endregion

        #region Nullify methods

        /// <summary>
        /// Returns null if the string contains only whitespace.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <returns>Returns null if the string contains only whitespace; otherwise, the string.</returns>
        public static Strip NullifyWhiteSpace(Strip value)
        {
            if (Strip.IsNullOrWhiteSpace(value))
                return null;
            return value;
        }

        /// <summary>
        /// Returns null if the string is empty.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <returns>Returns null if the string is empty; otherwise, the string.</returns>
        public static Strip NullifyEmpty(Strip value)
        {
            if (Strip.IsNullOrEmpty(value))
                return null;
            return value;
        }

        /// <summary>
        /// Returns null if the string verifies a predicate.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns null if the string verifies the predicate; otherwise, the string.</returns>
        public static Strip Nullify(Strip value, Predicate<Strip> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            if (value == null || predicate(value))
                return null;
            return value;
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes the specified chars from the string.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="args">The chars to remove.</param>
        /// <returns>The string without the specified chars.</returns>
        public static string Remove(this Strip self, params char[] args)
        {
            if (self == null || self.Length == 0)
                return self;

            var adapter = new StripAdapter(self);
            return adapter.Remove(args);
        }

        /// <summary>
        /// Removes the specified substrings from the string.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="args">The substrings to remove.</param>
        /// <returns>The string without the specified substrings.</returns>
        public static string Remove(this Strip self, params string[] args)
        {
            if (Strip.IsNullOrEmpty(self))
                return self;

            var sb = self.ToStringBuilder(self.Length);
            sb.Remove(args);
            return sb.ToString();
        }

        #endregion

        #region RemovePrefix/RemoveSuffix/RemoveAffixes

        public static string RemovePrefix(this Strip self, string prefix)
        {
            return RemovePrefix(self, prefix, StringComparison.CurrentCulture);
        }

        public static string RemovePrefix(this Strip self, string prefix, StringComparison comparison)
        {
            if (self != null && self.StartsWith(prefix, comparison))
                return self.Substring(prefix.Length);
            return self;
        }

        public static string RemoveSuffix(this Strip self, string suffix)
        {
            return RemoveSuffix(self, suffix, StringComparison.CurrentCulture);
        }

        public static string RemoveSuffix(this Strip self, string suffix, StringComparison comparison)
        {
            if (self != null && self.EndsWith(suffix, comparison))
                return self.Substring(0, self.Length - suffix.Length);
            return self;
        }

        public static string RemoveAffixes(this Strip self, string affix)
        {
            return RemoveAffixes(self, affix, StringComparison.CurrentCulture);
        }

        public static Strip RemoveAffixes(this Strip self, string affix, StringComparison comparison)
        {
            if (self == null || self.Length < affix.Length)
                return self;
            var starts = self.StartsWith(affix, comparison);
            var ends = self.EndsWith(affix, comparison);
            if (starts & ends) {
                if (self.Equals(affix, comparison))
                    return Strip.Empty;
                return self.Substring(affix.Length, self.Length - 2 * affix.Length);
            } else if (starts) {
                return self.Substring(affix.Length);
            } else if (ends) {
                return self.Substring(0, self.Length - affix.Length);
            }
            return self;
        }

        public static string RemoveAffixes(this Strip self, string prefix, string suffix)
        {
            return RemoveAffixes(self, prefix, suffix, StringComparison.CurrentCulture);
        }

        public static string RemoveAffixes(this Strip self, string prefix, string suffix, StringComparison comparison)
        {
            if (self == null)
                return self;
            var starts = self.StartsWith(prefix, comparison);
            var ends = self.EndsWith(suffix, comparison);
            if (starts & ends) {
                if ((prefix.Length + suffix.Length) >= self.Length)
                    return Strip.Empty;
                return self.Substring(prefix.Length, self.Length - prefix.Length - suffix.Length);
            } else if (starts) {
                return self.Substring(prefix.Length);
            } else if (ends) {
                return self.Substring(0, self.Length - suffix.Length);
            }
            return self;
        }

        #endregion

        #region StartsWith & EndsWith

        public static bool StartsWith(this Strip self, char c)
        {
            if (string.IsNullOrEmpty(self))
                return false;
            return self.StartsWith(char.ToString(c));
        }
        public static bool StartsWith(this Strip self, char c, StringComparison comparison)
        {
            if (string.IsNullOrEmpty(self))
                return false;
            return self.StartsWith(char.ToString(c), comparison);
        }
        public static bool StartsWith(this Strip self, char c, bool ignoreCase, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(self))
                return false;
            return self.StartsWith(char.ToString(c), ignoreCase, culture);
        }

        public static bool EndsWith(this Strip self, char c)
        {
            if (string.IsNullOrEmpty(self))
                return false;
            return self.EndsWith(char.ToString(c));
        }
        public static bool EndsWith(this Strip self, char c, StringComparison comparison)
        {
            if (string.IsNullOrEmpty(self))
                return false;
            return self.EndsWith(char.ToString(c), comparison);
        }
        public static bool EndsWith(this Strip self, char c, bool ignoreCase, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(self))
                return false;
            return self.EndsWith(char.ToString(c), ignoreCase, culture);
        }

        #endregion

        #region Substring

        /// <summary>
        /// Extracts the substring that precedes the <paramref name="value"/> char.
        /// </summary>
        /// <param name="self">The initial string.</param>
        /// <param name="value">The delimiter char to look for.</param>
        /// <returns>Returns the substring that precedes the <paramref name="value"/> char, or null if the delimiter is not found.</returns>
        public static Strip SubstringBefore(this Strip self, char value)
        {
            var index = self.IndexOf(value);
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
        [Obsolete("Too complex.", true)]
        public static Strip SubstringBeforeOrSelf(this Strip self, char find)
        {
            return SubstringBefore(self, find) ?? self;
        }

        /// <summary>
        /// Extracts the substring that precedes the <paramref name="value"/> string.
        /// </summary>
        /// <param name="self">The initial string.</param>
        /// <param name="value">The delimiter string to look for.</param>
        /// <returns>Returns the substring that precedes the <paramref name="value"/> string, or null if the delimiter is not found.</returns>
        public static Strip SubstringBefore(this Strip self, string value)
        {
            if (string.IsNullOrEmpty(value))
                return Strip.Empty;

            var index = self.IndexOf(value, StringComparison.Ordinal);
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
        [Obsolete("Too complex.", true)]
        public static Strip SubstringBeforeOrSelf(this Strip self, string find)
        {
            return SubstringBefore(self, find) ?? self;
        }

        /// <summary>
        /// Extracts the substring that follows the <paramref name="find"/> char.
        /// </summary>
        /// <param name="self">The initial string.</param>
        /// <param name="find">The delimiter char to look for.</param>
        /// <returns>Returns the substring that follows the <paramref name="find"/> char, or null if the delimiter is not found.</returns>
        public static Strip SubstringAfter(this Strip self, char find)
        {
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
        [Obsolete("Too complex.", true)]
        public static Strip SubstringAfterOrSelf(this Strip self, char find)
        {
            return SubstringAfter(self, find) ?? self;
        }

        /// <summary>
        /// Extracts the substring that follows the <paramref name="value"/> string.
        /// </summary>
        /// <param name="self">The initial string.</param>
        /// <param name="value">The delimiter string to look for.</param>
        /// <returns>Returns the substring that follows the <paramref name="value"/> string, or null if the delimiter is not found.</returns>
        public static Strip SubstringAfter(this Strip self, string value)
        {
            if (string.IsNullOrEmpty(value))
                return self;

            var index = self.IndexOf(value, StringComparison.Ordinal);
            if (index < 0)
                return null;
            return self.Substring(index + value.Length);
        }

        /// <summary>
        /// Extracts the substring that follows the <paramref name="find"/> string.
        /// </summary>
        /// <param name="self">The initial string.</param>
        /// <param name="find">The delimiter string to look for.</param>
        /// <returns>Returns the substring that follows the <paramref name="find"/> string, or the string if the delimiter is not found.</returns>
        [Obsolete("Too complex.", true)]
        public static Strip SubstringAfterOrSelf(this Strip self, string find)
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
        public static Strip SubstringBetween(this Strip self, string prefix, string suffix)
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
        [Obsolete("Too complex.", true)]
        public static Strip SubstringBetweenOrSelf(this Strip self, string prefix, string suffix)
        {
            return SubstringBetween(self, prefix, suffix) ?? self;
        }

        /// <summary>
        /// Extracts the <paramref name="length"/> chars at the left of the string.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="length">The length.</param>
        /// <returns>The substring.</returns>
        public static Strip Left(this Strip self, int length)
        {
            if (self == null)
                return self;
            if (length < 0)
                length = self.Length - length;
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
        public static Strip Right(this Strip self, int length)
        {
            if (self == null)
                return self;
            if (length < 0)
                length = self.Length - length;
            if (length > self.Length)
                return self;
            return self.Substring(self.Length - length, length);
        }

        #endregion

        #region Tokenize

#if false
        /// <summary>
        /// Returns the sequence of substrings of this instance using a tokenizer.
        /// </summary>
        /// <typeparam name="TTokenizer">The type of tokenizer.</typeparam>
        /// <param name="self">The strings.</param>
        /// <param name="tokenizer">The tokenizer.</param>
        /// <returns>The sequence of substrings.</returns>
        public static IEnumerable<string> Tokenize<TTokenizer>(this string self, TTokenizer tokenizer)
            where TTokenizer : ITokenizer<string, string> {
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
        public static IEnumerable<string> Tokenize(this string self, char separator) {
            return Tokenize(self, new CharTokenizer(separator));
        }

        /// <summary>
        /// Returns the sequence of substrings of this instance that are delimited by a separator from a set.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="separators">The set of separators</param>
        /// <returns>The sequence of substrings.</returns>
        public static IEnumerable<string> Tokenize(this string self, params char[] separators) {
            if (separators == null || separators.Length == 0)
                return Tokenize(self, new PredicateTokenizer(char.IsWhiteSpace));
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
        public static IEnumerable<string> Tokenize(this string self, Predicate<char> isSeparator) {
            return Tokenize(self, new PredicateTokenizer(isSeparator));
        }

        /// <summary>
        /// Returns the sequence of substrings of this instance that are delimited by whitespaces.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <returns>The sequence of substrings.</returns>
        public static IEnumerable<string> Tokenize(this string self) {
            return Tokenize(self, new PredicateTokenizer(char.IsWhiteSpace));
        }
#endif

        #endregion

        #region Translate

        /// <summary>
        /// Translates the chars of the <paramref name="source "/> into the corresponding chars in the <paramref name="target"/>.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="source">The source chars.</param>
        /// <param name="target">The target chars.</param>
        /// <returns>The translated string.</returns>
        public static string Translate(this Strip self, string source, string target)
        {
            return StringExtensions.Translate(self.ToCharArray(), source, target);
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
        public static string Truncate(this Strip self, int maxLength, string ellipsis = "\u2026")
        {
            if (ellipsis == null) throw new ArgumentNullException(nameof(ellipsis));
            if (maxLength < ellipsis.Length) throw new ArgumentOutOfRangeException(nameof(maxLength));

            if (!Strip.IsNullOrEmpty(self) && self.Length > maxLength)
                return self.Substring(0, maxLength - ellipsis.Length) + ellipsis;
            return self;
        }

        #endregion
    }
}
