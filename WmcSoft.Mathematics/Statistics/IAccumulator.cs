﻿#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft.Statistics
{
    /// <summary>
    /// Defines statistical accumulator.
    /// </summary>
    public interface IAccumulator
    {
        /// <summary>
        /// Accumulate a new value.
        /// </summary>
        /// <param name="value">The value to accumulate.</param>
        void Add(double value);

        /// <summary>
        /// The count of accumulated values.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// The mean of the values.
        /// </summary>
        double Mean { get; }

        /// <summary>
        /// The variance of the values.
        /// </summary>
        double Variance { get; }

        /// <summary>
        /// The standard deviation of the values.
        /// </summary>
        double Sigma { get; }
    }
}