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

namespace WmcSoft.Numerics
{
    /// <summary>
    /// The structure that models the dimensions of the <see cref="Valarray"/> or <see cref="Boolarray"/>.
    /// </summary>
    public struct Dimensions : IReadOnlyList<int>, IEquatable<Dimensions>
    {
        public static readonly Dimensions Empty;

        private readonly int[] dimensions;

        public Dimensions(params int[] dimensions)
        {
            this.dimensions = (int[])dimensions.Clone();
        }

        internal Dimensions(Dimensions min, Dimensions max)
        {
            var length = max.Count;
            dimensions = new int[length];
            for (var i = min.Count - 1; i >= 0; i--) {
                length--;
                dimensions[length] = Math.Max(min[i], max[length]);
            }
            Array.Copy(max.dimensions, dimensions, length);
        }

        public static explicit operator int(Dimensions d)
        {
            var count = d.Count;
            switch (count) {
            case 0:
                throw new InvalidCastException();
            case 1:
                return d[0];
            default:
                for (int i = 1; i < count; i++) {
                    if (d[i] != d[0])
                        throw new InvalidCastException();
                }
                return d[0];
            }
        }

        public void Deconstruct(out int m, out int n)
        {
            if (Count != 2)
                throw new InvalidCastException();

            m = dimensions[0];
            n = dimensions[1];
        }

        public void Deconstruct(out int m, out int n, out int o)
        {
            if (Count != 3)
                throw new InvalidCastException();

            m = dimensions[0];
            n = dimensions[1];
            o = dimensions[2];
        }

        public void Deconstruct(out int m, out int n, out int o, out int p)
        {
            if (Count != 4)
                throw new InvalidCastException();

            m = dimensions[0];
            n = dimensions[1];
            o = dimensions[2];
            p = dimensions[3];
        }

        public void Deconstruct(out int m, out int n, out int o, out int p, out int q)
        {
            if (Count != 5)
                throw new InvalidCastException();

            m = dimensions[0];
            n = dimensions[1];
            o = dimensions[2];
            p = dimensions[3];
            q = dimensions[4];
        }

        public static implicit operator Dimensions(int n)
        {
            return new Dimensions(n);
        }
        public static implicit operator Dimensions(int[] dimensions)
        {
            return new Dimensions(dimensions);
        }

        public static bool operator ==(Dimensions x, Dimensions y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Dimensions x, Dimensions y)
        {
            return !x.Equals(y);
        }

        #region Methods

        public int GetCardinality()
        {
            if (dimensions == null)
                return 0;
            return dimensions.Aggregate(1, (x, y) => x * y);
        }

        public int GetDimension(int i)
        {
            if (dimensions != null)
                return dimensions[Mod(i, dimensions.Length)];
            return 0;
        }

        static int Mod(int i, int n)
        {
            //handle negatives indexes
            return (i + n) % n;
        }

        internal int GetIndex(int[] indices)
        {
            if (indices == null)
                throw new ArgumentNullException(nameof(indices));

            var length = indices.Length;
            if (length != Count || length == 0)
                throw new ArgumentOutOfRangeException(nameof(indices));

            int index = Mod(indices[0], dimensions[0]);
            for (int i = 1; i < length; i++) {
                index = index * dimensions[i] + Mod(indices[i], dimensions[i]);
            }
            return index;
        }

        #endregion

        #region IReadOnlyList<int> Membres

        public int this[int index] {
            get { return dimensions[index]; }
        }

        #endregion

        #region IReadOnlyCollection<int> Membres

        public int Count => dimensions != null ? dimensions.Length : 0;

        #endregion

        #region IEnumerable<int> Membres

        public static Dimensions Combine(Dimensions x, Dimensions y)
        {
            if (x.Count < y.Count)
                return new Dimensions(x, y);
            return new Dimensions(y, x);
        }

        public IEnumerator<int> GetEnumerator()
        {
            var dimensions = this.dimensions ?? new int[0];
            return dimensions.AsEnumerable<int>().GetEnumerator();
        }

        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            var dimensions = this.dimensions ?? new int[0];
            return dimensions.GetEnumerator();
        }

        #endregion

        #region IEquatable<Dimensions> Membres

        public bool Equals(Dimensions other)
        {
            var length = Count;
            if (length != other.Count)
                return false;
            for (int i = 0; i < length; i++) {
                if (dimensions[i] != other.dimensions[i])
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return Equals((Dimensions)obj);
        }

        public override int GetHashCode()
        {
            if (dimensions == null)
                return 0;

            int length = dimensions.Length;
            int hash = length;
            for (int i = 0; i < length; i++) {
                hash = (hash << 4) ^ (hash >> 28) ^ dimensions[i];
            }
            return hash;
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return string.Join(" x ", dimensions);
        }
        #endregion
    }
}
