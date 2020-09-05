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

namespace WmcSoft.Text
{
    /// <summary>
    /// Utility class implementing the alternate base32 encoding as described
    /// by Douglas Crockford at: <https://www.crockford.com/base32.html>.
    ///
    /// He designed the encoding to be:
    ///   * Be human and machine readable
    ///   * Be compact
    ///   * Be error resistant
    ///   * Be pronounceable
    /// It uses a symbol set of 10 digits and 22 letters, excluding I, L O and
    /// U.Decoding is not case sensitive, and 'i' and 'l' are converted to '1'
    /// and 'o' is converted to '0'. Encoding uses only upper-case characters.
    /// 
    /// Hyphens may be present in symbol strings to improve readability, and
    /// are removed when decoding.
    /// 
    /// A check symbol can be appended to a symbol string to detect errors
    /// within the string.
    public static class Crockford32
    {
        const string Symbols = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";
        const string ChecksumSymbols = Symbols + "*~$=U";
        const int Base = 32; // Symbols.Length;

        private static readonly char[] Encoder = ChecksumSymbols.ToCharArray();
        private static readonly Dictionary<char, byte> Decoder = CreateDecoder(Symbols);
        private static readonly Dictionary<char, byte> ChecksumDecoder = CreateDecoder(ChecksumSymbols);

        static Dictionary<char, byte> CreateDecoder(string alphabet)
        {
            var result = new Dictionary<char, byte>();

            for (int i = 0; i < alphabet.Length; i++)
                result.Add(Encoder[i], unchecked((byte)i));
            for (int i = 10; i < Symbols.Length; i++)
                result.Add(char.ToLowerInvariant(Encoder[i]), unchecked((byte)i));
            foreach (var c in "Oo")
                result.Add(c, 0);
            foreach (var c in "IiLl")
                result.Add(c, 1);
            result['u'] = 36;

            return result;
        }

        public static string Encode(int value, bool checksum = false, int group = 0)
        {
            var processor = Encode(Decompose(value).Reverse(), checksum);
            if (group > 0)
                processor = Group(processor, group, '-');
            var chars = processor.ToArray();
            return new string(chars);
        }

        public static string Encode(long value, bool checksum = false, int group = 0)
        {
            var processor = Encode(Decompose(value), checksum);
            if (group > 0)
                processor = Group(processor, group, '-');
            var chars = processor.ToArray();
            return new string(chars);
        }

        public static string Encode(byte[] value, int offset, int length, bool checksum = false, int group = 0)
        {
            var processor = Encode(Decompose(value.Skip(offset).Take(length)), checksum);
            if (group > 0)
                processor = Group(processor, group, '-');
            var chars = processor.ToArray();
            return new string(chars);
        }

        static IEnumerable<char> Group(IEnumerable<char> chars, int groupSize, char groupSeparator)
        {
            var count = groupSize;
            foreach (var c in chars) {
                if (count-- == 0) {
                    count = groupSize;
                    yield return groupSeparator;
                }
                yield return c;
            }
        }

        static IEnumerable<char> Encode(IEnumerable<byte> bytes, bool checksum)
        {
            var carry = 0;

            foreach (var b in bytes) {
                yield return Encoder[b];
                carry = (carry * 32 + b) % 37;
            }

            if (checksum) {
                yield return Encoder[carry];
            }
        }

        internal static IEnumerable<byte> Decompose(long value)
        {
            do {
                yield return unchecked((byte)(value % 32));
                value /= 32;
            } while (value > 0);
        }

        internal static IEnumerable<byte> Decompose(IEnumerable<byte> bytes)
        {
            using (var enumerator = bytes.GetEnumerator()) {
                int carry = 0;
                var offset = 0;
                while (enumerator.MoveNext()) {
                    carry |= enumerator.Current << offset;
                    yield return unchecked((byte)(carry & 0b_0001_1111));
                    offset = (offset + 5) % 8;
                    carry = enumerator.Current >> offset;
                    if (offset < 3) {
                        yield return unchecked((byte)(carry & 0b_0001_1111));
                        offset += 5;
                        carry = enumerator.Current >> offset;
                    }
                }
                if (offset != 0)
                    yield return unchecked((byte)(carry & 0b_0001_1111));
            }
        }

        public static byte[] Decode(string text, bool checksum = false, bool strict = false)
        {
            throw new NotImplementedException();
        }

        public static string Normalize(string text)
        {
            if (text == null)
                return text;

            var ascii = Encoding.ASCII.GetBytes(text);
            var result = new char[ascii.Length];
            var startIndex = ascii.Length;
            if (ascii.Length > 1) {
                if (ChecksumDecoder.TryGetValue((char)ascii[startIndex - 1], out var checksum))
                    result[--startIndex] = ChecksumSymbols[checksum];
                else
                    throw new ArgumentException();
            }
            for (int i = startIndex - 1; i >= 0; i--) {
                char c = (char)ascii[i];
                if (c == '-')
#pragma warning disable CS0642 // No Op
                    ;
#pragma warning restore CS0642
                else if (Decoder.TryGetValue(c, out var offset))
                    result[--startIndex] = Symbols[offset];
                else
                    throw new ArgumentException();
            }
            return new string(result, startIndex, result.Length - startIndex);
        }
    }
}
