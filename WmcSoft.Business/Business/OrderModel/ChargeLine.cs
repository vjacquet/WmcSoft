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
using WmcSoft.Business.Accounting;
using WmcSoft.Business.ProductModel;

namespace WmcSoft.Business.OrderModel
{
    /// <summary>
    /// Represents an additional charge for an <see cref="OrderLine"/> over and above the <see cref="OrderLine"/> value
    /// or an extra charge added to an <see cref="Order"/>.
    /// </summary>
    public class ChargeLine
    {
        #region Fields

        private readonly OrderLineIdentifier _identifier;

        #endregion

        #region Lifecycle

        protected ChargeLine() {
            _identifier = new OrderLineIdentifier();

            Taxes = new List<TaxOnLine>();
        }

        #endregion

        #region Properties

        public OrderLineIdentifier Identifier {
            get { return _identifier; }
        }

        public Money Amount { get; set; }

        public string Description { get; set; }

        public ICollection<TaxOnLine> Taxes { get; private set; }

        #endregion
    }
}
