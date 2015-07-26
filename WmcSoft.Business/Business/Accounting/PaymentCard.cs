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

namespace WmcSoft.Business.Accounting
{
    /// <summary>
    /// Represents a physical token such as a plastic card that
    /// authorizes the Party identified on it to charge the cost of goods 
    /// or services to an account.
    /// </summary>
    public abstract class PaymentCard : PaymentMethod, ITemporal
    {
        #region Properties

        public string CardAssociationName { get; set; }

        public string CardNumber { get; set; }

        public string NameOnCard { get; set; }

        public DateTime ExpiryDate {
            get {
                return ValidUntil ?? DateTime.MinValue; // null only when not initiliazed.
            }
            set {
                ValidUntil = value;
            }
        }

        public string BillingAddress { get; set; }

        public string CarVerificationCode { get; set; }

        public string IssueNumber { get; set; }

        #endregion

        #region ITemporal Members

        public DateTime? ValidSince { get; set; }

        public DateTime? ValidUntil { get; set; }

        #endregion
    }
}
