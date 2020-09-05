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

namespace WmcSoft.Text
{
    public sealed class Tokenizer : ITokenizer<string, string>
    {
        private readonly Predicate<char> _isSeparator;
        private readonly bool _keepEmptyToken;

        public Tokenizer(StringSplitOptions options, params char[] separators)
        {
            _keepEmptyToken = options == StringSplitOptions.None;
            if (separators == null || separators.Length == 0) {
                _isSeparator = c => c == ' ';
            } else if (separators.Length == 1) {
                var separator = separators[0];
                _isSeparator = c => c == separator;
            } else {
                Array.Sort(separators);
                _isSeparator = c => c.BinaryAny(separators);
            }
        }

        public Tokenizer(params char[] separators)
            : this(StringSplitOptions.RemoveEmptyEntries, separators)
        {
        }

        public Tokenizer(StringSplitOptions options, Predicate<char> isSeparator)
        {
            _keepEmptyToken = options == StringSplitOptions.None;
            _isSeparator = isSeparator;
        }

        public Tokenizer(Predicate<char> isSeparator)
            : this(StringSplitOptions.RemoveEmptyEntries, isSeparator)
        {
        }

        public IEnumerable<string> Tokenize(string value)
        {
            var start = 0;
            var length = 0;
            if (_keepEmptyToken) {
                foreach (var c in value) {
                    if (_isSeparator(c)) {
                        yield return value.Substring(start, length);
                        start += length + 1;
                        length = 0;
                    } else {
                        length++;
                    }
                }
                yield return value.Substring(start, length);
            } else {
                foreach (var c in value) {
                    if (_isSeparator(c)) {
                        if (length > 0)
                            yield return value.Substring(start, length);
                        start += length + 1;
                        length = 0;
                    } else {
                        length++;
                    }
                }
                if (length > 0)
                    yield return value.Substring(start, length);
            }
        }
    }
}
