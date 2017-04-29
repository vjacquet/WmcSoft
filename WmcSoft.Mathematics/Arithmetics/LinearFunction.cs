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

namespace WmcSoft.Arithmetics
{
    public struct LinearFunction : IFunction<double>
    {
        readonly double _slope;
        readonly double _intercept;

        public LinearFunction(double slope, double intercept)
        {
            _slope = slope;
            _intercept = intercept;
        }

        public double Slope { get { return _slope; } }
        public double Intercept { get { return _intercept; } }

        #region IFunction<double> Members

        public double Eval(double x)
        {
            return _slope * x + _intercept;
        }

        #endregion

        #region IEquatable<IFunction<double>> Members

        public bool Equals(IFunction<double> other)
        {
            if (other == null || GetType() != other.GetType())
                return false;
            var that = (LinearFunction)other;
            return _slope == that._slope
                && _intercept == that._intercept;
        }

        #endregion
    }
}
