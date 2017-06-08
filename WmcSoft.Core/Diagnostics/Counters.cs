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

using System.Collections.Generic;

namespace WmcSoft.Diagnostics
{
    public sealed class Counters<TName>
    {
        private readonly Dictionary<TName, int> _occurences;

        public Counters()
        {
            _occurences = new Dictionary<TName, int>();
        }

        public int Increment(TName name)
        {
            if (!_occurences.TryGetValue(name, out int count)) {
                count = 1;
                _occurences.Add(name, count);
            } else {
                count++;
                _occurences[name] = count;
            }
            return count;
        }

        public int Tally(TName name)
        {
            _occurences.TryGetValue(name, out int count);
            return count;
        }

        public int this[TName name] {
            get { return Tally(name); }
        }
    }
}