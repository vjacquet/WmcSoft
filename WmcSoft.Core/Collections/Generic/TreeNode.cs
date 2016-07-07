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

using static System.Math;

namespace WmcSoft.Collections.Generic
{
    public class TreeNode<T>
    {
        public TreeNode<T> Parent { get; private set; }
        public TreeNode<T> FirstChild { get; private set; }
        public TreeNode<T> FollowingSibling { get; private set; }
        public TreeNode<T> PrecedingSibling { get; private set; }

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
    }
}
