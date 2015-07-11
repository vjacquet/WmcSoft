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

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    /// The RegisteredIdentifier represents an identifier for a Party that
    /// has been assigned by a recognized or statutory body.
    /// </summary>
    public class RegisteredIdentifier : ITemporal
    {
        #region Fields

        readonly string _identifier;
        readonly string _registrationAuthority;

        #endregion

        #region Lifecycle

        public RegisteredIdentifier(string identifier, string registrationAuthority) {
            _identifier = identifier;
            _registrationAuthority = registrationAuthority;
        }

        #endregion

        #region Properties

        public string Identifier {
            get { return _identifier; }
        }

        public string RegistrationAuthority {
            get { return _registrationAuthority; }
        }

        #endregion

        #region ITemporal Members

        public DateTime ValidSince { get; set; }

        public DateTime? ValidUntil { get; set; }

        #endregion
    }
}
