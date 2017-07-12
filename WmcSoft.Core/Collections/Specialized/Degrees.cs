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
    public class Degrees
    {
        private readonly int[] _indegrees;
        private readonly int[] _outdegrees;

        protected int VerticeCount { get { return _indegrees.Length; } }

        public Degrees(IDirectedGraph graph) {
            var length = graph.VerticeCount;
            _indegrees = new int[length];
            _outdegrees = new int[length];

            var contains = new bool[length];
            for (int v = 0; v < length; v++) {
                var bag = graph.Adjacents(v);

                _outdegrees[v] = bag.Count;

                foreach (var w in bag) {
                    contains[w] = true;
                }
                for (int w = 0; w < length; w++) {
                    if (contains[w]) {
                        _indegrees[w]++;
                        contains[w] = false;
                    }
                }
            }
        }

        public int Indegree(int v) {
            return _indegrees[v];
        }

        public int Outdegree(int v) {
            return _outdegrees[v];
        }

        public IEnumerable<int> Sources() {
            var length = VerticeCount;
            for (int v = 0; v < length; v++) {
                if (Indegree(v) == 0)
                    yield return v;
            }
        }

        public IEnumerable<int> Sinks() {
            var length = VerticeCount;
            for (int v = 0; v < length; v++) {
                if (Outdegree(v) == 0)
                    yield return v;
            }
        }

        public bool IsMap() {
            var length = VerticeCount;
            for (int v = 0; v < length; v++) {
                if (Outdegree(v) != 1)
                    return false;
            }
            return true;
        }
    }
}
