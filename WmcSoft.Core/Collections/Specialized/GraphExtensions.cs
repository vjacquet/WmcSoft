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

using System.Collections.Generic;

namespace WmcSoft.Collections.Specialized
{
    public static class GraphExtensions
    {
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
        public static ConnectedComponentsAlgorithm ConnectedComponents(this IGraph graph) {
            return new ConnectedComponentsAlgorithm(graph);
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
        public static bool IsBipartite(this IGraph graph) {
            var a = new BipartiteAlgorithm(graph);
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

        public static TGraph Connect<TGraph>(this TGraph graph, int v, params int[] w)
            where TGraph : IGraphBuilder {
            for (int i = 0; i < w.Length; i++) {
                graph.Connect(v, w[i]);
            }
            return graph;
        }
    }
}