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
using static System.Math;

namespace WmcSoft.Statistics
{
    /// <summary>
    /// Computes Sum, Mean, Variance and other statical entities in linear time of the measures.
    /// </summary>
    /// <remarks>Values can be removed.</remarks>
    public class StraightforwardAccumulator : IAccumulator
    {
        #region Fields

        int _count;
        double _sum;
        double _squaredSum;

        #endregion

        #region Lifecycle

        public StraightforwardAccumulator() {
        }

        public StraightforwardAccumulator(params double[] values)
            : this() {
            AddRange(values);
        }

        #endregion

        #region Methods

        public void Reset() {
            _count = 0;
            _sum = 0d;
            _squaredSum = 0d;
        }

        public void AddRange(IEnumerable<double> values) {
            if (values == null)
                return;

            foreach (var value in values) {
                Add(value);
            }
        }

        public void Add(double value) {
            _count++;
            _sum += value;
            _squaredSum += value * value;
        }

        public void Add(StraightforwardAccumulator accumulator) {
            _count += accumulator._count;
            _sum += accumulator._sum;
            _squaredSum += accumulator._squaredSum;
        }

        public void RemoveRange(IEnumerable<double> values) {
            var accumulator = new StraightforwardAccumulator();
            accumulator.AddRange(values);
            if (accumulator._count >= _count)
                throw new InvalidOperationException();

            Remove(accumulator);
        }

        public void Remove(double value) {
            if (_count == 0) throw new InvalidOperationException();
            if (_count == 1) {
                if (!_sum.Equals(value)) throw new ArgumentOutOfRangeException("value");

                Reset();
                return;
            }

            _count--;
            _sum -= value;
            _squaredSum -= value * value;
        }

        public void Remove(StraightforwardAccumulator accumulator) {
            if (_count < accumulator._count) throw new InvalidOperationException();
            if (_count == accumulator._count) {
                if (!_sum.Equals(accumulator._sum)) throw new ArgumentOutOfRangeException("accumulator");

                Reset();
                return;
            }

            _count -= accumulator._count;
            _sum -= accumulator._sum;
            _squaredSum -= accumulator._squaredSum;
        }

        #endregion

        #region Properties

        public int Count { get { return _count; } }
        public double Sum { get { return _sum; } }
        public double SquaredSum { get { return _squaredSum; } }
        public double Mean { get { return (_count == 0) ? 0d : _sum / _count; } }
        public double SquaredMean { get { return (_count == 0) ? 0d : _squaredSum / _count; } }
        public double Variance {
            get {
                if (_count == 0)
                    return 0d;
                var mean = Mean;
                return SquaredMean - mean * mean;
            }
        }
        public double Sigma { get { return Sqrt(Variance); } }
        public double ErrorEstimate { get { return Sigma / Sqrt(_count); } }

        #endregion
    }
}
