﻿#region Licence

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
using static System.Math;

namespace WmcSoft.Collections.Generic
{
    public class TreeNode<T>
    {
        public TreeNode(T value) {
            Value = value;
        }

        public TreeNode<T> Parent { get; private set; }
        public TreeNode<T> FollowingSibling { get; private set; }
        public TreeNode<T> PrecedingSibling { get; private set; }
        public TreeNode<T> FirstChild { get; private set; }
        public TreeNode<T> LastChild {
            get {
                if (FirstChild == null)
                    return null;
                return FirstChild.PrecedingSibling;
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
                PrecedingSibling = PrecedingSibling,
                FollowingSibling = this,
            };
            PrecedingSibling = node;
            return node;
        }

        public TreeNode<T> InsertAfter(T value) {
            var node = new TreeNode<T>(value) {
                Parent = Parent,
                PrecedingSibling = this,
                FollowingSibling = FollowingSibling,
            };
            FollowingSibling = node;
            return node;
        }

        private TreeNode<T> AddEmpty(T value) {
            var node = new TreeNode<T>(value) {
                Parent = this,
            };
            node.PrecedingSibling = node;
            node.FollowingSibling = node;
            FirstChild = node;
            return node;
        }
        public TreeNode<T> Append(T value) {
            var head = FirstChild;
            if (head == null)
                return AddEmpty(value);
            var tail = LastChild;
            var node = new TreeNode<T>(value) {
                Parent = this,
                PrecedingSibling = tail,
                FollowingSibling = tail.FollowingSibling,
            };
            tail.FollowingSibling = node;
            return node;
        }

        public TreeNode<T> Preprend(T value) {
            var head = FirstChild;
            if (head == null)
                return AddEmpty(value);
            var node = new TreeNode<T>(value) {
                Parent = this,
                PrecedingSibling = head.PrecedingSibling,
                FollowingSibling = head,
            };
            head.PrecedingSibling = node;
            FirstChild = node;
            return node;
        }

        public bool Remove(T value) {
            if (FirstChild == null)
                return false;
            throw new NotImplementedException();
        }
    }
}
