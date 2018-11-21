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

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Hashing device to simplify the implementation of GetHashCode with multiple values.
    /// </summary>
    /// <remarks>So far, this class is 1.5 % slower than doing it manually, and 3% slower than using <see cref="EqualityComparer.CombineHashCodes"/>.</remarks>
    public struct Hasher
    {
        private int _hash;

        private Hasher(int hash)
        {
            _hash = hash;
        }

        public static implicit operator Hasher(int hash)
        {
            return new Hasher(hash);
        }

        public static implicit operator int(Hasher hasher)
        {
            return hasher._hash;
        }

        public static explicit operator Hasher(string hash)
        {
            return hash == null ? 0 : hash.GetHashCode();
        }

        public static Hasher operator ^(Hasher x, Hasher y)
        {
            return new Hasher(EqualityComparer.CombineHashCodes(x._hash, y._hash));
        }

        public static Hasher operator ^(Hasher x, string hash)
        {
            return hash != null
                ? (Hasher)EqualityComparer.CombineHashCodes(x._hash, hash.GetHashCode())
                : x;
        }

        public static Hasher operator ^(Hasher x, int hash)
        {
            return EqualityComparer.CombineHashCodes(x._hash, hash);
        }

        public static Hasher operator ^(Hasher x, bool hash)
        {
            return EqualityComparer.CombineHashCodes(x._hash, hash.GetHashCode());
        }

        public static Hasher operator ^(Hasher x, DateTime hash)
        {
            return EqualityComparer.CombineHashCodes(x._hash, hash.GetHashCode());
        }

        public static Hasher operator ^(Hasher x, decimal hash)
        {
            return EqualityComparer.CombineHashCodes(x._hash, hash.GetHashCode());
        }
    }
}
