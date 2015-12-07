﻿#region Licence

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

namespace WmcSoft.Geometry2D
{
    public struct Point : IEquatable<Point>
    {
        #region Lifecycle

        public Point(string name, double x, double y) {
            Name = name;
            X = x;
            Y = y;
        }
        public Point(double x, double y) : this(null, x, y) {
        }

        #endregion

        #region Properties

        public double X { get; }
        public double Y { get; }
        public string Name { get; }

        #endregion

        #region Methods

        static double Distance2(double dx, double dy) {
            return dx * dx + dy * dy;
        }

        public static int CounterClockwise(Point p0, Point p1, Point p2) {
            var dx1 = p1.X - p0.X;
            var dy1 = p1.Y - p0.Y;
            var dx2 = p2.X - p0.X;
            var dy2 = p2.Y - p0.Y;
            if (dx1 * dy2 > dy1 * dx2) return +1;
            if (dx1 * dy2 < dy1 * dx2) return -1;
            if ((dx1 * dx2 < 0) || (dy1 * dy2 < 0)) return -1;
            if (Distance2(dx1, dy1) < Distance2(dx2, dy2)) return +1;
            return 0;
        }

        #endregion

        #region Operators

        public static bool operator ==(Point x, Point y) {
            return x.Equals(y);
        }

        public static bool operator !=(Point x, Point y) {
            return !x.Equals(y);
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != typeof(Point))
                return false;
            return Equals((Point)obj);
        }

        public override int GetHashCode() {
            return X.GetHashCode() * 397 ^ Y.GetHashCode();
        }

        public override string ToString() {
            if (Name != null)
                return $"{Name} ({X},{Y})";
            return $"({X},{Y})";
        }

        #endregion

        #region IEquatable<Point> members

        public bool Equals(Point other) {
            return X == other.X && Y == other.Y;
        }

        #endregion
    }
}