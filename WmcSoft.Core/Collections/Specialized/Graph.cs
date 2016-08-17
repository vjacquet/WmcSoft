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
using System.Diagnostics;
using System.Text;
using WmcSoft.Collections.Generic;

using static System.Math;

namespace WmcSoft.Collections.Specialized
{
    /// <summary>
    /// Represents an undirected graph.
    /// </summary>
    /// <remarks>This implementation allows self loops and parallel edges.</remarks>
    [DebuggerDisplay("Vertices={Vertices,nq}, Edges={Edges,nq}")]
    [DebuggerTypeProxy(typeof(Graph.DebugView))]
    public class Graph
    {
        internal class DebugView
        {
            private readonly Graph _graph;

            public DebugView(Graph graph) {
                if (graph == null)
                    throw new ArgumentNullException("collection");

                _graph=graph;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public Bag<int>[] Items {
                get {
                    return _graph._adj;
                }
            }
        }

        private readonly Bag<int>[] _adj;
        private int _edges;

        public Graph(int vertices) {
            _adj = new Bag<int>[vertices];
            for (int i = 0; i < _adj.Length; i++) {
                _adj[i] = new Bag<int>();
            }
        }

        public int Vertices { get { return _adj.Length; } }
        public int Edges { get { return _edges; } }

        public void Connect(int v, int w) {
            _adj[v].Add(w);
            _adj[w].Add(v);
            ++_edges;
        }

        public void Disconnect(int v, int w) {
            var nw = _adj[v].RemoveAll(x => x == w);
            var nv = _adj[w].RemoveAll(x => x == v);
            Debug.Assert(nv == nw);
            _edges -= nv;
        }

        public ReadOnlyBag<int> Adjacents(int v) {
            return new ReadOnlyBag<int>(_adj[v]);
        }

        public override string ToString() {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} vertices, {1} edges", Vertices, Edges);
            sb.AppendLine();
            for (int v = 0; v < _adj.Length; v++) {
                sb.AppendFormat("{0}:", v);
                foreach (var w in _adj[v])
                    sb.Append(' ').Append(w);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        #region Helpers

        public int Degree(int v) {
            return _adj[v].Count;
        }

        public int MaxDegree() {
            if (_adj.Length == 0)
                return 0;

            var degree = Degree(0);
            for (int i = 1; i < _adj.Length; i++) {
                degree = Max(degree, Degree(i));
            }
            return degree;
        }

        public double AvgDegree() {
            return 2d * Edges / Vertices;
        }

        public int NumberOfSelfLoops() {
            int count = 0;
            for (int v = 0; v < _adj.Length; v++) {
                foreach (var w in _adj[v])
                    if (v == w)
                        count++;
            }
            return count / 2;
        }

        #endregion
    }
}