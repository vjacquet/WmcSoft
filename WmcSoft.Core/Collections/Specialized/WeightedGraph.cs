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
using WmcSoft.Collections.Generic.Internals;

namespace WmcSoft.Collections.Specialized
{
    /// <summary>
    /// Represents an edge weighted undirected graph.
    /// </summary>
    /// <remarks>This implementation allows self loops and parallel edges.</remarks>
    [DebuggerDisplay("Vertices={VerticeCount,nq}, Edges={EdgeCount,nq}")]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class WeightedGraph : IWeightedGraph<WeightedGraph.Edge>
    {
        internal class DebugView
        {
            private readonly WeightedGraph _graph;

            public DebugView(WeightedGraph graph) {
                if (graph == null) throw new ArgumentNullException("collection");
                _graph = graph;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public Bag<Edge>[] Items { get { return _graph._adj; } }
        }

        [DebuggerDisplay("{ToString(), nq}")]
        public struct Edge : IComparable<Edge>
        {
            internal readonly int _v;
            internal readonly int _w;

            public Edge(int v, int w, double weight) {
                _v = v;
                _w = w;
                Weight = weight;
            }

            public int Either { get { return _v; } }
            public int Other(int vertex) {
                if (vertex == _v) return _w;
                if (vertex == _w) return _v;
                throw new ArgumentException(nameof(vertex));
            }

            public double Weight { get; }

            public int CompareTo(Edge other) {
                return Comparer<double>.Default.Compare(Weight, other.Weight);
            }

            public override string ToString() {
                return string.Format("{0}↔{1} {2}", _v, _w, Weight);
            }
        }

        private readonly Bag<Edge>[] _adj;
        private int _edges;

        public WeightedGraph(int vertices) {
            _adj = new Bag<Edge>[vertices];
            for (int i = 0; i < _adj.Length; i++) {
                _adj[i] = new Bag<Edge>();
            }
        }

        public int VerticeCount { get { return _adj.Length; } }
        public int EdgeCount { get { return _edges; } }

        public void Connect(int v, int w, double weight) {
            if (v < 0 | v >= VerticeCount) throw new ArgumentOutOfRangeException(nameof(v));
            if (w < 0 | w >= VerticeCount) throw new ArgumentOutOfRangeException(nameof(w));

            _adj[v].Add(new Edge(v, w, weight));
            _adj[w].Add(new Edge(w, v, weight));
            ++_edges;
        }

        public void Disconnect(int v, int w) {
            if (v < 0 | v >= VerticeCount) throw new ArgumentOutOfRangeException(nameof(v));
            if (w < 0 | w >= VerticeCount) throw new ArgumentOutOfRangeException(nameof(w));

            var nw = _adj[v].RemoveAll(x => x._v == v && x._w == w);
            var nv = _adj[w].RemoveAll(x => x._w == v && x._v == w);
            Debug.Assert(nv == nw);
            _edges -= nv;
        }

        public ReadOnlyBag<Edge> Edges(int v) {
            if (v < 0 | v >= VerticeCount) throw new ArgumentOutOfRangeException(nameof(v));

            return new ReadOnlyBag<Edge>(_adj[v]);
        }

        #region IWeightedGraph implementation

        IReadOnlyCollection<Edge> IWeightedGraph<Edge>.Edges(int v) {
            return Edges(v);
        }

        public IReadOnlyCollection<int> Adjacents(int v) {
            return new ConvertingCollectionAdapter<Edge, int>(Edges(v), e => e.Other(v));
        }

        #endregion
    }
}