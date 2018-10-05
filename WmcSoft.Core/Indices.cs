using System;

namespace WmcSoft
{
    internal class Indices
    {
        readonly int[] dimensions;
        readonly int[] indices;

        public Indices(Array array) : this(array.GetDimensions())
        {
        }

        public Indices(params int[] dimensions)
        {
            this.dimensions = dimensions;
            indices = new int[this.dimensions.Length];
        }

        /// <summary>
        /// Increments the indices to reference the next cell in the array.
        /// </summary>
        /// <returns>Returns <c>true</c> if the indices were incremented to point to a new cell; otherwise, <c>false</c>.</returns>
        public bool Increment()
        {
            var rank = dimensions.Length;
            for (int i = rank - 1; i >= 0; i--) {
                if (++indices[i] != dimensions[i])
                    return true;
                indices[i] = 0;
            }
            return false;
        }

        public int this[int index] {
            get { return indices[index]; }
        }
    }
}
