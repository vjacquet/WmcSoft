#region Licence

/****************************************************************************
          Copyright 1999-2017 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft.Collections.Generic.Accumulators
{
    /// <summary>
    /// Adds double values, minimizing the accumulation of approximation errors.
    /// </summary>
    /// <remarks>See <https://en.wikipedia.org/wiki/Kahan_summation_algorithm>.</remarks>
    public struct KahanAdder : IAccumulator<double, double>
    {
        private double c;

        public double Accumulate(double result, double value)
        {
            var y = value - c;    // So far, so good: c is zero.
            var t = result + y;   // Alas, sum is big, y small, so low-order digits of y are lost.
            c = (t - result) - y; // (t - sum) cancels the high-order part of y; subtracting y recovers negative (low part of y)
            result = t;           // Algebraically, c should always be zero. Beware overly-aggressive optimizing compilers!
            return result;
        }
    }
}
