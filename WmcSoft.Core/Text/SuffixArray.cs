#region Licence

/****************************************************************************
          Copyright 1999-2017 Vincent J. Jacquet.  All rights reserved.

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
using WmcSoft.Collections.Generic;

using static System.Math;
using static WmcSoft.Algorithms;

namespace WmcSoft.Text
{
    /// <summary>
    /// Sorted array of all suffixes of a string.
    /// </summary>
    public class SuffixArray : IReadOnlyList<string>
    {
        #region Comparer

        struct SuffixComparer : IComparer<int>
        {
            public readonly string Value;
            public readonly StringComparison Comparison;

            public SuffixComparer(string value, StringComparison comparison)
            {
                Value = value;
                Comparison = comparison;
            }

            public int Compare(int x, int y)
            {
                var length = Value.Length - Min(x, y); // length of the longest suffix.
                return string.Compare(Value, x, Value, y, length, Comparison);
            }
        }

        #endregion

        readonly string _value;
        readonly StringComparison _comparison;
        readonly int[] _suffixes;

        public SuffixArray(string value, StringComparison comparison = StringComparison.CurrentCulture)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException(nameof(value));

            _value = value;
            _comparison = comparison;

            // contains the offsets from the begining of the string
            _suffixes = Iota(value.Length);

            var comparer = new SuffixComparer(value, comparison);
            Array.Sort(_suffixes, comparer);
        }

        /// <summary>
        /// Returns the count of suffixes.
        /// </summary>
        public int Count => _suffixes.Length;

        /// <summary>
        /// Returns the suffix at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this[int index] => _value.Substring(_suffixes[index]);

        /// <summary>
        /// Returns the length of the suffix at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the suffix.</param>
        /// <returns>The length of the suffix.</returns>
        public int GetLengthOf(int index)
        {
            return _value.Length - _suffixes[index];
        }

        /// <summary>
        /// Returns the index of the selected suffix in the unsorted array.
        /// </summary>
        /// <param name="index">The index of the suffix.</param>
        /// <returns>The original index.</returns>
        public int IndexOf(int index)
        {
            return _suffixes[index];
        }

        public IEnumerator<string> GetEnumerator()
        {
            var length = _suffixes.Length;
            for (int i = 0; i < length; i++) {
                yield return _value.Substring(_suffixes[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        int Mismatch(int x, int y)
        {
            var length = _value.Length - Max(x, y); // length of the shortest suffix
            for (int i = 0; i < length; i++) {
                if (string.Compare(_value, x + i, _value, y + i, 1, _comparison) != 0)
                    return i;
            }
            return length;
        }

        /// <summary>
        /// Returns the length of the longest common prefix between an item and the previous one.
        /// </summary>
        /// <param name="index">The index of the suffix.</param>
        /// <returns>The length of the longest common prefix.</returns>
        public int GetLongestCommonPrefix(int index)
        {
            if (index < 0 || index >= _value.Length) throw new ArgumentOutOfRangeException(nameof(index));
            return (index == 0) ? 0 : Mismatch(_suffixes[index], _suffixes[index - 1]);
        }

        /// <summary>
        /// Returns the number of suffixes less than the <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key to get the rank of.</param>
        /// <returns>The number of suffixes less than the <paramref name="key"/>.</returns>
        public int Rank(string key)
        {

            return _suffixes.BinaryRank(x => {
                var suffixLength = _value.Length - x;
                var length = Max(key.Length, suffixLength);
                return string.Compare(_value, x, key, 0, length, _comparison);
            });
        }
    }
}
