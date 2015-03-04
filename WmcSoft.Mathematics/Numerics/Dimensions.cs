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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Numerics
{
    /// <summary>
    /// The structure that models the dimensions of the Valarray.
    /// </summary>
    public struct Dimensions : IReadOnlyList<int>, IEquatable<Dimensions>
    {
        private readonly int[] _dimensions;

        public Dimensions(params int[] dimensions) {
            _dimensions = (int[])dimensions.Clone();
        }

        internal Dimensions(Dimensions min, Dimensions max) {
            var length = max.Count;
            _dimensions = new int[length];
            for (var i = min.Count - 1; i >= 0; i--) {
                length--;
                _dimensions[length] = Math.Max(min[i], max[length]);
            }
            Array.Copy(max._dimensions, _dimensions, length);
        }

        public static implicit operator Dimensions(int n) {
            return new Dimensions(n);
        }
        public static implicit operator Dimensions(int[] dimensions) {
            return new Dimensions(dimensions);
        }

        #region Methods

        public int GetDimension(int i) {
            if (_dimensions == null)
                return 0;
            return _dimensions[Mod(i, _dimensions.Length)];
        }

        static int Mod(int i, int n) {
            //handle negatives indexes
            return (i + n) % n;
        }

        internal int GetIndex(int[] indices) {
            if (indices == null)
                throw new ArgumentNullException("indices");

            var length = indices.Length;
            if (length != Count || length == 0)
                throw new ArgumentOutOfRangeException("indices");

            int index = Mod(indices[0], _dimensions[0]);
            for (int i = 1; i < length; i++) {
                index = index * _dimensions[i] + Mod(indices[i], _dimensions[i]);
            }
            return index;
        }

        #endregion

        #region IReadOnlyList<int> Membres

        public int this[int index] {
            get { return _dimensions[index]; }
        }

        #endregion

        #region IReadOnlyCollection<int> Membres

        public int Count {
            get { return _dimensions == null ? 0 : _dimensions.Length; }
        }

        #endregion

        #region IEnumerable<int> Membres

        public static Dimensions Combine(Dimensions x, Dimensions y) {
            if (x.Count < y.Count)
                return new Dimensions(x, y);
            return new Dimensions(y, x);
        }

        public IEnumerator<int> GetEnumerator() {
            var dimensions = _dimensions ?? new int[0];
            return dimensions.AsEnumerable<int>().GetEnumerator();
        }

        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            var dimensions = _dimensions ?? new int[0];
            return dimensions.GetEnumerator();
        }

        #endregion

        #region IEquatable<Dimensions> Membres

        public static bool operator ==(Dimensions x, Dimensions y) {
            return x.Equals(y);
        }
        public static bool operator !=(Dimensions x, Dimensions y) {
            return !x.Equals(y);
        }

        public bool Equals(Dimensions other) {
            var length = Count;
            if (length != other.Count)
                return false;
            for (int i = 0; i < length; i++) {
                if (_dimensions[i] != other._dimensions[i])
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return Equals((Dimensions)obj);
        }

        public override int GetHashCode() {
            if (_dimensions == null)
                return 0;

            int length = _dimensions.Length;
            int hash = length;
            for (int i = 0; i < length; i++) {
                hash = (hash << 4) ^ (hash >> 28) ^ _dimensions[i];
            }
            return hash;
        }

        #endregion
    }
}
