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
    public struct TriangularMembershipFunction : IMembershipFunction<double>
    {
        public TriangularMembershipFunction(double lo, double mid, double hi)
        {
            if (lo > mid) throw new ArgumentOutOfRangeException(nameof(lo));
            if (mid > hi) throw new ArgumentOutOfRangeException(nameof(hi));

            Low = lo;
            Mid = mid;
            High = hi;
        }

        public double Low { get; }
        public double Mid { get; }
        public double High { get; }

        public FuzzyVar Evaluate(double x)
        {
            if ((x <= Low) || (x >= High))
                return 0d;
            else if (x > Mid)
                return (High - x) / (High - Mid);
            else if (x < Mid)
                return (Low - x) / (Low - Mid);
            else
                return 1d;
        }
    }
}
