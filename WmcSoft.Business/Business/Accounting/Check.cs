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
    /// on demand -- in other words, a written order to pay <see cref="Money"/> to a <see cref="Party"/>.
    /// </summary>
    public class Check : PaymentMethod
    {
        /// <summary>
        /// The name of the account on which the <see cref="Check"/> is drawn.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// The number of the account on which the <see cref="Check"/> is drawn.
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// The number of the <see cref="Check"/>.
        /// </summary>
        /// <remarks>This is a unique identifier for the <see cref="Check"/> within the 
        /// context of the account on which it is drawn.</remarks>
        public string CheckNumber { get; set; }

        /// <summary>
        /// The name of the <see cref="Party"/> to which the <see cref="Check"/> is made payable.
        /// </summary>
        public string Payee { get; set; }

        /// <summary>
        /// The date on which the <see cref="Check"/> was written.
        /// </summary>
        /// <remarks>Usually <see cref="Check"/>s are valid only for a set of time after this date.</remarks>
        public DateTime WrittenOn { get; set; }

        /// <summary>
        /// The name of the Bank that issued the <see cref="Check"/>.
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// The address of the Bank that issued the <see cref="Check"/>.
        /// </summary>
        public string BankAddress { get; set; }

        /// <summary>
        /// The unique identifier of the bank that issued the <see cref="Check"/>.
        /// </summary>
        public string BankIdentificationNumber { get; set; }
    }
}
