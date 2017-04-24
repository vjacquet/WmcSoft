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
using System.Text;
using WmcSoft.Collections;

namespace WmcSoft
{
    public struct OrdinalSet<T, TOrdinal> : IEquatable<OrdinalSet<T, TOrdinal>>
        , IReadOnlyCollection<T>
        where TOrdinal : IOrdinal<T>, IReadOnlyList<T>, new()
    {
        private static readonly TOrdinal Ordinal = new TOrdinal();

        private readonly BitArray _storage;

        public int Count {
            get { return _storage.Cardinality(); }
        }

        private OrdinalSet(BitArray storage)
        {
            _storage = storage;
        }

        public OrdinalSet(IEnumerable<T> collection)
        {
            _storage = new BitArray(Ordinal.Count);
            var lowerBound = Ordinal[0];
            foreach (var item in collection) {
                _storage.Set(Ordinal.Distance(lowerBound, item), true);
            }
        }

        #region Operators

        public static OrdinalSet<T, TOrdinal> operator |(OrdinalSet<T, TOrdinal> x, OrdinalSet<T, TOrdinal> y)
        {
            var result = x._storage.Clone<BitArray>();
            return new OrdinalSet<T, TOrdinal>(result.Or(y._storage));
        }
        public static OrdinalSet<T, TOrdinal> Union(OrdinalSet<T, TOrdinal> x, OrdinalSet<T, TOrdinal> y)
        {
            return x | y;
        }

        public static OrdinalSet<T, TOrdinal> operator &(OrdinalSet<T, TOrdinal> x, OrdinalSet<T, TOrdinal> y)
        {
            var result = x._storage.Clone<BitArray>();
            return new OrdinalSet<T, TOrdinal>(result.And(y._storage));
        }
        public static OrdinalSet<T, TOrdinal> Intersect(OrdinalSet<T, TOrdinal> x, OrdinalSet<T, TOrdinal> y)
        {
            return x & y;
        }

        public static OrdinalSet<T, TOrdinal> operator ^(OrdinalSet<T, TOrdinal> x, OrdinalSet<T, TOrdinal> y)
        {
            var result = x._storage.Clone<BitArray>();
            return new OrdinalSet<T, TOrdinal>(result.Xor(y._storage));
        }
        public static OrdinalSet<T, TOrdinal> SymmetricDifference(OrdinalSet<T, TOrdinal> x, OrdinalSet<T, TOrdinal> y)
        {
            return x ^ y;
        }

        public static OrdinalSet<T, TOrdinal> operator ~(OrdinalSet<T, TOrdinal> x)
        {
            var result = x._storage.Clone<BitArray>();
            return new OrdinalSet<T, TOrdinal>(result.Not());
        }
        public static OrdinalSet<T, TOrdinal> Complement(OrdinalSet<T, TOrdinal> x)
        {
            return ~x;
        }

        public static OrdinalSet<T, TOrdinal> operator -(OrdinalSet<T, TOrdinal> x, OrdinalSet<T, TOrdinal> y)
        {
            var result = x._storage.Clone<BitArray>();
            return new OrdinalSet<T, TOrdinal>(result.Xor(y._storage).And(x._storage));
        }
        public static OrdinalSet<T, TOrdinal> Difference(OrdinalSet<T, TOrdinal> x, OrdinalSet<T, TOrdinal> y)
        {
            return x - y;
        }

        public static bool operator ==(OrdinalSet<T, TOrdinal> x, OrdinalSet<T, TOrdinal> y)
        {
            return x.Equals(y);
        }
        public static bool operator !=(OrdinalSet<T, TOrdinal> x, OrdinalSet<T, TOrdinal> y)
        {
            return !x.Equals(y);
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != typeof(OrdinalSet<T, TOrdinal>))
                return false;
            return Equals((OrdinalSet<T, TOrdinal>)obj);
        }

        public override int GetHashCode()
        {
            if (_storage == null || _storage.None())
                return 0;
            return _storage.GetHashCode();
        }

        #endregion

        #region IEquatable<T> Members

        public bool Equals(OrdinalSet<T, TOrdinal> other)
        {
            var x = BitArrayExtensions.DataOf(_storage);
            var y = BitArrayExtensions.DataOf(other._storage);
            var length = x.Length;
            for (int i = 0; i < length; i++) {
                if (x[i] != y[i])
                    return false;
            }
            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _storage.Length; i++) {
                if (_storage.Get(i))
                    yield return Ordinal[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('{');
            using (var enumerator = GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    sb.Append(enumerator.Current);
                    while (enumerator.MoveNext()) {
                        sb.Append(',');
                        sb.Append(enumerator.Current);
                    }
                }
            }
            sb.Append('}');
            return sb.ToString();
        }
    }
}
