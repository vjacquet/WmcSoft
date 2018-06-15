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
using System.Diagnostics;
using System.Linq;

using static System.Math;

namespace WmcSoft.Collections.Generic.Forests
{
    [DebuggerDisplay("{Value,nq}")]
    public class TreeNode<T>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly List<TreeNode<T>> _children = new List<TreeNode<T>>();

        public TreeNode(T value, params TreeNode<T>[] children)
        {
            Value = value;
            _children.AddRange(children);
            foreach (var child in children)
                child.Parent = this;
        }

        public T Value { get; set; }

        public bool IsLeaf => _children.Count == 0;

        public int Depth => (Parent?.Depth ?? 0) + 1;

        public int Weight => this.Enumerate(DepthFirst.PreOrder).Count();

        public int Height => _children.Aggregate(0, (a, x) => Max(a, x.Height)) + 1;

        public TreeNode<T> Parent { get; private set; }

        public TreeNode<T> FirstChild => _children.FirstOrDefault();

        public TreeNode<T> LastChild => _children.LastOrDefault();

        public IReadOnlyCollection<TreeNode<T>> Children => _children.AsReadOnly();

        public TreeNode<T> Add(T value)
        {
            return Add(new TreeNode<T>(value));
        }

        public TreeNode<T> Add(TreeNode<T> tree)
        {
            _children.Add(tree);
            tree.Parent = this;
            return tree;
        }

        public bool Remove(T value)
        {
            var found = _children.FindIndex(t => EqualityComparer<T>.Default.Equals(value, t.Value));
            if (found >= 0) {
                var node = _children[found];
                _children.RemoveAt(found);
                node.Parent = null;
                return true;
            }
            return false;
        }

        public bool Remove(TreeNode<T> tree)
        {
            if (_children.Remove(tree)) {
                tree.Parent = null;
                return true;
            }
            return false;
        }

        public TreeNode<T> Find(T value)
        {
            var found = _children.FindIndex(t => EqualityComparer<T>.Default.Equals(value, t.Value));
            if (found >= 0)
                return _children[found];
            return null;
        }
        public int Count => _children.Count;
    }
}
