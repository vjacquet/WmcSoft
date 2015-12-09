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
using static System.Math;

namespace WmcSoft.Geometry2D
{
    public struct Point : IEquatable<Point>, IFormattable
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

        public static int Clockwise(Point p0, Point p1, Point p2) {
            return -CounterClockwise(p0, p1, p2);
        }

        /// <summary>
        /// Compute the pseudo angle with the horizontal axis.
        /// </summary>
        /// <param name="p1">The start point of the segment</param>
        /// <param name="p2">The end point of the segment</param>
        /// <returns>The pseudo angle with the horizontal axis.</returns>
        public static double Theta(Point p1, Point p2) {
            var dx = p2.X - p1.X;
            var ax = Abs(dx);
            var dy = p2.Y - p1.Y;
            var ay = Abs(dx);
            var t = ax + ay == 0d ? 0d : dy / (ax + ay);
            if (dx < 0d)
                t = 2 - t;
            else if (dy < 0d)
                t += 4d;
            return t * 90d;
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

        public sealed override string ToString() {
            return ToString("G", null);
        }

        #endregion

        #region IEquatable<Point> members

        public bool Equals(Point other) {
            return X == other.X && Y == other.Y;
        }

        #endregion

        #region IFormattable Membres

        public string ToString(string format) {
            return ToString(format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider) {
            switch (format) {
            case "c":
            case "C":
                return $"({X},{Y})";
            case "n":
            case "N":
                if (Name != null)
                    return Name;
                goto case "C";
            case "g":
            case "G":
            default:
                if (Name != null)
                    return $"{Name} ({X},{Y})";
                goto case "C";
            }
        }

        #endregion
    }
}
