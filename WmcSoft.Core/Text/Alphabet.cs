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

using static WmcSoft.BitArithmetics;

namespace WmcSoft.Text
{
    /// <summary>
    /// Represents an alphabet
    /// </summary>
    public class Alphabet
    {
        #region Known alphabets

        public static readonly Alphabet Binary = new Alphabet("01");
        public static readonly Alphabet Octal = new Alphabet("01234567");
        public static readonly Alphabet Decimal = new Alphabet("0123456789");
        public static readonly Alphabet HexaDecimal = new Alphabet("0123456789ABCDEF");
        public static readonly Alphabet DNA = new Alphabet("ACGT");
        public static readonly Alphabet LowerCase = new Alphabet("abcdefghijklmnopqrstuvwxyz");
        public static readonly Alphabet UpperCase = new Alphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        public static readonly Alphabet Protein = new Alphabet("ACDEFGHIKLMNPQRSTVWY");
        public static readonly Alphabet Base64 = new Alphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/");
        public static readonly Alphabet ASCII = new Alphabet(128);
        public static readonly Alphabet ExtendedASCII = new Alphabet(256);
        public static readonly Alphabet Unicode = new Alphabet(65536);

        #endregion

        private readonly char[] _alphabet;
        private readonly int[] _inverse;
        private readonly int _radix;

        static void ThrowOnDuplicate(IEnumerable<char> alphabet)
        {
            var unicode = new bool[char.MaxValue];
            foreach (var c in alphabet) {
                if (unicode[c])
                    throw new ArgumentException(nameof(alphabet));
                unicode[c] = true;
            }
        }

        public Alphabet(string alphabet)
        {
            if (alphabet == null) throw new ArgumentNullException(nameof(alphabet));
            ThrowOnDuplicate(alphabet);

            _alphabet = alphabet.ToCharArray();
            _radix = alphabet.Length;

            _inverse = new int[Char.MaxValue];
            for (int i = 0; i < _inverse.Length; i++)
                _inverse[i] = -1;

            for (int c = 0; c < _radix; c++)
                _inverse[alphabet[c]] = c;
        }

        private Alphabet(int radix)
        {
            _radix = radix;
            _alphabet = new char[radix];
            _inverse = new int[radix];
            for (int i = 0; i < _radix; ++i) {
                _alphabet[i] = (char)i;
                _inverse[i] = i;
            }
        }

        public Alphabet() : this(256)
        {
        }

        public bool Contains(char c)
        {
            return _inverse[c] != -1;
        }

        public int Radix { get { return _radix; } }

        public int LbRadix { get { return Lb(Radix); } }

        public int this[char c] {
            get {
                var index = _inverse[c];
                if (index == -1) throw new ArgumentException(nameof(c));
                return index;
            }
        }

        public IEnumerable<int> ToIndices(IEnumerable<char> sequence)
        {
            return sequence.Select(c => this[c]).ToArray();
        }

        public char this[int index] {
            get { return _alphabet[index]; }
        }

        public IEnumerable<char> ToChars(IEnumerable<int> sequence)
        {
            return sequence.Select(i => _alphabet[i]).ToArray();
        }
    }
}