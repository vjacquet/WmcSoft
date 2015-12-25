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

namespace WmcSoft.AI
{
    /// <summary>
    /// Gray representation of integers, which differs only by one bit when incrementing.
    /// </summary>
    public struct Gray
    {
        static readonly byte[] GrayCodeTable;
        static Gray() {
            GrayCodeTable = new byte[256];
            for (int i = 0; i < 256; i++) {
                GrayCodeTable[i] = ToGray(i);
            }
        }

        static byte ToGray(int i) {
            var gray = (uint)i;
            var sum = 0u;
            var bit = 128u;
            var parity = false;
            while (bit != 0) {
                if ((bit & gray) != 0)
                    parity = !parity;
                if (parity)
                    sum |= bit;
                bit >>= 1;
            }
            return (byte)sum;
        }

        byte _value;

        public Gray(int n) {
            if (n <= 0 || n > 255)
                throw new ArgumentOutOfRangeException("n");
            _value = GrayCodeTable[n];
        }

        public static implicit operator Gray(int n) {
            return new Gray(n);
        }

        public static explicit operator double (Gray g) {
            var n = Array.IndexOf(GrayCodeTable, g._value);
            return (n - 127.5d) * 0.0392d;
        }
    }
}
