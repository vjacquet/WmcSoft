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

namespace WmcSoft
{
    /// <remarks>See Knuth's TAoCP, Vol 3, Page 47.</remarks>
    public class Tableau
    {
        private List<List<int>> _data;

        public Tableau() {
            _data = new List<List<int>>();
        }

        public void Add(int x) {
            var i = 0;
            var p = x;
            var j = _data.Count;
        }

        public int this[int i, int j]
        {
            get { return _data[j][i]; }
        }
    }
}
