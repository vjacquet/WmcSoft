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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.AI.FuzzyLogic
{
    public struct TrapezoidMembershipFunction : IMembershipFunction<double>
    {
        public TrapezoidMembershipFunction(double lo, double left, double right, double hi) {
            if (lo > left) throw new ArgumentOutOfRangeException("lo");
            if (right > hi) throw new ArgumentOutOfRangeException("hi");
            if (right < left) throw new ArgumentOutOfRangeException("right");

            Low = lo;
            Left = left;
            Right = right;
            High = hi;
        }

        public double Low { get; }
        public double Left { get; }
        public double Right { get; }
        public double High { get; }

        public FuzzyVar Evaluate(double x) {
            if (x <= Right) {
                if (Double.IsNegativeInfinity(Low) || x >= Left)
                    return 1d;
                if (x < Low)
                    return 0d;
                return (Low - x) / (Low - Left);
            } else if (x >= Left) {
                if (Double.IsPositiveInfinity(High))
                    return 1d;
                if (x > High)
                    return 0d;
                return (High - x) / (High - Right);
            } else {
                return 1d;
            }
        }
    }
}
