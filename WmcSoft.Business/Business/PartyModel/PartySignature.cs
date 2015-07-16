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
using System.Text;

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    /// Represents the identifying mark of a Party.
    /// </summary>
    public class PartySignature
    {
        #region Fields

        private readonly PartyIdentifier _partyIdentifier;
        private readonly PartyAuthentication _partyAuthentication;
        private readonly DateTime _when;
        private readonly string _reason;

        #endregion

        #region Lifecycle

        public PartySignature(PartyIdentifier partyIdentifier, PartyAuthentication partyAuthentication, DateTime when, string reason) {
            _partyIdentifier = partyIdentifier;
            _partyAuthentication = partyAuthentication;
            _when = when;
            _reason = reason;
        }

        #endregion

        #region Properties

        public PartyIdentifier PartyIdentifier {
            get { return _partyIdentifier; }
        }

        public PartyAuthentication Authentication {
            get { return _partyAuthentication; }
        }

        public DateTime When {
            get { return _when; }
        }

        public string Reason {
            get { return _reason; }
        }

        #endregion
    }
}
