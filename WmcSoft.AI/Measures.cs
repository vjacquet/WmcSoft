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

namespace WmcSoft.AI
{
    public sealed class Measures
    {
        internal Measures(IList<double> targets, double mean, double squaredMean, IEnumerable<double> outputs) {
            var n = targets.Count;
            var i = 0;
            var mse = 0d;
            var mre = 0d;
            foreach (var o in outputs) {
                var t = targets[i];
                var delta = t - o;
                mse += delta * delta;
                mre += Math.Abs(delta / t);
                i++;
            }
            if (i != n)
                throw new ArgumentException("outputs");

            MeanSquareError = mse / n;
            RootMeanSquareError = Math.Sqrt(MeanSquareError);
            RootMeanSquareOverSquaredMeanError = Math.Sqrt(MeanSquareError / squaredMean);
            RootMeanSquareOverVarianceMeanError = Math.Sqrt(MeanSquareError / (squaredMean - mean * mean));
            MeanRelativeError = mre / n;
        }

        public double MeanSquareError { get; private set; }
        public double RootMeanSquareError { get; private set; }
        public double RootMeanSquareOverSquaredMeanError { get; private set; }
        public double RootMeanSquareOverVarianceMeanError { get; private set; }
        public double MeanRelativeError { get; private set; }
    }
}
