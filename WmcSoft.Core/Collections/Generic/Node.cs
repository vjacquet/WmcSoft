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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Collections.Generic
{
    internal abstract class Node<T>
    {
        public Node(T value = default(T)) {
            Value = value;
        }

        public T Value { get; set; }

        protected abstract IEnumerable<Node<T>> Neighbors();
        protected virtual Node<T> FindNeighborByValue(T value) {
            return Neighbors().FirstOrDefault(n => value.Equals(n.Value));
        }
    }

    internal class BinaryTreeNode<T> : Node<T>
    {
        private BinaryTreeNode<T> _left;
        private BinaryTreeNode<T> _right;

        public BinaryTreeNode() : base() {
        }
        public BinaryTreeNode(T value) : base(value) {
        }
        public BinaryTreeNode(T value, BinaryTreeNode<T> left, BinaryTreeNode<T> right)
            : base(value) {
            _left = left;
            _right = right;
        }

        public BinaryTreeNode<T> Left {
            get { return _left; }
            set { _left = value; }
        }
        public BinaryTreeNode<T> Right {
            get { return _right; }
            set { _right = value; }
        }

        protected override IEnumerable<Node<T>> Neighbors() {
            if (_left != null)
                yield return _left;
            if (_right != null)
                yield return _right;
        }
    }

    internal class BinaryTree<T>
    {
        public BinaryTree() {
        }

        public BinaryTreeNode<T> Root {
            get; set;
        }

        public virtual void Clear() {
            Root = null;
        }
    }

    internal class BinarySearchTree<T> : ICollection<T>
    {
        private readonly IComparer<T> _comparer;
        private BinaryTreeNode<T> _root;
        private int _count;

        public BinarySearchTree(IComparer<T> comparer = null) {
            _comparer = comparer ?? Comparer<T>.Default;
        }

        public int Count {
            get { return _count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        /// <summary>
        ///  Removes all items from the binary search tree.
        /// </summary>
        public void Clear() {
            _root = null;
        }

        /// <summary>
        /// Searches the tree for a node that contains a specific value.
        /// </summary>
        /// <param name="item" The object to locate in the tree.</param>
        /// <returns>true if item is found in the binary search tree; otherwise, false.</returns>
        public bool Contains(T item) {
            // 
            var current = _root;
            int result;
            while (current != null) {
                result = _comparer.Compare(current.Value, item);
                if (result == 0)
                    return true; // we found data
                else if (result > 0)
                    current = current.Left; // current.Value > data, search current's left subtree
                else if (result < 0)
                    current = current.Right;   // current.Value < data, search current's right subtree
            }
            return false; // didn't find data
        }

        public void Add(T data) {
            var n = new BinaryTreeNode<T>(data);
            int result;

            // now, insert n into the tree
            // trace down the tree until we hit a NULL
            BinaryTreeNode<T> current = _root, parent = null;
            while (current != null) {
                result = _comparer.Compare(current.Value, data);
                if (result == 0) {
                    // they are equal - attempting to enter a duplicate - do nothing
                    return;
                } else if (result > 0) {
                    // current.Value > data, must add n to current's left subtree
                    parent = current;
                    current = current.Left;
                } else if (result < 0) {
                    // current.Value < data, must add n to current's right subtree
                    parent = current;
                    current = current.Right;
                }
            }

            // We're ready to add the node!
            _count++;
            if (parent == null) {
                // the tree was empty, make n the root
                _root = n;
            } else {
                result = _comparer.Compare(parent.Value, data);
                if (result > 0)
                    // parent.Value > data, therefore n must be added to the left subtree
                    parent.Left = n;
                else
                    // parent.Value < data, therefore n must be added to the right subtree
                    parent.Right = n;
            }
        }

        public bool Remove(T item) {
            if (_root == null)
                return false; // no items to remove

            // Now, try to find data in the tree
            BinaryTreeNode<T> current = _root, parent = null;
            int result = _comparer.Compare(current.Value, item);
            while (result != 0) {
                if (result > 0) {
                    // current.Value > data, if data exists it's in the left subtree
                    parent = current;
                    current = current.Left;
                } else if (result < 0) {
                    // current.Value < data, if data exists it's in the right subtree
                    parent = current;
                    current = current.Right;
                }

                // If current == null, then we didn't find the item to remove
                if (current == null)
                    return false;

                result = _comparer.Compare(current.Value, item);
            }

            // At this point, we've found the node to remove
            _count--;

            // We now need to "rethread" the tree
            // CASE 1: If current has no right child, then current's left child becomes
            //         the node pointed to by the parent
            if (current.Right == null) {
                if (parent == null)
                    _root = current.Left;
                else {
                    result = _comparer.Compare(parent.Value, current.Value);
                    if (result > 0)
                        // parent.Value > current.Value, so make current's left child a left child of parent
                        parent.Left = current.Left;
                    else if (result < 0)
                        // parent.Value < current.Value, so make current's left child a right child of parent
                        parent.Right = current.Left;
                }
            }
            // CASE 2: If current's right child has no left child, then current's right child
            //         replaces current in the tree
            else if (current.Right.Left == null) {
                current.Right.Left = current.Left;

                if (parent == null)
                    _root = current.Right;
                else {
                    result = _comparer.Compare(parent.Value, current.Value);
                    if (result > 0)
                        // parent.Value > current.Value, so make current's right child a left child of parent
                        parent.Left = current.Right;
                    else if (result < 0)
                        // parent.Value < current.Value, so make current's right child a right child of parent
                        parent.Right = current.Right;
                }
            }
            // CASE 3: If current's right child has a left child, replace current with current's
            //          right child's left-most descendent
            else {
                // We first need to find the right node's left-most child
                BinaryTreeNode<T> leftmost = current.Right.Left, lmParent = current.Right;
                while (leftmost.Left != null) {
                    lmParent = leftmost;
                    leftmost = leftmost.Left;
                }

                // the parent's left subtree becomes the leftmost's right subtree
                lmParent.Left = leftmost.Right;

                // assign leftmost's left and right to current's left and right children
                leftmost.Left = current.Left;
                leftmost.Right = current.Right;

                if (parent == null)
                    _root = leftmost;
                else {
                    result = _comparer.Compare(parent.Value, current.Value);
                    if (result > 0)
                        // parent.Value > current.Value, so make leftmost a left child of parent
                        parent.Left = leftmost;
                    else if (result < 0)
                        // parent.Value < current.Value, so make leftmost a right child of parent
                        parent.Right = leftmost;
                }
            }

            return true;
        }

        IEnumerable<T> PreorderTraversal(BinaryTreeNode<T> root) {
            // A single stack is sufficient here - it simply maintains the correct
            // order with which to process the children.
            Stack<BinaryTreeNode<T>> toVisit = new Stack<BinaryTreeNode<T>>(Count);
            BinaryTreeNode<T> current = root;
            if (current != null) toVisit.Push(current);

            while (toVisit.Count != 0) {
                // take the top item from the stack
                current = toVisit.Pop();

                // add the right and left children, if not null
                if (current.Right != null) toVisit.Push(current.Right);
                if (current.Left != null) toVisit.Push(current.Left);

                // return the current node
                yield return current.Value;
            }
        }

        IEnumerable<T> InorderTraversal(BinaryTreeNode<T> root) {
            Stack<BinaryTreeNode<T>> toVisit = new Stack<BinaryTreeNode<T>>(Count);
            for (BinaryTreeNode<T> current = root; current != null || toVisit.Count != 0; current = current.Right) {
                // Get the left-most item in the subtree, remembering the path taken
                while (current != null) {
                    toVisit.Push(current);
                    current = current.Left;
                }

                current = toVisit.Pop();
                yield return current.Value;
            }
        }

        IEnumerable<T> PostorderTraversal(BinaryTreeNode<T> root) {
            Stack<BinaryTreeNode<T>> toVisit = new Stack<BinaryTreeNode<T>>(Count);
            Stack<bool> hasBeenProcessed = new Stack<bool>(Count);
            BinaryTreeNode<T> current = root;
            if (current != null) {
                toVisit.Push(current);
                hasBeenProcessed.Push(false);
                current = current.Left;
            }

            while (toVisit.Count != 0) {
                if (current != null) {
                    // add this node to the stack with a false processed value
                    toVisit.Push(current);
                    hasBeenProcessed.Push(false);
                    current = current.Left;
                } else {
                    // see if the node on the stack has been processed
                    bool processed = hasBeenProcessed.Pop();
                    BinaryTreeNode<T> node = toVisit.Pop();
                    if (!processed) {
                        // if it's not been processed, "recurse" down the right subtree
                        toVisit.Push(node);
                        hasBeenProcessed.Push(true);    // it's now been processed
                        current = node.Right;
                    } else {
                        yield return node.Value;
                    }
                }
            }
        }

        public IEnumerator<T> GetEnumerator() {
            return GetEnumerator(TraversalMethods.PostOrder);
        }

        public IEnumerator<T> GetEnumerator(TraversalMethods method) {
            Func<BinaryTreeNode<T>, IEnumerable<T>> traversal;
            switch (method) {
            case TraversalMethods.Preorder:
                traversal = PreorderTraversal;
                break;
            case TraversalMethods.Inorder:
                traversal = InorderTraversal;
                break;
            case TraversalMethods.PostOrder:
            default:
                traversal = PostorderTraversal;
                break;
            }
            return traversal(_root).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void CopyTo(T[] array, int arrayIndex) {
            CopyTo(array, arrayIndex, TraversalMethods.PostOrder);
        }

        public void CopyTo(T[] array, int arrayIndex, TraversalMethods traversal) {
            using (var enumerator = GetEnumerator(traversal)) {
                while (enumerator.MoveNext()) {
                    array[arrayIndex++] = enumerator.Current;
                }
            }
        }

    }

    internal enum TraversalMethods
    {
        Preorder,
        Inorder,
        PostOrder,
    }

    //internal class NodeList<T> : ICollection<Node<T>>
    //{

    //}
}
