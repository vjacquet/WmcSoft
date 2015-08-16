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
using WmcSoft.Benchmark;
using WmcSoft.Numerics;

namespace MathematicsBench
{
    [Benchmark(Iterations=500)]
    public class VectorBench
    {
        const int Count = 8192;

        static double[] x;
        static double[] y;
        static double[] z;

        static Vector vx;
        static Vector vy;
        static Vector vz;

        public static void Init(string[] args) {
            x = new double[Count];
            y = new double[Count];

            var random = new Random(1664);
            for (int i = 0; i < Count; i++) {
                x[i] = random.NextDouble() * 1000000;
                y[i] = random.NextDouble() * 1000000;
            }
            vx = new Vector(x);
            vy = new Vector(y);
        }

        public static void Reset() {
        }

        public static void Check() {
        }

        [Measure]
        public static void MeasureAddOnRawArray() {
            z = new double[Count];
            for (int i = 0; i < Count; i++) {
                z[i] = x[i] + y[i];
            }
        }

        [Measure]
        public static void MeasureAddOnStrideEnumerator() {
            z = new double[Count];
            var ex = new StrideEnumerator<double>(x);
            var ey = new StrideEnumerator<double>(y);
            int i=0;
            while (ex.MoveNext() && ey.MoveNext()) {
                z[i++] = ex.Current + ey.Current;
            }
        }

        [Measure]
        public static void MeasureAddOnVector() {
            vz = vx + vy;
        }
    }
}
