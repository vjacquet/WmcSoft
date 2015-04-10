using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Collections.Generic
{
    public class ArrayEqualityComparer<T> : IEqualityComparer<Array>
    {
        readonly IEqualityComparer<T> _comparer;

        public ArrayEqualityComparer(IEqualityComparer<T> comparer) {
            _comparer = comparer;
        }
        public ArrayEqualityComparer()
            : this(EqualityComparer<T>.Default) {
        }

        static bool Increment(int rank, int[] dimensions, int[] indices) {
            for (int i = rank - 1; i >= 0; i--) {
                if (++indices[i] != dimensions[i])
                    return true;
                indices[i] = 0;
            }
            return false;
        }
        #region IEqualityComparer<Array> Membres

        public bool Equals(Array x, Array y) {
            if (x == y)
                return true;
            if (x == null)
                return false;

            var rank = x.Rank;
            if (rank != y.Rank)
                return false;
            var dimensions = new int[rank];
            for (int i = 0; i < rank; i++) {
                dimensions[i] = x.GetLength(i);
                if (dimensions[i] != y.GetLength(i))
                    return false;
            }
            var indices = new int[rank];
            do {
                if (!_comparer.Equals((T)x.GetValue(indices), (T)y.GetValue(indices)))
                    return false;
            } while (Increment(rank, dimensions, indices));
            return true;
        }

        public int GetHashCode(Array obj) {
            return obj.GetHashCode();
        }

        #endregion
    }
}
