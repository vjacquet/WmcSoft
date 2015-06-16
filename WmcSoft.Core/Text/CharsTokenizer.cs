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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Text
{
    struct CharsTokenizer : ITokenizer<string, string>
    {
        readonly char[] _separators;

        public CharsTokenizer(params char[] separators) {
            if (separators == null)
                throw new ArgumentNullException("separators");
            if (separators.Length == 0)
                throw new ArgumentOutOfRangeException("separators");

            _separators = separators;
        }

        public IEnumerable<string> Tokenize(string value) {
            var start = 0;
            var length = 0;
            var pos = value.IndexOfAny(_separators, start);
            while (pos != -1) {
                length = pos - start;
                if (length > 0)
                    yield return value.Substring(start, length);
                start += length + 1;
                pos = value.IndexOfAny(_separators, start);
            }
            length = value.Length - start;
            if (length > 0)
                yield return value.Substring(start, length);
        }
    }
}
