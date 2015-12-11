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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WmcSoft.Geometry2D
{
    public sealed class Polygon : IReadOnlyList<Point>
    {
        #region Fields

        readonly Point[] _points;

        #endregion

        #region Lifecycle

        public Polygon(params Point[] points) {
            if (points == null) throw new ArgumentNullException("points");
            if (points.Length < 3) throw new ArgumentOutOfRangeException("points");

            var n = points.Length;
            _points = new Point[n + 2];
            Array.Copy(points, 0, _points, 1, n);
            _points[0] = points[n-1];
            _points[n + 1] = points[0];
        }

        #endregion

        #region Properties

        public Point this[int index] {
            get {
                if (index >= Count) throw new ArgumentOutOfRangeException("index");

                return _points[index - 1];
            }
        }

        #endregion

        #region Methods

        public bool Inside(Point t) {
            uint n = 0;
            var length = Count;

            var st = new Segment(t, new Point(Double.PositiveInfinity, t.Y));
            var j = 0;
            for (int i = 1; i <= length; i++) {
                var sp = new Segment(_points[i], _points[i]);
                if(!sp.Intersectwith(st)) {
                    sp = new Segment(_points[i], _points[j]);
                    j = i;
                    if (sp.Intersectwith(st))
                        n++;
                }
            }

            return (n & 1) == 1;
        }

        #endregion

        #region IReadOnlyList<Point> members

        public int Count {
            get { return _points.Length - 2; }
        }

        public IEnumerator<Point> GetEnumerator() {
            var n = _points.Length - 2;
            for (int i = 0; i < n; i++) {
                yield return _points[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
