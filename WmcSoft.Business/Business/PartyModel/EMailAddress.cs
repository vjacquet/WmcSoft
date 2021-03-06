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
using System.Diagnostics;

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    /// Specifies a way of contacting a <see cref="Party"/> via e-mail.
    /// </summary>
    public class EmailAddress : AddressBase
    {
        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private string email;

        #endregion

        #region Properties

        public string Email {
            get => email;
            set => email = GuardEmailAddress(value);
        }

        public override string Address => email;

        #endregion

        #region Lifecycle

        public EmailAddress()
        {
            email = "undefined@example.com";
        }

        public EmailAddress(string email)
        {
            Email = email;
        }

        #endregion

        #region Helpers

        static string GuardEmailAddress(string email)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));

            try {
                var address = new System.Net.Mail.MailAddress(email);
                return email;
            } catch (Exception e) {
                throw new ArgumentException(nameof(email), e);
            }
        }

        #endregion
    }
}
