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

namespace WmcSoft.AI.FuzzyLogic
{
    /// <summary>
    /// A membership function evaluating to µA(x) = 1 / (1 + K(x-a)²)
    /// </summary>
    public struct HeavyTailedMembershipFunction : IMembershipFunction<double>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeavyTailedMembershipFunction"/>.
        /// </summary>
        /// <param name="K">Controls the width of the fuzzy set.</param>
        /// <param name="a">Controls the position of the fuzzy set.</param>
        public HeavyTailedMembershipFunction(double K, double a)
        {
            this.K = K;
            this.a = a;
        }

        public double K { get; }
        public double a { get; }

        public FuzzyVar Evaluate(double x)
        {
            x -= a;
            return 1d / (1d + K * x * x);
        }
    }
}
