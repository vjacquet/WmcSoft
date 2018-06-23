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
using System.ComponentModel;
using System.Linq;

namespace WmcSoft.Collections.Generic.Forests
{
    public static class DepthFirst
    {
        #region Algorithms

        [EditorBrowsable(EditorBrowsableState.Never)]
        public struct PreOrderAlgorithm
        {
            public IEnumerable<TreeNode<T>> Traverse<T>(TreeNode<T> node)
            {
                if (node != null) {
                    var stack = new Stack<TreeNode<T>>();
                    stack.Push(node);
                    while (stack.Count > 0) {
                        var top = stack.Pop();
                        yield return top;

                        var length = top.Count;
                        for (int i = length - 1; i >= 0; i--) {
                            stack.Push(top._children[i]);
                        }
                    }
                }
            }
        };

        [EditorBrowsable(EditorBrowsableState.Never)]
        public struct PostOrderAlgorithm
        {
            public IEnumerable<TreeNode<T>> Traverse<T>(TreeNode<T> node)
            {
                if (node != null) {
                    var stack = new Stack<TreeNode<T>>();
                    var result = new Stack<TreeNode<T>>();
                    stack.Push(node);

                    while (stack.Count > 0) {
                        var top = stack.Pop();
                        result.Push(top);

                        var length = top.Count;
                        for (int i = 0; i < length; i++) {
                            stack.Push(top._children[i]);
                        }
                    }

                    while (result.Count > 0) {
                        yield return result.Pop();
                    }
                }
            }
        };

        #endregion

        public static PreOrderAlgorithm PreOrder;

        public static PostOrderAlgorithm PostOrder;
    }

    public static class DepthFirstExtensions
    {
        public static IEnumerable<T> Enumerate<T>(this Forest<T> forest, DepthFirst.PreOrderAlgorithm traversal)
        {
            return forest.Trees.SelectMany(t => traversal.Traverse(t)).Select(n => n.Value);
        }

        public static IEnumerable<T> Enumerate<T>(this Forest<T> forest, DepthFirst.PostOrderAlgorithm traversal)
        {
            return forest.Trees.SelectMany(t => traversal.Traverse(t)).Select(n => n.Value);
        }

        public static IEnumerable<T> Enumerate<T>(this Tree<T> tree, DepthFirst.PreOrderAlgorithm traversal)
        {
            return traversal.Traverse(tree.Root).Select(n => n.Value);
        }

        public static IEnumerable<T> Enumerate<T>(this Tree<T> tree, DepthFirst.PostOrderAlgorithm traversal)
        {
            return tree.Root != null ? traversal.Traverse(tree.Root).Select(n => n.Value) : Enumerable.Empty<T>();
        }

        public static IEnumerable<T> Enumerate<T>(this TreeNode<T> node, DepthFirst.PreOrderAlgorithm traversal)
        {
            if (node == null) node.GetHashCode(); // throws NullReferenceException
            return traversal.Traverse(node).Select(n => n.Value);
        }

        public static IEnumerable<T> Enumerate<T>(this TreeNode<T> node, DepthFirst.PostOrderAlgorithm traversal)
        {
            if (node == null) node.GetHashCode(); // throws NullReferenceException
            return traversal.Traverse(node).Select(n => n.Value);
        }
    }
}
