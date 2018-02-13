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
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace WmcSoft.Security.Cryptography
{
    /// <summary>
    /// Computes the CRC32 has for the input data.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "CRC", Justification = "Matching algorithm acronym.")]
    public abstract class CRC32 : HashAlgorithm
    {
        protected CRC32()
        {
            HashSizeValue = 32;
        }

        /// <summary>
        /// Creates an instance of the default implementation of <see cref="CRC32"/>.
        /// </summary>
        /// <returns></returns>
        new public static CRC32 Create()
        {
            return new CRC32Managed();
        }

        public static CRC32 Create(uint polynomial)
        {
            return new CRC32Managed(polynomial);
        }

        new public static CRC32 Create(string hashName)
        {
            throw new NotImplementedException();
        }
    }
}
