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
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Business.Accounting
{
    /// <summary>
    /// Represents Money paid by one Party to another, in return for goods or services.
    /// </summary>
    public class Payment
    {
        #region Fields

        private readonly Money _money;
        private readonly PaymentMethod _method;

        #endregion

        #region Lifecycle

        public Payment(Money money) {
            _money = money;
        }

        public Payment(Money money, PaymentMethod method) {
            _money = money;
            _method = method;
        }

        #endregion

        #region Properties

        public decimal Amount {
            get { return _money.Amount; }
        }

        public Currency Currency {
            get { return _money.Currency; }
        }

        public PaymentMethod PaidBy {
            get { return _method; }
        }

        /// <summary>
        /// The date on which the Payment was made by the payer.
        /// </summary>
        public DateTime? MadeOn { get; set; }

        /// <summary>
        /// The date on which the Payment was received by the payee.
        /// </summary>
        public DateTime? ReveivedOn { get; set; }

        /// <summary>
        /// The date on which the payee exepcts to receive the Payment.
        /// </summary>
        public DateTime? DueBy { get; set; }

        /// <summary>
        /// The date on which the Payment is cleared by a banking system 
        /// or other payment processing mechanism.
        /// </summary>
        public DateTime? ClearedBy { get; set; }

        #endregion
    }
}
