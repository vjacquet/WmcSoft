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
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WmcSoft
{
    /// <summary>
    /// Represents a Young Tableau of shape where N integers are stored in an 
    /// array of left-justified rows, with Ni elements in row i, such that
    /// the entries of each row are in increasing order from left to right, and
    /// the entries of each column are in increasing order from top to bottom.
    /// </summary>
    /// <remarks>See Knuth's TAoCP, Vol 3, Page 47.</remarks>
    [DebuggerTypeProxy(typeof(Tableau.DebugView))]
    [DebuggerDisplay("{DebugView.Flatten(ToString()),nq}")]
    public class Tableau : ICollection<int>
    {
        public class DebugView
        {
            public class RowView
            {
                private readonly int[] _data;
                public RowView(int count)
                {
                    _data = new int[count];
                }

                public int this[int index] {
                    get { return _data[index]; }
                    set { _data[index] = value; }
                }

                public override string ToString()
                {
                    var sb = new StringBuilder();
                    for (int i = 0; i < _data.Length; i++) {
                        sb.AppendFormat("{0, 5}", _data[i]);
                    }
                    return sb.ToString();
                }
            }

            private readonly Tableau _tableau;

            public DebugView(Tableau tableau)
            {
                _tableau = tableau;
            }

            public static string Flatten(string value)
            {
                return '{' + value.Replace("\r\n", "|") + '}';
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public RowView[] Rows {
                get {
                    if (_tableau._data.Count == 0) {
                        return new RowView[0];
                    }

                    var columns = _tableau._data.Count;
                    var rows = new RowView[_tableau._data[0].Count];
                    for (int i = 0; i < rows.Length; i++) {
                        while (_tableau._data[columns - 1].Count <= i)
                            --columns;

                        var row = new RowView(columns);
                        rows[i] = row;
                        for (int j = 0; j < columns; j++) {
                            row[j] = _tableau._data[j][i];
                        }
                    }
                    return rows;
                }
            }
        }

        public const int Infinity = Int32.MaxValue;

        struct Band
        {
            int[] _storage;
            int _count;

            public Band(int capacity)
            {
                _count = 0;
                _storage = new int[capacity];
            }

            public int this[int index] {
                get {
                    return _storage[index];
                }
                set {
                    if (index >= _count) {
                        if (_count == 0) {
                            _count = 4;
                            _storage = new int[_count];
                        } else {
                            _count *= 2;

                            var storage = new int[_count];
                            Array.Copy(_storage, storage, _storage.Length);
                            _storage = storage;
                        }
                    }
                    _storage[index] = value;
                }
            }

            public override string ToString()
            {
                return string.Join(" ", _storage.Select(i => i == Infinity ? "∞" : i.ToString()));
            }
        }

        private readonly List<List<int>> _data;

        public Tableau()
        {
            _data = new List<List<int>>();
        }

        public int Count => _data.Sum(x => x.Count);
        public bool IsReadOnly => false;

        private bool IsZero(int i, int j)
        {
            return i <= 0 | j <= 0;
        }
        private bool IsInfinite(int i, int j)
        {
            if (j >= _data.Count)
                return true;
            return i >= _data[j].Count;
        }

        private bool LessThan(int value, int i, int j)
        {
            if (i < 0 || j < 0)
                return false;
            if (j >= _data.Count || i >= _data[j].Count)
                return true;
            return value < this[i, j];
        }

        public void Add(int item)
        {
            if (item <= 0 || item == Infinity)
                throw new ArgumentOutOfRangeException("item");

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

        public bool Remove(int item)
        {
            if (item <= 0 || item == Infinity)
                throw new ArgumentOutOfRangeException("item");

            int i, j;
            if (!Find(item, out i, out j))
                return false;

            var x = new Band();
            var r = new Band();

            x[i + 1] = Infinity;
            do {
                while (this[i, j + 1] < x[i + 1])
                    j++;
                x[i] = this[i, j];
                r[i] = j;
                this[i, j] = x[i + 1];
            }
            while (i-- > 0);

            return true;
        }

        public void Clear()
        {
            _data.Clear();
        }

        private int Find(IEnumerable<int> list, int x)
        {
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

        public bool Find(int item, out int i, out int j)
        {
            if (item <= 0 || item == Infinity)
                throw new ArgumentOutOfRangeException("item");

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

        public bool Contains(int item)
        {
            if (item <= 0 || item == Infinity)
                throw new ArgumentOutOfRangeException("item");

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

        public void CopyTo(int[] array, int arrayIndex)
        {
            var length = _data.Count;
            for (int i = 0; i < length; i++) {
                var row = _data[i];
                row.CopyTo(array, arrayIndex);
                arrayIndex += row.Count;
            }
        }

        public IEnumerator<int> GetEnumerator()
        {
            return _data.SelectMany(x => x).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int this[int i, int j] {
            get {
                if (i < 0 || j < 0)
                    return 0;
                if (j >= _data.Count || i >= _data[j].Count)
                    return Infinity;
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

        public override string ToString()
        {
            if (_data.Count == 0) {
                return "";
            }

            var sb = new StringBuilder();
            var columns = _data.Count;
            var length = _data[0].Count;
            for (int i = 0; i < length; i++) {
                while (_data[columns - 1].Count <= i)
                    --columns;
                if (i > 0)
                    sb.AppendLine();
                var row = new int[columns];
                for (int j = 0; j < columns; j++) {
                    sb.AppendFormat("{0, 5}", _data[j][i]);
                }
            }
            return sb.ToString();
        }
    }
}