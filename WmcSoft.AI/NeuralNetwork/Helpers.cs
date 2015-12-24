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
using static System.Math;

namespace WmcSoft.AI.NeuralNetwork
{
    public static class Helpers
    {
        public static void Equilat(double[] matrix) {
            if (matrix == null) throw new ArgumentNullException("matrix");
            if (matrix.Length < 2) throw new ArgumentOutOfRangeException("matrix");

            double r;
            var n = matrix.Length;
            var nm1 = n - 1;
            matrix[0] = -1d;
            matrix[nm1] = 1d;

            for (int k = 2; k < n; k++) {
                // scale amtrix so far
                r = k;
                var f = Sqrt(r * r - 1d) / r;
                for (int i = 0; i < k; i++) {
                    for (int j = 0; j < k - 1; j++) {
                        matrix[i * nm1 + j] *= f;
                    }
                }

                // append a column of all -1/k
                r = 1d / r;
                for (int i = 0; i < k; i++) {
                    matrix[k * nm1 + k - 1] = r;
                }

                // append new row of all 0's except 1 at end
                for (int i = 0; i < k - 1; i++) {
                    matrix[k * nm1 + i] = 0d;
                }
                matrix[k * nm1 + k - 1] = 1d;
            }
        }

        public static void RescaleEquilat(double[] matrix) {
            var length = matrix.Length;
            for (int i = 0; i < length; i++) {
                matrix[i] = 0.4d * matrix[i] + 0.5d;
            }
        }

        /// <summary>
        /// Returns the number of value greater than or equal to the threshold
        /// </summary>
        /// <param name="values">The array of values</param>
        /// <param name="threshold">The threshold</param>
        /// <returns>Number of values ggreater than or equal to the threshold.</returns>
        /// <remarks>Assumes the array is sorted.</remarks>
        public static int Count(double[] values, double threshold) {
            var n = values.Length;
            if (threshold > values[n - 1])
                return 0;
            if (threshold < values[0])
                return n;
            var lo = 0;
            var hi = n - 1;
            while (true) {
                var mid = lo + (hi - lo) / 2;
                if (mid == lo)
                    return n - hi;
                if (values[mid] < threshold)
                    lo = mid;
                else
                    hi = mid;
            }
        }
    }
}
