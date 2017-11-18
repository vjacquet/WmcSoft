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

using static WmcSoft.Collections.Generic.EqualityComparer;
using TWeight = System.Double;

namespace WmcSoft.Collections.Specialized
{
    /// <summary>
    /// Represents an edge weighted directed graph.
    /// </summary>
    /// <remarks>This implementation allows self loops and parallel edges.</remarks>
    [DebuggerDisplay("Vertices={VerticeCount,nq}, Edges={EdgeCount,nq}")]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class WeightedDigraph : IWeightedGraph<WeightedDigraph.Edge>, IDirectedGraph
    {
        internal class DebugView
        {
            private readonly WeightedDigraph _graph;

            public DebugView(WeightedDigraph graph) {
                _graph = graph;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public Bag<Edge>[] Items { get { return _graph?._adj; } }
        }

        [DebuggerDisplay("{ToString(), nq}")]
        public struct Edge : IComparable<Edge>, IEquatable<Edge>
        {
            internal readonly int _v;
            internal readonly int _w;

            public Edge(int v, int w, TWeight weight) {
                _v = v;
                _w = w;
                Weight = weight;
            }

            public int From { get { return _v; } }
            public int To { get { return _w; } }

            public TWeight Weight { get; }

            public int CompareTo(Edge other) {
                return Comparer<TWeight>.Default.Compare(Weight, other.Weight);
            }

            public override string ToString() {
                return string.Format("{0}→{1} {2}", _w, _v, Weight);
            }

            public override int GetHashCode() {
                return CombineHashCodes(_v, _w, Weight.GetHashCode());
            }

            public override bool Equals(object obj) {
                if (obj == null || obj.GetType() != typeof(Edge))
                    return false;
                return Equals((Edge)obj);
            }

            public bool Equals(Edge other) {
                return _v.Equals(other._v) && _w.Equals(other._w) && Weight.Equals(other.Weight);
            }

            #region Operators
            public static bool operator ==(Edge x, Edge y) {
                return x.Equals(y);
            }

            public static bool operator !=(Edge a, Edge b) {
                return !a.Equals(b);
            }

            #endregion
        }

        private readonly Bag<Edge>[] _adj;
        private int _edges;

        public WeightedDigraph(int vertices) {
            _adj = new Bag<Edge>[vertices];
            for (int i = 0; i < _adj.Length; i++) {
                _adj[i] = new Bag<Edge>();
            }
        }

        public int VerticeCount { get { return _adj.Length; } }
        public int EdgeCount { get { return _edges; } }

        public void Connect(int v, int w, TWeight weight) {
            if (v < 0 | v >= VerticeCount) throw new ArgumentOutOfRangeException(nameof(v));
            if (w < 0 | w >= VerticeCount) throw new ArgumentOutOfRangeException(nameof(w));

            _adj[v].Add(new Edge(v, w, weight));
            ++_edges;
        }

        public void Disconnect(int v, int w) {
            if (v < 0 | v >= VerticeCount) throw new ArgumentOutOfRangeException(nameof(v));
            if (w < 0 | w >= VerticeCount) throw new ArgumentOutOfRangeException(nameof(w));

            var n = _adj[v].RemoveAll(x => x._w == w);
            _edges -= n;
        }

        public ReadOnlyBag<Edge> Edges(int v) {
            if (v < 0 | v >= VerticeCount) throw new ArgumentOutOfRangeException(nameof(v));

            return new ReadOnlyBag<Edge>(_adj[v]);
        }

        public WeightedDigraph Reverse() {
            var digraph = new WeightedDigraph(VerticeCount);
            for (int v = 0; v < _adj.Length; v++) {
                foreach (var e in _adj[v])
                    digraph.Connect(e.To, e.From, e.Weight);
            }
            digraph._edges = _edges;
            return digraph;
        }

        #region IWeightedGraph implementation

        IReadOnlyCollection<Edge> IWeightedGraph<Edge>.Edges(int v) {
            return Edges(v);
        }

        public IReadOnlyCollection<int> Adjacents(int v) {
            return new ConvertingCollectionAdapter<Edge, int>(Edges(v), e => e.To);
        }

        IDirectedGraph IDirectedGraph.Reverse() {
            return Reverse();
        }

        #endregion
    }
}
