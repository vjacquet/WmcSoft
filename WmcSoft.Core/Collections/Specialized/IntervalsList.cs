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

using System.Collections;
using System.Collections.Generic;

namespace WmcSoft.Collections.Specialized
{
    public class IntervalsList<T> : IReadOnlyList<(T lowerBound, T upperBound)>
    {
        private List<T> _storage;
        private IComparer<T> _comparer;

        public IntervalsList(T minValue, T maxValue, IComparer<T> comparer)
        {
            _storage = new List<T> { minValue, maxValue };
            _comparer = comparer ?? Comparer<T>.Default;
        }

        public IntervalsList(T minValue, T maxValue)
            : this(minValue, maxValue, null)
        {
        }

        static bool IsOdd(int i)
        {
            return 1 == (i & 1);
        }

        public void Add(T x, T y)
        {
            var lo = _storage.BinarySearch(x, _comparer);
            var hi = _storage.BinarySearch(y, _comparer);

            // 4 cases for lo & hi
            if (hi < 0) {
                hi = ~hi;
                if (IsOdd(hi)) {
                    _storage.Insert(hi, y);
                } else {
                    // y is inside a range. Nothing to do.
                }
            } else {
                if (IsOdd(hi)) {
                    // y is at the begining of a range. Merge.
                    hi++;
                } else {
                    // y is the new upper bound
                    _storage[hi] = y;
                }
            }

            if (lo < 0) {
                lo = ~lo;
                if (IsOdd(lo)) {
                    _storage.Insert(lo, x);
                    lo++;
                    hi++;
                } else {
                    // x is inside a range, nothing to do.
                }
            } else {
                if (IsOdd(lo)) {
                    // x is the new lower bound
                    _storage[lo++] = x;
                } else {
                    // x is at the end of a range. Merge
                }
            }

            _storage.RemoveRange(lo, hi - lo);
        }

        public void Add((T, T) range)
        {
            Add(range.Item1, range.Item2);
        }

        public int Count => (_storage.Count - 2) / 2;

        public (T lowerBound, T upperBound) this[int index] {
            get {
                var i = 1 + index * 2;
                return (_storage[i], _storage[i + 1]);
            }
        }

        public IEnumerator<(T lowerBound, T upperBound)> GetEnumerator()
        {
            var length = _storage.Count - 2;
            for (int i = 1; i < length; i += 2) {
                yield return (_storage[i], _storage[i + 1]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
