#region Licence

/****************************************************************************
          Copyright 1999-2018 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft.Security.Cryptography
{
    /// <summary>
    /// Computes the CRC32 has for the input data using the managed library.
    /// </summary>
    public class CRC32Managed : CRC32
    {
        private const uint AllOnes = 0xffffffff;
        private static Dictionary<uint, uint[]> Tables;

        private uint[] _table;
        private uint _crc;

        /// <summary>
        /// Returns the default polynomial (used in PKZip, Ethernet, etc)
        /// </summary>
        public const uint DefaultPolynomial = 0xEDB88320U; // Often the polynomial is shown reversed as 0x04C11DB7.

        static CRC32Managed()
        {
            Tables = new Dictionary<uint, uint[]>();
            Tables.Add(DefaultPolynomial, BuildCrc32Table(DefaultPolynomial));
        }

        static uint[] BuildCrc32Table(uint polynomial)
        {
            var table = new uint[256];

            // 256 values representing ASCII character codes.
            unchecked {
                for (int i = 0; i < 256; i++) {
                    var crc = (uint)i;
                    for (int j = 8; j > 0; j--) {
                        if ((crc & 1) == 1)
                            crc = polynomial ^ (crc >> 1);
                        else
                            crc >>= 1;
                    }
                    table[i] = crc;
                }
            }
            return table;
        }

        /// <summary>
        /// Creates a CRC32 object using the DefaultPolynomial
        /// </summary>
        public CRC32Managed() : this(DefaultPolynomial)
        {
        }

        /// <summary>
        /// Creates a Crc32 object using the specified Creates a Crc32 object 
        /// </summary>
        public CRC32Managed(uint polynomial)
        {
            if (!Tables.TryGetValue(polynomial, out _table)) {
                lock (Tables) {
                    if (!Tables.TryGetValue(polynomial, out _table)) {
                        _table = BuildCrc32Table(polynomial);
                        Tables.Add(polynomial, _table);
                    }
                }
            }
        }

        public override bool CanReuseTransform => true;
        public override bool CanTransformMultipleBlocks => true;

        /// <summary>
        /// Initializes an implementation of HashAlgorithm.
        /// </summary>
        public override void Initialize()
        {
            _crc = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        protected override void HashCore(byte[] buffer, int offset, int count)
        {
            unchecked {
                count += offset;
                uint crc = _crc ^ AllOnes;
                for (int i = offset; i < count; i++) {
                    crc = _table[(crc ^ buffer[i]) & 0xff] ^ (crc >> 8);
                }
                _crc = crc ^ AllOnes;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override byte[] HashFinal()
        {
            uint crc = _crc;
            HashValue = BitConverter.GetBytes(crc);
            return HashValue;
        }
    }
}
