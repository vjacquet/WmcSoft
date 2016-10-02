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
using System.Diagnostics;
using static System.Math;

namespace WmcSoft.Collections.Generic
{
    [DebuggerDisplay("{Value,nq}")]
    public class TreeNode<T>
    {
        private TreeNode<T> _followingSibling;
        private TreeNode<T> _precedingSibling;

        public TreeNode(T value) {
            Value = value;
        }

        public TreeNode<T> Parent { get; private set; }
        public TreeNode<T> FollowingSibling {
            get {
                if (Parent == null || _followingSibling == Parent.FirstChild)
                    return null;
                return _followingSibling;
            }
            private set {
                _followingSibling = value;
            }
        }
        public TreeNode<T> PrecedingSibling {
            get {
                if (Parent == null || this == Parent.FirstChild)
                    return null;
                return _precedingSibling;
            }
            private set {
                _precedingSibling = value;
            }
        }
        public TreeNode<T> FirstChild { get; private set; }
        public TreeNode<T> LastChild {
            get {
                if (FirstChild == null)
                    return null;
                return FirstChild.PrecedingSibling;
            }
        }

        public IEnumerable<TreeNode<T>> Children {
            get {
                var p = FirstChild;
                while (p != null) {
                    yield return p;
                    p = p.FollowingSibling;
                }
            }
        }

        public T Value { get; set; }

        public bool IsLeaf { get { return FirstChild == null; } }

        public int Weight {
            get {
                var n = 0;
                var p = FirstChild;
                while (p != null) {
                    n += p.Weight;
                    p = p.FollowingSibling;
                }
                return n + 1;
            }
        }

        public int Height {
            get {
                var n = 0;
                var p = FirstChild;
                while (p != null) {
                    n = Max(n, p.Height);
                    p = p.FollowingSibling;
                }
                return n + 1;
            }
        }

        public TreeNode<T> InsertBefore(T value) {
            var node = new TreeNode<T>(value) {
                Parent = Parent,
                PrecedingSibling = _precedingSibling,
                FollowingSibling = this,
            };
            _precedingSibling._followingSibling = node;
            _precedingSibling = node;
            return node;
        }

        public TreeNode<T> InsertAfter(T value) {
            var node = new TreeNode<T>(value) {
                Parent = Parent,
                PrecedingSibling = this,
                FollowingSibling = _followingSibling,
            };
            _followingSibling._precedingSibling = node;
            _followingSibling = node;
            return node;
        }

        private TreeNode<T> AddEmpty(T value) {
            var node = new TreeNode<T>(value) {
                Parent = this,
            };
            node._precedingSibling = node;
            node._followingSibling = node;
            FirstChild = node;
            return node;
        }
        public TreeNode<T> Append(T value) {
            var head = FirstChild;
            if (head == null)
                return AddEmpty(value);
            return head.InsertBefore(value);
        }

        public TreeNode<T> Preprend(T value) {
            var head = FirstChild;
            if (head == null)
                return AddEmpty(value);
            return FirstChild = head.InsertBefore(value);
        }

        public TreeNode<T> Find(T value) {
            var comparer = EqualityComparer<T>.Default;
            var p = FirstChild;
            while (p != null) {
                if (comparer.Equals(p.Value, value))
                    return p;
                p = p.FollowingSibling;
            }
            return null;
        }

        public bool Remove(T value) {
            var found = Find(value);
            if (found == null)
                return false;
            found._followingSibling._precedingSibling = found._precedingSibling;
            found._precedingSibling._followingSibling = found._followingSibling;
             if(found == FirstChild) {
                FirstChild = found._followingSibling;
                if (found == FirstChild)
                    FirstChild = null;
            }
            found.Value = default(T);
           return true;
        }
    }
}
