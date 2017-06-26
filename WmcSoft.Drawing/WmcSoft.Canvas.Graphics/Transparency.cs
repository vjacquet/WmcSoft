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

namespace WmcSoft.Canvas
{
    public struct Transparency
    {
        private readonly byte _storage;

        public Transparency(float value)
        {
            if (value < 0f || value > 1f) throw new ArgumentOutOfRangeException(nameof(value));

            _storage = (byte)(255f * value);
        }

        public static implicit operator int(Transparency value)
        {
            return value._storage;
        }
        public static implicit operator Transparency(int value)
        {
            if (value < 0 || value > 255) throw new ArgumentOutOfRangeException(nameof(value));
            return new Transparency(value / 255f);
        }

        public static implicit operator float(Transparency value)
        {
            return value._storage / 255f;
        }
        public static implicit operator Transparency(float value)
        {
            return new Transparency(value);
        }
    }
}
