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
    /// Marks all the vertices connected to a given source.
    /// </summary>
    /// <remarks>The operation is performed in time proportional to the sum of their degree.</remarks>
    public struct DepthFirstSearchAlgorithm
    {
        private readonly bool[] _marked;
        private int _count;

        /// <summary>
        /// Single-source reachability constructor.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="s">The source.</param>
        public DepthFirstSearchAlgorithm(IGraph graph, int s) {
            if (graph == null) throw new ArgumentNullException(nameof(graph));

            _marked = new bool[graph.VerticeCount];
            _count = 0;
            Process(graph, s);
        }

        /// <summary>
        /// Multi-source reachability constructor.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources.</param>
        public DepthFirstSearchAlgorithm(IGraph graph, IEnumerable<int> sources) {
            if (graph == null) throw new ArgumentNullException(nameof(graph));
            if (sources == null) throw new ArgumentNullException(nameof(sources));

            _marked = new bool[graph.VerticeCount];
            _count = 0;
            foreach (var s in sources)
                if (!_marked[s])
                    Process(graph, s);
        }

        public bool this[int w] { get { return _marked[w]; } }

        public int Count { get { return _count; } }

        private void Process(IGraph graph, int v) {
            _marked[v] = true;
            _count++;
            foreach (var w in graph.Adjacents(v))
                if (!_marked[w])
                    Process(graph, w);
        }
    }
}
