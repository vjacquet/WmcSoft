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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace WmcSoft.Text
{
    /// <summary>
    /// Represents a lazy substring of a string.
    /// </summary>
    public sealed class Strip : IEquatable<string>, IComparable<string>, IReadOnlyList<char>, ICloneable
    {
        #region Public fields

        internal static readonly Strip Null = new Strip();
        public static readonly Strip Empty = new Strip(string.Empty, 0, 0);

        #endregion

        #region Fields

        private readonly string s;
        private readonly int start;
        private readonly int end;

        #endregion

        #region Lifecycle

        private Strip(Strip s, int start, int end)
        {
            this.s = s.s;
            this.start = start;
            this.end = end;
        }

        public Strip()
        {
        }

        public Strip(string s)
        {
            if (s == null) throw new ArgumentNullException("s");

            this.s = s;
            start = 0;
            end = s.Length;
        }

        public Strip(string s, int startIndex, int length)
        {
            if (s == null) throw new ArgumentNullException("s");
            if (startIndex < 0 || startIndex > s.Length) throw new ArgumentOutOfRangeException("startIndex");
            if (length < 0 || startIndex > (s.Length - length)) throw new ArgumentOutOfRangeException("length");

            this.s = s;
            start = startIndex;
            end = startIndex + length;
        }

        public static implicit operator string(Strip s)
        {
            return s?.ToString();
        }

        public static explicit operator Strip(string s)
        {
            return new Strip(s);
        }

        #endregion

        #region Operators

        public static bool operator ==(Strip a, Strip b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(Strip a, Strip b)
        {
            return !Equals(a, b);
        }

        #endregion

        #region String-like properties & methods

        public int Length => end - start;

        public Strip Substring(int startIndex, int length)
        {
            var start = this.start + startIndex;
            if (start == end || Length == 0)
                return Empty;
            return new Strip(s, start, start + length);
        }

        public Strip Substring(int startIndex)
        {
            var start = this.start + startIndex;
            if (start == end)
                return Empty;
            return new Strip(s, start, Length - startIndex);
        }

        public static bool IsNullOrEmpty(Strip value)
        {
            return value == null || value.s == null || value.Length == 0;
        }

        public static bool IsNullOrWhiteSpace(Strip value)
        {
            return value == null || value.s == null || value.All(char.IsWhiteSpace);
        }

        public bool Contains(string value)
        {
            return s.IndexOf(value, start, Length) >= 0;
        }

        public int IndexOf(char value)
        {
            return RebaseIndex(s.IndexOf(value, start, Length));
        }

        public int IndexOf(string value)
        {
            return RebaseIndex(s.IndexOf(value, start, Length));
        }

        public int IndexOfAny(params char[] anyOf)
        {
            return RebaseIndex(s.IndexOfAny(anyOf, start, Length));
        }

        public int IndexOfAny(char[] anyOf, int startIndex)
        {
            return RebaseIndex(s.IndexOfAny(anyOf, start + startIndex, Length));
        }

        public int IndexOfAny(char[] anyOf, int startIndex, int count)
        {
            return RebaseIndex(s.IndexOfAny(anyOf, start + startIndex, count));
        }

        public int IndexOf(string value, StringComparison comparison)
        {
            return RebaseIndex(s.IndexOf(value, start, Length, comparison));
        }

        public int IndexOf(string value, int startIndex, StringComparison comparison)
        {
            return RebaseIndex(s.IndexOf(value, start + startIndex, Length, comparison));
        }

        public int LastIndexOf(char value)
        {
            return RebaseIndex(s.LastIndexOf(value, end - 1, Length));
        }

        public int LastIndexOf(string value)
        {
            return RebaseIndex(s.LastIndexOf(value, end - 1, Length));
        }

        public int LastIndexOf(string value, StringComparison comparisonType)
        {
            return RebaseIndex(s.LastIndexOf(value, end - 1, Length, comparisonType));
        }

        public int LastIndexOf(char value, int startIndex)
        {
            return RebaseIndex(s.LastIndexOf(value, start + startIndex, Length));
        }

        public int LastIndexOf(string value, int startIndex)
        {
            return RebaseIndex(s.LastIndexOf(value, start + startIndex, Length));
        }

        public int LastIndexOf(string value, int startIndex, StringComparison comparisonType)
        {
            return RebaseIndex(s.LastIndexOf(value, start + startIndex, Length, comparisonType));
        }

        public int LastIndexOf(char value, int startIndex, int count)
        {
            return RebaseIndex(s.LastIndexOf(value, start + startIndex, count));
        }

        public int LastIndexOf(string value, int startIndex, int count)
        {
            return RebaseIndex(s.LastIndexOf(value, start + startIndex, count));
        }

        public int LastIndexOf(string value, int startIndex, int count, StringComparison comparisonType)
        {
            return RebaseIndex(s.LastIndexOf(value, start + startIndex, count, comparisonType));
        }

        public int LastIndexOfAny(params char[] anyOf)
        {
            return RebaseIndex(s.LastIndexOfAny(anyOf, end - 1, Length));
        }

        public int LastIndexOfAny(char[] anyOf, int startIndex)
        {
            return RebaseIndex(s.LastIndexOfAny(anyOf, start + startIndex, Length));
        }

        public int LastIndexOfAny(char[] anyOf, int startIndex, int count)
        {
            return RebaseIndex(s.LastIndexOfAny(anyOf, start + startIndex, count));
        }

        public Strip Trim()
        {
            var end = this.end - 1; ;
            for (; end >= this.start; end--) {
                if (!char.IsWhiteSpace(s[end]))
                    break;
            }

            var start = this.start;
            for (; start < end; start++) {
                if (!char.IsWhiteSpace(s[start]))
                    break;
            }

            return new Strip(this, start, end + 1);
        }

        public Strip Trim(params char[] trimChars)
        {
            if (trimChars == null || trimChars.Length == 0)
                return Trim();

            var end = this.end - 1;
            var start = this.start;
            if (trimChars.Length == 1) {
                var ch = trimChars[0];

                for (; end >= this.start; end--) {
                    if (s[end] != ch)
                        break;
                }

                for (; start < this.end; start++) {
                    if (s[start] != ch)
                        break;
                }
            } else {
                for (; end >= this.start; end--) {
                    if (Array.IndexOf(trimChars, s[end]) < 0)
                        break;
                }

                for (; start < end; start++) {
                    if (Array.IndexOf(trimChars, s[start]) < 0)
                        break;
                }
            }

            return new Strip(this, start, end + 1);
        }

        public Strip TrimEnd(params char[] trimChars)
        {
            var end = this.end - 1;
            if (trimChars == null || trimChars.Length == 0) {
                for (; end >= start; end--) {
                    if (!char.IsWhiteSpace(s[end]))
                        break;
                }
            } else if (trimChars.Length == 1) {
                var ch = trimChars[0];
                for (; end >= start; end--) {
                    if (s[end] != ch)
                        break;
                }
            } else {
                for (; end >= start; end--) {
                    if (Array.IndexOf(trimChars, s[end]) < 0)
                        break;
                }
            }

            return new Strip(this, start, end + 1);
        }

        public Strip TrimStart(params char[] trimChars)
        {
            var start = this.start;
            if (trimChars == null || trimChars.Length == 0) {
                for (; start < end; start++) {
                    if (!char.IsWhiteSpace(s[start]))
                        break;
                }
            } else if (trimChars.Length == 1) {
                var ch = trimChars[0];
                for (; start < end; start++) {
                    if (s[start] != ch)
                        break;
                }
            } else {
                for (; start < end; start++) {
                    if (Array.IndexOf(trimChars, s[start]) < 0)
                        break;
                }
            }

            return new Strip(this, start, end);
        }

        public bool StartsWith(Strip value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            return DoEqual(s, start, value.s, value.start, length, CultureInfo.CurrentCulture, CompareOptions.None);
        }

        public bool StartsWith(Strip value, bool ignoreCase, CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            var options = ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None;
            return DoEqual(s, start, value.s, value.start, length, culture ?? CultureInfo.CurrentCulture, options);
        }

        public bool StartsWith(Strip value, StringComparison comparisonType)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            return DoEqual(s, start, value.s, value.start, length, comparisonType);
        }

        public bool StartsWith(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            return DoEqual(s, start, value, 0, length, CultureInfo.CurrentCulture, CompareOptions.None);
        }

        public bool StartsWith(string value, bool ignoreCase, CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            var options = ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None;
            return DoEqual(s, start, value, 0, length, culture ?? CultureInfo.CurrentCulture, options);
        }

        public bool StartsWith(string value, StringComparison comparisonType)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            return DoEqual(s, start, value, 0, length, comparisonType);
        }

        public bool EndsWith(Strip value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            return DoEqual(s, end - length, value.s, value.start, length, CultureInfo.CurrentCulture, CompareOptions.None);
        }

        public bool EndsWith(Strip value, bool ignoreCase, CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            var options = ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None;
            return DoEqual(s, end - length, value.s, value.start, length, culture ?? CultureInfo.CurrentCulture, options);
        }

        public bool EndsWith(Strip value, StringComparison comparisonType)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            return DoEqual(s, end - length, value.s, value.start, length, comparisonType);
        }

        public bool EndsWith(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            return DoEqual(s, end - length, value, 0, length, CultureInfo.CurrentCulture, CompareOptions.None);
        }

        public bool EndsWith(string value, bool ignoreCase, CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            var options = ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None;
            return DoEqual(s, end - length, value, 0, length, culture ?? CultureInfo.CurrentCulture, options);
        }

        public bool EndsWith(string value, StringComparison comparisonType)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            return DoEqual(s, end - length, value, 0, length, comparisonType);
        }

        public char[] ToCharArray()
        {
            return s.ToCharArray(start, Length);
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return s.Substring(start, Length);
        }

        public override bool Equals(object obj)
        {
            var that = obj as Strip;
            if (that == null) {
                var s = obj as string;
                if (s == null)
                    return false;

                that = new Strip(s);
            }
            return 0 == DoCompare(this, that, CultureInfo.CurrentCulture, CompareOptions.None);
        }

        public override int GetHashCode()
        {
            if (start == end)
                return 0;
            var h = s.GetHashCode();
            h = h * 65537 ^ start;
            h = h * 65537 ^ end;
            return h;
        }

        #endregion

        #region Compare

        public bool Equals(string other)
        {
            return CompareTo(other) == 0;
        }

        public bool Equals(string other, StringComparison comparisonType)
        {
            if (other == null || Length != other.Length)
                return false;
            return DoEqual(s, start, other, 0, other.Length, comparisonType);
        }

        public int CompareTo(string other)
        {
            var culture = CultureInfo.CurrentCulture;
            return culture.CompareInfo.Compare(s, start, Length, other, 0, other == null ? 0 : other.Length, CompareOptions.None);
        }

        public static bool Equals(Strip a, Strip b)
        {
            return 0 == DoCompare(a ?? Null, b ?? Null, CultureInfo.CurrentCulture, CompareOptions.None);
        }

        public static bool Equals(Strip a, Strip b, StringComparison comparisonType)
        {
            return 0 == Compare(a ?? Null, b ?? Null, comparisonType);

        }
        public static int Compare(Strip strA, Strip strB)
        {
            return DoCompare(strA ?? Null, strB ?? Null, CultureInfo.CurrentCulture, CompareOptions.None);
        }

        public static int CompareOrdinal(Strip strA, Strip strB)
        {
            return DoCompare(strA ?? Null, strB ?? Null, CultureInfo.InvariantCulture, CompareOptions.Ordinal);
        }

        public static int Compare(Strip strA, Strip strB, StringComparison comparisonType)
        {
            switch (comparisonType) {
            case StringComparison.CurrentCulture:
                return DoCompare(strA ?? Null, strB ?? Null, CultureInfo.CurrentCulture, CompareOptions.None);
            case StringComparison.CurrentCultureIgnoreCase:
                return DoCompare(strA ?? Null, strB ?? Null, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase);
            case StringComparison.InvariantCulture:
                return DoCompare(strA ?? Null, strB ?? Null, CultureInfo.InvariantCulture, CompareOptions.None);
            case StringComparison.InvariantCultureIgnoreCase:
                return DoCompare(strA ?? Null, strB ?? Null, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase);
            case StringComparison.Ordinal:
                return DoCompare(strA ?? Null, strB ?? Null, CultureInfo.InvariantCulture, CompareOptions.Ordinal);
            case StringComparison.OrdinalIgnoreCase:
                return DoCompare(strA ?? Null, strB ?? Null, CultureInfo.InvariantCulture, CompareOptions.OrdinalIgnoreCase);
            default:
                throw new NotSupportedException();
            }
        }

        public static int Compare(Strip strA, Strip strB, bool ignoreCase)
        {
            return DoCompare(strA ?? Null, strB ?? Null, CultureInfo.CurrentCulture, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
        }

        public static int Compare(Strip strA, Strip strB, bool ignoreCase, CultureInfo culture)
        {
            if (culture == null) throw new ArgumentNullException("culture");

            return DoCompare(strA ?? Null, strB ?? Null, culture, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
        }

        public static int Compare(Strip strA, Strip strB, CultureInfo culture, CompareOptions options)
        {
            if (culture == null) throw new ArgumentNullException("culture");

            return DoCompare(strA ?? Null, strB ?? Null, culture, options);
        }

        static int DoCompare(Strip strA, Strip strB, CultureInfo culture, CompareOptions options)
        {
            return culture.CompareInfo.Compare(strA.s, strA.start, strA.Length, strB.s, strB.start, strB.Length, options);
        }

        static bool DoEqual(string strA, int idxA, string strB, int idxB, int length, StringComparison comparisonType)
        {
            switch (comparisonType) {
            case StringComparison.CurrentCulture:
                return DoEqual(strA, idxA, strB, idxB, length, CultureInfo.CurrentCulture, CompareOptions.None);
            case StringComparison.CurrentCultureIgnoreCase:
                return DoEqual(strA, idxA, strB, idxB, length, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase);
            case StringComparison.InvariantCulture:
                return DoEqual(strA, idxA, strB, idxB, length, CultureInfo.InvariantCulture, CompareOptions.None);
            case StringComparison.InvariantCultureIgnoreCase:
                return DoEqual(strA, idxA, strB, idxB, length, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase);
            case StringComparison.Ordinal:
                return DoEqual(strA, idxA, strB, idxB, length, CultureInfo.InvariantCulture, CompareOptions.Ordinal);
            case StringComparison.OrdinalIgnoreCase:
                return DoEqual(strA, idxA, strB, idxB, length, CultureInfo.InvariantCulture, CompareOptions.OrdinalIgnoreCase);
            default:
                throw new NotSupportedException();
            }
        }

        static bool DoEqual(string strA, int idxA, string strB, int idxB, int length, CultureInfo culture, CompareOptions options)
        {
            return 0 == culture.CompareInfo.Compare(strA, idxA, length, strB, idxB, length, options);
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return this;
        }

        #endregion

        #region IReadOnlyList<char> Members

        int IReadOnlyCollection<char>.Count => Length;

        public char this[int index] {
            get { return s[start + index]; }
        }

        public IEnumerator<char> GetEnumerator()
        {
            for (int i = start; i < end; i++) {
                yield return s[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Helpers

        int RebaseIndex(int index)
        {
            return (index != -1) ? index - start : -1;
        }

        // The following are internal because, for convenience, they are exposed as extensions on StringBuilder
        // Making the private fields internal was not an option to protect the encapsulation of the type.
        internal StringBuilder AppendTo(StringBuilder sb)
        {
            return sb.Append(s, start, Length);
        }

        internal StringBuilder AppendTo(StringBuilder sb, int startIndex, int count)
        {
            return sb.Append(s, start + startIndex, count);
        }

        internal StringBuilder PrependTo(StringBuilder sb)
        {
            if (string.IsNullOrEmpty(s) || Length == 0)
                return sb;
            return sb.Insert(0, s.Substring(start, Length));
        }

        internal StringBuilder InsertInto(StringBuilder sb, int index)
        {
            if (string.IsNullOrEmpty(s) || Length == 0)
                return sb;
            return sb.Insert(index, s.Substring(start, Length));
        }

        internal StringBuilder ToStringBuilder(int capacity = 0)
        {
            return new StringBuilder(s, start, Length, capacity);
        }

        #endregion
    }
}
