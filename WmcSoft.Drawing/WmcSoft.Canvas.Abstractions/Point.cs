#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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

 ****************************************************************************
 * Based on <https://html.spec.whatwg.org/multipage/scripting.html#2dcontext>
 ****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Drawing
{
    public struct Point : IEquatable<Point>
    {
        private double x, y, z, w;

        private Point(double x, double y, double z, double w) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public static Point Create(double x = 0d, double y = 0d, double z = 0d, double w = 1d) {
            return new Point(x, y, z, w);
        }

        public double X { get { return x; } }
        public double Y { get { return y; } }
        public double Z { get { return z; } }
        public double W { get { return w; } }

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != typeof(Point))
                return false;
            return Equals((Point)obj);
        }
        public override int GetHashCode() {
            return CombineHashCodes(x.GetHashCode(), y.GetHashCode(), z.GetHashCode(), w.GetHashCode());
        }
        public bool Equals(Point other) {
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z) && w.Equals(other.w);
        }

        public override string ToString() {
            var sb = new StringBuilder();
            sb.Append('(');
            sb.Append(x).Append(',');
            sb.Append(y).Append(',');
            sb.Append(z).Append(',');
            sb.Append(w);
            sb.Append(')');
            return base.ToString();
        }

        internal static int CombineHashCodes(int h1, int h2) {
            return (((h1 << 5) + h1) ^ h2);
        }
        internal static int CombineHashCodes(int h1, int h2, int h3, int h4) {
            return CombineHashCodes(CombineHashCodes(h1, h2), CombineHashCodes(h3, h4));
        }
    }

    public class PointBuilder
    {
        private double x, y, z, w;

        public PointBuilder(double x = 0d, double y = 0d, double z = 0d, double w = 1d) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }
}
