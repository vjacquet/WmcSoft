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

using System.Text;

namespace WmcSoft.Text
{
    public static class RomanConverter
    {
        static readonly (int numeral, string roman)[] Numerals = new[] {
            (1000, "M"),
            ( 900, "CM"),
            ( 500, "D"),
            ( 400, "CD"),
            ( 100, "C"),
            (  90, "XC"),
            (  50, "L"),
            (  40, "XL"),
            (  10, "X"),
            (   9, "IX"),
            (   5, "V"),
            (   4, "IV"),
            (   1, "I"),
        };

        public static string FromInt32(int number)
        {
            var sb = new StringBuilder();
            foreach (var (value, letters) in Numerals) {
                sb.Append(letters, number / value);
                number %= value;
            }
            return sb.ToString();
        }

        public static int ToInt32(string text)
        {
            var result = 0;
            var i = 0;
            var index = 0;
            while (index < text.Length) {
                while (string.CompareOrdinal(Numerals[i].roman, 0, text, index, Numerals[i].roman.Length) != 0)
                    i++;
                result += Numerals[i].numeral;
                index += Numerals[i].roman.Length;
            }
            return result;
        }
    }
}
