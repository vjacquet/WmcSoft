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
using System.Net.Mail;

namespace WmcSoft.Net.Mail
{
    public static class SmtpExtensions
    {
        #region MailAddressEqualityComparer class

        class MailAddressEqualityComparer : IEqualityComparer<MailAddress>
        {
            public static readonly MailAddressEqualityComparer Default = new MailAddressEqualityComparer();

            public bool Equals(MailAddress x, MailAddress y)
            {
                if (ReferenceEquals(x, y))
                    return true;
                if (x == null || y == null)
                    return false;
                return string.Equals(x.Address, y.Address, StringComparison.InvariantCultureIgnoreCase);
            }

            public int GetHashCode(MailAddress obj)
            {
                if (obj == null || obj.Address == null)
                    return 0;
                return obj.Address.ToLowerInvariant().GetHashCode();
            }
        }

        #endregion

        /// <summary>
        /// Adds the <paramref name="addresses"/> to the list of mail addresses.
        /// </summary>
        /// <param name="self">The list of mail addresses.</param>
        /// <param name="addresses">The addresses to add.</param>
        /// <param name="comparer">The comparer to use, or <c>null</c> to use the default comparer.</param>
        /// <returns>The number of added addresses.</returns>
        public static int AddRange(this MailAddressCollection self, IEnumerable<MailAddress> addresses, IEqualityComparer<MailAddress> comparer = null)
        {
            comparer = comparer ?? MailAddressEqualityComparer.Default;
            var count = 0;
            foreach (var address in addresses) {
                if (!self.Contains(address, comparer)) {
                    self.Add(address);
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Removes the <paramref name="addresses"/> from the list of mail addresses.
        /// </summary>
        /// <param name="self">The list of mail addresses.</param>
        /// <param name="addresses">The addresses to add.</param>
        /// <param name="comparer">The comparer to use, or <c>null</c> to use the default comparer.</param>
        /// <returns>The number of removed addresses.</returns>
        public static int RemoveAll(this MailAddressCollection self, IEnumerable<MailAddress> addresses, IEqualityComparer<MailAddress> comparer = null)
        {
            comparer = comparer ?? MailAddressEqualityComparer.Default;
            var count = 0;
            foreach (var address in addresses) {
                if (self.Remove(address))
                    count++;
            }
            return count;
        }
    }
}
