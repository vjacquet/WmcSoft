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
    /// Checks if the <see cref="Graph"/> is bipartite.
    /// </summary>
    public struct BipartiteAlgorithm
    {
        private readonly bool[] _marked;
        private readonly bool[] _color;
        private bool _isBipartite;

        public BipartiteAlgorithm(IGraph graph) {
            if (graph == null) throw new ArgumentNullException(nameof(graph));

            _marked = new bool[graph.VerticeCount];
            _color = new bool[graph.VerticeCount];
            _isBipartite = true;
            for (int s = 0; s < graph.VerticeCount; s++) {
                if (!_marked[s])
                    Process(graph, s);
            }
        }

        public bool IsBipartite {
            get { return _isBipartite; }
        }

        private void Process(IGraph graph, int v) {
            _marked[v] = true;
            foreach (var w in graph.Adjacents(v)) {
                if (!_marked[w]) {
                    _color[w] = !_color[v];
                    Process(graph, w);
                } else if (_color[w] == _color[v]) {
                    _isBipartite = false; // explores the complete graph eventhough we know the ansswer...
                }
            }
        }
    }
}