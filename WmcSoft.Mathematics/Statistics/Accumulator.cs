using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Tests.Statistics
{
    /// <summary>
    /// Computes Sum, Mean, Variance and other statical entities in linear time of the measures
    /// </summary>
    public class Accumulator
    {
        #region Fields

        int _count;
        double _sum;
        double _squaredSum;

        #endregion

        #region Lifecycle

        public Accumulator() {
        }

        public Accumulator(params double[] values)
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

        public void Add(Accumulator accumulator) {
            _count += accumulator._count;
            _sum += accumulator._sum;
            _squaredSum += accumulator._squaredSum;
        }

        public void RemoveRange(IEnumerable<double> values) {
            var accumulator = new Accumulator();
            accumulator.AddRange(values);
            if (accumulator._count >= _count)
                throw new InvalidOperationException();

            Remove(accumulator);
        }

        public void Remove(double value) {
            if (_count == 0)
                throw new InvalidOperationException();
            if (_count == 1) {
                if (!_sum.Equals(value))
                    throw new ArgumentOutOfRangeException("value");
                Reset();
                return;
            }

            _count--;
            _sum -= value;
            _squaredSum -= value * value;
        }

        public void Remove(Accumulator accumulator) {
            if (_count < accumulator._count)
                throw new InvalidOperationException();
            if (_count == accumulator._count) {
                if (!_sum.Equals(accumulator._sum))
                    throw new ArgumentOutOfRangeException("accumulator");
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
        public double MeanSquared { get { return (_count == 0) ? 0d : _squaredSum / _count; } }
        public double Variance {
            get {
                if (_count == 0)
                    return 0d;
                var mean = Mean;
                return MeanSquared - mean * mean;
            }
        }
        public double Sigma { get { return Math.Sqrt(Variance); } }
        public double ErrorEstimate { get { return Sigma / Math.Sqrt(_count); } }

        #endregion
    }
}
