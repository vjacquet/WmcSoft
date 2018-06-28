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

namespace WmcSoft.Collections.Graphs
{
    /// <summary>
    /// Checks if the <see cref="Digraph"/> has a cycle.
    /// </summary>
    public struct DirectedCycleAlgorithm
    {
        private readonly bool[] _marked;
        private readonly int[] _edgeTo;
        private readonly bool[] _onStack;
        private Stack<int> _cycle;

        public DirectedCycleAlgorithm(IDirectedGraph graph) {
            if (graph == null) throw new ArgumentNullException(nameof(graph));

            _marked = new bool[graph.VerticeCount];
            _edgeTo = new int[graph.VerticeCount];
            _onStack = new bool[graph.VerticeCount];
            _cycle = null;

            for (int s = 0; s < graph.VerticeCount; s++) {
                if (!_marked[s])
                    Process(graph, s, s);
            }
        }

        public bool HasCycle {
            get { return _cycle != null; }
        }
        public IEnumerable<int> Cycle { get { return _cycle; } }

        private void Process(IDirectedGraph graph, int v, int u) {
            _marked[v] = true;
            _onStack[v] = true;
            foreach (var w in graph.Adjacents(v)) {
                if (HasCycle) return;
                if (!_marked[w]) {
                    _edgeTo[w] = v;
                    Process(graph, w, v);
                } else if (_onStack[w]) {
                    _cycle = new Stack<int>();
                    for (int x = 0; x != w; x = _edgeTo[x])
                        _cycle.Push(x);
                    _cycle.Push(w);
                    _cycle.Push(v);
                }
            }
            _onStack[v] = false;
        }
    }
}
