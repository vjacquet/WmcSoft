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

namespace WmcSoft.Arithmetics
{
    ///<summary>
    /// Linear interpolation is a method of curve fitting using linear polynomials.
    ///</summary>
    public class LinearInterpolation : IInterpolation<double, double>
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

            int end = ~offset;
            if (end >= _points.Length)
                end = _points.Length - 1;
            else if (end == 0)
                end = 1;
            int start = end - 1;

            return _values[start] + (x - _points[start]) * (_values[end] - _values[start]) / (_points[end] - _points[start]);
        }

        #endregion
    }
}
