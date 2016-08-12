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

namespace WmcSoft.Collections.Specialized
{
    public class Graph
    {
        private readonly ISet<int>[] _adj;
        private int _edges;

        public Graph(int vertices) {
            _adj = new ISet<int>[vertices];
            for (int i = 0; i < _adj.Length; i++) {
                _adj[i] = new BagSet<int>();
            }
        }

        public int Vertices { get { return _adj.Length; } }
        public int Edges { get { return _edges; } }

        public void Connect(int v, int w) {
            if (_adj[v].Add(w) & _adj[w].Add(v))
                ++_edges;
        }

        public void Disconnect(int v, int w) {
            if (_adj[v].Remove(w) & _adj[w].Remove(v))
                --_edges;
        }

        public IEnumerable<int> Adjacents(int v) {
            return _adj[v];
        }
    }
}
