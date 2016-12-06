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
using System.Collections.Generic;
using System.Linq;

namespace WmcSoft.Collections.Specialized
{
    public class AcyclicShortestPathsAlgorithm : IShortestPaths<int, WeightedDigraph.Edge, double>
    {
        private readonly WeightedDigraph.Edge[] _edgeTo;
        private readonly double[] _distanceTo;

        public AcyclicShortestPathsAlgorithm(WeightedDigraph graph, int s) {
            var V = graph.VerticeCount;
            _edgeTo = new WeightedDigraph.Edge[V];
            _distanceTo = new double[V];

            _distanceTo.UnguardedFill(Double.PositiveInfinity);
            _distanceTo[s] = 0d;

            var topological = graph.Topological();
            foreach(int v in topological.Order)
                Relax(graph, v);
        }

        private void Relax(WeightedDigraph graph, int v) {
            foreach (var e in graph.Edges(v)) {
                int w = e.To;
                var weight = _distanceTo[v] + e.Weight;
                if (_distanceTo[w] > weight) {
                    _distanceTo[w] = weight;
                    _edgeTo[w] = e;
                }
            }
        }

        public double DistanceTo(int v) {
            return _distanceTo[v];
        }

        public bool HasPathTo(int v) {
            return _distanceTo[v] < Double.PositiveInfinity;
        }

        public IEnumerable<WeightedDigraph.Edge> PathTo(int v) {
            if (!HasPathTo(v))
                Enumerable.Empty<WeightedDigraph.Edge>();
            var path = new Stack<WeightedDigraph.Edge>();
            for(var e = _edgeTo[v]; e != default(WeightedDigraph.Edge); e = _edgeTo[e.From]) {
                path.Push(e);
            }
            return path;
        }
    }
}
