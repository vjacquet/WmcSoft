
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Arithmetics
{
    ///<summary>
    /// Linear interpolation is a method of curve fitting using linear polynomials.
    ///</summary>
    public class LinearInterpolation : IInterpolation<double,double>
    {
        #region fields

        private readonly double[] _points;
        private readonly IReadOnlyList<double> _values;

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearInterpolation"/> class.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="values">The values.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public LinearInterpolation(double[] points, IReadOnlyList<double> values) {
            if (points == null)
                throw new ArgumentNullException("points");
            if (values == null)
                throw new ArgumentNullException("values");

            if (points.Length < 1 || values.Count < 1 || points.Length != values.Count) {
                throw new ArgumentOutOfRangeException("Bas size for arrays");
            }

            _points = points;
            _values = values;
        }

        #endregion

        #region IInterpolation Members

        /// <summary>
        /// Interpolate at point x.
        /// </summary>
        /// <param name="xCoordinate">Point t to interpolate at.</param>
        /// <returns>Interpolated value x(xCoordinate).</returns>
        public double Interpolate(double x) {
            var offset = Array.BinarySearch(_points, x);
            if (offset >= 0)
                return _values[offset];

            int start = ~offset;
            if (start >= _points.Length) {
                start = _points.Length - 1;
            }
            int end = start + 1;

            return _values[start] + (_values[end] - _values[start]) / (_points[end] - _points[start]);
        }

        #endregion
    }
}
