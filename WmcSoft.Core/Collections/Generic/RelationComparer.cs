#region Licence

/****************************************************************************
          Copyright 1999-2015 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Implements a <see cref="IComparer{T}"/> based on a relation.
    /// </summary>
    /// <typeparam name="T">The type of elements.</typeparam>
    public sealed class RelationComparer<T> : IComparer<T>
    {
        private readonly Relation<T> _relation;

        public RelationComparer(Relation<T> relation) {
            if (relation == null) throw new ArgumentNullException(nameof(relation));

            _relation = relation;
        }

        public RelationComparer(Func<T, T, bool> func) {
            if (func == null)
                throw new ArgumentNullException("func");

            _relation = (x, y) => func(x, y);
        }

        public int Compare(T x, T y) {
            if (x == null)
                return y != null ? 1 : 0;
            if (y == null)
                return -1;
            if (_relation(x, y))
                return -1;
            if (_relation(y, x))
                return 1;
            return 0;
        }
    }
}
