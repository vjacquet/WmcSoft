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
using WmcSoft.Collections.Generic;

namespace WmcSoft.Collections.Specialized
{
    public static class GraphExtensions
    {
        #region Algorithms

        /// <summary>
        /// Marks all the vertices connected to a given source.
        /// </summary>
        public static DepthFirstSearchAlgorithm DepthFirstSearch(this IGraph graph, int s) {
            return new DepthFirstSearchAlgorithm(graph, s);
        }

        /// <summary>
        /// Marks all the vertices connected to a given set of sources.
        /// </summary>
        public static DepthFirstSearchAlgorithm DepthFirstSearch(this IGraph graph, IEnumerable<int> sources) {
            return new DepthFirstSearchAlgorithm(graph, sources);
        }

        /// <summary>
        /// Finds paths to all the vertices in a graph that are connected to a given start vertex.
        /// </summary>
        public static DepthFirstPathsAlgorithm DepthFirstPaths(this IGraph graph, int s) {
            return new DepthFirstPathsAlgorithm(graph, s);
        }

        /// <summary>
        /// Finds paths to all the vertices in a graph that are connected to a given start vertex.
        /// </summary>
        public static BreathFirstPathsAlgorithm BreathFirstPaths(this IGraph graph, int s) {
            return new BreathFirstPathsAlgorithm(graph, s);
        }

        /// <summary>
        /// Divides the vertices into equivalence classes (the connected components).
        /// </summary>
        public static ConnectedComponentsAlgorithm<TGraph> ConnectedComponents<TGraph>(this TGraph graph)
            where TGraph : IGraph {
            return new ConnectedComponentsAlgorithm<TGraph>(graph);
        }

        /// <summary>
        /// Checks if the <paramref name="graph"/> has a cycle.
        /// </summary>
        public static bool HasCycle(this IGraph graph) {
            var a = new CycleAlgorithm(graph);
            return a.HasCycle;
        }

        /// <summary>
        /// Checks if the <paramref name="graph"/> is bipartite.
        /// </summary>
        public static bool IsBipartite<TGraph>(this TGraph graph)
            where TGraph : IGraph {
            var a = new BipartiteAlgorithm<TGraph>(graph);
            return a.IsBipartite;
        }

        /// <summary>
        /// Returns the <paramref name="graph"/> directed cycles.
        /// </summary>
        public static DirectedCycleAlgorithm Cycle(this IDirectedGraph graph) {
            return new DirectedCycleAlgorithm(graph);
        }

        public static TopologicalAlgorithm Topological(this IDirectedGraph graph) {
            return new TopologicalAlgorithm(graph);
        }

        public static KosarajuSharirStrongConnectedComponentsAlgorithm StrongConnectedComponents(this IDirectedGraph graph) {
            return new KosarajuSharirStrongConnectedComponentsAlgorithm(graph);
        }

        /// <summary>
        /// The transitive closure of a digraph is another digraph with the same set of vertices, 
        /// but with an edge from v to w if and only if w is reachable from v.
        /// </summary>
        /// <param name="graph">The digraph</param>
        /// <returns>The transitive closure</returns>
        public static TransitiveClosureAlgorithm TransitiveClosure(this IDirectedGraph graph) {
            return new TransitiveClosureAlgorithm(graph);
        }

        public static DijkstraShortestPathsAlgorithm DijkstraShortestPaths(this WeightedDigraph graph, int s) {
            return new DijkstraShortestPathsAlgorithm(graph, s);
        }

        #endregion

        #region Typical graph processing methods

        /// <summary>
        /// Computes the degree of the vertex <paramref name="v"/>.
        /// </summary>
        /// <typeparam name="TGraph">The type of graph.</typeparam>
        /// <param name="graph">The graph.</param>
        /// <param name="v">The vertex.</param>
        /// <returns>The degree of the vertex.</returns>
        public static int Degree<TGraph>(this TGraph graph, int v)
            where TGraph : IGraph {
            return graph.Adjacents(v).Count;
        }

        /// <summary>
        /// Computes the minimum degree.
        /// </summary>
        /// <typeparam name="TGraph">The type of graph.</typeparam>
        /// <param name="graph">The graph.</param>
        /// <returns>The minimum degree.</returns>
        public static int MinDegree<TGraph>(this TGraph graph)
            where TGraph : IGraph {
            return Enumerable.Range(0, graph.VerticeCount)
                .Max(v => graph.Adjacents(v).Count);
        }

        /// <summary>
        /// Computes the maximum degree.
        /// </summary>
        /// <typeparam name="TGraph">The type of graph.</typeparam>
        /// <param name="graph">The graph.</param>
        /// <returns>The maximum degree.</returns>
        public static int MaxDegree<TGraph>(this TGraph graph)
            where TGraph : IGraph {
            return Enumerable.Range(0, graph.VerticeCount)
                .Max(v => graph.Adjacents(v).Count);
        }

        public static Tuple<int,int> MinMaxDegree<TGraph>(this TGraph graph)
            where TGraph : IGraph {
            return Enumerable.Range(0, graph.VerticeCount)
                .MinMax(v => graph.Adjacents(v).Count);
        }

        /// <summary>
        /// Computes the average degree.
        /// </summary>
        /// <typeparam name="TGraph">The type of graph.</typeparam>
        /// <param name="graph">The graph.</param>
        /// <returns>The average degree.</returns>
        public static double AverageDegree<TGraph>(this TGraph graph)
            where TGraph : IGraph {
            if (graph is IDirectedGraph)
                throw new NotSupportedException(); // verify if true for directed graphs
            return 2d * graph.EdgeCount / graph.VerticeCount;
        }

        /// <summary>
        /// Computes the average degree.
        /// </summary>
        /// <typeparam name="TGraph">The type of graph.</typeparam>
        /// <param name="graph">The graph.</param>
        /// <returns>The average degree.</returns>
        public static double NumberOfSelfLoops<TGraph>(this TGraph graph)
            where TGraph : IGraph {
            int count = 0;
            int V = graph.VerticeCount;
            for (int v = 0; v < V; v++)
                foreach (var w in graph.Adjacents(v))
                    if (v == w) count++;
            return count / 2; // each edge counted twice.
        }

        #endregion

        #region Connect

        public static TGraph Connect<TGraph, TVertex>(this TGraph graph, TVertex v, params TVertex[] w)
            where TGraph : IGraphBuilder<TVertex> {
            for (int i = 0; i < w.Length; i++) {
                graph.Connect(v, w[i]);
            }
            return graph;
        }

        #endregion
    }
}