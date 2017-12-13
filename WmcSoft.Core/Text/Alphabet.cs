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
    /// Represents an alphabet.
    /// </summary>
    public class Alphabet
    {
        #region Known alphabets

        static Alphabet() { } // for lazyness.

        public static readonly Alphabet Binary = new Alphabet("01");
        public static readonly Alphabet Octal = new Alphabet("01234567");
        public static readonly Alphabet Decimal = new Alphabet("0123456789");
        public static readonly Alphabet HexaDecimal = new Alphabet("0123456789ABCDEF");
        public static readonly Alphabet DNA = new Alphabet("ACGT");
        public static readonly Alphabet LowerCase = new Alphabet("abcdefghijklmnopqrstuvwxyz");
        public static readonly Alphabet UpperCase = new Alphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        public static readonly Alphabet Protein = new Alphabet("ACDEFGHIKLMNPQRSTVWY");
        public static readonly Alphabet Base32 = new Alphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZ234567");
        public static readonly Alphabet ZBase32 = new Alphabet("ybndrfg8ejkmcpqxot1uwisza345h769");
        public static readonly Alphabet CrockfordBase32 = new Alphabet("0123456789ABCDEFGHJKMNPQRSTVWXYZ");
        public static readonly Alphabet Base64 = new Alphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/");
        public static readonly Alphabet Base64Url = new Alphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_");
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

        /// <summary>
        /// Creates a new alphabet with the given letters.
        /// </summary>
        /// <param name="letters">The string ot letters of the alphabet.</param>
        public Alphabet(string letters)
        {
            if (letters == null) throw new ArgumentNullException(nameof(letters));
            ThrowOnDuplicate(letters);

            _alphabet = letters.ToCharArray();
            _radix = letters.Length;

            _inverse = new int[char.MaxValue];
            for (int i = 0; i < _inverse.Length; i++)
                _inverse[i] = -1;

            for (int c = 0; c < _radix; c++)
                _inverse[letters[c]] = c;
        }

        /// <summary>
        /// Creates a new alphabet with the given number of chars.
        /// </summary>
        /// <param name="radix">The number of chars in the alphabet.</param>
        public Alphabet(int radix)
        {
            _radix = radix;
            _alphabet = new char[radix];
            _inverse = new int[radix];
            for (int i = 0; i < _radix; ++i) {
                _alphabet[i] = (char)i;
                _inverse[i] = i;
            }
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="c"/> is in the alphabet.
        /// </summary>
        /// <param name="c">The char.</param>
        /// <returns><c>true</c> if <paramref name="c"/> is in the alphabet; otherwise, <c>false</c>.</returns>
        public bool Contains(char c)
        {
            return _inverse[c] != -1;
        }

        /// <summary>
        /// The number of characters in the alphabet.
        /// </summary>
        public int Radix => _radix;

        /// <summary>
        /// The number of bits needed to represent and index.
        /// </summary>
        public int LbRadix => Lb(Radix);

        /// <summary>
        /// Converts c to an index between 0 and <see cref="Radix"/> - 1.
        /// </summary>
        /// <param name="c">The char.</param>
        /// <returns>The index.</returns>
        public int this[char c] {
            get {
                return ToIndex(c);
            }
        }

        /// <summary>
        /// Converts c to an index between 0 and <see cref="Radix"/> - 1.
        /// </summary>
        /// <param name="c">The char.</param>
        /// <returns>The index.</returns>
        public int ToIndex(char c)
        {
            var index = _inverse[c];
            if (index == -1) throw new ArgumentException(nameof(c));
            return index;
        }

        /// <summary>
        /// Converts an index to the corresponding char in the alphabet.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The char.</returns>
        public char this[int index] {
            get { return ToChar(index); }
        }

        /// <summary>
        /// Converts an index to the corresponding char in the alphabet.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The char.</returns>
        public char ToChar(int index)
        {
            return _alphabet[index];
        }

        /// <summary>
        /// Gets the indices corresponding to the requested chars.
        /// </summary>
        /// <param name="sequence">The sequence of chars to get the indices of.</param>
        /// <returns>The sequence of indices.</returns>
        public IEnumerable<int> ToIndices(IEnumerable<char> sequence)
        {
            return sequence.Select(ToIndex).ToList();
        }

        /// <summary>
        /// Gets the chars at the given indices.
        /// </summary>
        /// <param name="sequence">The sequence of indices.</param>
        /// <returns>The sequence of chars.</returns>
        public IEnumerable<char> ToChars(IEnumerable<int> sequence)
        {
            return sequence.Select(ToChar).ToList();
        }
    }
}
