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

namespace WmcSoft.Internals
{
    internal struct Identifier : IComparable<Identifier>
    {
        private readonly string storage;

        public Identifier(string value)
        {
            storage = value;
        }

        public int Length => storage != null ? storage.Length : 0;
        public bool IsNumeric => storage != null && char.IsDigit(storage[0]);
        public bool IsAlpha => storage != null && !char.IsDigit(storage[0]);

        public int CompareTo(Identifier other)
        {
            if (IsNumeric) {
                if (!other.IsNumeric) return 1;

                var result = Length - other.Length;
                if (result != 0)
                    return result;
            }
            return StringComparer.InvariantCulture.Compare(storage, other.storage);
        }
    }
}
