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

namespace WmcSoft.Business.Accounting
{
    /// <summary>
    /// Represents a bill of exchange drawn on a bank and payable
    /// on demand -- in other words, a written order to pay Money to a Party.
    /// </summary>
    public class Check : PaymentMethod
    {
        #region Properties

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public string CheckNumber { get; set; }

        public string Payee { get; set; }

        public DateTime WrittenOn { get; set; }

        public string BankName { get; set; }

        public string BankAddress { get; set; }

        public string BankIdentificationNumber { get; set; }

        #endregion
    }
}
