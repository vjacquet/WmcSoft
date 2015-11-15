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

namespace WmcSoft.AI
{
    public static class Series
    {
        public static double[] Detrend(double[] input, double slope, double intercept) {
            var output = new double[input.Length];
            var y = intercept;
            for (int i = 0; i < input.Length; i++) {
                output[i] = input[i] - y;
                y += slope;
            }
            return output;
        }

        public static double[] Detrend<A>(double[] input) where A : ITrendEvaluator, new() {
            var a = new A();
            double slope;
            double intercept;
            a.Eval(input, out slope, out intercept);
            return Detrend(input, slope, intercept);
        }

        public static double[] Retrend(double[] input, double slope, double intercept) {
            var output = new double[input.Length];
            var y = intercept;
            for (int i = 0; i < input.Length; i++) {
                output[i] = input[i] + y;
                y += slope;
            }
            return output;
        }
    }
}
