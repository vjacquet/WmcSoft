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
using System.IO;
using WmcSoft.Security.Cryptography;

namespace WmcSoft.IO
{
    /// <summary>
    /// Performs a lexicographical comparison of two streams.
    /// </summary>
    /// <remarks>The streams are consumed so it is up to the caller to eventually rewind them. 
    /// Therefore it is not suitable to use in a <see cref="IDictionary{Stream, TValue}"/>.</remarks>
    public class StreamComparer : IComparer<Stream>, IEqualityComparer<Stream>
    {
        public int Compare(Stream x, Stream y)
        {
            if (x == null)
                return y == null ? 0 : 1;
            if (y == null)
                return -1;

            var vx = x.ReadByte();
            var vy = y.ReadByte();

            while (vx != -1 && vy != -1) {
                var comparison = vx.CompareTo(vy);
                if (comparison != 0)
                    return comparison;
                vx = x.ReadByte();
                vy = y.ReadByte();
            }
            if (vx != -1)
                return 1;
            if (vy != -1)
                return -1;
            return 0;
        }

        public bool Equals(Stream x, Stream y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;

            if (x.CanSeek && y.CanSeek) {
                if (x.Length != y.Length)
                    return false;
            }

            int b;
            while ((b = x.ReadByte()) == y.ReadByte()) {
                if (b == -1)
                    return true;
            }
            return false;
        }

        public int GetHashCode(Stream obj)
        {
            if (obj == null)
                return 0;

            var crc = CRC32.Create();
            var h = crc.ComputeHash(obj);
            return BitConverter.ToInt32(h, 0);
        }
    }
}
