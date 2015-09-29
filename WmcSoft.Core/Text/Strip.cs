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

namespace WmcSoft.Text
{
    public sealed class Strip : IComparable<string>, IReadOnlyList<char>, ICloneable<Strip>
    {
        static readonly Strip Null = new Strip((string)null, 0, 0);
        public static readonly Strip Empty = new Strip(String.Empty, 0, 0);

        #region Fields

        private readonly string _s;
        private readonly int _start;
        private readonly int _end;

        #endregion

        #region Lifecycle

        private Strip(Strip s, int start, int end) {
            _s = s._s;
            _start = start;
            _end = end;
        }

        public Strip(string s, int startIndex, int length) {
            if (s == null) throw new ArgumentNullException("s");
            if (startIndex < 0 || startIndex > s.Length) throw new ArgumentOutOfRangeException("startIndex");
            if (length < 0 || startIndex > (s.Length - length)) throw new ArgumentOutOfRangeException("length");

            _s = s;
            _start = startIndex;
            _end = startIndex + length;
        }

        #endregion

        #region String-like properties & methods

        public int Length {
            get { return _end - _start; }
        }

        public Strip Substring(int startIndex, int length) {
            var start = _start + startIndex;
            if (start == _end || Length == 0)
                return Empty;
            return new Strip(_s, start, start + length);
        }

        public Strip Substring(int startIndex) {
            var start = _start + startIndex;
            if (start == _end)
                return Empty;
            return new Strip(_s, start, Length - startIndex);
        }

        public static bool IsNullOrEmpty(Strip value) {
            return value == null || value._s == null || value.Length == 0;
        }

        public static bool IsNullOrWhiteSpace(Strip value) {
            return value == null || value._s == null || value.All(Char.IsWhiteSpace);
        }

        public bool Contains(string value) {
            return IndexOf(value) >= 0;
        }

        public int IndexOf(char value) {
            return _s.IndexOf(value, _start, Length);
        }

        public int IndexOf(string value) {
            return _s.IndexOf(value, _start, Length);
        }

        public int IndexOfAny(params char[] anyOf) {
            return _s.IndexOfAny(anyOf, _start, Length);
        }

        public int IndexOf(string value, StringComparison comparisonType) {
            return _s.IndexOf(value, _start, Length, comparisonType);
        }

        public int LastIndexOf(char value) {
            return _s.LastIndexOf(value, _start, Length);
        }

        public int LastIndexOf(string value) {
            return _s.IndexOf(value, _start, Length);
        }

        public int LastIndexOf(string value, StringComparison comparisonType) {
            return _s.IndexOf(value, _start, Length, comparisonType);
        }

        public int LastIndexOfAny(params char[] anyOf) {
            return _s.LastIndexOfAny(anyOf, _start, Length);
        }

        #endregion

        #region Overrides

        public override string ToString() {
            return _s.Substring(_start, Length);
        }

        #endregion

        #region Compare

        public int CompareTo(string other) {
            var culture = CultureInfo.CurrentCulture;
            return culture.CompareInfo.Compare(_s, _start, Length, other, 0, other == null ? 0 : other.Length, CompareOptions.None);
        }

        public static bool Equals(Strip a, Strip b) {
            return 0 == DoCompare(a ?? Null, b ?? Null, CultureInfo.CurrentCulture, CompareOptions.None);
        }

        public static bool Equals(Strip a, Strip b, StringComparison comparisonType) {
            return 0 == Compare(a ?? Null, b ?? Null, comparisonType);

        }
        public static int Compare(Strip strA, Strip strB) {
            return DoCompare(strA ?? Null, strB ?? Null, CultureInfo.CurrentCulture, CompareOptions.None);
        }

        public static int CompareOrdinal(Strip strA, Strip strB) {
            return DoCompare(strA ?? Null, strB ?? Null, CultureInfo.InvariantCulture, CompareOptions.Ordinal);
        }

        public static int Compare(Strip strA, Strip strB, StringComparison comparisonType) {
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

        public static int Compare(Strip strA, Strip strB, bool ignoreCase) {
            return DoCompare(strA ?? Null, strB ?? Null, CultureInfo.CurrentCulture, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
        }

        public static int Compare(Strip strA, Strip strB, bool ignoreCase, CultureInfo culture) {
            if (culture == null) throw new ArgumentNullException("culture");

            return DoCompare(strA ?? Null, strB ?? Null, culture, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
        }

        public static int Compare(Strip strA, Strip strB, CultureInfo culture, CompareOptions options) {
            if (culture == null) throw new ArgumentNullException("culture");

            return DoCompare(strA ?? Null, strB ?? Null, culture, options);
        }

        static int DoCompare(Strip strA, Strip strB, CultureInfo culture, CompareOptions options) {
            return culture.CompareInfo.Compare(strA._s, strA._start, strA.Length, strB._s, strB._start, strB.Length, options);
        }

        #endregion

        #region IReadOnlyList<char> Members

        int IReadOnlyCollection<char>.Count {
            get { return Length; }
        }

        public char this[int index] {
            get { return _s[_start + index]; }
        }

        public IEnumerator<char> GetEnumerator() {
            for (int i = _start; i < _end; i++) {
                yield return _s[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

        #region ICloneable<Strip> Members

        public Strip Clone() {
            return this;
        }

        object ICloneable.Clone() {
            return this;
        }

        #endregion
    }
}
