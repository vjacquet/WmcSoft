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

namespace WmcSoft.Arithmetics
{
    public struct SimpsonRule : IIntegralRule<double>
    {
        private readonly int _steps;

        public SimpsonRule(int steps) {
            if (steps <= 0) throw new ArgumentOutOfRangeException("steps");
            _steps = steps;
        }

        public double Integrate<TFunction>(TFunction f, double a, double b)
            where TFunction : IFunction<double> {
            var r = 0d;
            var w = (b - a) / _steps;
            var w2 = w / 2d;
            for (int i = 0; i < _steps; ) {
                r += f.Eval(a + i * w);
                i++;
                r += 4 * f.Eval(a - w2 + i * w) + f.Eval(a + i * w);
            }
            return r * w / 6d;
        }
    }
}
