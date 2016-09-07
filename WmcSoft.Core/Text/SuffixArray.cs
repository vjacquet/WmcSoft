#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft.Text
{
    public class SuffixArray : IEnumerable<string>
    {
        #region Comparer

        struct SuffixComparer : IComparer<int>
        {
            public readonly string Value;
            public readonly StringComparison Comparison;

            public SuffixComparer(string value, StringComparison comparison) {
                Value = value;
                Comparison = comparison;
            }

            public int Compare(int x, int y) {
                int result = 0;
                if (x < y) {
                    result = String.Compare(Value, x, Value, y, Value.Length - y, Comparison);
                    if (result == 0)
                        return 1;
                } else if (x > y) {
                    result = String.Compare(Value, x, Value, y, Value.Length - x, Comparison);
                    if (result == 0)
                        return -1;
                }
                return result;
            }
        }

        #endregion

        readonly string _value;
        public readonly StringComparison _comparison;
        readonly int[] _suffixes;

        public SuffixArray(string value, StringComparison comparison = StringComparison.CurrentCulture) {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException(nameof(value));

            _value = value;
            _comparison = comparison;

            _suffixes = new int[value.Length];
            _suffixes.Iota();

            var comparer = new SuffixComparer(value, comparison);
            Array.Sort(_suffixes, comparer);
        }

        public IEnumerator<string> GetEnumerator() {
            var length = _suffixes.Length;
            for (int i = 0; i < length; i++) {
                yield return _value.Substring(_suffixes[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
