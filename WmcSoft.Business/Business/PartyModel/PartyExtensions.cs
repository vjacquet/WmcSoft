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
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Business.PartyModel
{
    public static class PartyExtensions
    {
        public static bool CanPlayRole<R>(this Party party) where R : PartyRole {
            bool isTypeCompatible = PartyRole.GetConstraintsOf<R>().Any(r => r.CanPlayRole(party));
            //if (isTypeCompatible && party is ISupportCapabilities) {
            //    // The capabilities is a RuleContext so use this to apply rules.
            //    var capabilities = ((ISupportCapabilities)party).Capabilities;
            //}
            return isTypeCompatible;
        }

        public static R AddRole<R>(this Party party) where R : PartyRole {
            var role = (R)Activator.CreateInstance(typeof(R), party);
            return role;
        }

        public static R EnsureRole<R>(this Party party) where R : PartyRole {
            var role = party.Roles.OfType<R>().FirstOrDefault();
            if (role == null)
                role = party.AddRole<R>();
            return role;
        }

        public static bool HasRole<R>(this Party party) where R : PartyRole {
            var role = party.Roles.OfType<R>().FirstOrDefault();
            return role != null;
        }
    }
}
