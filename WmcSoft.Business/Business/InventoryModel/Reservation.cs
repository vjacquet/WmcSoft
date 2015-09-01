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
using WmcSoft.Business.PartyModel;
using WmcSoft.Business.ProductModel;

namespace WmcSoft.Business.InventoryModel
{
    /// <summary>
    /// Represents the assignments of one or more <see cref="ProductInstance"/>s to one or more
    /// <see cref="Party"/> - that is, an arrangement by which a <see cref="ProductInstance"/> is kept for the
    /// use of a specific see cref="Party"/> at some point in time.
    /// </summary>
    public class Reservation
    {
        #region Fields

        readonly ReservationIdentifier _identifier;

        #endregion

        #region Lifecycle

        protected Reservation()
            : this(new ReservationIdentifier()) {
        }

        protected Reservation(ReservationIdentifier identifier) {
            _identifier = identifier;
        }

        #endregion

        public ReservationIdentifier Identifier {
            get { return _identifier; }
        }


    }
}
