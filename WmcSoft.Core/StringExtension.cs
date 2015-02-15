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
using System.Linq;
using System.Text;
using System.Globalization;
using WmcSoft.Collections.Generic;

namespace WmcSoft
{
    public static class StringExtension
    {
        #region Substring

        public static string SubstringBefore(this string self, char find) {
            if (String.IsNullOrEmpty(self))
                return self;

            int index = self.IndexOf(find);
            if (index < 0)
                return null;
            return self.Substring(0, index);
        }

        public static string SubstringBefore(this string self, string find) {
            if (String.IsNullOrEmpty(self) || String.IsNullOrEmpty(find))
                return self;

            int index = self.IndexOf(find, StringComparison.Ordinal);
            if (index < 0)
                return null;
            return self.Substring(0, index);
        }

        public static string SubstringAfter(this string self, char find) {
            if (String.IsNullOrEmpty(self))
                return self;

            int index = self.IndexOf(find);
            if (index < 0)
                return null;
            return self.Substring(index + 1);
        }

        public static string SubstringAfter(this string self, string find) {
            if (String.IsNullOrEmpty(self) || String.IsNullOrEmpty(find))
                return self;

            int index = self.IndexOf(find, StringComparison.Ordinal);
            if (index < 0)
                return null;
            return self.Substring(index + find.Length);
        }

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

        public static string Left(this string value, int length) {
            return value.Substring(0, length);
        }
        public static string Mid(this string value, int start, int length) {
            return value.Substring(start, length);
        }
        public static string Right(this string value, int length) {
            return value.Substring(value.Length - length, length);
        }

        #endregion

        #region Case operations

        public static string Capitalize(this string self, CultureInfo culture) {
            if (String.IsNullOrEmpty(self))
                return self;
            return Char.ToUpper(self[0], culture) + self.Substring(1).ToLower(culture);
        }

        public static string Capitalize(this string self) {
            if (String.IsNullOrEmpty(self))
                return self;
            return Char.ToUpper(self[0]) + self.Substring(1).ToLower();
        }

        public static string[] Capitalize(this string[] self, CultureInfo culture) {
            List<string> list = new List<string>(self.Length);
            foreach (string s in self) {
                list.Add(Capitalize(s, culture));
            }
            return list.ToArray();
        }

        public static string[] Capitalize(this string[] self) {
            List<string> list = new List<string>(self.Length);
            foreach (string s in self) {
                list.Add(Capitalize(s));
            }
            return list.ToArray();
        }

        public static string[] ToUpper(this string[] self, CultureInfo culture) {
            List<string> list = new List<string>(self.Length);
            foreach (string s in self) {
                list.Add(s.ToUpper(culture));
            }
            return list.ToArray();
        }

        public static string[] ToUpper(this string[] self) {
            List<string> list = new List<string>(self.Length);
            foreach (string s in self) {
                list.Add(s.ToUpper());
            }
            return list.ToArray();
        }

        public static string[] ToUpperInvariant(this string[] self) {
            List<string> list = new List<string>(self.Length);
            foreach (string s in self) {
                list.Add(s.ToUpperInvariant());
            }
            return list.ToArray();
        }

        public static string[] ToLower(this string[] self, CultureInfo culture) {
            List<string> list = new List<string>(self.Length);
            foreach (string s in self) {
                list.Add(s.ToLower(culture));
            }
            return list.ToArray();
        }

        public static string[] ToLower(this string[] self) {
            List<string> list = new List<string>(self.Length);
            foreach (string s in self) {
                list.Add(s.ToLower());
            }
            return list.ToArray();
        }

        public static string[] ToLowerInvariant(this string[] self) {
            List<string> list = new List<string>(self.Length);
            foreach (string s in self) {
                list.Add(s.ToLowerInvariant());
            }
            return list.ToArray();
        }

        #endregion

        #region Join

        public static string Join(this IEnumerable<string> self) {
            StringBuilder sb = new StringBuilder();
            foreach (string s in self) {
                sb.Append(s);
            }
            return sb.ToString();
        }

        public static string Join(this IEnumerable<string> self, string separator) {
            var sb = new StringBuilder();
            var enumerator = self.GetEnumerator();
            if (enumerator.MoveNext()) {
                sb.Append(enumerator.Current);
                while (enumerator.MoveNext())
                    sb.Append(separator).Append(enumerator.Current);
            }
            return sb.ToString();
        }

        public static string Join(this IEnumerable<string> self, char separator) {
            var sb = new StringBuilder();
            var enumerator = self.GetEnumerator();
            if (enumerator.MoveNext()) {
                sb.Append(enumerator.Current);
                while (enumerator.MoveNext())
                    sb.Append(separator).Append(enumerator.Current);
            }
            return sb.ToString();
        }

        #endregion

        #region FormatWith

        public static string FormatWith(this string format, params object[] args) {
            return String.Format(format, args);
        }

        public static string FormatWith(this string format, object arg0) {
            return String.Format(format, arg0);
        }

        public static string FormatWith(this string format, object arg0, object arg1) {
            return String.Format(format, arg0, arg1);
        }

        public static string FormatWith(this string format, object arg0, object arg1, object arg2) {
            return String.Format(format, arg0, arg1, arg2);
        }

        public static string FormatWith(this string format, IFormatProvider provider, params object[] args) {
            return String.Format(format, provider, args);
        }

        #endregion

        #region Remove

        public static string Remove(this string self, params char[] args) {
            StringBuilder sb = new StringBuilder(self);
            foreach (var arg in args) {
                sb.Replace(arg.ToString(), "");
            }
            return sb.ToString();
        }

        public static string Remove(this string self, params string[] args) {
            StringBuilder sb = new StringBuilder(self);
            foreach (var arg in args) {
                sb.Replace(arg, "");
            }
            return sb.ToString();
        }

        #endregion

        #region Translate

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

        public static string ToSlug(this string self) {
            if (String.IsNullOrEmpty(self))
                return String.Empty;
            var sb = new StringBuilder(self.Trim().ToLowerInvariant().Translate(
                "àéèêëîïôùüçñ•+/ &|[](){}?!;:,.°",
                "aeeeeiiouucn--____"));
            sb.Replace("œ", "oe")
                .Replace("æ", "ae")
                .Replace("\"", "")
                .Replace("l\'", "")
                .Replace("l’", "")
                .Replace("d\'", "")
                .Replace("d’", "")
                .Replace("'", "")
                .Replace("’", "")
                .Replace("_-_-_", "-")
                .Replace("_-_", "-")
                .Replace("___", "_")
                .Replace("__", "_")
                ;
            return sb.ToString();
        }

        #endregion

        #region AnyOf

        /// <summary>
        /// Check if the char is any of the candidates chars.
        /// </summary>
        /// <param name="self">The char to test</param>
        /// <param name="candidates">Candidates char to test against the char</param>
        /// <returns>true if the char is any of the candidates, otherwise false.</returns>
        public static bool AnyOf(this char self, params char[] candidates) {
            for (int i = 0; i < candidates.Length; i++) {
                if (self == candidates[i])
                    return true;
            }
            return false;
        }

        public static bool ContainsAnyOf(this string self, params char[] candidates) {
            Array.Sort(candidates);
            return BinaryContainsAnyOf(self, candidates);
        }

        public static bool BinaryContainsAnyOf(this string self, params char[] candidates) {
            foreach (var c in self) {
                if (Array.BinarySearch(candidates, c) >= 0)
                    return true;
            }
            return false;
        }

        public static bool AnyOf(this string self, StringComparison comparisonType, params string[] candidates) {
            if (String.IsNullOrEmpty(self))
                return false;
            for (int i = 0; i < candidates.Length; i++) {
                if (self.Equals(candidates[i], comparisonType))
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
        public static bool BinaryAnyOf(this char self, char[] candidates) {
            return Array.BinarySearch(candidates, self) >= 0;
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
