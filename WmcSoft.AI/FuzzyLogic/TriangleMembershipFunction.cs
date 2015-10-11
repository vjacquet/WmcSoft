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

namespace WmcSoft.AI.FuzzyLogic
{
    public struct TriangleMembershipFunction : IMembershipFunction<double>
    {
        public TriangleMembershipFunction(double lo, double mid, double hi) {
            if (hi < 0.0 || hi > 1.0) throw new ArgumentOutOfRangeException("hi");
            if (mid < 0.0 || mid > 1.0) throw new ArgumentOutOfRangeException("mid");
            if (lo < 0.0 || lo > 1.0) throw new ArgumentOutOfRangeException("lo");

            Low = lo;
            Mid = mid;
            High = hi;
        }

        public double Low { get; }
        public double Mid { get; }
        public double High { get; }

        public FuzzyVar Evaluate(double x) {
            if ((x <= Low) || (x >= High))
                return 0d;
            else if (x > Mid)
                return (High - x) / (High - Mid);
            else if (x < Mid)
                return (Low - x) / (Low - Mid);
            else
                return 1;
        }
    }
}
