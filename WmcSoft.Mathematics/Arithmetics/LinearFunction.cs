using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Arithmetics
{
    public struct LinearFunction : IFunction<double>
    {
        readonly double _slope;
        readonly double _intercept;

        public LinearFunction(double slope, double intercept) {
            _slope = slope;
            _intercept = intercept;
        }

        public double Slope { get { return _slope; } }
        public double Intercept { get { return _intercept; } }

        #region IFunction<double> Members

        public double Eval(double x) {
            return _slope * x + _intercept;
        }

        #endregion

        #region IEquatable<IFunction<double>> Members

        public bool Equals(IFunction<double> other) {
            if (other == null || GetType() != other.GetType())
                return false;
            var that = (LinearFunction)other;
            return _slope == that._slope
                && _intercept == that._intercept;
        }

        #endregion
    }
}
