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
using WmcSoft.Collections.Generic;

namespace WmcSoft.Collections.Graphs
{
    /// <summary>
    /// Enables clients to iterate through the verties in various order defined by depth-first search.
    /// </summary>
    public struct DepthFirstOrderAlgorithm
    {
        private readonly bool[] _marked;
        private readonly List<int> _pre;
        private readonly List<int> _post;

        public DepthFirstOrderAlgorithm(IDirectedGraph graph) {
            if (graph == null) throw new ArgumentNullException(nameof(graph));

            _marked = new bool[graph.VerticeCount];
            _pre = new List<int>();
            _post = new List<int>();

            for (int v = 0; v < graph.VerticeCount; v++) {
                if (!_marked[v]) {
                    Process(graph, v);
                }
            }
        }

        public IReadOnlyCollection<int> PreOrder {
            get { return _pre; }
        }

        public IReadOnlyCollection<int> PostOrder {
            get { return _post; }
        }

        public IReadOnlyCollection<int> ReversePostOrder {
            get { return _post.Backwards<int>(); }
        }

        private void Process(IGraph graph, int v) {
            _pre.Add(v);

            _marked[v] = true;
            foreach (var w in graph.Adjacents(v))
                if (!_marked[w])
                    Process(graph, w);

            _post.Add(v);
        }
    }
}
