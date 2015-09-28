using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Text
{
    public sealed class Strip : IComparable<string>, IReadOnlyList<char>
    {
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

        public int CompareTo(string other) {
            return String.Compare(_s, _start, other, 0, Length);
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
