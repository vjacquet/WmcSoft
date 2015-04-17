﻿#region Licence

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
        class MailAddressEqualityComparer : IEqualityComparer<MailAddress>
        {
            #region IEqualityComparer<MailAddress> Membres

            public bool Equals(MailAddress x, MailAddress y) {
                return String.Equals(x.Address, y.Address, StringComparison.InvariantCultureIgnoreCase);
            }

            public int GetHashCode(MailAddress obj) {
                return obj.Address.ToLowerInvariant().GetHashCode();
            }

            #endregion
        }

        public static void AddRange(this MailAddressCollection self, IEnumerable<MailAddress> addresses) {
            var comparer = new MailAddressEqualityComparer();
            foreach (var address in addresses) {
                if (!self.Contains(address, comparer))
                    self.Add(address);
            }
        }
    }
}