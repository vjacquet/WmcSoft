using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace WmcSoft.Geometry2D
{
    public static class Helpers
    {
        public static double Distance2(double dx, double dy) {
            return dx * dx + dy * dy;
        }

        public static double Distance(double dx, double dy) {
            return Sqrt(Distance2(dx, dy));
        }

        public static double Distance(Point p1, Point p2) {
            return Distance(p2.X - p1.X, p2.Y - p1.Y);
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
    }
}
