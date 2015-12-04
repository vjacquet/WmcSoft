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

namespace WmcSoft
{
    /// <summary>
    /// Defines an order on elements with means to jump from one to another.
    /// </summary>
    /// <typeparam name="T">The type of items to order.</typeparam>
    /// <remarks>The Compare method returns the distance between two elements.</remarks>
    public interface IOrdinal<T> : IComparer<T>
    {
        T Advance(T x, int n);
    }

    public struct Int32Ordinal : IOrdinal<int>
    {
        #region IOrdinal<int> Members

        public int Advance(int x, int n) {
            return checked(x + n);
        }

        #endregion

        #region IComparer<int> Members

        public int Compare(int x, int y) {
            return checked(x -y);
        }

        #endregion
    }

    public struct DateTimeOrdinal : IOrdinal<DateTime>
    {
        #region IOrdinal<DateTime> Members

        public DateTime Advance(DateTime x, int n) {
            return x.AddDays(n);
        }

        #endregion

        #region IComparer<DateTime> Members

        public int Compare(DateTime x, DateTime y) {
            return (int)Math.Truncate((x - y).TotalDays);
        }

        #endregion
    }

    public struct YearOrdinal : IOrdinal<DateTime>
    {
        #region IOrdinal<DateTime> Members

        public DateTime Advance(DateTime x, int n) {
            return x.AddYears(n);
        }

        #endregion

        #region IComparer<DateTime> Members

        public int Compare(DateTime x, DateTime y) {
            return (x.Year - y.Year);
        }

        #endregion
    }
}
