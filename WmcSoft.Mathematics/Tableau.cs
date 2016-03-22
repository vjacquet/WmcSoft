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
using System.Linq;

namespace WmcSoft
{
    /// <summary>
    /// Represents a Young Tableau of shape where N integers are stored in an 
    /// array of left-justified rows, with Ni elements in row i, such that
    /// the entries of each row are in increasing order from left to right, and
    /// the entries of each column are in increasing order from top to bottom.
    /// </summary>
    /// <remarks>See Knuth's TAoCP, Vol 3, Page 47.</remarks>
    public class Tableau : ICollection<int>
    {
        private readonly List<List<int>> _data;

        public Tableau() {
            _data = new List<List<int>>();
        }

        public int Count => _data.Sum(x => x.Count);
        public bool IsReadOnly => false;

        private bool IsInfinite(int i, int j) {
            if (j >= _data.Count)
                return true;
            return i >= _data[j].Count;
        }

        private bool LessThan(int value, int i, int j) {
            if (i < 0 || j < 0)
                return false;
            if (j >= _data.Count || i >= _data[j].Count)
                return true;
            return value < this[i, j];
        }

        public void Add(int item) {
            var x = new List<int>();
            var r = new List<int>();
            var i = 0;
            var j = _data.Count;
            x.Add(item);

            while (i < x.Count) {
                while (LessThan(x[i], i, j - 1))
                    j--;
                if (!IsInfinite(i, j))
                    x.Add(this[i, j]);
                r.Add(j);
                this[i, j] = x[i];
                i++;
            }
        }

        public void Clear() {
            _data.Clear();
        }

        private int Find(IEnumerable<int> list, int x) {
            int index = 0;
            foreach (var item in list) {
                if (item < x) {
                    index++;
                    continue;
                }
                if (item == x)
                    return index;
                break;
            }
            return ~index;
        }

        public bool Find(int item, out int i, out int j) {
            var length = _data.Count;
            for (int k = 0; k < length; k++) {
                var row = _data[k];
                var found = Find(row, item);
                if (found >= 0) {
                    j = k;
                    i = found;
                    return true;
                }
                if (found == -1)
                    break;
            }
            i = 0;
            j = 0;
            return false;
        }

        public bool Contains(int item) {
            var length = _data.Count;
            for (int i = 0; i < length; i++) {
                var row = _data[i];
                var found = Find(row, item);
                if (found >= 0)
                    return true;
                if (found == -1)
                    return false;
            }
            return false;
        }

        public void CopyTo(int[] array, int arrayIndex) {
            var length = _data.Count;
            for (int i = 0; i < length; i++) {
                var row = _data[i];
                row.CopyTo(array, arrayIndex);
                arrayIndex += row.Count;
            }
        }

        public bool Remove(int item) {
            throw new NotImplementedException();
        }

        public IEnumerator<int> GetEnumerator() {
            return _data.SelectMany(x => x).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public int this[int i, int j]
        {
            get {
                return _data[j][i];
            }
            private set {
                if (j == _data.Count)
                    _data.Add(new List<int>());
                if (i == _data[j].Count)
                    _data[j].Add(value);
                else
                    _data[j][i] = value;
            }
        }
    }
}