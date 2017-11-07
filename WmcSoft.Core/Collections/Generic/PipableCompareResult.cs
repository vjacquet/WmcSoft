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

using System.Diagnostics;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Utility device allowing to pipe the trichotomy compare result while maintaining the shortcut evaluation.
    /// </summary>
    public struct PipableCompareResult
    {
        private int _storage;

        public PipableCompareResult(int value)
        {
            _storage = value;
        }

        public static implicit operator PipableCompareResult(int x)
        {
            return new PipableCompareResult(x);
        }

        public static implicit operator int(PipableCompareResult x)
        {
            return x._storage;
        }

        public static bool operator true(PipableCompareResult x)
        {
            return x._storage != 0;
        }

        public static bool operator false(PipableCompareResult x)
        {
            return x._storage == 0;
        }

        public static PipableCompareResult operator |(PipableCompareResult x, PipableCompareResult y)
        {
            Debug.Assert(x._storage == 0);
            return y._storage;
        }
    }
}
