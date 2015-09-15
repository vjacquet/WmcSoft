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
    /// Represents pats of an <see cref="Order"/> that is a summary of particular
    /// goods or services ordered by a buyer.
    /// </summary>
    public class OrderLine
    {
        #region Fields

        private readonly OrderLineIdentifier _identifier;

        #endregion

        #region Lifecycle

        protected OrderLine() {
            _identifier = new OrderLineIdentifier();
        }

        #endregion

        #region Properties

        public OrderLineIdentifier Identifier {
            get { return _identifier; }
        }

        public ProductIdentifier ProductType { get; set; }

        public string Description { get; set; }

        public SerialNumber SerialNumber { get; set; }

        public int NumberOrdered { get; set; }

        public Money UnitPrice { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }

        #endregion
    }
}
