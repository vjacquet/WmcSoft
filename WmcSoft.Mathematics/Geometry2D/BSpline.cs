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

namespace WmcSoft.Geometry2D
{
    public sealed class BSpline
    {
        private readonly double[] _knots;

        public BSpline(params double[] knots)
        {
            _knots = knots;
        }

        public double N(int i, int p, double x)
        {
            switch (p) {
            case 0:
                return (_knots[i] <= x && x < _knots[i + 1]) ? 1d : 0d;
            case 1:
                if (x < _knots[i] || x >= _knots[i + 2]) return 0d;
                if (x < _knots[i + 1]) return (x - _knots[i]) / (_knots[i + p] - _knots[i]);
                return (_knots[i + p + 1] - x) / (_knots[i + p + 1] - _knots[i + 1]);
            default:
                return ((x - _knots[i]) / (_knots[i + p] - _knots[i])) * N(i, p - 1, x)
                    + ((_knots[i + p + 1] - x) / (_knots[i + p + 1] - _knots[i + 1])) * N(i + 1, p - 1, x);
            }
        }
    }
}
