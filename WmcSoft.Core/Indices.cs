using System;

namespace WmcSoft
{
    internal class Indices
    {
        readonly int[] _dimensions;
        readonly int[] _indices;

        public Indices(Array array)
        {
            _dimensions = array.GetDimensions();
            var rank = _dimensions.Length;
            _indices = new int[rank];
        }

        /// <summary>
        /// Increments the indices to reference the next cell in the array.
        /// </summary>
        /// <returns>Returns <c>true</c> if the indices were incremented to point to a new cell; otherwise, <c>false</c>.</returns>
        public bool Increment()
        {
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
