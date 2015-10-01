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

namespace WmcSoft.Business
{
    /// <summary>
    /// Provides properties useful to define validity dates on a Business entity.
    /// </summary>
    /// <remarks><see cref="ValidSince"/> is inclusive while <see cref=" ValidUntil"/> is exclusive.</remarks>
    public interface ITemporal
    {
        DateTime? ValidSince { get; set; }
        DateTime? ValidUntil { get; set; }
    }

    public static class ITemporalExtensions
    {
        /// <summary>
        /// Check if the specified <see cref="ITemporal"/> is defined at the specified <see cref="DateTime"/>.
        /// </summary>
        /// <param name="self">The temporal.</param>
        /// <param name="dateTime">The date time.</param>
        /// <returns>Returns true if the <see cref="ITemporal"/> is valid at the specified date & time.</returns>
        public static bool IsValidOn(this ITemporal self, DateTime dateTime) {
            return (self.ValidSince == null || dateTime >= self.ValidSince.GetValueOrDefault())
                && (self.ValidUntil == null || dateTime < self.ValidUntil.GetValueOrDefault());
        }
    }
}
