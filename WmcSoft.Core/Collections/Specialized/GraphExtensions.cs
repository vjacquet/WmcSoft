﻿#region Licence

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

namespace WmcSoft.Collections.Specialized
{
    public static class GraphExtensions
    {
        /// <summary>
        /// Marks all the vertices connected to a given source.
        /// </summary>
        public static DepthFirstSearchAlgorithm DepthFirstSearch(this Graph graph, int s) {
            return new DepthFirstSearchAlgorithm(graph, s);
        }

        /// <summary>
        /// Finds paths to all the vertices in a graph that are connected to a given start vertex.
        /// </summary>
        public static DepthFirstPathsAlgorithm DepthFirstPaths(this Graph graph, int s) {
            return new DepthFirstPathsAlgorithm(graph, s);
        }

        /// <summary>
        /// Finds paths to all the vertices in a graph that are connected to a given start vertex.
        /// </summary>
        public static BreathFirstPathsAlgorithm BreathFirstPaths(this Graph graph, int s) {
            return new BreathFirstPathsAlgorithm(graph, s);
        }

        /// <summary>
        /// Divides the vertices into equivalence classes (the connected components).
        /// </summary>
        public static ConnectedComponentsAlgorithm ConnectedComponents(this Graph graph) {
            return new ConnectedComponentsAlgorithm(graph);
        }
    }
}
