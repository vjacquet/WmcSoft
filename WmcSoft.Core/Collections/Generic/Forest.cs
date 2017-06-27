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

using System.Collections;
using System.Collections.Generic;

namespace WmcSoft.Collections.Generic
{
    public class Forest<T> : ICollection<Tree<T>>
    {
        private readonly List<Tree<T>> _trees;

        public Forest(IEnumerable<Tree<T>> trees)
        {
            _trees = new List<Tree<T>>(trees);
        }

        public Forest()
        {
            _trees = new List<Tree<T>>();
        }

        public int Count { get { return _trees.Count; } }

        public bool IsReadOnly {
            get {
                return ((ICollection<Tree<T>>)_trees).IsReadOnly;
            }
        }

        public void Add(Tree<T> item)
        {
            _trees.Add(item);
        }

        public void Clear()
        {
            _trees.Clear();
        }

        public bool Contains(Tree<T> item)
        {
            return _trees.Contains(item);
        }

        public void CopyTo(Tree<T>[] array, int arrayIndex)
        {
            _trees.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Tree<T>> GetEnumerator()
        {
            return ((ICollection<Tree<T>>)_trees).GetEnumerator();
        }

        public bool Remove(Tree<T> item)
        {
            return _trees.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<Tree<T>>)_trees).GetEnumerator();
        }
    }
}