#region Licence

/****************************************************************************
          Copyright 1999-2017 Vincent J. Jacquet.  All rights reserved.

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
using System.Linq;

namespace WmcSoft.IO
{
    public class QuotedString : IQuotedString
    {
        private readonly string _storage;

        public QuotedString(string s)
        {
            _storage = s ?? "";
        }

        public string ToQuotedString()
        {
            return _storage;
        }

        public override string ToString()
        {
            return _storage;
        }

        public static IQuotedString Quote(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new QuotedString("\"\"");

            if (UnguardedIsQuoted(text)) {
                if (text.Count(c => c == '\"') % 2 == 1) throw new ArgumentException("Unbalanced quotes", nameof(text));
                return new QuotedString(text);
            }

            return new QuotedString(UnguardedQuote(text));
        }

        #region Unguarded helpers

        internal static string UnguardedQuote(string text)
        {
            return '\"' + text.Replace("\"", "\"\"") + '\"';
        }

        internal static bool UnguardedIsQuoted(string text)
        {
            return text.StartsWith('\"') && text.EndsWith('\"');
        }

        #endregion
    }
}
