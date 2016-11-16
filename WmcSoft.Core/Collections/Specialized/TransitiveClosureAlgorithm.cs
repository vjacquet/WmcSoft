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
using WmcSoft.Collections.Generic.Internals;

namespace WmcSoft.Collections.Specialized
{
    public class TransitiveClosureAlgorithm : IDirectedGraph
    {
        private readonly DepthFirstSearchAlgorithm[] _all;
        private readonly int _edges;

        internal TransitiveClosureAlgorithm(IDirectedGraph graph) {
            if (graph == null) throw new ArgumentNullException(nameof(graph));

            var V = graph.VerticeCount;
            _all = new DepthFirstSearchAlgorithm[V];
            var edges = 0;
            for (int v = 0; v < V; v++) {
                var a = new DepthFirstSearchAlgorithm(graph, v);
                _all[v] = a;
                edges += a.Count;
            }
            _edges = edges;
        }

        private bool Reachable(int v, int w) {
            return _all[v][w];
        }

        public bool this[int v, int w] { get { return Reachable(v, w); } }

        #region IDirectedGraph methods

        public int EdgeCount { get { return _edges; } }
        public int VerticeCount { get { return _all.Length; } }

        public IReadOnlyCollection<int> Adjacents(int v) {
            var adj = _all[v];
            var enumerable = from w in Enumerable.Range(0, VerticeCount)
                             where adj[w]
                             select w;
            return new ReadOnlyCollectionAdapter<int>(adj.Count, enumerable);
        }

        public IDirectedGraph Reverse() {
            var length = VerticeCount;
            var digraph = new Digraph(length);
            for (int i = 0; i < length; i++) {
                for (int j = 0; j < length; j++) {
                    if (Reachable(i, j))
                        digraph.Connect(j, i);
                }
            }
            return digraph;
        }

        #endregion
    }
}