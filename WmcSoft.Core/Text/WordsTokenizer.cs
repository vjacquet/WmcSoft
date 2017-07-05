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

using System.Collections.Generic;

namespace WmcSoft.Text
{
    struct WordsTokenizer : ITokenizer<string, string>
    {
        public IEnumerable<string> Tokenize(string value)
        {
            var state = AsWordsState.Start;
            var first = 0;
            for (var i = 0; i < value.Length; i++) {
                var c = value[i];
                if (IsWordDelimiter(c)) {
                    if (state != AsWordsState.Start) {
                        yield return value.Substring(first, i - first);
                        do { i++; }
                        while (i < value.Length && IsWordDelimiter(value[i]));
                        state = AsWordsState.NewWord;
                    }
                    first = i;
                } else if (char.IsUpper(c)) {
                    switch (state) {
                    case AsWordsState.Upper:
                    case AsWordsState.ConsecutiveUpper:
                        state = AsWordsState.ConsecutiveUpper;
                        break;
                    case AsWordsState.Lower:
                    case AsWordsState.NewWord:
                        yield return value.Substring(first, i - first);
                        first = i;
                        goto default;
                    default:
                        state = AsWordsState.Upper;
                        break;
                    }

                } else {
                    switch (state) {
                    case AsWordsState.ConsecutiveUpper:
                        yield return value.Substring(first, i - first - 1);
                        first = i - 1;
                        goto default;
                    default:
                        state = AsWordsState.Lower;
                        break;
                    }
                }
            }
            if (first < value.Length)
                yield return value.Substring(first, value.Length - first);
        }

        enum AsWordsState
        {
            Start,
            Lower,
            Upper,
            ConsecutiveUpper,
            NewWord,
        }

        static bool IsWordDelimiter(char c)
        {
            return char.IsWhiteSpace(c) || c == '_' || c == '-';
        }
    }
}
