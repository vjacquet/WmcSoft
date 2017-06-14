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
    public static class ActivationFunctions
    {
        public static Func<double, double> Logistic()
        {
            return x => 1d / (1 + Exp(-x));
        }

        public static Func<double, double> Threshold(double threshold = 0d)
        {
            return x => x < threshold ? 0d : 1d;
        }

        public static Func<double, double> HyperbolicTangent()
        {
            return x => Tanh(x);
        }

        static double Interpolate(double[] values, double[] derivated, double xd)
        {
            var i = (int)xd;
            var count = values.Length - 1;
            return (i >= count) ? values[count] : (values[i] + derivated[i] * (xd - i));
        }
        public static Func<double, double> InterpolateSymmetric(Func<double, double> func, double maxValue, int count)
        {
            var factor = (count - 1) / maxValue;
            var values = new double[count];
            var derivated = new double[count];
            for (int i = 0; i < count; i++) {
                values[i] = func(i / factor);
            }
            for (int i = 1; i < count; i++) {
                derivated[i - 1] = values[i] - values[i - 1];
            }
            return x => x >= 0d
                ? Interpolate(values, derivated, x * factor)
                : 1d - Interpolate(values, derivated, -x * factor);
        }
    }
}
