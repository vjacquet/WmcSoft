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
        public struct PreOrderAlgorithm : ITraversalAlgorithm
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
        public struct PostOrderAlgorithm : ITraversalAlgorithm
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

    public interface ITraversalAlgorithm
    {
        IEnumerable<TreeNode<T>> Traverse<T>(TreeNode<T> node);
    }

    public static class DepthFirstExtensions
    {
        #region Traverse

        public static IEnumerable<TreeNode<T>> Traverse<T, TTraversal>(this Forest<T> forest, TTraversal traversal)
            where TTraversal : struct, ITraversalAlgorithm
        {
            return forest.Trees.SelectMany(t => traversal.Traverse(t));
        }

        public static IEnumerable<TreeNode<T>> Traverse<T, TTraversal>(this Tree<T> tree, TTraversal traversal)
            where TTraversal : struct, ITraversalAlgorithm
        {
            return traversal.Traverse(tree.Root);
        }

        public static IEnumerable<TreeNode<T>> Traverse<T, TTraversal>(this TreeNode<T> node, TTraversal traversal)
            where TTraversal : struct, ITraversalAlgorithm
        {
            if (node == null) node.GetHashCode(); // throws NullReferenceException
            return traversal.Traverse(node);
        }

        #endregion

        #region Enumerate

        public static IEnumerable<T> Enumerate<T, TTraversal>(this Forest<T> forest, TTraversal traversal)
            where TTraversal : struct, ITraversalAlgorithm
        {
            return Traverse(forest, traversal).Select(n => n.Value);
        }

        public static IEnumerable<T> Enumerate<T, TTraversal>(this Tree<T> tree, TTraversal traversal)
            where TTraversal : struct, ITraversalAlgorithm
        {
            return tree.Root != null ? Traverse(tree.Root, traversal).Select(n => n.Value) : Enumerable.Empty<T>();
        }

        public static IEnumerable<T> Enumerate<T, TTraversal>(this TreeNode<T> node, TTraversal traversal)
            where TTraversal : struct, ITraversalAlgorithm
        {
            if (node == null) node.GetHashCode(); // throws NullReferenceException
            return Traverse(node, traversal).Select(n => n.Value);
        }

        #endregion
    }
}
