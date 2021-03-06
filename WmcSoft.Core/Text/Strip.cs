﻿#region Licence

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

        private readonly string _s;
        private readonly int _start;
        private readonly int _end;

        #endregion

        #region Lifecycle

        private Strip(Strip s, int start, int end)
        {
            _s = s._s;
            _start = start;
            _end = end;
        }

        public Strip()
        {
        }

        public Strip(string s)
        {
            if (s == null) throw new ArgumentNullException("s");

            _s = s;
            _start = 0;
            _end = s.Length;
        }

        public Strip(string s, int startIndex, int length)
        {
            if (s == null) throw new ArgumentNullException("s");
            if (startIndex < 0 || startIndex > s.Length) throw new ArgumentOutOfRangeException("startIndex");
            if (length < 0 || startIndex > (s.Length - length)) throw new ArgumentOutOfRangeException("length");

            _s = s;
            _start = startIndex;
            _end = startIndex + length;
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

        public int Length => _end - _start;

        public Strip Substring(int startIndex, int length)
        {
            var start = _start + startIndex;
            if (start == _end || Length == 0)
                return Empty;
            return new Strip(_s, start, start + length);
        }

        public Strip Substring(int startIndex)
        {
            var start = _start + startIndex;
            if (start == _end)
                return Empty;
            return new Strip(_s, start, Length - startIndex);
        }

        public static bool IsNullOrEmpty(Strip value)
        {
            return value == null || value._s == null || value.Length == 0;
        }

        public static bool IsNullOrWhiteSpace(Strip value)
        {
            return value == null || value._s == null || value.All(char.IsWhiteSpace);
        }

        public bool Contains(string value)
        {
            return _s.IndexOf(value, _start, Length) >= 0;
        }

        public int IndexOf(char value)
        {
            return RebaseIndex(_s.IndexOf(value, _start, Length));
        }

        public int IndexOf(string value)
        {
            return RebaseIndex(_s.IndexOf(value, _start, Length));
        }

        public int IndexOfAny(params char[] anyOf)
        {
            return RebaseIndex(_s.IndexOfAny(anyOf, _start, Length));
        }

        public int IndexOfAny(char[] anyOf, int startIndex)
        {
            return RebaseIndex(_s.IndexOfAny(anyOf, _start + startIndex, Length));
        }

        public int IndexOfAny(char[] anyOf, int startIndex, int count)
        {
            return RebaseIndex(_s.IndexOfAny(anyOf, _start + startIndex, count));
        }

        public int IndexOf(string value, StringComparison comparison)
        {
            return RebaseIndex(_s.IndexOf(value, _start, Length, comparison));
        }

        public int IndexOf(string value, int startIndex, StringComparison comparison)
        {
            return RebaseIndex(_s.IndexOf(value, _start + startIndex, Length, comparison));
        }

        public int LastIndexOf(char value)
        {
            return RebaseIndex(_s.LastIndexOf(value, _end - 1, Length));
        }

        public int LastIndexOf(string value)
        {
            return RebaseIndex(_s.LastIndexOf(value, _end - 1, Length));
        }

        public int LastIndexOf(string value, StringComparison comparisonType)
        {
            return RebaseIndex(_s.LastIndexOf(value, _end - 1, Length, comparisonType));
        }

        public int LastIndexOf(char value, int startIndex)
        {
            return RebaseIndex(_s.LastIndexOf(value, _start + startIndex, Length));
        }

        public int LastIndexOf(string value, int startIndex)
        {
            return RebaseIndex(_s.LastIndexOf(value, _start + startIndex, Length));
        }

        public int LastIndexOf(string value, int startIndex, StringComparison comparisonType)
        {
            return RebaseIndex(_s.LastIndexOf(value, _start + startIndex, Length, comparisonType));
        }

        public int LastIndexOf(char value, int startIndex, int count)
        {
            return RebaseIndex(_s.LastIndexOf(value, _start + startIndex, count));
        }

        public int LastIndexOf(string value, int startIndex, int count)
        {
            return RebaseIndex(_s.LastIndexOf(value, _start + startIndex, count));
        }

        public int LastIndexOf(string value, int startIndex, int count, StringComparison comparisonType)
        {
            return RebaseIndex(_s.LastIndexOf(value, _start + startIndex, count, comparisonType));
        }

        public int LastIndexOfAny(params char[] anyOf)
        {
            return RebaseIndex(_s.LastIndexOfAny(anyOf, _end - 1, Length));
        }

        public int LastIndexOfAny(char[] anyOf, int startIndex)
        {
            return RebaseIndex(_s.LastIndexOfAny(anyOf, _start + startIndex, Length));
        }

        public int LastIndexOfAny(char[] anyOf, int startIndex, int count)
        {
            return RebaseIndex(_s.LastIndexOfAny(anyOf, _start + startIndex, count));
        }

        public Strip Trim()
        {
            var end = _end - 1; ;
            for (; end >= _start; end--) {
                if (!char.IsWhiteSpace(_s[end]))
                    break;
            }

            var start = _start;
            for (; start < end; start++) {
                if (!char.IsWhiteSpace(_s[start]))
                    break;
            }

            return new Strip(this, start, end + 1);
        }

        public Strip Trim(params char[] trimChars)
        {
            if (trimChars == null || trimChars.Length == 0)
                return Trim();

            var end = _end - 1;
            var start = _start;
            if (trimChars.Length == 1) {
                var ch = trimChars[0];

                for (; end >= _start; end--) {
                    if (_s[end] != ch)
                        break;
                }

                for (; start < _end; start++) {
                    if (_s[start] != ch)
                        break;
                }
            } else {
                for (; end >= _start; end--) {
                    if (Array.IndexOf(trimChars, _s[end]) < 0)
                        break;
                }

                for (; start < end; start++) {
                    if (Array.IndexOf(trimChars, _s[start]) < 0)
                        break;
                }
            }

            return new Strip(this, start, end + 1);
        }

        public Strip TrimEnd(params char[] trimChars)
        {
            var end = _end - 1;
            if (trimChars == null || trimChars.Length == 0) {
                for (; end >= _start; end--) {
                    if (!char.IsWhiteSpace(_s[end]))
                        break;
                }
            } else if (trimChars.Length == 1) {
                var ch = trimChars[0];
                for (; end >= _start; end--) {
                    if (_s[end] != ch)
                        break;
                }
            } else {
                for (; end >= _start; end--) {
                    if (Array.IndexOf(trimChars, _s[end]) < 0)
                        break;
                }
            }

            return new Strip(this, _start, end + 1);
        }

        public Strip TrimStart(params char[] trimChars)
        {
            var start = _start;
            if (trimChars == null || trimChars.Length == 0) {
                for (; start < _end; start++) {
                    if (!char.IsWhiteSpace(_s[start]))
                        break;
                }
            } else if (trimChars.Length == 1) {
                var ch = trimChars[0];
                for (; start < _end; start++) {
                    if (_s[start] != ch)
                        break;
                }
            } else {
                for (; start < _end; start++) {
                    if (Array.IndexOf(trimChars, _s[start]) < 0)
                        break;
                }
            }

            return new Strip(this, start, _end);
        }

        public bool StartsWith(Strip value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            return DoEqual(_s, _start, value._s, value._start, length, CultureInfo.CurrentCulture, CompareOptions.None);
        }

        public bool StartsWith(Strip value, bool ignoreCase, CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            var options = ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None;
            return DoEqual(_s, _start, value._s, value._start, length, culture ?? CultureInfo.CurrentCulture, options);
        }

        public bool StartsWith(Strip value, StringComparison comparisonType)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            return DoEqual(_s, _start, value._s, value._start, length, comparisonType);
        }

        public bool StartsWith(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            return DoEqual(_s, _start, value, 0, length, CultureInfo.CurrentCulture, CompareOptions.None);
        }

        public bool StartsWith(string value, bool ignoreCase, CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            var options = ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None;
            return DoEqual(_s, _start, value, 0, length, culture ?? CultureInfo.CurrentCulture, options);
        }

        public bool StartsWith(string value, StringComparison comparisonType)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            return DoEqual(_s, _start, value, 0, length, comparisonType);
        }

        public bool EndsWith(Strip value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            return DoEqual(_s, _end - length, value._s, value._start, length, CultureInfo.CurrentCulture, CompareOptions.None);
        }

        public bool EndsWith(Strip value, bool ignoreCase, CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            var options = ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None;
            return DoEqual(_s, _end - length, value._s, value._start, length, culture ?? CultureInfo.CurrentCulture, options);
        }

        public bool EndsWith(Strip value, StringComparison comparisonType)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            return DoEqual(_s, _end - length, value._s, value._start, length, comparisonType);
        }

        public bool EndsWith(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            return DoEqual(_s, _end - length, value, 0, length, CultureInfo.CurrentCulture, CompareOptions.None);
        }

        public bool EndsWith(string value, bool ignoreCase, CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            var options = ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None;
            return DoEqual(_s, _end - length, value, 0, length, culture ?? CultureInfo.CurrentCulture, options);
        }

        public bool EndsWith(string value, StringComparison comparisonType)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var length = value.Length;
            if (Length < length)
                return false;
            return DoEqual(_s, _end - length, value, 0, length, comparisonType);
        }

        public char[] ToCharArray()
        {
            return _s.ToCharArray(_start, Length);
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return _s.Substring(_start, Length);
        }

        public override bool Equals(object obj)
        {
            var that = obj as Strip;
            if (that == null) {
                if (!(obj is string s))
                    return false;

                that = new Strip(s);
            }
            return 0 == DoCompare(this, that, CultureInfo.CurrentCulture, CompareOptions.None);
        }

        public override int GetHashCode()
        {
            if (_start == _end)
                return 0;
            var h = _s.GetHashCode();
            h = h * 65537 ^ _start;
            h = h * 65537 ^ _end;
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
            return DoEqual(_s, _start, other, 0, other.Length, comparisonType);
        }

        public int CompareTo(string other)
        {
            var culture = CultureInfo.CurrentCulture;
            return culture.CompareInfo.Compare(_s, _start, Length, other, 0, other == null ? 0 : other.Length, CompareOptions.None);
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
            return culture.CompareInfo.Compare(strA._s, strA._start, strA.Length, strB._s, strB._start, strB.Length, options);
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
            get { return _s[_start + index]; }
        }

        public IEnumerator<char> GetEnumerator()
        {
            for (int i = _start; i < _end; i++) {
                yield return _s[i];
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
            return (index != -1) ? index - _start : -1;
        }

        // The following are internal because, for convenience, they are exposed as extensions on StringBuilder
        // Making the private fields internal was not an option to protect the encapsulation of the type.
        internal StringBuilder AppendTo(StringBuilder sb)
        {
            return sb.Append(_s, _start, Length);
        }

        internal StringBuilder AppendTo(StringBuilder sb, int startIndex, int count)
        {
            return sb.Append(_s, _start + startIndex, count);
        }

        internal StringBuilder PrependTo(StringBuilder sb)
        {
            if (string.IsNullOrEmpty(_s) || Length == 0)
                return sb;
            return sb.Insert(0, _s.Substring(_start, Length));
        }

        internal StringBuilder InsertInto(StringBuilder sb, int index)
        {
            if (string.IsNullOrEmpty(_s) || Length == 0)
                return sb;
            return sb.Insert(index, _s.Substring(_start, Length));
        }

        internal StringBuilder ToStringBuilder(int capacity = 0)
        {
            return new StringBuilder(_s, _start, Length, capacity);
        }

        #endregion
    }
}
