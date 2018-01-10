#region Licence

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

using System.Diagnostics;

using static System.Math;

namespace WmcSoft.Statistics
{
    /// <summary>
    /// Computes Sum, Mean, Variance and other statical entities in linear time of the measures.
    /// The measures are the complete population.
    /// </summary>
    /// <remarks>This implementation is less susceptible to roundoff error than the
    /// straightforward implementation (see Robert Sedgewick & Kevin Wayne, Algorithms, Fourth edition, Page 118).</remarks>
    [DebuggerDisplay("µ={Mean,nq}, Var={Variance,nq}, σ={Sigma,nq}")]
    public class PopulationAccumulator : IAccumulator
    {
        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        int _count;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double _mean;
        double _s;

        #endregion

        #region Lifecycle

        public PopulationAccumulator()
        {
        }

        public PopulationAccumulator(params double[] values)
            : this()
        {
            for (int i = 0; i < values.Length; i++) {
                Add(values[i]);
            }
        }

        #endregion

        #region Methods

        public void Add(double value)
        {
            _count++;
            var delta = (value - _mean);
            var N = (double)_count;
            _s += (N - 1d) * delta * delta / N;
            _mean += delta / N;
        }

        #endregion

        #region Properties

        public int Count { get { return _count; } }
        public double Mean { get { return _mean; } }
        public double Variance {
            get {
                if (_count < 2)
                    return 0d;
                return _s / _count;
            }
        }
        public double Sigma { get { return Sqrt(Variance); } }

        #endregion
    }
}
