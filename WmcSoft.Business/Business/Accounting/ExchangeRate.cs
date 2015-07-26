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
    /// Represents the price of one type of money relative to another.
    /// </summary>
    public class ExchangeRate : ITemporal
    {
        #region Fields

        private readonly Currency _from;
        private readonly Currency _to;
        private readonly decimal _rate;

        #endregion

        #region Lifecycle

        public ExchangeRate(Currency from, Currency to, decimal rate) {
            _from = from;
            _to = to;
            _rate = rate;
        }

        #endregion

        #region Properties

        public Currency FromCurrency { get { return _from; } }
        public Currency ToCurrency { get { return _to; } }
        public decimal Rate { get { return _rate; } }

        #endregion

        #region ITemporal Members

        public DateTime? ValidSince { get; set; }

        public DateTime? ValidUntil { get; set; }

        #endregion
    }
}
