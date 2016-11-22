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

namespace WmcSoft.Collections.Specialized
{
    /// <summary>
    /// Finds paths to all the vertices in a graph that are connected to a given start vertex.
    /// </summary>
    /// <remarks>The operation is performed in time proportional to 
    /// <see cref="Graph.VerticeCount"/>+<see cref="Graph.EdgeCount"/> in the worst case.</remarks>
    public struct BreathFirstPathsAlgorithm : IPaths<int,int>
    {
        private readonly bool[] _marked;
        private readonly int[] _edgeTo;
        private readonly int _s;

        public BreathFirstPathsAlgorithm(IGraph graph, int s) {
            if (graph == null) throw new ArgumentNullException(nameof(graph));

            _marked = new bool[graph.VerticeCount];
            _edgeTo = new int[graph.VerticeCount];
            _s = s;
            Process(graph, s);
        }

        public bool HasPathTo(int w) {
            return _marked[w];
        }

        public IEnumerable<int> PathTo(int v) {
            var path = new Stack<int>();
            if (HasPathTo(v)) {
                for (int x = v; x != _s; x = _edgeTo[x])
                    path.Push(x);
                path.Push(_s);
            }
            return path;
        }

        private void Process(IGraph graph, int s) {
            var q = new Queue<int>();
            _marked[s] = true;
            q.Enqueue(s);
            while (q.Count > 0) {
                var v = q.Dequeue();
                foreach (var w in graph.Adjacents(v)) {
                    if (!_marked[w]) {
                        _edgeTo[w] = v;
                        _marked[w] = true;
                        q.Enqueue(w);
                    }
                }
            }
        }
    }
}