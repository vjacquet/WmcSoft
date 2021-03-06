﻿#region Licence

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

namespace WmcSoft.Collections.Generic.Forests
{
    /// <summary>
    /// Represents a tree.
    /// </summary>
    /// <typeparam name="T">The type of element in the nodes.</typeparam>
    public class Tree<T>
    {
        public Tree()
        {
        }

        public Tree(T value, params TreeNode<T>[] children)
        {
            Root = new TreeNode<T>(value, children);
        }

        /// <summary>
        /// The root of the tree.
        /// </summary>
        public TreeNode<T> Root { get; set; }

        public T Value {
            get {
                if (Root == null) throw new InvalidOperationException();
                return Root.Value;
            }
            set {
                if (Root != null)
                    Root.Value = value;
                else
                    Root = new TreeNode<T>(value);
            }
        }

        public void Clear()
        {
            Root = null;
        }

        public int Weight => Root?.Weight ?? 0;

        public int Height => Root?.Height ?? 0;
    }
}
