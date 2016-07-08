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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WmcSoft.Collections.Generic
{
    public class Tree<T> : IEnumerable<T>
    {
        public Tree(T value) {
            Root = new TreeNode<T> { Value = value };
        }

        public TreeNode<T> Root { get; private set; }

        public int Weight {
            get {
                if (Root == null)
                    return 0;
                return Root.Weight;
            }
        }

        public int Height {
            get {
                if (Root == null)
                    return 0;
                return Root.Height;
            }
        }

        public IEnumerator<T> GetEnumerator() {
            if (Root == null)
                return Enumerable.Empty<T>().GetEnumerator();
            return ((IEnumerable<T>)Root).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
