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
using static WmcSoft.Geometry2D.Helpers;

namespace WmcSoft.Geometry2D
{
    public sealed class Segment : IEquatable<Segment>
    {
        #region Lifecycle

        public Segment(Point p1, Point p2) {
            P1 = p1;
            P2 = p2;
        }

        #endregion

        #region Properties

        public Point P1 { get; }
        public Point P2 { get; }
        public double Length => Distance(P1, P2);

        #endregion

        #region Methods

        public bool Intersectwith(Segment s) {
            return (CounterClockwise(P1, P2, s.P1) * CounterClockwise(P1, P2, s.P2) <= 0)
                && (CounterClockwise(s.P1, s.P2, P1) * CounterClockwise(s.P1, s.P2, P2) <= 0);
        }

        /// <summary>
        /// Compute the pseudo angle with the horizontal axis.
        /// </summary>
        /// <param name="s">The segment</param>
        /// <returns>The pseudo angle with the horizontal axis.</returns>
        public double Theta() {
            return Helpers.Theta(P1, P2);
        }

        #endregion

        #region Operators

        public static bool operator ==(Segment x, Segment y) {
            return x.Equals(y);
        }

        public static bool operator !=(Segment x, Segment y) {
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
            return P1.GetHashCode() ^ P2.GetHashCode();
        }

        public override string ToString() {
            if (String.IsNullOrWhiteSpace(P1.Name) || String.IsNullOrWhiteSpace(P2.Name))
                return $"[{P1},{P2}]";
            return $"[{P1.Name}{P2.Name}]";
        }

        #endregion

        #region IEquatable<Point> members

        public bool Equals(Segment other) {
            return P1 == other.P1 && P2 == other.P2;
        }

        #endregion
    }
}
