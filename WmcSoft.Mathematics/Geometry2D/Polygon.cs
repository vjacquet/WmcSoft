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

using System.Collections;
using System.Collections.Generic;

namespace WmcSoft.Geometry2D
{
    public sealed class Polygon : IReadOnlyList<Point>
    {
        #region Fields

        readonly Point[] _points;

        #endregion

        #region Lifecycle

        public Polygon(params Point[] points) {
            _points = (Point[])points.Clone();
        }

        #endregion

        #region Properties

        public Point this[int index] {
            get {
                return _points[index];
            }
        }

        #endregion

        #region IReadOnlyList<Point> members

        public int Count {
            get { return _points.Length; }
        }

        public IEnumerator<Point> GetEnumerator() {
            return ((IReadOnlyList<Point>)_points).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _points.GetEnumerator();
        }

        #endregion
    }
}
