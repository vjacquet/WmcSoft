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

 ****************************************************************************/

#endregion

using System;
using System.Linq;

namespace WmcSoft.Collections.Graphs
{
    public class GraphProperties
    {
        public GraphProperties(IGraph graph) {
            if (graph == null) throw new ArgumentNullException(nameof(graph));
            if (graph.VerticeCount <= 0) throw new ArgumentException(nameof(graph));

            // compute eccentricities
            var length = graph.VerticeCount;

            var algorithms = new BreathFirstPathsAlgorithm[length];
            for (int i = 0; i < length; i++) {
                algorithms[i] = new BreathFirstPathsAlgorithm(graph, i);
            }

            var eccentricities = new int[length];
            for (int i = 0; i < length; i++) {
                eccentricities[i] = algorithms.Max(a => a.DistanceTo(i));
            }

            var diameter = eccentricities[0];
            var radius = eccentricities[0];
            var center = 0;
            for (int i = 1; i < length; i++) {
                if (eccentricities[i] < radius) {
                    radius = eccentricities[i];
                    center = i;
                } else if (eccentricities[i] > diameter) {
                    diameter = eccentricities[i];
                }
            }

            Eccentricity = eccentricities;
            Diameter = diameter;
            Radius = radius;
            Center = center;
        }

        public int[] Eccentricity { get; }
        public int Diameter { get; }
        public int Radius { get; }
        public int Center { get; }
    }
}
