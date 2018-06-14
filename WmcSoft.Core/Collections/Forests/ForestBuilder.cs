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

using System;
using System.Collections.Generic;
using System.Linq;

namespace WmcSoft.Collections.Generic.Forests
{
    public abstract class ForestBuilder<T>
    {
        private readonly char _pathDelimiter;

        protected ForestBuilder(char pathDelimiter = '\\')
        {
            _pathDelimiter = pathDelimiter;
        }

        public abstract string GetPath(T item);

        protected virtual void OnMissingParent(T item) { }

        public bool IsRoot(T item)
        {
            var path = GetPath(item);
            return path.LastIndexOf(_pathDelimiter) == -1;
        }

        public string GetParentPath(T item)
        {
            var path = GetPath(item);
            var indexOfLastDelimiter = path.LastIndexOf('\\');
            if (indexOfLastDelimiter != -1)
                return path.Substring(0, indexOfLastDelimiter);
            return null;
        }

        public Forest<T> Build(IEnumerable<T> items)
        {
            var index = new Dictionary<string, TreeNode<T>>(StringComparer.OrdinalIgnoreCase);
            var forest = new Forest<T>();
            var sorted = items.OrderBy(x => GetPath(x)).ToList();

            foreach (var item in sorted)
            {
                if (IsRoot(item))
                    index.Add(GetPath(item), forest.Add(item));
                else if (index.TryGetValue(GetParentPath(item), out TreeNode<T> node))
                    index.Add(GetPath(item), node.Add(item));
                else
                    OnMissingParent(item);
            }

            return forest;
        }

        public Forest<T> Prune(Forest<T> forest, Func<T, bool> preserve)
        {
            var trees = forest.Trees;
            var result = new Forest<T>();
            var q = new Queue<Replicator>();
            foreach (var tree in trees)
            {
                if (preserve(tree.Value))
                {
                    CopyDescendants(tree, result.Add(tree.Value));
                }
                else if (tree.Enumerate(DepthFirst.PreOrder).Any(preserve))
                {
                    q.Enqueue(new Replicator { Source = tree, Target = result.Add(tree.Value) });
                    while (q.Count > 0)
                    {
                        var top = q.Dequeue();
                        foreach (var child in top.Source.Children)
                        {
                            if (preserve(child.Value))
                                CopyDescendants(child, top.Target.Add(child.Value));
                            else if (child.Enumerate(DepthFirst.PreOrder).Any(preserve))
                                q.Enqueue(new Replicator { Source = child, Target = top.Target.Add(child.Value) });
                        }
                    }
                }
            }
            return result;
        }

        void CopyDescendants(TreeNode<T> source, TreeNode<T> target)
        {
            Queue<Replicator> q = new Queue<Replicator>();
            q.Enqueue(new Replicator { Source = source, Target = target });
            while (q.Count > 0)
            {
                var top = q.Dequeue();
                foreach (var child in top.Source.Children)
                {
                    q.Enqueue(new Replicator { Source = child, Target = top.Target.Add(child.Value) });
                }
            }
        }

        struct Replicator
        {
            public TreeNode<T> Source;
            public TreeNode<T> Target;
        }
    }
}
