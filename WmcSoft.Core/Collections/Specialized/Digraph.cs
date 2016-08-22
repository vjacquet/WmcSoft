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
using System.Diagnostics;
using WmcSoft.Collections.Generic;

namespace WmcSoft.Collections.Specialized
{
    /// <summary>
    /// Represents a directed graph.
    /// </summary>
    /// <remarks>This implementation allows self loops and parallel edges.</remarks>
    [DebuggerDisplay("Vertices={VerticeCount,nq}, Edges={EdgeCount,nq}")]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class Digraph : IGraph
    {
        internal class DebugView
        {
            private readonly Digraph _graph;

            public DebugView(Digraph graph) {
                if (graph == null) throw new ArgumentNullException("collection");
                _graph = graph;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public Bag<int>[] Items { get { return _graph._adj; } }
        }

        private readonly Bag<int>[] _adj;
        private int _edges;

        public Digraph(int vertices) {
            _adj = new Bag<int>[vertices];
            for (int i = 0; i < _adj.Length; i++) {
                _adj[i] = new Bag<int>();
            }
        }

        public int VerticeCount { get { return _adj.Length; } }
        public int EdgeCount { get { return _edges; } }

        public void Connect(int v, int w) {
            if (v < 0 | v >= VerticeCount) throw new ArgumentOutOfRangeException(nameof(v));
            if (w < 0 | w >= VerticeCount) throw new ArgumentOutOfRangeException(nameof(w));

            _adj[v].Add(w);
            ++_edges;
        }

        public void Disconnect(int v, int w) {
            if (v < 0 | v >= VerticeCount) throw new ArgumentOutOfRangeException(nameof(v));
            if (w < 0 | w >= VerticeCount) throw new ArgumentOutOfRangeException(nameof(w));

            var n = _adj[v].RemoveAll(x => x == w);
            _edges -= n;
        }

        public ReadOnlyBag<int> Adjacents(int v) {
            if (v < 0 | v >= VerticeCount) throw new ArgumentOutOfRangeException(nameof(v));

            return new ReadOnlyBag<int>(_adj[v]);
        }

        public Digraph Reverse() {
            var digraph = new Digraph(VerticeCount);
            for (int v = 0; v < _adj.Length; v++) {
                foreach (var w in _adj[v])
                    digraph._adj[w].Add(v);
            }
            digraph._edges = _edges;
            return digraph;
        }

        #region IGraph implementation

        IReadOnlyCollection<int> IGraph.Adjacents(int v) {
            return Adjacents(v);
        }

        #endregion
    }
}