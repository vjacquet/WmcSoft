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
using System.Text;
using System.Globalization;

namespace WmcSoft
{
    public static class StringExtension
    {
        #region Substring

        public static string SubstringBefore(this string self, string find) {
            if (String.IsNullOrEmpty(self) || String.IsNullOrEmpty(find))
                return self;

            int index = self.IndexOf(find, StringComparison.Ordinal);
            if (index < 0)
                return self;
            return self.Substring(0, index);
        }

        public static string SubstringAfter(this string self, string find) {
            if (String.IsNullOrEmpty(self) || String.IsNullOrEmpty(find))
                return self;

            int index = self.IndexOf(find, StringComparison.Ordinal);
            if (index < 0)
                return self;
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

        #endregion

        #region Case operations

        public static string Capitalize(this string self, CultureInfo culture) {
            if (String.IsNullOrEmpty(self))
                return String.Empty;
            return Char.ToUpper(self[0], culture) + self.Substring(1).ToLower(culture);
        }

        public static string Capitalize(this string self) {
            if (String.IsNullOrEmpty(self))
                return String.Empty;
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
    }
}
