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

namespace WmcSoft.Collections.Specialized
{
    // <https://en.wikipedia.org/wiki/Fenwick_tree>
    public class FenwickTree
    {
        int[] _storage;

        public FenwickTree(int size)
        {
            _storage = new int[size];
        }

        public FenwickTree(params int[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            var length = data.Length;
            _storage = (int[])data.Clone();
            for (int i = 0; i < length; i++) {
                var j = i + LeastSignificantBit(i + 1);
                if (j < length)
                    _storage[j] += _storage[i];
            }
        }

        public int Sum(int i)
        {
            var sum = 0;
            while (i != 0) {
                sum += _storage[i - 1];
                i -= LeastSignificantBit(i);
            }
            return sum;
        }

        public void Add(int i, int delta)
        {
            var length = _storage.Length;
            while (i < length) {
                _storage[i] += delta;
                i += LeastSignificantBit(i + 1);
            }
        }

        public int Range(int i, int j)
        {
            var sum = 0;
            while (j > i) {
                sum += _storage[j - 1];
                j -= LeastSignificantBit(j);
            }
            while (i > j) {
                sum -= _storage[i - 1];
                i -= LeastSignificantBit(i);
            }
            return sum;
        }

        public int Tally()
        {
            return Range(0, _storage.Length);
        }

        public int this[int i] {
            get {
                return Range(i, i + 1);
            }
            set {
                Add(i, value - Range(i, i + 1));
            }
        }

        #region Helpers

        static int LeastSignificantBit(int x)
        {
            return (int)LeastSignificantBit((uint)x);
        }
        static uint LeastSignificantBit(uint x)
        {
            return unchecked(x & (uint)(-x));
        }

        #endregion
    }
}
