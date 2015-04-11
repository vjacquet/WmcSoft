using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft
{
    public class Indices
    {
        readonly int[] _dimensions;
        readonly int[] _indices;

        public Indices(Array array) {
            _dimensions = array.GetDimensions();
            var rank = _dimensions.Length;
            _indices = new int[rank];
        }

        public bool Increment() {
            var rank = _dimensions.Length;
            for (int i = rank - 1; i >= 0; i--) {
                if (++_indices[i] != _dimensions[i])
                    return true;
                _indices[i] = 0;
            }
            return false;
        }

        public int this[int index] {
            get { return _indices[index]; }
        }
    }
}
