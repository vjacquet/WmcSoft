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
using System.Linq;
using System.Text;
using WmcSoft.Collections.Generic;

namespace WmcSoft.Text
{
    /// <summary>
    /// Represents a naive lorem ipsum generator that takes a list of words 
    /// and choose randomly from them to create sentences 
    /// and paragraphs of random length.
    /// </summary>
    /// <remarks>A better implementation would use templates for sentences.</remarks>
    public class LoremIpsumGenerator
    {
        private readonly string[] _words;
        private readonly Random _random;

        public LoremIpsumGenerator(Random random, params string[] words)
        {
            _words = words;
            _random = random;
        }

        public IEnumerable<string> Words(int count)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

            _words.PartialShuffle(count, _random);
            return _words.Take(count);
        }

        static string Pop<TEnumerator>(TEnumerator enumerator) where TEnumerator : IEnumerator<string>
        {
            enumerator.MoveNext();
            return enumerator.Current;
        }
        static string Capitalize(string sentence)
        {
            return Char.ToUpper(sentence[0]) + sentence.Substring(1);
        }

        StringBuilder AppendSentence(StringBuilder sb, int minWordCount, int maxWordCount)
        {
            int count = _random.Next(minWordCount, maxWordCount);
            using (var enumerator = Words(count).GetEnumerator()) {
                var first = Capitalize(Pop(enumerator));
                sb.Append(first);
                while (enumerator.MoveNext()) {
                    sb.Append(' ');
                    sb.Append(enumerator.Current);
                }
                sb.Append('.');
            }
            return sb;
        }

        public string Sentence(int minwWordCount = 4)
        {
            if (minwWordCount <= 3) throw new ArgumentOutOfRangeException(nameof(minwWordCount));

            var sb = new StringBuilder();
            AppendSentence(sb, minwWordCount, minwWordCount + 5);
            return sb.ToString();
        }

        StringBuilder AppendSentences(StringBuilder sb, int minWordCount, int maxWordCount, int count)
        {
            AppendSentence(sb, minWordCount, maxWordCount);
            for (int i = 1; i < count; i++) {
                sb.Append(' ');
                AppendSentence(sb, minWordCount, maxWordCount);
            }
            return sb;
        }

        public IEnumerable<string> Sentences(int count)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

            for (int i = 0; i < count; i++) {
                yield return Sentence();
            }
        }

        public string Paragraph(int minSentenceCount = 3)
        {
            if (minSentenceCount <= 3) throw new ArgumentOutOfRangeException(nameof(minSentenceCount));

            var maxSentencecount = minSentenceCount + 3;
            int count = _random.Next(minSentenceCount, maxSentencecount);
            var sb = new StringBuilder();
            AppendSentences(sb, 4, 9, count);
            return sb.ToString();
        }

        public IEnumerable<string> Paragraphs(int count)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

            for (int i = 0; i < count; i++) {
                yield return Sentence();
            }
        }
    }
}
