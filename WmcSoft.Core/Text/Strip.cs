using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Text
{
    public sealed class Strip : IComparable<string>, IReadOnlyList<char>
    {
        static readonly Strip Null = new Strip(null, 0, 0);
        public static readonly Strip Empty = new Strip(String.Empty, 0, 0);

        #region Fields

        private readonly string _s;
        private readonly int _start;
        private readonly int _end;

        #endregion

        #region Lifecycle

        public Strip(string s, int startIndex, int length) {
            _s = s;
            _start = startIndex;
            _end = startIndex + length;
        }

        #endregion

        #region Properties

        public int Length {
            get { return _end - _start; }
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
    }
}
