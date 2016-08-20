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
    /// Divides the vertices into equivalence classes (the connected components).
    /// </summary>
    public struct ConnectedComponentsAlgorithm
    {
        private readonly bool[] _marked;
        private readonly int[] _ids;
        private int _count;

        public ConnectedComponentsAlgorithm(IGraph graph) {
            if (graph == null) throw new ArgumentNullException(nameof(graph));

            _marked = new bool[graph.VerticeCount];
            _ids = new int[graph.VerticeCount];
            _count = 0;
            for (int s = 0; s < graph.VerticeCount; s++) {
                if (!_marked[s]) {
                    Process(graph, s);
                    _count++;
                }
            }
        }

        public bool Connected(int v, int w) {
            return IdOf(v) == IdOf(w);
        }

        public int Count { get { return _count; } }

        public int IdOf(int v) {
            return _ids[v];
        }

        public IEnumerable<int> Components(int id) {
            if (id < 0 || id >= _count) throw new ArgumentOutOfRangeException(nameof(id));
            for (int i = 0; i < _ids.Length; i++) {
                if (_ids[i] == id)
                    yield return i;
            }
        }

        private void Process(IGraph graph, int v) {
            _marked[v] = true;
            _ids[v] = _count;
            foreach (var w in graph.Adjacents(v))
                if (!_marked[w])
                    Process(graph, w);
        }
    }
}