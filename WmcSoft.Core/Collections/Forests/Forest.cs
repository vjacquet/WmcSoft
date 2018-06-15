#region Licence

/****************************************************************************
          Copyright 1999-2018 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft.Collections.Generic.Forests
{
    public class Forest<T>
    {
        private readonly List<TreeNode<T>> _trees = new List<TreeNode<T>>();

        public IReadOnlyCollection<TreeNode<T>> Trees => _trees;

        public TreeNode<T> Add(T value)
        {
            var tree = new TreeNode<T>(value);
            _trees.Add(tree);
            return tree;
        }

        public TreeNode<T> Add(TreeNode<T> tree)
        {
            _trees.Add(tree);
            return tree;
        }

        public bool Remove(T value)
        {
            var found = _trees.FindIndex(t => EqualityComparer<T>.Default.Equals(value, t.Value));
            if (found >= 0)
            {
                _trees.RemoveAt(found);
                return true;
            }
            return false;
        }

        public bool Remove(TreeNode<T> tree)
        {
            return _trees.Remove(tree);
        }

        public TreeNode<T> Find(T value)
        {
            var found = _trees.FindIndex(t => EqualityComparer<T>.Default.Equals(value, t.Value));
            if (found >= 0)
                return _trees[found];
            return null;
        }
    }
}
