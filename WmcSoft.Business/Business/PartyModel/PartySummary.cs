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
    /// <summary>
    /// Represents a snapshot of summary contact information about a <see cref="Party"/>,
    /// in relation to a particular context.
    /// </summary>
    public class PartySummary
    {
        #region Fields

        readonly PartyIdentifier _partyIdentifier;
        readonly PartyRoleIdentifier _roleIdentifier;

        #endregion

        #region Lifetime

        public PartySummary(PartyIdentifier partyIdentifier, PartyRoleIdentifier roleIdentifier) {
            _partyIdentifier = partyIdentifier;
            _roleIdentifier = roleIdentifier;
        }

        #endregion

        #region Properties

        public PartyIdentifier PartyIdentifier {
            get { return _partyIdentifier; }
        }

        public PartyRoleIdentifier RoleIdentifier {
            get { return _roleIdentifier; }
        }

        public string Name { get; set; }
        public string Address { get; set; }
        public string TelephoneNumber { get; set; }
        public string Email { get; set; }

        #endregion
    }
}
